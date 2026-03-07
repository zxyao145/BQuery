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
  eventNames?: readonly string[];
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

const getListenerMap = (listenerId: string) => {
  let listenerMap = listenersByRef.get(listenerId);
  if (!listenerMap) {
    listenerMap = new Map<string, ListenerRegistration>();
    listenersByRef.set(listenerId, listenerMap);
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
      invokeDotNet(dotNetRef, "WindowDoubleClick", obj);
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

const createEventHandler = <TEvent extends Event>(
  methodIdentifier: string,
  convert: (event: TEvent) => object,
  options?: { throttleMs?: number },
): ((dotNetRef: DotNetEventSink) => ListenerRegistration) => {
  const createHandler = (dotNetRef: DotNetEventSink): EventHandler => (event: Event) => {
    const args = convert(event as TEvent);
    invokeDotNet(dotNetRef, methodIdentifier, args);
  };

  return (dotNetRef) => ({
    handler: (options?.throttleMs
      ? throttle(createHandler(dotNetRef), options.throttleMs)
      : createHandler(dotNetRef)) as EventHandler,
  });
};

const createArglessEventHandler = (
  methodIdentifier: string,
  options?: { eventNames?: readonly string[]; throttleMs?: number },
): ((dotNetRef: DotNetEventSink) => ListenerRegistration) => {
  const createHandler = (dotNetRef: DotNetEventSink): EventHandler => () => {
    invokeDotNet(dotNetRef, methodIdentifier, {});
  };

  return (dotNetRef) => ({
    handler: (options?.throttleMs
      ? throttle(createHandler(dotNetRef), options.throttleMs)
      : createHandler(dotNetRef)) as EventHandler,
    eventNames: options?.eventNames,
  });
};

const createResizeHandler = (dotNetRef: DotNetEventSink): ListenerRegistration => ({
  handler: throttle(() => {
    const viewport = Viewport.getWidthAndHeight();
    invokeDotNet(dotNetRef, "WindowResize", { width: viewport[0], height: viewport[1] });
  }, defaultThrottleTicks) as EventHandler,
});

interface IWindowEventMap {
  [key: string]: (dotNetRef: DotNetEventSink) => ListenerRegistration;
}

const eventMap: IWindowEventMap = {
  click: createClickHandler,
  contextmenu: createEventHandler("WindowContextMenu", (event: PointerEvent) => eventConvertor.toMouseEvent(event)),
  mousedown: createEventHandler("WindowMouseDown", (event: PointerEvent) => eventConvertor.toMouseEvent(event)),
  mouseup: createEventHandler("WindowMouseUp", (event: PointerEvent) => eventConvertor.toMouseEvent(event)),
  mouseover: createEventHandler("WindowMouseOver", (event: MouseEvent) => eventConvertor.toMouseEvent(event), { throttleMs: defaultThrottleTicks }),
  mouseout: createEventHandler("WindowMouseOut", (event: MouseEvent) => eventConvertor.toMouseEvent(event), { throttleMs: defaultThrottleTicks }),
  mousemove: createEventHandler("WindowMouseMove", (event: MouseEvent) => eventConvertor.toMouseEvent(event), { throttleMs: defaultThrottleTicks }),
  resize: createResizeHandler,
  scroll: createArglessEventHandler("WindowScroll", { throttleMs: defaultThrottleTicks }),
  close: createArglessEventHandler("WindowClose", { eventNames: ["beforeunload", "pagehide"] }),
  focus: createEventHandler("WindowFocus", (event: FocusEvent) => eventConvertor.toFocusEventArgs(event)),
  blur: createEventHandler("WindowBlur", (event: FocusEvent) => eventConvertor.toFocusEventArgs(event)),
  touchstart: createEventHandler("WindowTouchStart", (event: TouchEvent) => eventConvertor.toTouchEventArgs(event), { throttleMs: defaultThrottleTicks }),
  touchmove: createEventHandler("WindowTouchMove", (event: TouchEvent) => eventConvertor.toTouchEventArgs(event), { throttleMs: defaultThrottleTicks }),
  touchend: createEventHandler("WindowTouchEnd", (event: TouchEvent) => eventConvertor.toTouchEventArgs(event), { throttleMs: defaultThrottleTicks }),
  touchcancel: createEventHandler("WindowTouchCancel", (event: TouchEvent) => eventConvertor.toTouchEventArgs(event), { throttleMs: defaultThrottleTicks }),
  keydown: createEventHandler("WindowKeyDown", (event: KeyboardEvent) => eventConvertor.toKeyboardEventArgs(event)),
  keypress: createEventHandler("WindowKeyPress", (event: KeyboardEvent) => eventConvertor.toKeyboardEventArgs(event)),
  keyup: createEventHandler("WindowKeyUp", (event: KeyboardEvent) => eventConvertor.toKeyboardEventArgs(event)),
};

const addWindowEventListener = (eventName: string, listenerId: string, dotNetRef: DotNetEventSink) => {
  const listenerMap = getListenerMap(listenerId);
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
  const targetEventNames = registration.eventNames ?? [eventName];
  for (const targetEventName of targetEventNames) {
    window.addEventListener(targetEventName, registration.handler);
  }
};

const removeWindowEventListener = (eventName: string, listenerId: string) => {
  const listenerMap = listenersByRef.get(listenerId);
  const registration = listenerMap?.get(eventName);
  if (!listenerMap || !registration) {
    return;
  }

  const targetEventNames = registration.eventNames ?? [eventName];
  for (const targetEventName of targetEventNames) {
    window.removeEventListener(targetEventName, registration.handler);
  }
  registration.handler.cancel?.();
  registration.dispose?.();
  listenerMap.delete(eventName);
  if (listenerMap.size === 0) {
    listenersByRef.delete(listenerId);
  }
};

const bindAllWindowEvent = (listenerId: string, dotNetRef: DotNetEventSink) => {
  Object.keys(eventMap).forEach((eventName) => {
    addWindowEventListener(eventName, listenerId, dotNetRef);
  });
};

const bindWindowEvents = (events: Array<EventInfo>, listenerId: string, dotNetRef: DotNetEventSink) => {
  if (!events || events.length === 0) return;
  if (events.some((e) => e.name === "*")) {
    bindAllWindowEvent(listenerId, dotNetRef);
    return;
  }

  for (let i = 0; i < events.length; i++) {
    if (!events[i].name) {
      console.error("error event name is required");
      continue;
    }

    addWindowEventListener(events[i].name, listenerId, dotNetRef);
  }
};

const removeWindowEventsListener = (events: Array<EventInfo>, listenerId: string, _dotNetRef: DotNetEventSink) => {
  if (!events || events.length === 0) return;
  if (events.some((e) => e.name === "*")) {
    Object.keys(eventMap).forEach((eventName) => {
      removeWindowEventListener(eventName, listenerId);
    });
    return;
  }

  for (let i = 0; i < events.length; i++) {
    if (!events[i].name) {
      continue;
    }

    removeWindowEventListener(events[i].name, listenerId);
  }
};

export { bindAllWindowEvent, bindWindowEvents, removeWindowEventsListener };
