using System;
using System.Collections.Generic;
using System.Text;

namespace BQuery
{
    public class JsInteropConstants
    {
        public const string Prefix = "bQuery.";
        public static readonly string ViewportPrefix = $"{Prefix}viewport.";


        public static string GetUserAgent => $"{Prefix}getUserAgent";


        public static string GetWidth => $"{Prefix}getWidth";
        public static string GetHeight => $"{Prefix}getHeight";
        public static string GetWidthAndHeight => $"{Prefix}getWidthAndHeight";


        public static string GetScrollWidth => $"{Prefix}getScrollWidth";
        public static string GetScrollHeight => $"{Prefix}getScrollHeight";
        public static string GetScrollWidthAndHeight => $"{Prefix}getScrollWidthAndHeight";

        public static string GetScrollLeft => $"{Prefix}getScrollLeft";
        public static string GetScrollTop => $"{Prefix}getScrollTop";
        public static string GetScrollLeftAndTop => $"{Prefix}getScrollLeftAndTop";


        public static string GetPositionInViewport => $"{Prefix}getPositionInViewport";
        public static string GetPositionInDoc => $"{Prefix}getPositionInDoc";


        public static string Focus => $"{Prefix}focus";
        public static string BindDrag => $"{Prefix}bindDrag";

        #region BqViewport

        public static string VpGetScrollLeft => $"{ViewportPrefix}getScrollLeft";
        public static string VpGetScrollTop => $"{ViewportPrefix}getScrollTop";
        public static string VpGetScrollLeftAndTop => $"{ViewportPrefix}getScrollLeftAndTop";


        public static string VpGetWidth => $"{ViewportPrefix}getWidth";
        public static string VpGetHeight => $"{ViewportPrefix}getHeight";
        public static string VpGetWidthAndHeight => $"{ViewportPrefix}getWidthAndHeight";

        public static string VpGetScrollWidth => $"{ViewportPrefix}getScrollWidth";
        public static string VpGetScrollHeight => $"{ViewportPrefix}getScrollHeight";
        public static string VpGetScrollWidthAndHeight => $"{ViewportPrefix}getScrollWidthAndHeight";

        #endregion



    }
}
