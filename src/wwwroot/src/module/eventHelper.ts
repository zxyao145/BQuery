import "../global";
import { throttle } from "./common";
import Viewport from "./Viewport";

export interface EventInfo {
  name: string;
}

type DotNetEventSink = {
  invokeMethodAsync(methodIdentifier: string, ...args: unknown[]): Promise<unknown>;
};

type EventHandler = EventListener & {
  cancel?: () => void;
};

type ListenerRegistration = {
  handler: EventHandler;
  dispose?: () => void;
};

interface IEventConvertor {
  toMouseEvent: (e: MouseEvent) => object;
  toFocusEventArgs: (e: FocusEvent) => object;
  toTouchEventArgs: (e: TouchEvent) => object;
  toKeyboardEventArgs: (e: KeyboardEvent) => object;
  toTouchPoints: (pts: TouchList) => object[];
  toTouchPoint: (pt: Touch) => object;
}

const eventConvertor: IEventConvertor = {
  toMouseEvent(e: MouseEvent) {
    return {
      detail: e.detail,
      screenX: e.screenX,
      screenY: e.screenY,
      clientX: e.clientX,
      clientY: e.clientY,
      button: e.button,
      buttons: e.buttons,
      ctrlKey: e.ctrlKey,
      shiftKey: e.shiftKey,
      altKey: e.altKey,
      metaKey: e.metaKey,
      type: e.type,
    };
  },
  toFocusEventArgs(e: FocusEvent) {
    return {
      type: e.type,
    };
  },
  toTouchEventArgs(e: TouchEvent) {
    return {
      detail: e.detail,
      touches: eventConvertor.toTouchPoints(e.touches),
      targetTouches: eventConvertor.toTouchPoints(e.targetTouches),
      changedTouches: eventConvertor.toTouchPoints(e.changedTouches),
      ctrlKey: e.ctrlKey,
      shiftKey: e.shiftKey,
      altKey: e.altKey,
      metaKey: e.metaKey,
      type: e.type,
    };
  },
  toKeyboardEventArgs(e: KeyboardEvent) {
    return {
      key: e.key,
      code: e.code,
      location: e.location,
      repeat: e.repeat,
      ctrlKey: e.ctrlKey,
      shiftKey: e.shiftKey,
      altKey: e.altKey,
      metaKey: e.metaKey,
      type: e.type,
    };
  },
  toTouchPoints(pts: TouchList) {
    const touches = [];
    for (let i = 0; i < pts.length; i++) {
      touches.push(eventConvertor.toTouchPoint(pts[i]));
    }
    return touches;
  },
  toTouchPoint(pt: Touch) {
    return {
      identifier: pt.identifier,
      screenX: pt.screenX,
      screenY: pt.screenY,
      clientX: pt.clientX,
      clientY: pt.clientY,
      pageX: pt.pageX,
      pageY: pt.pageY,
    };
  },
};

// used for:
// window.onmouseover、window.onmouseout、window.onmousemove
// window.onresize、window.onscroll
// window.ontouchstart、window.ontouchmove、 window.ontouchend、window.ontouchcancel
const defaultThrottleTicks = 50;
const doubleClickTimeInterval = 230;

const listenersByRef = new Map<string, Map<string, ListenerRegistration>>();

const getDotNetRefKey = (dotNetRef: DotNetEventSink) => {
  const ref = dotNetRef as DotNetEventSink & {
    _id?: string | number;
    id?: string | number;
    dotNetObjectId?: string | number;
    __dotNetObject?: string | number;
  };

  const key = ref._id ?? ref.id ?? ref.dotNetObjectId ?? ref.__dotNetObject;
  if (key === undefined || key === null) {
    throw new Error("Unable to resolve DotNetObjectReference identity.");
  }

  return String(key);
};

const getListenerMap = (dotNetRef: DotNetEventSink) => {
  const refKey = getDotNetRefKey(dotNetRef);
  let listenerMap = listenersByRef.get(refKey);
  if (!listenerMap) {
    listenerMap = new Map<string, ListenerRegistration>();
    listenersByRef.set(refKey, listenerMap);
  }
  return listenerMap;
};

const isDisposedDotNetReferenceError = (error: unknown) => {
  const message = error instanceof Error ? error.message : String(error);
  return message.includes("There is no tracked object with id")
    || message.includes("dotNetObjectId");
};

