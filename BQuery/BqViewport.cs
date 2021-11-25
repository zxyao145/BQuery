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
        private BqViewport()
        {
        }

        private static BqViewport _instance;
        private static int _lock = 0;

        internal static BqViewport CreateInstance()
        {
            if (_instance == null)
            {
                if (Interlocked.CompareExchange(ref _lock, 1, 0) == 0)
                {
                    _instance = new BqViewport();
                    Interlocked.Increment(ref _lock);
                }

                while (Interlocked.CompareExchange(ref _lock, 1, 1) == 1)
                {
                    Thread.Sleep(100);
                }
            }

            return _instance;
        }

        #region width height

        /// <summary>
        /// get viewport width
        /// </summary>
        /// <returns></returns>
        public async Task<double> GetWidthAsync(IJSRuntime jsRuntime = null)
        {
            return await Bq.ProxyAsync(async (js) =>
            {
                return await js.InvokeAsync<double>(JsInteropConstants.VpGetWidth);
            }, jsRuntime);
        }

        /// <summary>
        /// get viewport height
        /// </summary>
        /// <returns></returns>
        public async Task<double> GetHeightAsync(IJSRuntime jsRuntime = null)
        {
            return await Bq.ProxyAsync(async (js) =>
            {
                return await js.InvokeAsync<double>(JsInteropConstants.VpGetHeight);
            }, jsRuntime);
        }

        /// <summary>
        /// get viewport width and height
        /// </summary>
        /// <returns>double array: [width,height]</returns>
        public async Task<double[]> GetWidthAndHeightAsync(IJSRuntime jsRuntime = null)
        {
            return await Bq.ProxyAsync(async (js) =>
            {
                return await js.InvokeAsync<double[]>(JsInteropConstants.VpGetWidthAndHeight);
            }, jsRuntime);
        }

        #endregion
      
        #region Scroll

        /// <summary>
        /// get viewport scroll width
        /// </summary>
        /// <returns></returns>
        public async Task<double> GetScrollWidthAsync(IJSRuntime jsRuntime = null)
        {
            return await Bq.ProxyAsync(async (js) =>
            {
                return await js.InvokeAsync<double>(JsInteropConstants.VpGetScrollWidth);
            }, jsRuntime);
        }

        /// <summary>
        /// get viewport scroll height
        /// </summary>
        /// <returns></returns>
        public async Task<double> GetScrollHeightAsync(IJSRuntime jsRuntime = null)
        {
            return await Bq.ProxyAsync(async (js) =>
            {
                return await js.InvokeAsync<double>(JsInteropConstants.VpGetScrollHeight);
            }, jsRuntime);
        }

        /// <summary>
        /// get viewport scroll width and height
        /// </summary>
        /// <returns>double array: [width,height]</returns>
        public async Task<double[]> GetScrollWidthAndHeightAsync(IJSRuntime jsRuntime = null)
        {
            return await Bq.ProxyAsync(async (js) =>
            {
                return await js.InvokeAsync<double[]>(JsInteropConstants.VpGetScrollWidthAndHeight);
            }, jsRuntime);
        }

        #endregion

        /// <summary>
        /// get viewport scroll left
        /// </summary>
        /// <returns></returns>
        public async Task<double> GetScrollLeftAsync(IJSRuntime jsRuntime = null)
        {
            return await Bq.ProxyAsync(async (js) =>
            {
                return await js.InvokeAsync<double>(JsInteropConstants.VpGetScrollLeft);
            }, jsRuntime);
        }

        /// <summary>
        /// get viewport scroll top
        /// </summary>
        /// <returns></returns>
        public async Task<double> GetScrollTopAsync(IJSRuntime jsRuntime = null)
        {
            return await Bq.ProxyAsync(async (js) =>
            {
                return await js.InvokeAsync<double>(JsInteropConstants.VpGetScrollTop);
            }, jsRuntime);
        }

        /// <summary>
        /// get viewport scroll left and top
        /// </summary>
        /// <returns>double array: [left, top]</returns>
        public async Task<double[]> GetScrollLeftAndTopAsync(IJSRuntime jsRuntime = null)
        {
            return await Bq.ProxyAsync(async (js) =>
            {
                return await js.InvokeAsync<double[]>(JsInteropConstants.VpGetScrollLeftAndTop);
            }, jsRuntime);
        }
    }
}
