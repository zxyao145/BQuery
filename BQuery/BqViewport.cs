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
        public async Task<double> GetWidthAsync()
        {
            var jsRuntime = Bq.GetJsRuntime(out var scope);
            try
            {
                var result = await jsRuntime.InvokeAsync<double>(JsInteropConstants.VpGetWidth);

                return result;
            }
            finally
            {
                scope.BqDispose();
            }
        }

        /// <summary>
        /// get viewport height
        /// </summary>
        /// <returns></returns>
        public async Task<double> GetHeightAsync()
        {
            var jsRuntime = Bq.GetJsRuntime(out var scope);
            try
            {
                var result = await jsRuntime.InvokeAsync<double>(JsInteropConstants.VpGetHeight);

                return result;
            }
            finally
            {
                scope.BqDispose();
            }
        }

        /// <summary>
        /// get viewport width and height
        /// </summary>
        /// <returns>double array: [width,height]</returns>
        public async Task<double[]> GetWidthAndHeightAsync()
        {
            var jsRuntime = Bq.GetJsRuntime(out var scope);
            try
            {
                var result = await jsRuntime.InvokeAsync<double[]>(JsInteropConstants.VpGetWidthAndHeight);

                return result;
            }
            finally
            {
                scope.BqDispose();
            }
        }

        #endregion
      
        #region Scroll

        /// <summary>
        /// get viewport scroll width
        /// </summary>
        /// <returns></returns>
        public async Task<double> GetScrollWidthAsync()
        {
            var jsRuntime = Bq.GetJsRuntime(out var scope);
            try
            {
                var result = await jsRuntime.InvokeAsync<double>(JsInteropConstants.VpGetScrollWidth);

                return result;
            }
            finally
            {
                scope.BqDispose();
            }
        }

        /// <summary>
        /// get viewport scroll height
        /// </summary>
        /// <returns></returns>
        public async Task<double> GetScrollHeightAsync()
        {
            var jsRuntime = Bq.GetJsRuntime(out var scope);
            try
            {
                var result = await jsRuntime.InvokeAsync<double>(JsInteropConstants.VpGetScrollHeight);

                return result;
            }
            finally
            {
                scope.BqDispose();
            }
        }

        /// <summary>
        /// get viewport scroll width and height
        /// </summary>
        /// <returns>double array: [width,height]</returns>
        public async Task<double[]> GetScrollWidthAndHeightAsync()
        {
            var jsRuntime = Bq.GetJsRuntime(out var scope);
            try
            {
                var result = await jsRuntime.InvokeAsync<double[]>(JsInteropConstants.VpGetScrollWidthAndHeight);

                return result;
            }
            finally
            {
                scope.BqDispose();
            }
        }

        #endregion

        /// <summary>
        /// get viewport scroll left
        /// </summary>
        /// <returns></returns>
        public async Task<double> GetScrollLeftAsync()
        {
            var jsRuntime = Bq.GetJsRuntime(out var scope);
            try
            {
                var result = await jsRuntime.InvokeAsync<double>(JsInteropConstants.VpGetScrollLeft);

                return result;
            }
            finally
            {
                scope.BqDispose();
            }
        }

        /// <summary>
        /// get viewport scroll top
        /// </summary>
        /// <returns></returns>
        public async Task<double> GetScrollTopAsync()
        {
            var jsRuntime = Bq.GetJsRuntime(out var scope);
            try
            {
                var result = await jsRuntime.InvokeAsync<double>(JsInteropConstants.VpGetScrollTop);

                return result;
            }
            finally
            {
                scope.BqDispose();
            }
        }

        /// <summary>
        /// get viewport scroll left and top
        /// </summary>
        /// <returns>double array: [left, top]</returns>
        public async Task<double[]> GetScrollLeftAndTopAsync()
        {
            var jsRuntime = Bq.GetJsRuntime(out var scope);
            try
            {
                var result = await jsRuntime.InvokeAsync<double[]>(JsInteropConstants.VpGetScrollLeftAndTop);

                return result;
            }
            finally
            {
                scope.BqDispose();
            }
        }
    }
}
