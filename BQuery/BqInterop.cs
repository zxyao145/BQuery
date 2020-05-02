using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.JSInterop;

namespace BQuery
{
    public static class BqInterop
    {
        /// <summary>
        /// js callback on window.onresize trigger
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        [JSInvokable]
        public static void WindowResize(int width, int height)
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
    }
}
