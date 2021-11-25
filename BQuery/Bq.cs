using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;

namespace BQuery
{
    public static class Bq
    {
        public static bool IsServerSide { get; private set; }

        public static IJSRuntime JsRuntime { get; set; }

        internal static IServiceProvider Services { get; set; }

        internal static T GetScopeService<T>(out IServiceScope scope)
        {
            scope = Services.CreateScope();
            try
            {
                return scope.ServiceProvider.GetService<T>();
            }
            catch (Exception)
            {
                scope.BqDispose();
                throw;
            }
        }

        internal static IJSRuntime GetJsRuntime(out IServiceScope scope)
        {
            if (!IsServerSide)
            {
                var jsRuntime = GetScopeService<IJSRuntime>(out scope);
                return jsRuntime;
            }

            throw new NotSupportedException("Server side must pass IJSRuntime instance!");
        }

        internal static void BqDispose(this IServiceScope scope)
        {
            if (!IsServerSide)
            {
                scope?.Dispose();
            }
        }

        internal static async void Init(IServiceProvider services, bool isServerSide = false)
        {
            Services = services;
            Viewport ??= BqViewport.CreateInstance();
            Events ??= BqEvents.CreateInstance();
            IsServerSide = isServerSide;
            if (!IsServerSide)
            {
                var jsRuntime = GetJsRuntime(out var scope);

                await jsRuntime.InvokeVoidAsync(JsInteropConstants.BQueryReady);
                scope.BqDispose();
            }
        }

        /// <summary>
        /// Viewport operation
        /// </summary>
        public static BqViewport Viewport { get; private set; }

        /// <summary>
        /// window events(specifically) 
        /// </summary>
        public static BqEvents Events { get; private set; }

        /// <summary>
        /// get browser useragent
        /// </summary>
        /// <returns></returns>
        public static async Task<string> GetUserAgentAsync(IJSRuntime jsRuntime = null)
        {
            return await ProxyAsync(async (js) =>
            {
                return await js.InvokeAsync<string>(JsInteropConstants.GetUserAgent);
            }, jsRuntime);
        }

        #region WidthAndHeight

        /// <summary>
        /// get <param name="element"></param> width
        /// </summary>
        /// <param name="element"></param>
        /// <param name="isOuter"></param>
        /// <returns></returns>
        public static async Task<double> GetWidthAsync(this ElementReference element, bool isOuter = true, IJSRuntime jsRuntime = null)
        {
            return await ProxyAsync(async (js) =>
            {
                return await js.InvokeAsync<double>(JsInteropConstants.GetWidth, element, isOuter);
            }, jsRuntime);
        }

        /// <summary>
        /// get <param name="element"></param> height
        /// </summary>
        /// <param name="element"></param>
        /// <param name="isOuter"></param>
        /// <returns></returns>
        public static async Task<double> GetHeightAsync(this ElementReference element, bool isOuter = true, IJSRuntime jsRuntime = null)
        {
            return await ProxyAsync(async (js) =>
            {
                return await js.InvokeAsync<double>(JsInteropConstants.GetHeight, element, isOuter);
            }, jsRuntime);
        }

        /// <summary>
        /// get <param name="element"></param> width and height
        /// </summary>
        /// <param name="element"></param>
        /// <param name="isOuter"></param>
        /// <returns></returns>
        public static async Task<double[]> GetWidthAndHeightAsync(this ElementReference element, bool isOuter = true, IJSRuntime jsRuntime = null)
        {
            return await ProxyAsync(async (js) =>
            {
                return await js.InvokeAsync<double[]>(JsInteropConstants.GetWidthAndHeight, element, isOuter);
            }, jsRuntime);
        }

        #endregion

        #region Scroll

        /// <summary>
        /// get <param name="element"></param> scroll width
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static async Task<double> GetScrollWidthAsync(this ElementReference element, IJSRuntime jsRuntime = null)
        {
            return await ProxyAsync(async (js) =>
            {
                return await js.InvokeAsync<double>(JsInteropConstants.GetScrollWidth, element);
            }, jsRuntime);
        }

