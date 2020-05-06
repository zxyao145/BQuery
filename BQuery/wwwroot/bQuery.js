(() => {
    /**
     * 防抖，适合多次事件一次响应的情况
     * 应用场合：提交按钮的点击事件。
     * @param fn
     * @param wait
     */
    function debounce(fn, wait = 1000) {
        var timer = null;
        return function (...args) {
            var context = this;
            if (timer) {
                clearTimeout(timer);
            }
            timer = window.setTimeout(() => {
                //var arr = Array.prototype.slice.call(args);
                fn.apply(context, args);
            }, wait);
        };
    }
    
    /**
     * 节流，强制函数以固定的速率执行
     * 会用在比input, keyup更频繁触发的事件中，如resize, touchmove, mousemove, scroll。
     * throttle 会强制函数以固定的速率执行。因此这个方法比较适合应用于动画相关的场景。
     * @param fn
     * @param threshold
     */
    function throttle(fn, threshold = 160) {
        let timeout;
        var start = +new Date;
        return function (...args) {
            let context = this, curTime = +new Date() - 0;
            //总是干掉事件回调
            window.clearTimeout(timeout);
            if (curTime - start >= threshold) {
                //只执行一部分方法，这些方法是在某个时间段内执行一次
                fn.apply(context, args);
                start = curTime;
            }
            else {
                //让方法在脱离事件后也能执行一次
                timeout = window.setTimeout(() => {
                    //@ts-ignore
                    fn.apply(this, args);
                }, threshold);
            }
        };
    }

    var eventConvertor = {
        toMouseEvent: function (e) {
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
        toDragEventArgs: function (e) {
            var obj = eventConvertor.toMouseEvent(e);
            obj.dataTransfer = {
                dropEffect: e.dropEffect,
                effectAllowed: e.effectAllowed,
                files: e.files,
                items: e.items,
                types: e.types
            };

            return obj;
        },
        toFocusEventArgs: function(e) {
            return {
               type:e.type
            }
        },
        toTouchEventArgs: function (e) {
            return {
                detail: e.detail,
                touches: eventConvertor.toTouchPoints(e.touches),
                targetTouches: eventConvertor.toTouchPoints(e.targetTouches),
                changedTouches: eventConvertor.toTouchPoints(e.changedTouches),
                ctrlKey: e.ctrlKey,
                shiftKey: e.shiftKey,
                altKey: e.altKey,
                metaKey: e.metaKey ,
                type: e.type
            }
        },
        toKeyboardEventArgs: function(e) {
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
        toTouchPoints: function (pts) {
            var touches = [];
            for (var i = 0; i < pts.length; i++) {
                touches.push(eventConvertor.toTouchPoint(pts[i]));
            }
            return touches;
        },
        toTouchPoint: function(pt) {
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
    
    function getBq() {
        return window.bQuery;
    }

    function BQuery() {

    }

    //#region viewport
    const viewport = {
        getWidth: () => {
            return document.documentElement.clientWidth;
        },
        getHeight: () => {
            return document.documentElement.clientHeight;
        },
        getWidthAndHeight: () => {
            var r= [
                getBq().viewport.getWidth(),
                getBq().viewport.getHeight()
            ];
            return r;
        },

        getScrollWidth: function () {
            return document.documentElement.scrollWidth;
        },
        getScrollHeight: function () {
            return document.documentElement.scrollHeight;
        },
        getScrollWidthAndHeight : function () {
            return [
                getBq().viewport.getScrollWidth(),
                getBq().viewport.getScrollHeight()
            ];
        },
        getScrollLeft: function () {
            return document.documentElement.scrollLeft;
        },
        getScrollTop: function() {
            return document.documentElement.scrollTop;
        },
        getScrollLeftAndTop: function () {
            return [
                getBq().viewport.getScrollLeft(),
                getBq().viewport.getScrollTop()
            ];
        }

    }

    BQuery.prototype.viewport = viewport;
    //#endregion

    //#region width height

    BQuery.prototype.getWidth = (element, outer) => {
        if (outer) {
            return element.offsetWidth;
        } else {
            return element.clientWidth;
        }
    };
    BQuery.prototype.getHeight = (element, outer) => {
        if (outer) {
            return element.offsetHeight;
        } else {
            return element.clientHeight;
        }
    };
    BQuery.prototype.getWidthAndHeight = (element, outer) => {
        var r = [
            getBq().getWidth(element, outer),
            getBq().getHeight(element, outer)
        ];
        return r;
    };

    //#endregion

    BQuery.prototype.getScrollWidth = function (element) {
        return element.scrollWidth;
    }

    BQuery.prototype.getScrollHeight = function (element) {
        return element.scrollHeight;
    }

    BQuery.prototype.getScrollWidthAndHeight = function (element) {
        return [
            getBq().getScrollWidth(element),
            getBq().getScrollHeight(element)
        ];
    }

    BQuery.prototype.getUserAgent = function() {
        return navigator.userAgent;
    }

    //#region position

    BQuery.prototype.getPositionInViewport = (element) => {
        var rect = element.getBoundingClientRect();
        return {
            x: rect.left,
            y: rect.top,
            width: rect.width,
            height: rect.height
        }
    };

    BQuery.prototype.getElementLeftInDoc = function (element) {
        var actualLeft = element.offsetLeft;
        var current = element.offsetParent;

        while (current !== null) {
            actualLeft += current.offsetLeft;
            current = current.offsetParent;
        }

        return actualLeft;
    }

    BQuery.prototype.getElementTopInDoc = function (element) {
        var actualTop = element.offsetTop;
        var current = element.offsetParent;
        while (current !== null) {
            actualTop += current.offsetTop;
            current = current.offsetParent;
        }

        return actualTop;
    }

    BQuery.prototype.getPositionInDoc = (element) => {
        var that = getBq();
        var rect = {
            x: that.getElementLeftInDoc(element),
            y: that.getElementTopInDoc(element),
            width: that.getWidth(element, true),
            height: that.getHeight(element, true)
        };
        return rect;
    };

    //#endregion position

    //#region Scroll Left

    BQuery.prototype.getScrollLeft = function(element) {
        return element.scrollLeft;
    };
    BQuery.prototype.getScrollTop = function(element) {
        return element.scrollTop;
    };
    BQuery.prototype.getScrollLeftAndTop = function() {
        return [
            getBq().getScrollLeft(),
            getBq().getScrollTop()
        ];
    };

    //#endregion

    //#region window events
    var defaultThrottleTicks = 160;

    var lastClick;
    var clickTimeOut;
    window.onclick = function(e) {
        var obj = eventConvertor.toMouseEvent(e);
        var now = (new Date()).getTime();
        if (lastClick && ((now - lastClick) < 230)) {
            window.clearTimeout(clickTimeOut);
            DotNet.invokeMethod("BQuery", "WindowDbClick", obj);
            lastClick = null;
        } else {
            clickTimeOut = window.setTimeout(function() {
                    DotNet.invokeMethod("BQuery", "WindowClick", obj);
                },
                230);
            lastClick = now;
        }
    };

    window.oncontextmenu = function(e) {
        var obj = eventConvertor.toMouseEvent(e);
        DotNet.invokeMethod("BQuery", "WindowContextMenu", obj);
    };

    window.onmousedown = function(e) {
        var obj = eventConvertor.toMouseEvent(e);
        DotNet.invokeMethod("BQuery", "WindowMouseDown", obj);
    };

    window.onmouseup = function(e) {
        var obj = eventConvertor.toMouseEvent(e);
        DotNet.invokeMethod("BQuery", "WindowMouseUp", obj);
    };

    window.onmouseover = throttle(function (e) {
        var obj = eventConvertor.toMouseEvent(e);
        DotNet.invokeMethod("BQuery", "WindowMouseOver", obj);
    }, defaultThrottleTicks);
    window.onmouseout = throttle(function (e) {
        var obj = eventConvertor.toMouseEvent(e);
        DotNet.invokeMethod("BQuery", "WindowMouseOut", obj);
    }, defaultThrottleTicks);
    window.onmousemove = throttle(function (e) {
        var obj = eventConvertor.toMouseEvent(e);
        DotNet.invokeMethod("BQuery", "WindowMouseMove", obj);
    }, defaultThrottleTicks);


    window.onresize = throttle(function (e) {
        var vwhArr = getBq().viewport.getWidthAndHeight();
        DotNet.invokeMethod("BQuery", "WindowResize", vwhArr[0], vwhArr[1]);
    }, defaultThrottleTicks);
   
    window.onscroll = throttle(function (e) {
        DotNet.invokeMethod("BQuery", "WindowScroll", e);
    }, defaultThrottleTicks);

    window.onclose = function(e) {
        DotNet.invokeMethod("BQuery", "WindowClose", e);
    };

    window.onfocus = function(e) {
        var evt = eventConvertor.toFocusEventArgs(e);
        DotNet.invokeMethod("BQuery", "WindowFocus", evt);
    };
    window.onblur = function(e) {
        var evt = eventConvertor.toFocusEventArgs(e);
        DotNet.invokeMethod("BQuery", "WindowBlur", evt);
    };


    window.ontouchstart = throttle(function (e) {
        var evt = eventConvertor.toTouchEventArgs(e);
        DotNet.invokeMethod("BQuery", "WindowTouchStart", evt);
    }, defaultThrottleTicks); 
    window.ontouchmove = throttle(function (e) {
        var evt = eventConvertor.toTouchEventArgs(e);
        DotNet.invokeMethod("BQuery", "WindowTouchMove", evt);
    }, defaultThrottleTicks); 
    window.ontouchend = throttle(function (e) {
        var evt = eventConvertor.toTouchEventArgs(e);
        DotNet.invokeMethod("BQuery", "WindowTouchEnd", evt);
    }, defaultThrottleTicks); 
    window.ontouchcancel = throttle(function (e) {
        var evt = eventConvertor.toTouchEventArgs(e);
        DotNet.invokeMethod("BQuery", "WindowTouchCancel", evt);
    }, defaultThrottleTicks);


    window.onkeydown = function(e) {
        var evt = eventConvertor.toKeyboardEventArgs(e);
        DotNet.invokeMethod("BQuery", "WindowKeyDown", evt);
    };
    window.onkeypress = function(e) {
        var evt = eventConvertor.toKeyboardEventArgs(e);
        DotNet.invokeMethod("BQuery", "WindowKeyPress", evt);
    };
    window.onkeyup = function(e) {
        var evt = eventConvertor.toKeyboardEventArgs(e);
        DotNet.invokeMethod("BQuery", "WindowKeyUp", evt);
    };

    //#endregion



    window.bQuery = new BQuery();
    window.bQuery.throttle = throttle;
    window.bQuery.debounce = debounce;
})();

