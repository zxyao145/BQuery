import "../global";
import { throttle } from "./common";
import Viewport from "./Viewport";

export interface EventInfo {
  name: string;
}

type DotNetEventSink = {
  invokeMethodAsync(
    methodIdentifier: string,
    ...args: unknown[]
  ): Promise<unknown>;
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

/**
 * listenerId -->  dotNetRef
 */
const listenersDotNetRef = new Map<string, DotNetEventSink>();
/**
 * event name  --> listenerId list
 */
const eventListeners = new Map<string, Set<string>>();

const isDisposedDotNetReferenceError = (error: unknown) => {
  const message = error instanceof Error ? error.message : String(error);
  return (
    message.includes("There is no tracked object with id") ||
    message.includes("dotNetObjectId")
  );
};

const invokeDotNet = (
  eventName: string,
  methodIdentifier: string,
  ...args: unknown[]
) => {
  const listenerIds = eventListeners.get(eventName);
  if (!listenerIds) {
    return;
  }
  for (const listenerId of listenerIds) {
    const dotNetRef = listenersDotNetRef.get(listenerId);
    if (!dotNetRef) {
      continue;
    }
    void dotNetRef
      .invokeMethodAsync(methodIdentifier, ...args)
      .catch((error) => {
        if (isDisposedDotNetReferenceError(error)) {
          return;
        }

        console.error(`failed to invoke '${methodIdentifier}'`, error);
      });
  }
};

const createClickHandler = (): ListenerRegistration => {
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
    const isDbClick =
      lastClick !== null && now - lastClick < doubleClickTimeInterval;

    if (isDbClick) {
      if (clickTimeout !== null) {
        window.clearTimeout(clickTimeout);
        clickTimeout = null;
      }
      invokeDotNet("click", "WindowDoubleClick", obj);
      lastClick = null;
      return;
    }

    clickTimeout = window.setTimeout(() => {
      if (disposed) {
        return;
      }
      invokeDotNet("click", "WindowClick", obj);
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

type EventHandlerOptions = {
  eventNames?: readonly string[];
  throttleMs?: number;
};
const createEventHandler = <TEvent extends Event>(
  methodIdentifier: string,
  convert: (event: TEvent) => object,
  options?: EventHandlerOptions,
): ListenerRegistration => {
  const createHandler = (): EventHandler => (event: Event) => {
    const args = convert(event as TEvent);
    invokeDotNet(event.type, methodIdentifier, args);
  };

  return {
    handler: options?.throttleMs
      ? throttle(createHandler(), options.throttleMs)
      : createHandler(),
  };
};

const createArglessEventHandler = (
  methodIdentifier: string,
  options?: EventHandlerOptions,
): ListenerRegistration => {
  const createHandler = (): EventHandler => (e: Event) => {
    invokeDotNet(e.type, methodIdentifier, {});
  };

  return {
    handler: (options?.throttleMs
      ? throttle(createHandler(), options.throttleMs)
      : createHandler()) as EventHandler,
    eventNames: options?.eventNames,
  };
};

const createResizeHandler = (): ListenerRegistration => ({
  handler: throttle(() => {
    const viewport = Viewport.getWidthAndHeight();
    invokeDotNet("resize", "WindowResize", {
      width: viewport[0],
      height: viewport[1],
    });
  }, defaultThrottleTicks) as EventHandler,
});

interface IWindowEventMap {
  [key: string]: ListenerRegistration;
}

const eventMap: IWindowEventMap = {
  click: createClickHandler(),
  contextmenu: createEventHandler("WindowContextMenu", (event: PointerEvent) =>
    eventConvertor.toMouseEvent(event),
  ),
  mousedown: createEventHandler("WindowMouseDown", (event: PointerEvent) =>
    eventConvertor.toMouseEvent(event),
  ),
  mouseup: createEventHandler("WindowMouseUp", (event: PointerEvent) =>
    eventConvertor.toMouseEvent(event),
  ),
  mouseover: createEventHandler(
    "WindowMouseOver",
    (event: MouseEvent) => eventConvertor.toMouseEvent(event),
    { throttleMs: defaultThrottleTicks },
  ),
  mouseout: createEventHandler(
    "WindowMouseOut",
    (event: MouseEvent) => eventConvertor.toMouseEvent(event),
    { throttleMs: defaultThrottleTicks },
  ),
  mousemove: createEventHandler(
    "WindowMouseMove",
    (event: MouseEvent) => eventConvertor.toMouseEvent(event),
    { throttleMs: defaultThrottleTicks },
  ),
  resize: createResizeHandler(),
  scroll: createArglessEventHandler("WindowScroll", {
    throttleMs: defaultThrottleTicks,
  }),
  close: createArglessEventHandler("WindowClose", {
    eventNames: ["beforeunload", "pagehide"],
  }),
  focus: createEventHandler("WindowFocus", (event: FocusEvent) =>
    eventConvertor.toFocusEventArgs(event),
  ),
  blur: createEventHandler("WindowBlur", (event: FocusEvent) =>
    eventConvertor.toFocusEventArgs(event),
  ),
  touchstart: createEventHandler(
    "WindowTouchStart",
    (event: TouchEvent) => eventConvertor.toTouchEventArgs(event),
    { throttleMs: defaultThrottleTicks },
  ),
  touchmove: createEventHandler(
    "WindowTouchMove",
    (event: TouchEvent) => eventConvertor.toTouchEventArgs(event),
    { throttleMs: defaultThrottleTicks },
  ),
  touchend: createEventHandler(
    "WindowTouchEnd",
    (event: TouchEvent) => eventConvertor.toTouchEventArgs(event),
    { throttleMs: defaultThrottleTicks },
  ),
  touchcancel: createEventHandler(
    "WindowTouchCancel",
    (event: TouchEvent) => eventConvertor.toTouchEventArgs(event),
    { throttleMs: defaultThrottleTicks },
  ),
  keydown: createEventHandler("WindowKeyDown", (event: KeyboardEvent) =>
    eventConvertor.toKeyboardEventArgs(event),
  ),
  keypress: createEventHandler("WindowKeyPress", (event: KeyboardEvent) =>
    eventConvertor.toKeyboardEventArgs(event),
  ),
  keyup: createEventHandler("WindowKeyUp", (event: KeyboardEvent) =>
    eventConvertor.toKeyboardEventArgs(event),
  ),
};

const addWindowEventListener = (
  eventName: string,
  listenerId: string,
  dotNetRef: DotNetEventSink,
) => {
  listenersDotNetRef.set(listenerId, dotNetRef);
  if (eventListeners.has(eventName)) {
    eventListeners.get(eventName)!.add(listenerId);
    return;
  }
  eventListeners.set(eventName, new Set<string>([listenerId]));
  const listenerRegistration = eventMap[eventName];
  window.addEventListener(eventName, listenerRegistration.handler);
};

const removeWindowEventListener = (eventName: string, listenerId: string) => {
  const listenerIds = eventListeners.get(eventName);
  if (listenerIds && listenerIds.has(listenerId)) {
    listenerIds.delete(listenerId);
  }
  if (!listenerIds || listenerIds.size === 0) {
    eventListeners.delete(eventName);
    const listenerRegistration = eventMap[eventName];
    window.removeEventListener(eventName, listenerRegistration.handler);
  }
};

const bindAllWindowEvent = (listenerId: string, dotNetRef: DotNetEventSink) => {
  Object.keys(eventMap).forEach((eventName) => {
    addWindowEventListener(eventName, listenerId, dotNetRef);
  });
};

const bindWindowEvents = (
  events: Array<EventInfo>,
  listenerId: string,
  dotNetRef: DotNetEventSink,
) => {
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

const removeWindowEventsListener = (
  events: Array<EventInfo>,
  listenerId: string,
  _dotNetRef: DotNetEventSink,
) => {
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

export { addWindowEventListener, removeWindowEventListener, bindAllWindowEvent, bindWindowEvents, removeWindowEventsListener };
