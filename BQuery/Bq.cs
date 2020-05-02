using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BQuery
{
    public static class Bq
    {
        internal static void Init(IJSRuntime jsRuntime)
        {
            JsRuntime = jsRuntime;
            if (Viewport == null)
            {
                Viewport = BqViewport.CreateInstance(jsRuntime);
            }
            else
            {
                //Although it's not necessary
                Viewport.JsRuntime = jsRuntime;
            }

            if (Events == null)
            {
                Events = BqEvents.CreateInstance();
            }
        }

        internal static IJSRuntime JsRuntime { get; set; }

        /// <summary>
        /// Viewport operation
        /// </summary>
        public static BqViewport Viewport { get; private set; }

        /// <summary>
        /// window events
        /// </summary>
        public static BqEvents Events { get; private set; }


        #region WidthAndHeight

        /// <summary>
        /// get <param name="element"></param> width
        /// </summary>
        /// <param name="element"></param>
        /// <param name="isOuter"></param>
        /// <returns></returns>
        public static async Task<double> GetWidthAsync(this ElementReference element, bool isOuter = true)
        {
            return await JsRuntime.InvokeAsync<double>("bQuery.getWidth", element, isOuter);
        }

        /// <summary>
        /// get <param name="element"></param> height
        /// </summary>
        /// <param name="element"></param>
        /// <param name="isOuter"></param>
        /// <returns></returns>
        public static async Task<double> GetHeightAsync(this ElementReference element, bool isOuter = true)
        {
            return await JsRuntime.InvokeAsync<double>("bQuery.getHeight", element, isOuter);
        }

        /// <summary>
        /// get <param name="element"></param> width and height
        /// </summary>
        /// <param name="element"></param>
        /// <param name="isOuter"></param>
        /// <returns></returns>
        public static async Task<double[]> GetWidthAndHeightAsync(this ElementReference element, bool isOuter = true)
        {
            return await JsRuntime.InvokeAsync<double[]>("bQuery.getWidthAndHeight", element, isOuter);
        }

        #endregion

        #region Scroll

        /// <summary>
        /// get <param name="element"></param> scroll width
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static async Task<double> GetScrollWidthAsync(this ElementReference element)
        {
            return await JsRuntime.InvokeAsync<double>("bQuery.getScrollWidth", element);
        }

        /// <summary>
        /// get <param name="element"></param> scroll height
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static async Task<double> GetScrollHeightAsync(this ElementReference element)
        {
            return await JsRuntime.InvokeAsync<double>("bQuery.getScrollHeight", element);
        }

        /// <summary>
        /// get <param name="element"></param> scroll width and height
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static async Task<double[]> GetScrollWidthAndHeightAsync(this ElementReference element)
        {
            return await JsRuntime.InvokeAsync<double[]>("bQuery.getScrollWidthAndHeight", element);
        }

        #endregion
        
        #region scroll top left

        /// <summary>
        /// get viewport scroll left
        /// </summary>
        /// <returns></returns>
        public static async Task<double> GetScrollLeftAsync(this ElementReference element)
        {
            return await JsRuntime.InvokeAsync<double>("bQuery.getScrollLeft", element);
        }

        /// <summary>
        /// get viewport scroll top
        /// </summary>
        /// <returns></returns>
        public static async Task<double> GetScrollTopAsync(this ElementReference element)
        {
            return await JsRuntime.InvokeAsync<double>("bQuery.getScrollTop", element);
        }

        /// <summary>
        /// get viewport scroll left and top
        /// </summary>
        /// <returns>double array: [left, top]</returns>
        public static async Task<double[]> GetScrollLeftAndTopAsync(this ElementReference element)
        {
            return await JsRuntime.InvokeAsync<double[]>("bQuery.getScrollLeftAndTop", element);
        }

        #endregion



        #region position

        /// <summary>
        /// get <param name="element"></param> position in Viewport
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static async Task<ElePosition> GetPositionInViewport(this ElementReference element)
        {
            return await JsRuntime.InvokeAsync<ElePosition>("bQuery.getPositionInViewport", element);
        }

        /// <summary>
        ///  get <param name="element"></param> position in document
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static async Task<ElePosition> GetPositionInDoc(this ElementReference element)
        {
            return await JsRuntime.InvokeAsync<ElePosition>("bQuery.getPositionInDoc", element);
        }

        #endregion


    }
}
