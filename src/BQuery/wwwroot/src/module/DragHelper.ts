import { autoDebug, throttle } from "./common";

import { getDom } from "./domHelper";

const draggersByTrigger = new Map<HTMLElement, Dragger>();
const dragThrottleMs = 10;

interface DragConfig {
    inViewport: boolean;
}

interface DragState {
    isDragging: boolean;
    pointerId: number | null;
    startPointerX: number;
    startPointerY: number;
    startTranslateX: number;
    startTranslateY: number;
    translateX: number;
    translateY: number;
    baseLeft: number;
    baseTop: number;
    minLeft: number;
    minTop: number;
    maxLeft: number;
    maxTop: number;
}

interface IDragOptions {
    inViewport?: boolean;
    dragElement?: HTMLElement | string | null;
}

const defaultOptions: DragConfig = {
    inViewport: true,
};

class Dragger {
    private _trigger: HTMLElement;
    private _container: HTMLElement;
    private _options: DragConfig;
    private _state: DragState;
    private _isInitialized = false;
    private _isBound = false;
    private _style: string | null = null;
    private _triggerTouchAction = "";

    constructor(trigger: HTMLElement, container: HTMLElement, dragInViewport: boolean) {
        this._trigger = trigger;
        this._container = container;
        this._options = {
            ...defaultOptions,
            inViewport: dragInViewport,
        };
        this._state = {
            isDragging: false,
            pointerId: null,
            startPointerX: 0,
            startPointerY: 0,
            startTranslateX: 0,
            startTranslateY: 0,
            translateX: 0,
            translateY: 0,
            baseLeft: 0,
            baseTop: 0,
            minLeft: 0,
            minTop: 0,
            maxLeft: 0,
            maxTop: 0,
        };
    }

    update(trigger: HTMLElement, container: HTMLElement, dragInViewport: boolean) {
        if (this._isBound && this._trigger !== trigger) {
            this.unbindDrag();
        }

        this._trigger = trigger;
        this._container = container;
        this._options = {
            ...defaultOptions,
            inViewport: dragInViewport,
        };
        this.updateBounds();
    }

    private initializePosition() {
        if (this._isInitialized) {
            return;
        }

        if (this._style === null) {
            this._style = this._container.getAttribute("style");
        }

        const rect = this._container.getBoundingClientRect();
        this._state.baseLeft = rect.left + window.scrollX;
        this._state.baseTop = rect.top + window.scrollY;
        this._state.translateX = 0;
        this._state.translateY = 0;
        this._state.startTranslateX = 0;
        this._state.startTranslateY = 0;

        this._container.style.position = "absolute";
        this._container.style.left = `${this._state.baseLeft}px`;
        this._container.style.top = `${this._state.baseTop}px`;
        this._container.style.margin = "0";
        this._container.style.paddingBottom = "0";
        this._container.style.transform = "translate3d(0px, 0px, 0px)";
        this._container.style.willChange = "transform";

        this._isInitialized = true;
        this.updateBounds();
    }

    private updateBounds() {
        const viewportLeft = window.scrollX;
        const viewportTop = window.scrollY;
        const viewportRight = viewportLeft + document.documentElement.clientWidth - this._container.offsetWidth - 1;
        const viewportBottom = viewportTop + document.documentElement.clientHeight - this._container.offsetHeight - 1;

        this._state.minLeft = viewportLeft;
        this._state.minTop = viewportTop;
        this._state.maxLeft = Math.max(viewportLeft, viewportRight);
        this._state.maxTop = Math.max(viewportTop, viewportBottom);
    }

    private applyTransform() {
        this._container.style.transform = `translate3d(${this._state.translateX}px, ${this._state.translateY}px, 0)`;
    }

    private clampTranslate(nextTranslateX: number, nextTranslateY: number) {
        if (!this._options.inViewport) {
            return {
                x: nextTranslateX,
                y: nextTranslateY,
            };
        }

        const left = this._state.baseLeft + nextTranslateX;
        const top = this._state.baseTop + nextTranslateY;
        const clampedLeft = Math.min(Math.max(left, this._state.minLeft), this._state.maxLeft);
        const clampedTop = Math.min(Math.max(top, this._state.minTop), this._state.maxTop);

        return {
            x: clampedLeft - this._state.baseLeft,
            y: clampedTop - this._state.baseTop,
        };
    }

    onPointerDown = (e: PointerEvent) => {
        if (e.pointerType === "mouse" && e.button !== 0) {
            return;
        }

        e.stopPropagation();
        this.initializePosition();

        autoDebug(() => {
            console.log("onPointerDown");
        });

        this.updateBounds();
        this._state.isDragging = true;
        this._state.pointerId = e.pointerId;
        this._state.startPointerX = e.clientX;
        this._state.startPointerY = e.clientY;
        this._state.startTranslateX = this._state.translateX;
        this._state.startTranslateY = this._state.translateY;

        this._trigger.setPointerCapture?.(e.pointerId);
    };

