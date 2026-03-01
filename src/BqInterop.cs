using System;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;

namespace BQuery
{
    public static class BqInterop
    {
        /// <summary>
        /// js callback
        /// </summary>
        /// <param name="e"></param>
        [JSInvokable]
        public static void WindowDbClick(MouseEventArgs e)
        {
            Bq.Events.InvokeOnDbClick(e);
        }

        /// <summary>
        /// js callback
        /// </summary>
        /// <param name="e"></param>
        [JSInvokable]
        public static void WindowClick(MouseEventArgs e)
        {
            Bq.Events.InvokeOnClick(e);
        }

        /// <summary>
        /// js callback
        /// </summary>
        /// <param name="e"></param>
        [JSInvokable]
        public static void WindowContextMenu(MouseEventArgs e)
        {
            Bq.Events.InvokeOnContextMenu(e);
        }

        /// <summary>
        /// js callback
        /// </summary>
        /// <param name="e"></param>
        [JSInvokable]
        public static void WindowMouseDown(MouseEventArgs e)
        {
            Bq.Events.InvokeOnMouseDown(e);
        }

        /// <summary>
        /// js callback
        /// </summary>
        /// <param name="e"></param>
        [JSInvokable]
        public static void WindowMouseUp(MouseEventArgs e)
        {
            Bq.Events.InvokeOnMouseUp(e);
        }

        /// <summary>
        /// js callback
        /// </summary>
        /// <param name="e"></param>
        [JSInvokable]
        public static void WindowMouseOver(MouseEventArgs e)
        {
            Bq.Events.InvokeOnMouseOver(e);
        }

        /// <summary>
        /// js callback
        /// </summary>
        /// <param name="e"></param>
        [JSInvokable]
        public static void WindowMouseOut(MouseEventArgs e)
        {
            Bq.Events.InvokeOnMouseOut(e);
        }

        /// <summary>
        /// js callback on window.onscroll trigger
        /// </summary>
        /// <param name="e"></param>
        [JSInvokable]
        public static void WindowMouseMove(MouseEventArgs e)
        {
            Bq.Events.InvokeOnMouseMove(e);
        }


        /// <summary>
        /// js callback on window.onresize trigger
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        [JSInvokable]
        public static void WindowResize(double width, double height)
        {
            Bq.Events.InvokeOnResize(width, height);
        }

        /// <summary>
        /// js callback on window.onscroll trigger
        /// </summary>
        /// <param name="e"></param>
        [JSInvokable]
        public static void WindowScroll(EventArgs e)
        {
            Bq.Events.InvokeOnScroll(e);
        }

        /// <summary>
        /// js callback
        /// </summary>
        /// <param name="e"></param>
        [JSInvokable]
        public static void WindowClose(EventArgs e)
        {
            Bq.Events.InvokeOnClose(e);
        }

        /// <summary>
        /// js callback
        /// </summary>
        /// <param name="e"></param>
        [JSInvokable]
        public static void WindowFocus(FocusEventArgs e)
        {
            Bq.Events.InvokeOnFocus(e);
        }

        /// <summary>
        /// js callback
        /// </summary>
        /// <param name="e"></param>
        [JSInvokable]
        public static void WindowBlur(FocusEventArgs e)
        {
            Bq.Events.InvokeOnBlur(e);
        }

        [JSInvokable]
        public static void WindowTouchStart(TouchEventArgs e)
        {
            Bq.Events.InvokeOnTouchStart(e);

        }

        [JSInvokable]
        public static void WindowTouchMove(TouchEventArgs e)
        {
            Bq.Events.InvokeOnTouchMove(e);
        }

        [JSInvokable]
        public static void WindowTouchEnd(TouchEventArgs e)
        {
            Bq.Events.InvokeOnTouchEnd(e);
        }

        [JSInvokable]
        public static void WindowTouchCancel(TouchEventArgs e)
        {
            Bq.Events.InvokeOnTouchCancel(e);
        }


        [JSInvokable]
        public static void WindowKeyDown(KeyboardEventArgs e)
        {
            Bq.Events.InvokeOnKeyDown(e);
        }

        [JSInvokable]
        public static void WindowKeyPress(KeyboardEventArgs e)
        {
            Bq.Events.InvokeOnKeyPress(e);
        }

        [JSInvokable]
        public static void WindowKeyUp(KeyboardEventArgs e)
        {
            Bq.Events.InvokeOnKeyUp(e);
        }

        #region Drag & drop

        //[JSInvokable]
        //public static void WindowDragEnter(DragEventArgs e)
        //{
        //    try
        //    {
        //        Bq.Events.InvokeOnDragEnter(e);
        //    }
        //    catch (Exception exp)
        //    {
        //        Console.WriteLine(exp);
        //    }
        //}

        //[JSInvokable]
        //public static void WindowDragLeave(DragEventArgs e)
        //{
        //    try
        //    {
        //        Bq.Events.InvokeOnDragLeave(e);
        //    }
        //    catch (Exception exp)
        //    {
        //        Console.WriteLine(exp);
        //    }
        //}

        //[JSInvokable]
        //public static void WindowDragOver(DragEventArgs e)
        //{
        //    try
        //    {
        //        Bq.Events.InvokeOnDragOver(e);
        //    }
        //    catch (Exception exp)
        //    {
        //        Console.WriteLine(exp);
        //    }
        //}

        //[JSInvokable]
        //public static void WindowDrop(DragEventArgs e)
        //{
        //    try
        //    {
        //        Bq.Events.InvokeOnDrop(e);
        //    }
        //    catch (Exception exp)
        //    {
        //        Console.WriteLine(exp);
        //    }
        //}

        #endregion
    }
}