const invokeDotNet = (dotNetRef: DotNetEventSink, methodIdentifier: string, ...args: unknown[]) => {
  void dotNetRef.invokeMethodAsync(methodIdentifier, ...args).catch((error) => {
    if (isDisposedDotNetReferenceError(error)) {
      return;
    }

    console.error(`failed to invoke '${methodIdentifier}'`, error);
  });
};

const createClickHandler = (dotNetRef: DotNetEventSink): ListenerRegistration => {
  let lastClick: number | null = null;
  let clickTimeout: number | null = null;
  let disposed = false;

  const handler: EventHandler = (e: Event) => {
    if (disposed) {
      return;
    }

    const evt = e as PointerEvent;
    const obj = eventConvertor.toMouseEvent(evt);
    const now = Date.now();
    const isDbClick = lastClick !== null && now - lastClick < doubleClickTimeInterval;

    if (isDbClick) {
      if (clickTimeout !== null) {
        window.clearTimeout(clickTimeout);
        clickTimeout = null;
      }
      invokeDotNet(dotNetRef, "WindowDbClick", obj);
      lastClick = null;
      return;
    }

    clickTimeout = window.setTimeout(() => {
      if (disposed) {
        return;
      }

      invokeDotNet(dotNetRef, "WindowClick", obj);
      clickTimeout = null;
    }, doubleClickTimeInterval);
    lastClick = now;
  };

  return {
    handler,
    dispose: () => {
      disposed = true;
      lastClick = null;
      if (clickTimeout !== null) {
        window.clearTimeout(clickTimeout);
        clickTimeout = null;
      }
    },
  };
};

interface IWindowEventMap {
  [key: string]: (dotNetRef: DotNetEventSink) => ListenerRegistration;
}

const eventMap: IWindowEventMap = {
  click: createClickHandler,
  contextmenu: (dotNetRef) => ({
    handler: (e: Event) => {
    const obj = eventConvertor.toMouseEvent(e as PointerEvent);
    invokeDotNet(dotNetRef, "WindowContextMenu", obj);
  },
  }),
  mousedown: (dotNetRef) => ({
    handler: (e: Event) => {
    const obj = eventConvertor.toMouseEvent(e as PointerEvent);
    invokeDotNet(dotNetRef, "WindowMouseDown", obj);
  },
  }),
  mouseup: (dotNetRef) => ({
    handler: (e: Event) => {
    const obj = eventConvertor.toMouseEvent(e as PointerEvent);
    invokeDotNet(dotNetRef, "WindowMouseUp", obj);
  },
  }),
  mouseover: (dotNetRef) => ({
    handler: throttle((e: MouseEvent) => {
    const obj = eventConvertor.toMouseEvent(e);
    invokeDotNet(dotNetRef, "WindowMouseOver", obj);
  }, defaultThrottleTicks) as EventHandler,
  }),
  mouseout: (dotNetRef) => ({
    handler: throttle((e: MouseEvent) => {
    const obj = eventConvertor.toMouseEvent(e);
    invokeDotNet(dotNetRef, "WindowMouseOut", obj);
  }, defaultThrottleTicks) as EventHandler,
  }),
  mousemove: (dotNetRef) => ({
    handler: throttle((e: MouseEvent) => {
    const obj = eventConvertor.toMouseEvent(e);
    invokeDotNet(dotNetRef, "WindowMouseMove", obj);
  }, defaultThrottleTicks) as EventHandler,
  }),
  resize: (dotNetRef) => ({
    handler: throttle(() => {
    const viewport = Viewport.getWidthAndHeight();
    invokeDotNet(dotNetRef, "WindowResize", viewport[0], viewport[1]);
  }, defaultThrottleTicks) as EventHandler,
  }),
  scroll: (dotNetRef) => ({
    handler: throttle(() => {
    invokeDotNet(dotNetRef, "WindowScroll", {});
  }, defaultThrottleTicks) as EventHandler,
  }),
  close: (dotNetRef) => ({
    handler: () => {
    invokeDotNet(dotNetRef, "WindowClose", {});
  },
  }),
  focus: (dotNetRef) => ({
    handler: (e: Event) => {
    const evt = eventConvertor.toFocusEventArgs(e as FocusEvent);
    invokeDotNet(dotNetRef, "WindowFocus", evt);
  },
  }),
  blur: (dotNetRef) => ({
    handler: (e: Event) => {
    const evt = eventConvertor.toFocusEventArgs(e as FocusEvent);
    invokeDotNet(dotNetRef, "WindowBlur", evt);
  },
  }),
  touchstart: (dotNetRef) => ({
    handler: throttle((e: TouchEvent) => {
    const evt = eventConvertor.toTouchEventArgs(e);
    invokeDotNet(dotNetRef, "WindowTouchStart", evt);
  }, defaultThrottleTicks) as EventHandler,
  }),
  touchmove: (dotNetRef) => ({
    handler: throttle((e: TouchEvent) => {
    const evt = eventConvertor.toTouchEventArgs(e);
    invokeDotNet(dotNetRef, "WindowTouchMove", evt);
  }, defaultThrottleTicks) as EventHandler,
  }),
  touchend: (dotNetRef) => ({
    handler: throttle((e: TouchEvent) => {
    const evt = eventConvertor.toTouchEventArgs(e);
    invokeDotNet(dotNetRef, "WindowTouchEnd", evt);
  }, defaultThrottleTicks) as EventHandler,
  }),
  touchcancel: (dotNetRef) => ({
    handler: throttle((e: TouchEvent) => {
    const evt = eventConvertor.toTouchEventArgs(e);
    invokeDotNet(dotNetRef, "WindowTouchCancel", evt);
  }, defaultThrottleTicks) as EventHandler,
  }),
  keydown: (dotNetRef) => ({
    handler: (e: Event) => {
    const evt = eventConvertor.toKeyboardEventArgs(e as KeyboardEvent);
    invokeDotNet(dotNetRef, "WindowKeyDown", evt);
  },
  }),
  keypress: (dotNetRef) => ({
    handler: (e: Event) => {
    const evt = eventConvertor.toKeyboardEventArgs(e as KeyboardEvent);
    invokeDotNet(dotNetRef, "WindowKeyPress", evt);
  },
  }),
  keyup: (dotNetRef) => ({
    handler: (e: Event) => {
    const evt = eventConvertor.toKeyboardEventArgs(e as KeyboardEvent);
    invokeDotNet(dotNetRef, "WindowKeyUp", evt);
  },
  }),
};

