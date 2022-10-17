using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable

namespace BQuery
{
    public class DomHelper
    {
        /// <summary>
        ///  
        /// </summary>
        /// <returns></returns>
        public async Task Attr(
        ElementReference element,
        string key,
        string? value = null,
        IJSRuntime? jsRuntime = null)
        {
            await Bq.ProxyAsync(async (js) =>
            {
                await js.InvokeVoidAsync(JsInteropConstants.DomHelperAttr, element, key, value);
            }, jsRuntime);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task AddCls(
        ElementReference element,
        string className,
        IJSRuntime? jsRuntime = null)
        {
            await AddCls(element, new List<string> { className }, jsRuntime);
        }

        public async Task AddCls(
        ElementReference element,
        List<string> classNames,
        IJSRuntime? jsRuntime = null)
        {
            await Bq.ProxyAsync(async (js) =>
            {
                await js.InvokeVoidAsync(JsInteropConstants.DomHelperAddCls, element, classNames);
            }, jsRuntime);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task RemoveCls(
        ElementReference element,
        string className,
        IJSRuntime? jsRuntime = null)
        {
            await RemoveCls(element, new List<string> { className }, jsRuntime);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task RemoveCls(
        ElementReference element,
        List<string> classNames,
        IJSRuntime? jsRuntime = null)
        {
            await Bq.ProxyAsync(async (js) =>
            {
                await js.InvokeVoidAsync(JsInteropConstants.DomHelperRemoveCls, element, classNames);
            }, jsRuntime);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task AddCss(
        ElementReference element,
        string name,
        string value,
        IJSRuntime? jsRuntime = null)
        {
            await Css(element, name, value, jsRuntime);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task RemoveCss(
        ElementReference element,
        string name,
        IJSRuntime? jsRuntime = null)
        {
            await Css(element, name, null, jsRuntime);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task Css(
        ElementReference element,
        string name,
        string? value = null,
        IJSRuntime? jsRuntime = null)
        {
            await Bq.ProxyAsync(async (js) =>
            {
                await js.InvokeVoidAsync(JsInteropConstants.DomHelperCss, element, name, value);
            }, jsRuntime);
        }
    }
}
