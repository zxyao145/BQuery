using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace BQuery
{
    public class BqViewport
    {
        private BqViewport(IJSRuntime jsRuntime)
        {
            JsRuntime = jsRuntime;
        }

        private static BqViewport _instance;
        private static int _lock = 0;
        internal static BqViewport CreateInstance(IJSRuntime jsRuntime)
        {
            if (_instance == null)
            {
                if (Interlocked.CompareExchange(ref _lock, 1, 0) == 0)
                {
                    _instance = new BqViewport(jsRuntime);
                    Interlocked.Increment(ref _lock);
                }

                while (Interlocked.CompareExchange(ref _lock, 1, 1) == 1)
                {
                    Thread.Sleep(100);
                }
            }

            return _instance;
        }

        internal IJSRuntime JsRuntime { get; set; }

        #region width height

        /// <summary>
        /// get viewport width
        /// </summary>
        /// <returns></returns>
        public async Task<double> GetWidthAsync()
        {
            return await JsRuntime.InvokeAsync<double>("bQuery.viewport.getWidth");
        }

        /// <summary>
        /// get viewport height
        /// </summary>
        /// <returns></returns>
        public async Task<double> GetHeightAsync()
        {
            return await JsRuntime.InvokeAsync<double>("bQuery.viewport.getHeight");
        }

        /// <summary>
        /// get viewport width and height
        /// </summary>
        /// <returns>double array: [width,height]</returns>
        public async Task<double[]> GetWidthAndHeightAsync()
        {
            return await JsRuntime.InvokeAsync<double[]>("bQuery.viewport.getWidthAndHeight");
        }

        #endregion
      
        #region Scroll

        /// <summary>
        /// get viewport scroll width
        /// </summary>
        /// <returns></returns>
        public async Task<double> GetScrollWidthAsync()
        {
            return await JsRuntime.InvokeAsync<double>("bQuery.viewport.getScrollWidth");
        }

        /// <summary>
        /// get viewport scroll height
        /// </summary>
        /// <returns></returns>
        public async Task<double> GetScrollHeightAsync()
        {
            return await JsRuntime.InvokeAsync<double>("bQuery.viewport.getScrollHeight");
        }

        /// <summary>
        /// get viewport scroll width and height
        /// </summary>
        /// <returns>double array: [width,height]</returns>
        public async Task<double[]> GetScrollWidthAndHeightAsync()
        {
            return await JsRuntime.InvokeAsync<double[]>("bQuery.viewport.getScrollWidthAndHeight");
        }

        #endregion

        /// <summary>
        /// get viewport scroll left
        /// </summary>
        /// <returns></returns>
        public async Task<double> GetScrollLeftAsync()
        {
            return await JsRuntime.InvokeAsync<double>("bQuery.viewport.getScrollLeft");
        }

        /// <summary>
        /// get viewport scroll top
        /// </summary>
        /// <returns></returns>
        public async Task<double> GetScrollTopAsync()
        {
            return await JsRuntime.InvokeAsync<double>("bQuery.viewport.getScrollTop");
        }

        /// <summary>
        /// get viewport scroll left and top
        /// </summary>
        /// <returns>double array: [left, top]</returns>
        public async Task<double[]> GetScrollLeftAndTopAsync()
        {
            return await JsRuntime.InvokeAsync<double[]>("bQuery.viewport.getScrollLeftAndTop");
        }
    }
}