const addWindowEventListener = (eventName: string, dotNetRef: DotNetEventSink) => {
  const listenerMap = getListenerMap(dotNetRef);
  if (listenerMap.has(eventName)) {
    return;
  }

  const factory = eventMap[eventName];
  if (!factory) {
    console.warn(`event ${eventName} is not supported`);
    return;
  }

  const registration = factory(dotNetRef);
  listenerMap.set(eventName, registration);
  window.addEventListener(eventName, registration.handler);
};

const removeWindowEventListener = (eventName: string, dotNetRef: DotNetEventSink) => {
  const listenerMap = listenersByRef.get(getDotNetRefKey(dotNetRef));
  const registration = listenerMap?.get(eventName);
  if (!registration) {
    return;
  }

  window.removeEventListener(eventName, registration.handler);
  registration.handler.cancel?.();
  registration.dispose?.();
  listenerMap.delete(eventName);
  if (listenerMap.size === 0) {
    listenersByRef.delete(getDotNetRefKey(dotNetRef));
  }
};

const bindAllWindowEvent = (dotNetRef: DotNetEventSink) => {
  Object.keys(eventMap).forEach((eventName) => {
    addWindowEventListener(eventName, dotNetRef);
  });
};

const bindWindowEvents = (events: Array<EventInfo>, dotNetRef: DotNetEventSink) => {
  if (!events || events.length === 0) return;
  if (events.some((e) => e.name === "*")) {
    bindAllWindowEvent(dotNetRef);
    return;
  }

  for (let i = 0; i < events.length; i++) {
    if (!events[i].name) {
      console.error("error event name is required");
      continue;
    }

    addWindowEventListener(events[i].name, dotNetRef);
  }
};

const removeWindowEventsListener = (events: Array<EventInfo>, dotNetRef: DotNetEventSink) => {
  if (!events || events.length === 0) return;
  if (events.some((e) => e.name === "*")) {
    Object.keys(eventMap).forEach((eventName) => {
      removeWindowEventListener(eventName, dotNetRef);
    });
    return;
  }

  for (let i = 0; i < events.length; i++) {
    if (!events[i].name) {
      continue;
    }

    removeWindowEventListener(events[i].name, dotNetRef);
  }
};

export { bindAllWindowEvent, bindWindowEvents, removeWindowEventsListener };
