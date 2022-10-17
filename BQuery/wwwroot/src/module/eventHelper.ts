import "../global"
import { throttle } from "./common";
import Viewport from "./ViewPort";

interface IEventConvertor {
    toMouseEvent: Function,
    toFocusEventArgs: Function,
    toTouchEventArgs: Function,
    toKeyboardEventArgs: Function,
    toTouchPoints: Function,
    toTouchPoint: Function,
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
            type: e.type
        };
    },
    toFocusEventArgs(e: FocusEvent) {
        return {
            type: e.type
        }
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
            type: e.type
        }
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
            type: e.type
        }
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
            pageY: pt.pageY
        }
    }
}

// used for:
// window.onmouseover、window.onmouseout、window.onmousemove
// window.onresize、window.onscroll
// window.ontouchstart、window.ontouchmove、 window.ontouchend、window.ontouchcancel
const defaultThrottleTicks = 80;


const doubleClickTimeInterval = 230;

const bindEvent = () => {

    var DotNet = window.DotNet;

    //#region window events

    var lastClick: number;
    var clickTimeOut: number;

    window.onclick = e => {
        var obj = eventConvertor.toMouseEvent(e);
        var now = (new Date()).getTime();
        if (lastClick && ((now - lastClick) < doubleClickTimeInterval)) {
            window.clearTimeout(clickTimeOut);
            DotNet.invokeMethodAsync("BQuery", "WindowDbClick", obj);
            lastClick = null;
        } else {
            clickTimeOut = window.setTimeout(() => {
                DotNet.invokeMethodAsync("BQuery", "WindowClick", obj);
            },
                doubleClickTimeInterval);
            lastClick = now;
        }
    };

    window.oncontextmenu = e => {
        var obj = eventConvertor.toMouseEvent(e);
        DotNet.invokeMethodAsync("BQuery", "WindowContextMenu", obj);
    };

    window.onmousedown = e => {
        var obj = eventConvertor.toMouseEvent(e);
        DotNet.invokeMethodAsync("BQuery", "WindowMouseDown", obj);
    };

    window.onmouseup = e => {
        var obj = eventConvertor.toMouseEvent(e);
        DotNet.invokeMethodAsync("BQuery", "WindowMouseUp", obj);
    };

    window.onmouseover = throttle((e: MouseEvent) => {
        var obj = eventConvertor.toMouseEvent(e);
        DotNet.invokeMethodAsync("BQuery", "WindowMouseOver", obj);
    }, defaultThrottleTicks);
    window.onmouseout = throttle((e: MouseEvent) => {
        var obj = eventConvertor.toMouseEvent(e);
        DotNet.invokeMethodAsync("BQuery", "WindowMouseOut", obj);
    }, defaultThrottleTicks);
    window.onmousemove = throttle((e: MouseEvent) => {
        var obj = eventConvertor.toMouseEvent(e);
        DotNet.invokeMethodAsync("BQuery", "WindowMouseMove", obj);
    }, defaultThrottleTicks);

    window.onresize = throttle(() => {
        var vwhArr = Viewport.getWidthAndHeight();
        DotNet.invokeMethodAsync("BQuery", "WindowResize", vwhArr[0], vwhArr[1]);
    }, defaultThrottleTicks);

    window.onscroll = throttle((e: Event) => {
        DotNet.invokeMethodAsync("BQuery", "WindowScroll", e);
    }, defaultThrottleTicks);

    window.onclose = (e: Event) => {
        DotNet.invokeMethodAsync("BQuery", "WindowClose", e);
    };

    window.onfocus = (e: FocusEvent) => {
        var evt = eventConvertor.toFocusEventArgs(e);
        DotNet.invokeMethodAsync("BQuery", "WindowFocus", evt);
    };
    window.onblur = (e: FocusEvent) => {
        var evt = eventConvertor.toFocusEventArgs(e);
        DotNet.invokeMethodAsync("BQuery", "WindowBlur", evt);
    };

    window.ontouchstart = throttle((e: TouchEvent) => {
        var evt = eventConvertor.toTouchEventArgs(e);
        console.log("ontouchstart", evt);
        DotNet.invokeMethodAsync("BQuery", "WindowTouchStart", evt);
    }, defaultThrottleTicks);
    window.ontouchmove = throttle((e: TouchEvent) => {
        var evt = eventConvertor.toTouchEventArgs(e);
        console.log("ontouchmove", evt);
        DotNet.invokeMethodAsync("BQuery", "WindowTouchMove", evt);
    }, defaultThrottleTicks);
    window.ontouchend = throttle((e: TouchEvent) => {
        var evt = eventConvertor.toTouchEventArgs(e);
        DotNet.invokeMethodAsync("BQuery", "WindowTouchEnd", evt);
    }, defaultThrottleTicks);
    window.ontouchcancel = throttle((e: TouchEvent) => {
        var evt = eventConvertor.toTouchEventArgs(e);
        DotNet.invokeMethodAsync("BQuery", "WindowTouchCancel", evt);
    }, defaultThrottleTicks);


    window.onkeydown = e => {
        var evt = eventConvertor.toKeyboardEventArgs(e);
        DotNet.invokeMethodAsync("BQuery", "WindowKeyDown", evt);
    };
    window.onkeypress = e => {
        var evt = eventConvertor.toKeyboardEventArgs(e);
        DotNet.invokeMethodAsync("BQuery", "WindowKeyPress", evt);
    };
    window.onkeyup = e => {
        var evt = eventConvertor.toKeyboardEventArgs(e);
        DotNet.invokeMethodAsync("BQuery", "WindowKeyUp", evt);
    };

    //#endregion
};


export { bindEvent };