namespace BQuery;

public class JsModuleConstants
{
    public const string MJS = "./_content/BQuery/dist/bQuery.min.mjs";
    public static string GetMethod(params string[] keys)
    {
        return string.Join(".", keys);
    }

    public static readonly string ModuleName = "bQuery";

    public class Viewport
    {
        public static readonly string ModuleName = "viewport";

        public static string GetScrollLeft => "getScrollLeft";
        public static string GetScrollTop => "getScrollTop";
        public static string GetScrollLeftAndTop => "getScrollLeftAndTop";


        public static string GetWidth => "getWidth";
        public static string GetHeight => "getHeight";
        public static string GetWidthAndHeight => "getWidthAndHeight";

        public static string GetScrollWidth => "getScrollWidth";
        public static string GetScrollHeight => "getScrollHeight";
        public static string GetScrollWidthAndHeight => "getScrollWidthAndHeight";
    }

    public class Dom
    {
        public static readonly string ModuleName = "domHelper";

        public static string Attr => "attr";
        public static string AddCls => "addCls";
        public static string RemoveCls => "removeCls";
        public static string Css => "css";
    }

    public class ElementExtensions
    {
        public static readonly string ModuleName = "elementExtensions";

        public static string GetWidth => "getWidth";
        public static string GetHeight => "getHeight";
        public static string GetWidthAndHeight => "getWidthAndHeight";


        public static string GetScrollWidth => "getScrollWidth";
        public static string GetScrollHeight => "getScrollHeight";
        public static string GetScrollWidthAndHeight => "getScrollWidthAndHeight";

        public static string GetScrollLeft => "getScrollLeft";
        public static string GetScrollTop => "getScrollTop";
        public static string GetScrollLeftAndTop => "getScrollLeftAndTop";


        public static string GetPositionInViewport => "getPositionInViewport";
        public static string GetPositionInDoc => "getPositionInDoc";

        public static string Focus => "focus";
    }


    public class Drag
    {
        public static readonly string ModuleName = "drag";
        public static string BindDrag => "bindDrag";
    }

    public static string BQueryReady => "bqInit";
    public static string GetUserAgent => "getUserAgent";

    public static string AddWindowEventsListener => "addWindowEventsListener";
}
