export default class Viewport {
    public static getWidth() {
        return document.documentElement.clientWidth;
    }

    public static getHeight() {
        return document.documentElement.clientHeight;
    }

    public static getWidthAndHeight() {
        return [
            Viewport.getWidth(),
            Viewport.getHeight()
        ];
    }

    public static getScrollWidth() {
        return document.documentElement.scrollWidth;
    }

    public static getScrollHeight() {
        return document.documentElement.scrollHeight;
    }

    public static getScrollWidthAndHeight() {
        return [
            Viewport.getScrollWidth(),
            Viewport.getScrollHeight()
        ];
    }

    public static getScrollLeft() {
        return document.documentElement.scrollLeft;
    }

    public static getScrollTop() {
        return document.documentElement.scrollTop;
    }

    public static getScrollLeftAndTop() {
        return [
            Viewport.getScrollLeft(),
            Viewport.getScrollTop()
        ];
    }

    static getScrollDistToTop() {
        const scrollTop =
            document.documentElement.scrollTop || document.body.scrollTop || 0;
        return Math.round(scrollTop);
    }

    static getScrollDistToBottom() {
        const dist =
            document.documentElement.scrollHeight -
            document.documentElement.scrollTop -
            document.documentElement.clientHeight;
        return Math.round(dist);
    }
}