    onPointerUp = (e: PointerEvent) => {
        if (this._state.pointerId !== null && e.pointerId !== this._state.pointerId) {
            return;
        }

        this._state.isDragging = false;
        this._state.pointerId = null;
        this._trigger.releasePointerCapture?.(e.pointerId);
    };

    onPointerMove = throttle((e: PointerEvent) => {
        if (!this._state.isDragging || e.pointerId !== this._state.pointerId) {
            return;
        }

        const deltaX = e.clientX - this._state.startPointerX;
        const deltaY = e.clientY - this._state.startPointerY;
        const nextTranslateX = this._state.startTranslateX + deltaX;
        const nextTranslateY = this._state.startTranslateY + deltaY;
        const clamped = this.clampTranslate(nextTranslateX, nextTranslateY);

        this._state.translateX = clamped.x;
        this._state.translateY = clamped.y;
        this.applyTransform();
    }, dragThrottleMs).bind(this);

    onResize = throttle(() => {
        if (!this._isInitialized) {
            return;
        }

        this.updateBounds();
        const clamped = this.clampTranslate(this._state.translateX, this._state.translateY);
        this._state.translateX = clamped.x;
        this._state.translateY = clamped.y;
        this.applyTransform();
    }, dragThrottleMs).bind(this);

    bindDrag() {
        if (this._isBound) {
            return;
        }

        this._triggerTouchAction = this._trigger.style.touchAction;
        this._trigger.style.touchAction = "none";
        this._trigger.addEventListener("pointerdown", this.onPointerDown, false);
        window.addEventListener("pointerup", this.onPointerUp, false);
        window.addEventListener("pointercancel", this.onPointerUp, false);
        window.addEventListener("pointermove", this.onPointerMove, false);
        window.addEventListener("resize", this.onResize, false);

        this._isBound = true;
    }

    unbindDrag() {
        if (!this._isBound) {
            return;
        }

        this._trigger.removeEventListener("pointerdown", this.onPointerDown, false);
        window.removeEventListener("pointerup", this.onPointerUp, false);
        window.removeEventListener("pointercancel", this.onPointerUp, false);
        window.removeEventListener("pointermove", this.onPointerMove, false);
        window.removeEventListener("resize", this.onResize, false);
        this._trigger.style.touchAction = this._triggerTouchAction;

        this.onPointerMove.cancel?.();
        this.onResize.cancel?.();
        this._state.isDragging = false;
        this._state.pointerId = null;
        this._isBound = false;
    }

    resetContainerStyle() {
        if (this._style !== null) {
            this._container.setAttribute("style", this._style);
        } else {
            this._container.removeAttribute("style");
        }

        this._state.translateX = 0;
        this._state.translateY = 0;
        this._state.startTranslateX = 0;
        this._state.startTranslateY = 0;
        this._state.isDragging = false;
        this._state.pointerId = null;
        this._isInitialized = false;
    }
}

function addDraggable(trigger: HTMLElement | string | null, container: HTMLElement | string, dragInViewport = true) {
    if (!trigger) {
        return;
    }

    const triggerEle = getDom(trigger) as HTMLElement;
    const containerEle = getDom(container) as HTMLElement;
    let dragger = draggersByTrigger.get(triggerEle);

    if (!dragger) {
        dragger = new Dragger(triggerEle, containerEle, dragInViewport);
        draggersByTrigger.set(triggerEle, dragger);
    } else {
        dragger.update(triggerEle, containerEle, dragInViewport);
    }

    dragger.bindDrag();
}

function removeDraggable(trigger: HTMLElement | string) {
    const triggerEle = getDom(trigger) as HTMLElement;
    const dragger = draggersByTrigger.get(triggerEle);
    if (!dragger) {
        return;
    }

    dragger.unbindDrag();
    draggersByTrigger.delete(triggerEle);
}

function resetDraggableElePosition(trigger: HTMLElement | string) {
    const triggerEle = getDom(trigger) as HTMLElement;
    const dragger = draggersByTrigger.get(triggerEle);
    if (!dragger) {
        return;
    }

    dragger.resetContainerStyle();
}

const bindDrag = (dom: HTMLElement, options?: IDragOptions | null) => {
    const resolvedOptions = {
        ...defaultOptions,
        ...options,
    };
    addDraggable(resolvedOptions.dragElement ?? dom, dom, resolvedOptions.inViewport);
};

export {
    bindDrag,
    addDraggable,
    removeDraggable,
    resetDraggableElePosition,
};
