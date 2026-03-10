using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace BQuery;

[ExcludeFromCodeCoverage]
public static class ElementReferenceExtensions
{
    [UnsafeAccessor(UnsafeAccessorKind.Field, Name = "<JSRuntime>k__BackingField")]
    private static extern ref IJSRuntime GetJsRuntime(WebElementReferenceContext context);

    internal static IJSRuntime? GetJSRuntime(this ElementReference elementReference)
    {
        if (elementReference.Context is WebElementReferenceContext context)
        {
            var jsRuntime = GetJsRuntime(context);

            return jsRuntime;
        }

        return null;
    }


    private static IJSRuntime GetJs(this ElementReference element, IJSRuntime? jsRuntime = null)
    {
        if (jsRuntime != null)
        {
            return jsRuntime;
        }
        return element.GetJSRuntime() ?? throw new NotSupportedException("cannot get IJSRuntime from ElementReference instance!");
    }

    #region domHelper


    /// <summary>
    ///  
    /// </summary>
    /// <returns></returns>
    public static async Task Attr(
        this ElementReference element,
        string key,
        string? value = null,
        IJSRuntime? jsRuntime = null
        )
    {
        await element.GetJs(jsRuntime)
            .InvokeVoidAsync(
            DomConstants.AttrMethod,
            element, key, value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public static async Task AddCls(
        this ElementReference element,
        string className,
        IJSRuntime? jsRuntime = null
        )
    {
        await AddCls(element, new List<string> { className }, jsRuntime);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="element"></param>
    /// <param name="classNames"></param>
    /// <param name="jsRuntime"></param>
    /// <returns></returns>
    public static async Task AddCls(
        this ElementReference element,
        List<string> classNames,
        IJSRuntime? jsRuntime = null
        )
    {
        await element.GetJs(jsRuntime)
            .InvokeVoidAsync(
            DomConstants.AddClsMethod,
            element, classNames
            );
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public static async Task RemoveCls(
        this ElementReference element,
        string className,
        IJSRuntime? jsRuntime = null
        )
    {
        await RemoveCls(element, new List<string> { className }, jsRuntime);
    }


    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public static async Task RemoveCls(
        this ElementReference element,
        List<string> classNames,
        IJSRuntime? jsRuntime = null
        )
    {
        await element.GetJs(jsRuntime)
            .InvokeVoidAsync(
            DomConstants.RemoveClsMethod,
            element, classNames);
    }


    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public static async Task AddCss(
        this ElementReference element,
        string name,
        string value,
        IJSRuntime? jsRuntime = null
        )
    {
        await Css(element, name, value, jsRuntime);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public static async Task RemoveCss(
        this ElementReference element,
        string name,
        IJSRuntime? jsRuntime = null
        )
    {
        await Css(element, name, null, jsRuntime);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public static async Task Css(
        this ElementReference element,
        string name,
        string? value = null,
        IJSRuntime? jsRuntime = null
        )
    {
        await element.GetJs(jsRuntime)
            .InvokeVoidAsync(
            DomConstants.CssMethod,
            element, name, value);
    }
    #endregion


    #region ElementExtensions


    #region WidthAndHeight

    /// <summary>
    /// get <param name="element"></param> width
    /// </summary>
    /// <param name="element"></param>
    /// <param name="isOuter"></param>
    /// <returns></returns>
    public static async Task<double> GetWidthAsync(this ElementReference element, bool isOuter = true)
    {
        return await element.GetJs().InvokeAsync<double>(ElementConstants.GetWidthMethod, element, isOuter);
    }

    /// <summary>
    /// get <param name="element"></param> height
    /// </summary>
    /// <param name="element"></param>
    /// <param name="isOuter"></param>
    /// <returns></returns>
    public static async Task<double> GetHeightAsync(this ElementReference element, bool isOuter = true)
    {
        return await element.GetJs().InvokeAsync<double>(ElementConstants.GetHeightMethod, element, isOuter);
    }

    /// <summary>
    /// get <param name="element"></param> width and height
    /// </summary>
    /// <param name="element"></param>
    /// <param name="isOuter"></param>
    /// <returns></returns>
    public static async Task<double[]> GetWidthAndHeightAsync(this ElementReference element, bool isOuter = true)
    {
        return await element.GetJs().InvokeAsync<double[]>(ElementConstants.GetWidthAndHeightMethod, element, isOuter);
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
        return await element.GetJs().InvokeAsync<double>(ElementConstants.GetScrollWidthMethod, element);
    }

    /// <summary>
    /// get <param name="element"></param> scroll height
    /// </summary>
    /// <param name="element"></param>
    /// <returns></returns>
    public static async Task<double> GetScrollHeightAsync(this ElementReference element)
    {
        return await element.GetJs().InvokeAsync<double>(ElementConstants.GetScrollHeightMethod, element);
    }

    /// <summary>
    /// get <param name="element"></param> scroll width and height
    /// </summary>
    /// <param name="element"></param>
    /// <returns></returns>
    public static async Task<double[]> GetScrollWidthAndHeightAsync(this ElementReference element)
    {
        return await element.GetJs().InvokeAsync<double[]>(ElementConstants.GetScrollWidthAndHeightMethod, element);
    }

    #endregion

    #region scroll top left

    /// <summary>
    /// get element scroll left
    /// </summary>
    /// <returns></returns>
    public static async Task<double> GetScrollLeftAsync(this ElementReference element)
    {
        return await element.GetJs().InvokeAsync<double>(ElementConstants.GetScrollLeftMethod, element);
    }

    /// <summary>
    /// get element scroll top
    /// </summary>
    /// <returns></returns>
    public static async Task<double> GetScrollTopAsync(this ElementReference element)
    {
        return await element.GetJs().InvokeAsync<double>(ElementConstants.GetScrollTopMethod, element);
    }

    /// <summary>
    /// get element scroll left and top
    /// </summary>
    /// <returns>double array: [left, top]</returns>
    public static async Task<double[]> GetScrollLeftAndTopAsync(this ElementReference element)
    {
        return await element.GetJs().InvokeAsync<double[]>(ElementConstants.GetScrollLeftAndTopMethod, element);
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
        return await element.GetJs().InvokeAsync<ElePosition>(ElementConstants.GetPositionInViewportMethod, element);
    }

    /// <summary>
    ///  get <param name="element"></param> position in document
    /// </summary>
    /// <param name="element"></param>
    /// <returns></returns>
    public static async Task<ElePosition> GetPositionInDocAsync(this ElementReference element)
    {
        return await element.GetJs().InvokeAsync<ElePosition>(ElementConstants.GetPositionInDocMethod, element);
    }

    #endregion

    #endregion
}
