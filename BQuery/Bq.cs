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
                scope.Dispose();
                throw;
            }
        }

        internal static T GetScopeService<T>(IServiceScope scope)
        {
            return scope.ServiceProvider.GetService<T>();
        }

        internal static IJSRuntime GetJsRuntime(out IServiceScope scope)
        {
            var jsRuntime = GetScopeService<IJSRuntime>(out scope);
            return jsRuntime;
        }

        internal static void Init(IServiceProvider services)
        {
            Services = services;
            if (Viewport == null)
            {
                Viewport = BqViewport.CreateInstance();
            }

            if (Events == null)
            {
                Events = BqEvents.CreateInstance();
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
        public static async Task<string> GetUserAgentAsync()
        {
            var jsRuntime = GetJsRuntime(out var scope);
            try
            {
                var result = await jsRuntime.InvokeAsync<string>(JsInteropConstants.GetUserAgent);
                return result;
            }
            finally
            {
                scope.Dispose();
            }
        }

        #region WidthAndHeight

        /// <summary>
        /// get <param name="element"></param> width
        /// </summary>
        /// <param name="element"></param>
        /// <param name="isOuter"></param>
        /// <returns></returns>
        public static async Task<double> GetWidthAsync(this ElementReference element, bool isOuter = true)
        {
            var jsRuntime = GetJsRuntime(out var scope);
            try
            {
                var result = await jsRuntime.InvokeAsync<double>(JsInteropConstants.GetWidth, element, isOuter);

                return result;
            }
            finally
            {
                scope.Dispose();
            }
        }

        /// <summary>
        /// get <param name="element"></param> height
        /// </summary>
        /// <param name="element"></param>
        /// <param name="isOuter"></param>
        /// <returns></returns>
        public static async Task<double> GetHeightAsync(this ElementReference element, bool isOuter = true)
        {
            var jsRuntime = GetJsRuntime(out var scope);
            try
            {
                var result = await jsRuntime.InvokeAsync<double>(JsInteropConstants.GetHeight, element, isOuter);

                return result;
            }
            finally
            {
                scope.Dispose();
            }
        }

        /// <summary>
        /// get <param name="element"></param> width and height
        /// </summary>
        /// <param name="element"></param>
        /// <param name="isOuter"></param>
        /// <returns></returns>
        public static async Task<double[]> GetWidthAndHeightAsync(this ElementReference element, bool isOuter = true)
        {
            var jsRuntime = GetJsRuntime(out var scope);
            try
            {
                var result = await jsRuntime.InvokeAsync<double[]>(JsInteropConstants.GetWidthAndHeight, element, isOuter);

                return result;
            }
            finally
            {
                scope.Dispose();
            }
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
            var jsRuntime = GetJsRuntime(out var scope);
            try
            {
                var result = await jsRuntime.InvokeAsync<double>(JsInteropConstants.GetScrollWidth, element);

                return result;
            }
            finally
            {
                scope.Dispose();
            }
        }

        /// <summary>
        /// get <param name="element"></param> scroll height
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static async Task<double> GetScrollHeightAsync(this ElementReference element)
        {
            var jsRuntime = GetJsRuntime(out var scope);
            try
            {
                var result = await jsRuntime.InvokeAsync<double>(JsInteropConstants.GetScrollHeight, element);

                return result;
            }
            finally
            {
                scope.Dispose();
            }
        }

        /// <summary>
        /// get <param name="element"></param> scroll width and height
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static async Task<double[]> GetScrollWidthAndHeightAsync(this ElementReference element)
        {
            var jsRuntime = GetJsRuntime(out var scope);
            try
            {
                var result = await jsRuntime.InvokeAsync<double[]>(JsInteropConstants.GetScrollWidthAndHeight, element);

                return result;
            }
            finally
            {
                scope.Dispose();
            }
        }

        #endregion

        #region scroll top left

        /// <summary>
        /// get element scroll left
        /// </summary>
        /// <returns></returns>
        public static async Task<double> GetScrollLeftAsync(this ElementReference element)
        {
            var jsRuntime = GetJsRuntime(out var scope);
            try
            {
                var result = await jsRuntime.InvokeAsync<double>(JsInteropConstants.GetScrollLeft, element);

                return result;
            }
            finally
            {
                scope.Dispose();
            }
        }

        /// <summary>
        /// get element scroll top
        /// </summary>
        /// <returns></returns>
        public static async Task<double> GetScrollTopAsync(this ElementReference element)
        {
            var jsRuntime = GetJsRuntime(out var scope);
            try
            {
                var result = await jsRuntime.InvokeAsync<double>(JsInteropConstants.GetScrollTop, element);

                return result;
            }
            finally
            {
                scope.Dispose();
            }
        }

        /// <summary>
        /// get element scroll left and top
        /// </summary>
        /// <returns>double array: [left, top]</returns>
        public static async Task<double[]> GetScrollLeftAndTopAsync(this ElementReference element)
        {
            var jsRuntime = GetJsRuntime(out var scope);
            try
            {
                var result = await jsRuntime.InvokeAsync<double[]>(JsInteropConstants.GetScrollLeftAndTop, element);

                return result;
            }
            finally
            {
                scope.Dispose();
            }
        }

        #endregion

        #region position

        /// <summary>
        /// get <param name="element"></param> position in Viewport
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static async Task<ElePosition> GetPositionInViewportAsync(this ElementReference element)
        {
            var jsRuntime = GetJsRuntime(out var scope);
            try
            {
                var result = await jsRuntime.InvokeAsync<ElePosition>(JsInteropConstants.GetPositionInViewport, element);

                return result;
            }
            finally
            {
                scope.Dispose();
            }
        }

        /// <summary>
        ///  get <param name="element"></param> position in document
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static async Task<ElePosition> GetPositionInDocAsync(this ElementReference element)
        {
            var jsRuntime = GetJsRuntime(out var scope);
            try
            {
                var result = await jsRuntime.InvokeAsync<ElePosition>(JsInteropConstants.GetPositionInDoc, element);

                return result;
            }
            finally
            {
                scope.Dispose();
            }
        }

        #endregion

        /// <summary>
        ///  focus <param name="element">element</param> 
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static async Task FocusAsync(this ElementReference element)
        {
            var jsRuntime = GetJsRuntime(out var scope);
            try
            {
                await jsRuntime.InvokeVoidAsync(JsInteropConstants.Focus, element);
            }
            finally
            {
                scope.Dispose();
            }
        }

        /// <summary>
        /// bind drag for <param name="element">element</param> 
        /// </summary>
        /// <param name="element"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static async Task BindDragAsync(this ElementReference element, DragOptions options = null)
        {
            var jsRuntime = GetJsRuntime(out var scope);
            try
            {
                options ??= new DragOptions();
                await jsRuntime.InvokeVoidAsync(JsInteropConstants.BindDrag, element, options);
            }
            finally
            {
                scope.Dispose();
            }
        }
    }
}
