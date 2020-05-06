using System;
using System.Collections.Generic;
using System.Text;
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
            try
            {
                Bq.Events.InvokeOnDbClick(e);
            }
            catch (Exception exp)
            {
                Console.WriteLine(exp);
            }
        }
        
        /// <summary>
        /// js callback
        /// </summary>
        /// <param name="e"></param>
        [JSInvokable]
        public static void WindowClick(MouseEventArgs e)
        {
            try
            {
                Bq.Events.InvokeOnClick(e);
            }
            catch (Exception exp)
            {
                Console.WriteLine(exp);
            }
        }

        /// <summary>
        /// js callback
        /// </summary>
        /// <param name="e"></param>
        [JSInvokable]
        public static void WindowContextMenu(MouseEventArgs e)
        {
            try
            {
                Bq.Events.InvokeOnContextMenu(e);
            }
            catch (Exception exp)
            {
                Console.WriteLine(exp);
            }
        }

        /// <summary>
        /// js callback
        /// </summary>
        /// <param name="e"></param>
        [JSInvokable]
        public static void WindowMouseDown(MouseEventArgs e)
        {
            try
            {
                Bq.Events.InvokeOnMouseDown(e);
            }
            catch (Exception exp)
            {
                Console.WriteLine(exp);
            }
        }

        /// <summary>
        /// js callback
        /// </summary>
        /// <param name="e"></param>
        [JSInvokable]
        public static void WindowMouseUp(MouseEventArgs e)
        {
            try
            {
                Bq.Events.InvokeOnMouseUp(e);
            }
            catch (Exception exp)
            {
                Console.WriteLine(exp);
            }
        }

        /// <summary>
        /// js callback
        /// </summary>
        /// <param name="e"></param>
        [JSInvokable]
        public static void WindowMouseOver(MouseEventArgs e)
        {
            try
            {
                Bq.Events.InvokeOnMouseOver(e);
            }
            catch (Exception exp)
            {
                Console.WriteLine(exp);
            }
        }
        
        /// <summary>
        /// js callback
        /// </summary>
        /// <param name="e"></param>
        [JSInvokable]
        public static void WindowMouseOut(MouseEventArgs e)
        {
            try
            {
                Bq.Events.InvokeOnMouseOut(e);
            }
            catch (Exception exp)
            {
                Console.WriteLine(exp);
            }
        }

        /// <summary>
        /// js callback on window.onscroll trigger
        /// </summary>
        /// <param name="e"></param>
        [JSInvokable]
        public static void WindowMouseMove(MouseEventArgs e)
        {

            try
            {
                Bq.Events.InvokeOnMouseMove(e);
            }
            catch (Exception exp)
            {
                Console.WriteLine(exp);
            }
        }


        /// <summary>
        /// js callback on window.onresize trigger
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        [JSInvokable]
        public static void WindowResize(double width, double height)
        {
            try
            {
                Bq.Events.InvokeOnResize(width, height);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        /// <summary>
        /// js callback on window.onscroll trigger
        /// </summary>
        /// <param name="e"></param>
        [JSInvokable]
        public static void WindowScroll(EventArgs e)
        {
            try
            {
                Bq.Events.InvokeOnScroll(e);
            }
            catch (Exception exp)
            {
                Console.WriteLine(exp);
            }
        }

        /// <summary>
        /// js callback
        /// </summary>
        /// <param name="e"></param>
        [JSInvokable]
        public static void WindowClose(EventArgs e)
        {
            try
            {
                Bq.Events.InvokeOnClose(e);
            }
            catch (Exception exp)
            {
                Console.WriteLine(exp);
            }
        }

        /// <summary>
        /// js callback
        /// </summary>
        /// <param name="e"></param>
        [JSInvokable]
        public static void WindowFocus(FocusEventArgs e)
        {
            try
            {
                Bq.Events.InvokeOnFocus(e);
            }
            catch (Exception exp)
            {
                Console.WriteLine(exp);
            }
        }

        /// <summary>
        /// js callback
        /// </summary>
        /// <param name="e"></param>
        [JSInvokable]
        public static void WindowBlur(FocusEventArgs e)
        {
            try
            {
                Bq.Events.InvokeOnBlur(e);
            }
            catch (Exception exp)
            {
                Console.WriteLine(exp);
            }
        }

        [JSInvokable]
        public static void WindowTouchStart(TouchEventArgs e)
        {
            try
            {
                Bq.Events.InvokeOnTouchStart(e);
            }
            catch (Exception exp)
            {
                Console.WriteLine(exp);
            }
        }

        [JSInvokable]
        public static void WindowTouchMove(TouchEventArgs e)
        {
            try
            {
                Bq.Events.InvokeOnTouchMove(e);
            }
            catch (Exception exp)
            {
                Console.WriteLine(exp);
            }
        }

        [JSInvokable]
        public static void WindowTouchEnd(TouchEventArgs e)
        {
            try
            {
                Bq.Events.InvokeOnTouchEnd(e);
            }
            catch (Exception exp)
            {
                Console.WriteLine(exp);
            }
        }

        [JSInvokable]
        public static void WindowTouchCancel(TouchEventArgs e)
        {
            try
            {
                Bq.Events.InvokeOnTouchCancel(e);
            }
            catch (Exception exp)
            {
                Console.WriteLine(exp);
            }
        }


        [JSInvokable]
        public static void WindowKeyDown(KeyboardEventArgs e)
        {
            try
            {
                Bq.Events.InvokeOnKeyDown(e);
            }
            catch (Exception exp)
            {
                Console.WriteLine(exp);
            }
        }

        [JSInvokable]
        public static void WindowKeyPress(KeyboardEventArgs e)
        {
            try
            {
                Bq.Events.InvokeOnKeyPress(e);
            }
            catch (Exception exp)
            {
                Console.WriteLine(exp);
            }
        }

        [JSInvokable]
        public static void WindowKeyUp(KeyboardEventArgs e)
        {
            try
            {
                Bq.Events.InvokeOnKeyUp(e);
            }
            catch (Exception exp)
            {
                Console.WriteLine(exp);
            }
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
