import "../global";
import { throttle } from "./common";
import Viewport from "./Viewport";

export interface EventInfo {
  name: string;
}

interface IEventConvertor {
  toMouseEvent: Function;
  toFocusEventArgs: Function;
  toTouchEventArgs: Function;
  toKeyboardEventArgs: Function;
  toTouchPoints: Function;
  toTouchPoint: Function;
}

var eventConvertor: IEventConvertor = {
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
    var touches = [];
    for (var i = 0; i < pts.length; i++) {
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

var lastClick: number | null;
var clickTimeOut: number;

const onClick = (e: PointerEvent) => {
    var obj = eventConvertor.toMouseEvent(e);
    var now = new Date().getTime();
    var isDbClick = lastClick && now - lastClick < doubleClickTimeInterval;
    if (isDbClick) {
      window.clearTimeout(clickTimeOut);
      window.DotNet.invokeMethodAsync("BQuery", "WindowDbClick", obj);
      lastClick = null;
    } else {
      clickTimeOut = window.setTimeout(() => {
        window.DotNet.invokeMethodAsync("BQuery", "WindowClick", obj);
      }, doubleClickTimeInterval);
      lastClick = now;
    }
};

interface IWindowEventMap {
  [key: string]: any;
}


const eventMap: IWindowEventMap = {
  click: onClick,
  contextmenu: (e: PointerEvent) => {
    var obj = eventConvertor.toMouseEvent(e);
    window.DotNet.invokeMethodAsync("BQuery", "WindowContextMenu", obj);
  },
  mousedown: (e: PointerEvent) => {
    var obj = eventConvertor.toMouseEvent(e);
    window.DotNet.invokeMethodAsync("BQuery", "WindowMouseDown", obj);
  },
  mouseup: (e: PointerEvent) => {
    var obj = eventConvertor.toMouseEvent(e);
    window.DotNet.invokeMethodAsync("BQuery", "WindowMouseUp", obj);
  },
  mouseover: throttle((e: MouseEvent) => {
    var obj = eventConvertor.toMouseEvent(e);
    window.DotNet.invokeMethodAsync("BQuery", "WindowMouseOver", obj);
  }, defaultThrottleTicks),
  mouseout: throttle((e: MouseEvent) => {
    var obj = eventConvertor.toMouseEvent(e);
    window.DotNet.invokeMethodAsync("BQuery", "WindowMouseOut", obj);
  }, defaultThrottleTicks),
  mousemove: throttle((e: MouseEvent) => {
    var obj = eventConvertor.toMouseEvent(e);
    window.DotNet.invokeMethodAsync("BQuery", "WindowMouseMove", obj);
  }, defaultThrottleTicks),

  resize: throttle(() => {
    var vwhArr = Viewport.getWidthAndHeight();
    window.DotNet.invokeMethodAsync(
      "BQuery",
      "WindowResize",
      vwhArr[0],
      vwhArr[1],
    );
  }, defaultThrottleTicks),

  scroll: throttle((e: Event) => {
    window.DotNet.invokeMethodAsync("BQuery", "WindowScroll", e);
  }, defaultThrottleTicks),

  close: (e: Event) => {
    window.DotNet.invokeMethodAsync("BQuery", "WindowClose", e);
  },

  focus: (e: FocusEvent) => {
    var evt = eventConvertor.toFocusEventArgs(e);
    window.DotNet.invokeMethodAsync("BQuery", "WindowFocus", evt);
  },
  blur: (e: FocusEvent) => {
    var evt = eventConvertor.toFocusEventArgs(e);
    window.DotNet.invokeMethodAsync("BQuery", "WindowBlur", evt);
  },

  touchstart: throttle((e: TouchEvent) => {
    var evt = eventConvertor.toTouchEventArgs(e);
    window.DotNet.invokeMethodAsync("BQuery", "WindowTouchStart", evt);
  }, defaultThrottleTicks),

  touchmove: throttle((e: TouchEvent) => {
    var evt = eventConvertor.toTouchEventArgs(e);
    window.DotNet.invokeMethodAsync("BQuery", "WindowTouchMove", evt);
  }, defaultThrottleTicks),

  touchend: throttle((e: TouchEvent) => {
    var evt = eventConvertor.toTouchEventArgs(e);
    window.DotNet.invokeMethodAsync("BQuery", "WindowTouchEnd", evt);
  }, defaultThrottleTicks),
  touchcancel: throttle((e: TouchEvent) => {
    var evt = eventConvertor.toTouchEventArgs(e);
    window.DotNet.invokeMethodAsync("BQuery", "WindowTouchCancel", evt);
  }, defaultThrottleTicks),

  keydown: (e: KeyboardEvent) => {
    var evt = eventConvertor.toKeyboardEventArgs(e);
    window.DotNet.invokeMethodAsync("BQuery", "WindowKeyDown", evt);
  },
  keypress: (e: KeyboardEvent) => {
    var evt = eventConvertor.toKeyboardEventArgs(e);
    window.DotNet.invokeMethodAsync("BQuery", "WindowKeyPress", evt);
  },
  keyup: (e: KeyboardEvent) => {
    var evt = eventConvertor.toKeyboardEventArgs(e);
    window.DotNet.invokeMethodAsync("BQuery", "WindowKeyUp", evt);
  },
};


const bindAllWindowEvent = () => {
  Object.keys(eventMap).forEach((eventName) => {
    window.addEventListener(eventName, eventMap[eventName]);
  });
};

const bindWindowEvents = (events: Array<EventInfo>) => {
  console.debug("bindWindowEvents", events);
  if (!events || events.length === 0) return;
  if (events.some((e) => e.name === "*")) {
    bindAllWindowEvent();
    return;
  }

  for (let i = 0; i < events.length; i++) {
    if (!events[i].name) {
      console.error("error event name is required");
      continue;
    }
    console.debug("event name", events[i].name, eventMap[events[i].name]);
    if(eventMap[events[i].name]){
      window.addEventListener(events[i].name, eventMap[events[i].name]);
    } else {
        console.warn(`event ${events[i].name} is not supported`);
    }
  }
};

export { bindAllWindowEvent, bindWindowEvents };