        /// <summary>
        /// get <param name="element"></param> scroll height
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static async Task<double> GetScrollHeightAsync(this ElementReference element, IJSRuntime jsRuntime = null)
        {
            return await ProxyAsync(async (js) =>
            {
                return await js.InvokeAsync<double>(JsInteropConstants.GetScrollHeight, element);
            }, jsRuntime);
        }

        /// <summary>
        /// get <param name="element"></param> scroll width and height
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static async Task<double[]> GetScrollWidthAndHeightAsync(this ElementReference element, IJSRuntime jsRuntime = null)
        {
            return await ProxyAsync(async (js) =>
            {
                return await js.InvokeAsync<double[]>(JsInteropConstants.GetScrollWidthAndHeight, element);
            }, jsRuntime);
        }

        #endregion

        #region scroll top left

        /// <summary>
        /// get element scroll left
        /// </summary>
        /// <returns></returns>
        public static async Task<double> GetScrollLeftAsync(this ElementReference element, IJSRuntime jsRuntime = null)
        {
            return await ProxyAsync(async (js) =>
            {
                return await js.InvokeAsync<double>(JsInteropConstants.GetScrollLeft, element);
            }, jsRuntime);
        }

        /// <summary>
        /// get element scroll top
        /// </summary>
        /// <returns></returns>
        public static async Task<double> GetScrollTopAsync(this ElementReference element, IJSRuntime jsRuntime = null)
        {
            return await ProxyAsync(async (js) =>
            {
                return await js.InvokeAsync<double>(JsInteropConstants.GetScrollTop, element);
            }, jsRuntime);
        }

        /// <summary>
        /// get element scroll left and top
        /// </summary>
        /// <returns>double array: [left, top]</returns>
        public static async Task<double[]> GetScrollLeftAndTopAsync(this ElementReference element, IJSRuntime jsRuntime = null)
        {
            return await ProxyAsync(async (js) =>
            {
                return await js.InvokeAsync<double[]>(JsInteropConstants.GetScrollTop, element);
            }, jsRuntime);
        }

        #endregion

        #region position

        /// <summary>
        /// get <param name="element"></param> position in Viewport
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static async Task<ElePosition> GetPositionInViewportAsync(this ElementReference element, IJSRuntime jsRuntime = null)
        {
            return await ProxyAsync(async (js) =>
            {
                return await js.InvokeAsync<ElePosition>(JsInteropConstants.GetPositionInViewport, element);
            }, jsRuntime);
        }

        /// <summary>
        ///  get <param name="element"></param> position in document
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static async Task<ElePosition> GetPositionInDocAsync(this ElementReference element, IJSRuntime jsRuntime = null)
        {
            return await ProxyAsync(async (js) =>
            {
                return await js.InvokeAsync<ElePosition>(JsInteropConstants.GetPositionInDoc, element);
            }, jsRuntime);
        }

        #endregion

        /// <summary>
        /// bind drag for <param name="element">element</param> 
        /// </summary>
        /// <param name="element"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static async Task BindDragAsync(this ElementReference element, DragOptions options = null, IJSRuntime jsRuntime = null)
        {
            await ProxyAsync(async (js) =>
            {
                options ??= new DragOptions();
                await js.InvokeVoidAsync(JsInteropConstants.BindDrag, element, options);
            }, jsRuntime);


            //IServiceScope scope = null;
            //jsRuntime ??= GetJsRuntime(out scope);
            //try
            //{
            //}
            //finally
            //{
            //    scope.BqDispose();
            //}
        }

        internal static async Task ProxyAsync(Func<IJSRuntime, Task> func, IJSRuntime jsRuntime = null)
        {
            IServiceScope scope = null;
            jsRuntime ??= GetJsRuntime(out scope);
            try
            {
                await func.Invoke(jsRuntime);
            }
            finally
            {
                scope.BqDispose();
            }
        }

        internal static async Task<T> ProxyAsync<T>(Func<IJSRuntime, Task<T>> func, IJSRuntime jsRuntime = null)
        {
            IServiceScope scope = null;
            jsRuntime ??= GetJsRuntime(out scope);
            try
            {
                return await func.Invoke(jsRuntime);
            }
            finally
            {
                scope.BqDispose();
            }
        }
    }
}
