import "../global";
import { autoDebug, throttle } from "./common";
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

type ListenerIdInfo = {
  dotNetRef: DotNetEventSink;
  events: Set<EventInfo>;
};

type ListenerRegistration = {
  handler: EventHandler;
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

/**
 * listenerId -->  dotNetRef
 */
const listenersDotNetRef = new Map<string, ListenerIdInfo>();
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
    const listenerIdInfo = listenersDotNetRef.get(listenerId);
    if (!listenerIdInfo) {
      continue;
    }
    void listenerIdInfo.dotNetRef
      .invokeMethodAsync(methodIdentifier, ...args)
      .catch((error) => {
        if (isDisposedDotNetReferenceError(error)) {
          return;
        }

        console.error(`failed to invoke '${methodIdentifier}'`, error);
      });
  }
};

type EventHandlerOptions = {
  throttleMs?: number;
};
const createEventHandler = <TEvent extends Event>(
  methodIdentifier: string,
  convert: (event: TEvent) => object,
  options?: EventHandlerOptions,
): ListenerRegistration => {
  const createHandler = (): EventHandler => (event: Event) => {
    const args = convert(event as TEvent);
    if(event.type === "keydown"){
      console.log(`event: ${event.type}, args: `, methodIdentifier, args);
    }
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
  //#region MouseEvent
  mousedown: createEventHandler("WindowMouseDown", (event: PointerEvent) =>
    eventConvertor.toMouseEvent(event),
  ),
  mouseup: createEventHandler("WindowMouseUp", (event: PointerEvent) =>
    eventConvertor.toMouseEvent(event),
  ),
  click: createEventHandler("WindowClick", (event: PointerEvent) =>
    eventConvertor.toMouseEvent(event),
  ),
  dblclick: createEventHandler("WindowDblClick", (event: PointerEvent) =>
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
  contextmenu: createEventHandler("WindowContextMenu", (event: PointerEvent) =>
    eventConvertor.toMouseEvent(event),
  ),
  //#endregion

  //#region size or state event
  resize: createResizeHandler(),
  scroll: createArglessEventHandler("WindowScroll", {
    throttleMs: defaultThrottleTicks,
  }),
  //#endregion

  //#region FocusEvent
  focus: createEventHandler("WindowFocus", (event: FocusEvent) =>
    eventConvertor.toFocusEventArgs(event),
  ),
  blur: createEventHandler("WindowBlur", (event: FocusEvent) =>
    eventConvertor.toFocusEventArgs(event),
  ),
  //#endregion

  //#region TouchEvent
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
  //#endregion

  //#region Keyboard Event

  keydown: createEventHandler("WindowKeyDown", (event: KeyboardEvent) => {
    return eventConvertor.toKeyboardEventArgs(event);
  }),
  keypress: createEventHandler("WindowKeyPress", (event: KeyboardEvent) =>
    eventConvertor.toKeyboardEventArgs(event),
  ),
  keyup: createEventHandler("WindowKeyUp", (event: KeyboardEvent) =>
    eventConvertor.toKeyboardEventArgs(event),
  ),
  //#endregion
};

const addWindowEventListener = (
  evt: EventInfo,
  listenerId: string,
  dotNetRef: DotNetEventSink,
) => {
  const eventName = evt.name;
  if (!listenersDotNetRef.has(listenerId)) {
    listenersDotNetRef.set(listenerId, {
      dotNetRef: dotNetRef,
      events: new Set<EventInfo>([evt]),
    });
  } else {
    listenersDotNetRef.get(listenerId)?.events.add(evt);
  }
  if (eventListeners.has(eventName)) {
    eventListeners.get(eventName)!.add(listenerId);
    return;
  }
  eventListeners.set(eventName, new Set<string>([listenerId]));
  const listenerRegistration = eventMap[eventName];
  autoDebug(() => {
    console.log(
      "add event listener for event: ",
      eventName,
      "listenerId: ",
      listenerId,
    );
  });
  window.addEventListener(eventName, listenerRegistration.handler);
};

const removeWindowEventListener = (evt: EventInfo, listenerId: string) => {
  const eventName = evt.name;
  const listenerIdInfo = listenersDotNetRef.get(listenerId);
  if (!listenerIdInfo) {
    return;
  }

  // new events
  const newEventInfos = [...listenerIdInfo.events].filter(
    (e) => e.name !== eventName,
  );
  listenerIdInfo.events = new Set(newEventInfos);

  // 不存在 event 了，删除
  if (listenerIdInfo.events.size === 0) {
    listenersDotNetRef.delete(listenerId);
  }

  // 从 event 的 callback 中删除 listenerId
  const listenerIds = eventListeners.get(eventName);

  if (listenerIds) {
    listenerIds.delete(listenerId);
  }
  if (!listenerIds || listenerIds.size === 0) {
    eventListeners.delete(eventName);
    // 没有关心事件的 listenerId， 删除事件监听
    const listenerRegistration = eventMap[eventName];
    autoDebug(() => {
      console.log(
        "remove event listener for event: ",
        eventName,
        "listenerId: ",
        listenerId,
      );
    });
    window.removeEventListener(eventName, listenerRegistration.handler);
  }
};

const bindAllWindowEvent = (listenerId: string, dotNetRef: DotNetEventSink) => {
  Object.keys(eventMap).forEach((eventName) => {
    addWindowEventListener(
      {
        name: eventName,
      },
      listenerId,
      dotNetRef,
    );
  });
};

const addWindowEventsListener = (
  events: Array<EventInfo>,
  listenerId: string,
  dotNetRef: DotNetEventSink,
) => {
  if (!events || events.length === 0) return;
  if (events.some((e) => e.name === "*")) {
    bindAllWindowEvent(listenerId, dotNetRef);
    return;
  }
  autoDebug(() => {
    console.log("add events: ", events);
  });
  for (let i = 0; i < events.length; i++) {
    if (!events[i].name) {
      console.error("error event name is required");
      continue;
    }

    addWindowEventListener(events[i], listenerId, dotNetRef);
  }
};

const removeWindowEventsListener = (
  events: Array<EventInfo>,
  listenerId: string,
) => {
  if (!events || events.length === 0) return;
  if (events.some((e) => e.name === "*")) {
    Object.keys(eventMap).forEach((eventName) => {
      removeWindowEventListener({ name: eventName }, listenerId);
    });
    return;
  }

  for (let i = 0; i < events.length; i++) {
    if (!events[i].name) {
      continue;
    }

    removeWindowEventListener(events[i], listenerId);
  }
};

const disposeWindowEventsListener = (listenerId: string) => {
  const listenerIdInfo = listenersDotNetRef.get(listenerId);
  if (listenerIdInfo) {
    const events = [...listenerIdInfo.events];
    for (let i = 0; i < events.length; i++) {
      removeWindowEventListener(events[i], listenerId);
    }
  }
};

export {
  addWindowEventListener,
  removeWindowEventListener,
  disposeWindowEventsListener,
  addWindowEventsListener,
  removeWindowEventsListener,
};
