import { getClassStaticFunc } from "./common";

class ElementWidthAndHeightHelper {
    //#region width height

    public static getWidth(element: HTMLElement, outer: boolean) {
        if (outer) {
            return element.offsetWidth;
        } else {
            return element.clientWidth;
        }
    };

    public static getHeight(element: HTMLElement, outer: boolean) {
        if (outer) {
            return element.offsetHeight;
        } else {
            return element.clientHeight;
        }
    };

    public static getWidthAndHeight = (element: HTMLElement, outer: boolean) => {
        return [
            this.getWidth(element, outer),
            this.getHeight(element, outer)
        ];
    };

    //#endregion
}

class ElementScrollHelper {

    //#region element's Scroll Width and Height

    public static getScrollWidth(element: HTMLElement) {
        return element.scrollWidth;
    }

    public static getScrollHeight(element: HTMLElement) {
        return element.scrollHeight;
    }

    public static getScrollWidthAndHeight(element: HTMLElement) {
        return [
            this.getScrollWidth(element),
            this.getScrollHeight(element)
        ];
    }

    //#endregion 

    //#region Scroll Left and top

    public static getScrollLeft(element: HTMLElement) {
        return element.scrollLeft;
    }

    public static getScrollTop(element: HTMLElement) {
        return element.scrollTop;
    }

    public static getScrollLeftAndTop(element: HTMLElement) {
        return [
            this.getScrollLeft(element),
            this.getScrollTop(element)
        ];
    }

    //#endregion

}

class ElementPositionHelper {

    //#region element's position

    public static getPositionInViewport(element: HTMLElement) {
        var rect = element.getBoundingClientRect();
        return {
            x: rect.left,
            y: rect.top,
            width: rect.width,
            height: rect.height
        }
    };

    public static getElementLeftInDoc(element: HTMLElement) {
        var actualLeft = element.offsetLeft;
        var current = element.offsetParent as HTMLElement;

        while (typeof current !== "undefined" && current !== null) {
            actualLeft += current.offsetLeft;
            current = current.offsetParent as HTMLElement;
        }

        return actualLeft;
    }


    public static getElementTopInDoc(element: HTMLElement) {
        var actualTop = element.offsetTop;
        var current = element.offsetParent as HTMLElement;
        while (typeof current !== "undefined" && current !== null) {
            actualTop += current.offsetTop;
            current = current.offsetParent as HTMLElement;
        }

        return actualTop;
    }

    public static getPositionInDoc(element: HTMLElement) {
        var rect = {
            x: this.getElementLeftInDoc(element),
            y: this.getElementTopInDoc(element),
            width: ElementWidthAndHeightHelper.getWidth(element, true),
            height: ElementWidthAndHeightHelper.getHeight(element, true)
        };
        return rect;
    };

    //#endregion position

}

class ElementHelper {

    // [obsolete]: Blazor has a native implementation from .net6
    public static focus(element: HTMLElement) {
        element.focus();
    }
}


const htmlElementHelper = {
    ...getClassStaticFunc(ElementWidthAndHeightHelper),
    ...getClassStaticFunc(ElementScrollHelper),
    ...getClassStaticFunc(ElementPositionHelper),
    ...getClassStaticFunc(ElementHelper),
}

export default htmlElementHelper;
