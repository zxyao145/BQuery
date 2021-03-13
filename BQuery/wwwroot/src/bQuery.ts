import "./global";

(() => {
	var version = "2.0.1";
    var DotNet = window.DotNet;

    //#region interfaces

    interface IEventConvertor {
        toMouseEvent: any,
        toDragEventArgs: any,
        toFocusEventArgs: any,
        toTouchEventArgs: any,
        toKeyboardEventArgs: any,
        toTouchPoints: any,
        toTouchPoint: any,
    }

    interface IDragOptions {
        inViewport: boolean;
        dragElement: HTMLElement | string | null;
    }

    //#endregion


    // used for:
    // drag document.mousemove、window.resize
    var dragThrottleTicks = 30;

    // used for:
    // window.onmouseover、window.onmouseout、window.onmousemove
    // window.onresize、window.onscroll
    // window.ontouchstart、window.ontouchmove、 window.ontouchend、window.ontouchcancel
    var defaultThrottleTicks = 80;

    //#region common

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

    function getDom(element: string | Element | null) {
        if (!element) {
            element = document.body;
        } else if (typeof element === 'string') {
            element = document.querySelector(element);
        }
        return element;
    }

    //#endregion

    //#region base

    var eventConvertor: IEventConvertor = {
        toMouseEvent(e) {
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
        toDragEventArgs(e) {
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
        toFocusEventArgs(e) {
            return {
                type: e.type
            }
        },
        toTouchEventArgs(e) {
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
        toKeyboardEventArgs(e) {
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
        toTouchPoints(pts) {
            var touches = [];
            for (var i = 0; i < pts.length; i++) {
                touches.push(eventConvertor.toTouchPoint(pts[i]));
            }
            return touches;
        },
        toTouchPoint(pt) {
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

    function getBq(): BQuery {
        return window.bQuery;
    }

    //#endregion 

    //#region viewport
    const viewport = {
        getWidth: () => {
            return document.documentElement.clientWidth;
        },
        getHeight: () => {
            return document.documentElement.clientHeight;
        },
        getWidthAndHeight: () => {
            var r = [
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
        getScrollWidthAndHeight: function () {
            return [
                getBq().viewport.getScrollWidth(),
                getBq().viewport.getScrollHeight()
            ];
        },
        getScrollLeft: function () {
            return document.documentElement.scrollLeft;
        },
        getScrollTop: function () {
            return document.documentElement.scrollTop;
        },
        getScrollLeftAndTop: function () {
            return [
                getBq().viewport.getScrollLeft(),
                getBq().viewport.getScrollTop()
            ];
        }

    }

    class BQuery {
        public viewport = viewport;
        public version = version;

        getUserAgent() {
            return navigator.userAgent;
        }

        // [obsolete]: Blazor has a native implementation
        focus(element) {
            element.focus();
        }

        //#region width height

        getWidth(element, outer) {
            if (outer) {
                return element.offsetWidth;
            } else {
                return element.clientWidth;
            }
        };

        getHeight(element, outer) {
            if (outer) {
                return element.offsetHeight;
            } else {
                return element.clientHeight;
            }
        };

        getWidthAndHeight = (element, outer) => {
            var r = [
                getBq().getWidth(element, outer),
                getBq().getHeight(element, outer)
            ];
            return r;
        };

        //#endregion


        //#region element's Scroll

        getScrollWidth(element) {
            return element.scrollWidth;
        }

        getScrollHeight(element) {
            return element.scrollHeight;
        }

        getScrollWidthAndHeight(element) {
            return [
                getBq().getScrollWidth(element),
                getBq().getScrollHeight(element)
            ];
        }

        //#endregion 


        //#region Scroll Left

        getScrollLeft(element) {
            return element.scrollLeft;
        }

        getScrollTop(element) {
            return element.scrollTop;
        }

        getScrollLeftAndTop(element) {
            return [
                getBq().getScrollLeft(element),
                getBq().getScrollTop(element)
            ];
        }

        //#endregion

        //#region element's position

        getPositionInViewport(element) {
            var rect = element.getBoundingClientRect();
            return {
                x: rect.left,
                y: rect.top,
                width: rect.width,
                height: rect.height
            }
        };

        getElementLeftInDoc(element) {
            var actualLeft = element.offsetLeft;
            var current = element.offsetParent;

            while (typeof current !== "undefined" && current !== null) {
                actualLeft += current.offsetLeft;
                current = current.offsetParent;
            }

            return actualLeft;
        }

        getElementTopInDoc(element) {
            var actualTop = element.offsetTop;
            var current = element.offsetParent;
            while (typeof current !== "undefined" && current !== null) {
                actualTop += current.offsetTop;
                current = current.offsetParent;
            }

            return actualTop;
        }

        getPositionInDoc(element) {
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


        /**
         *
         * @param {any} dom
         * @param {any} options
         *  {
                // draggable element must in viewport?
                bool InViewport { get; set; } = true;
        
                // which draggable element's sub element trigger drag event, default is draggable element
                ElementReference? DragElement { get; set; } = null;
            }
         */
        bindDrag(dom: HTMLElement, options: IDragOptions) {
            function getCss(ele, prop) {
                return parseInt(window.getComputedStyle(ele)[prop]);
            }


            var isInDrag = false;
            var mX = 0;
            var mY = 0;
            var domStartX = 0, domStartY = 0;
            var domMaxY, domMaxX;
            var panelHeader;

            function calcDomMax() {
                domMaxY = document.documentElement.clientHeight
                    - dom.offsetHeight;
                domMaxX = document.documentElement.clientWidth
                    - dom.offsetWidth;
            }

            calcDomMax();

            if (options.dragElement) {
                if (typeof options.dragElement === 'string') {
                    panelHeader = dom.querySelector(options.dragElement);
                } else {
                    panelHeader = options.dragElement;
                }
            } else {
                panelHeader = dom;
            }

            panelHeader.addEventListener("mousedown", e => {
                isInDrag = true;
                mX = e.clientX;
                mY = e.clientY;
                dom.style.position = "absolute";
                domStartX = getCss(dom, "left");
                domStartY = getCss(dom, "top");
            }, false);
            window.addEventListener("mouseup", e => {
                isInDrag = false;
                domStartX = getCss(dom, "left");
                domStartY = getCss(dom, "top");
            }, false);
            document.addEventListener("mousemove", throttle(e => {
                if (isInDrag) {
                    var nowX = e.clientX, nowY = e.clientY, disX = nowX - mX, disY = nowY - mY;
                    var newDomX = domStartX + disX;
                    var newDomY = domStartY + disY;
                    if (options.inViewport) {
                        if (newDomX < 0) {
                            newDomX = 0;
                        }
                        else if (newDomX > domMaxX) {
                            newDomX = domMaxX;
                        }
                        if (newDomY < 0) {
                            newDomY = 0;
                        }
                        else if (newDomY > domMaxY) {
                            newDomY = domMaxY;
                        }
                    }
                    dom.style.left = newDomX + "px";
                    dom.style.top = newDomY + "px";
                }
            }, dragThrottleTicks));
            if (options.inViewport) {
                window.addEventListener("resize", throttle(e => {
                    calcDomMax();

                    domStartY = parseInt(dom.style.top);
                    domStartX = parseInt(dom.style.left);
                    if (domStartY > domMaxY) {
                        if (domMaxY > 0) {
                            dom.style.top = domMaxY + "px";
                        }
                    }
                    if (domStartX > domMaxX) {
                        dom.style.left = domMaxX + "px";
                    }
                }, dragThrottleTicks), false);
            }
        }

    }

    BQuery.prototype.viewport = viewport;
    //#endregion



    //#region window events

    var lastClick;
    var clickTimeOut;
    window.onclick = e => {
        var obj = eventConvertor.toMouseEvent(e);
        var now = (new Date()).getTime();
        if (lastClick && ((now - lastClick) < 230)) {
            window.clearTimeout(clickTimeOut);
            DotNet.invokeMethodAsync("BQuery", "WindowDbClick", obj);
            lastClick = null;
        } else {
            clickTimeOut = window.setTimeout(function () {
                DotNet.invokeMethodAsync("BQuery", "WindowClick", obj);
            },
                230);
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

    window.onmouseover = throttle(e => {
        var obj = eventConvertor.toMouseEvent(e);
        DotNet.invokeMethodAsync("BQuery", "WindowMouseOver", obj);
    }, defaultThrottleTicks);
    window.onmouseout = throttle(e => {
        var obj = eventConvertor.toMouseEvent(e);
        DotNet.invokeMethodAsync("BQuery", "WindowMouseOut", obj);
    }, defaultThrottleTicks);
    window.onmousemove = throttle(e => {
        var obj = eventConvertor.toMouseEvent(e);
        DotNet.invokeMethodAsync("BQuery", "WindowMouseMove", obj);
    }, defaultThrottleTicks);

    window.onresize = throttle(e => {
        var vwhArr = getBq().viewport.getWidthAndHeight();
        DotNet.invokeMethodAsync("BQuery", "WindowResize", vwhArr[0], vwhArr[1]);
    }, defaultThrottleTicks);

    window.onscroll = throttle(e => {
        DotNet.invokeMethodAsync("BQuery", "WindowScroll", e);
    }, defaultThrottleTicks);

    window.onclose = e => {
        DotNet.invokeMethodAsync("BQuery", "WindowClose", e);
    };

    window.onfocus = e => {
        var evt = eventConvertor.toFocusEventArgs(e);
        DotNet.invokeMethodAsync("BQuery", "WindowFocus", evt);
    };
    window.onblur = e => {
        var evt = eventConvertor.toFocusEventArgs(e);
        DotNet.invokeMethodAsync("BQuery", "WindowBlur", evt);
    };

    window.ontouchstart = throttle(e => {
        var evt = eventConvertor.toTouchEventArgs(e);
        DotNet.invokeMethodAsync("BQuery", "WindowTouchStart", evt);
    }, defaultThrottleTicks);
    window.ontouchmove = throttle(e => {
        var evt = eventConvertor.toTouchEventArgs(e);
        DotNet.invokeMethodAsync("BQuery", "WindowTouchMove", evt);
    }, defaultThrottleTicks);
    window.ontouchend = throttle(e => {
        var evt = eventConvertor.toTouchEventArgs(e);
        DotNet.invokeMethodAsync("BQuery", "WindowTouchEnd", evt);
    }, defaultThrottleTicks);
    window.ontouchcancel = throttle(e => {
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

    let hasInited = false;
    var bQueryReady = () => {
        if (!hasInited) {
            hasInited = true;

            console.log("bQuery is Ready");
            window.bQuery = new BQuery();
            window.bQuery.throttle = throttle;
            window.bQuery.debounce = debounce;
        }
    }

    window.bqInit = bQueryReady;
})();

