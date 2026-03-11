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


    private static IJSRuntime GetJs(this ElementReference element)
    {
        return element.GetJSRuntime() ?? throw new NotSupportedException("cannot get IJSRuntime from ElementReference instance!");
    }

    #region domHelper


    /// <summary>
    ///  
    /// </summary>
    /// <returns></returns>
    [Obsolete("Use the more semantically explicit APIs getAttr, setAttr, and removeAttr.")]
    public static async ValueTask Attr(
        this ElementReference element,
        string key,
        string? value = null
        )
    {
        await element.GetJs()
            .InvokeVoidAsync(
            DomConstants.AttrMethod,
            element, key, value);
    }

    public static async ValueTask SetAttr(
       this ElementReference element,
       string key,
       string value
       )
    {
        await element.GetJs()
            .InvokeVoidAsync(
            DomConstants.SetAttrMethod,
            element, key, value);
    }

    public static ValueTask<string?> GetAttr(this ElementReference element, string key)
    {
        return element.GetJs().InvokeAsync<string?>(DomConstants.GetAttrMethod, element, key);
    }


    public static async ValueTask RemoveAttr(
    this ElementReference element,
    string key
    )
    {
        await element.GetJs()
            .InvokeVoidAsync(
            DomConstants.RemoveAttrMethod,
            element, key);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public static async ValueTask AddCls(
        this ElementReference element,
        string className
        )
    {
        await AddCls(element, new List<string> { className });
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="element"></param>
    /// <param name="classNames"></param>
    /// <param name="jsRuntime"></param>
    /// <returns></returns>
    public static async ValueTask AddCls(
        this ElementReference element,
        List<string> classNames
        )
    {
        await element.GetJs()
            .InvokeVoidAsync(
            DomConstants.AddClsMethod,
            element, classNames
            );
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public static async ValueTask RemoveCls(
        this ElementReference element,
        string className
        )
    {
        await RemoveCls(element, new List<string> { className });
    }


    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public static async ValueTask RemoveCls(
        this ElementReference element,
        List<string> classNames
        )
    {
        await element.GetJs()
            .InvokeVoidAsync(
            DomConstants.RemoveClsMethod,
            element, classNames);
    }


    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public static async ValueTask AddCss(
        this ElementReference element,
        string name,
        string value
        )
    {
        await Css(element, name, value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public static async ValueTask RemoveCss(
        this ElementReference element,
        string name
        )
    {
        await Css(element, name, null);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public static async ValueTask Css(
        this ElementReference element,
        string name,
        string? value = null
        )
    {
        await element.GetJs()
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
    public static async ValueTask<double> GetWidthAsync(this ElementReference element, bool isOuter = true)
    {
        return await element.GetJs().InvokeAsync<double>(ElementConstants.GetWidthMethod, element, isOuter);
    }

    /// <summary>
    /// get <param name="element"></param> height
    /// </summary>
    /// <param name="element"></param>
    /// <param name="isOuter"></param>
    /// <returns></returns>
    public static async ValueTask<double> GetHeightAsync(this ElementReference element, bool isOuter = true)
    {
        return await element.GetJs().InvokeAsync<double>(ElementConstants.GetHeightMethod, element, isOuter);
    }

    /// <summary>
    /// get <param name="element"></param> width and height
    /// </summary>
    /// <param name="element"></param>
    /// <param name="isOuter"></param>
    /// <returns></returns>
    public static async ValueTask<double[]> GetWidthAndHeightAsync(this ElementReference element, bool isOuter = true)
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
    public static async ValueTask<double> GetScrollWidthAsync(this ElementReference element)
    {
        return await element.GetJs().InvokeAsync<double>(ElementConstants.GetScrollWidthMethod, element);
    }

    /// <summary>
    /// get <param name="element"></param> scroll height
    /// </summary>
    /// <param name="element"></param>
    /// <returns></returns>
    public static async ValueTask<double> GetScrollHeightAsync(this ElementReference element)
    {
        return await element.GetJs().InvokeAsync<double>(ElementConstants.GetScrollHeightMethod, element);
    }

    /// <summary>
    /// get <param name="element"></param> scroll width and height
    /// </summary>
    /// <param name="element"></param>
    /// <returns></returns>
    public static async ValueTask<double[]> GetScrollWidthAndHeightAsync(this ElementReference element)
    {
        return await element.GetJs().InvokeAsync<double[]>(ElementConstants.GetScrollWidthAndHeightMethod, element);
    }

    #endregion

    #region scroll top left

    /// <summary>
    /// get element scroll left
    /// </summary>
    /// <returns></returns>
    public static async ValueTask<double> GetScrollLeftAsync(this ElementReference element)
    {
        return await element.GetJs().InvokeAsync<double>(ElementConstants.GetScrollLeftMethod, element);
    }

    /// <summary>
    /// get element scroll top
    /// </summary>
    /// <returns></returns>
    public static async ValueTask<double> GetScrollTopAsync(this ElementReference element)
    {
        return await element.GetJs().InvokeAsync<double>(ElementConstants.GetScrollTopMethod, element);
    }

    /// <summary>
    /// get element scroll left and top
    /// </summary>
    /// <returns>double array: [left, top]</returns>
    public static async ValueTask<double[]> GetScrollLeftAndTopAsync(this ElementReference element)
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
    public static async ValueTask<ElePosition> GetPositionInViewportAsync(this ElementReference element)
    {
        return await element.GetJs().InvokeAsync<ElePosition>(ElementConstants.GetPositionInViewportMethod, element);
    }

    /// <summary>
    ///  get <param name="element"></param> position in document
    /// </summary>
    /// <param name="element"></param>
    /// <returns></returns>
    public static async ValueTask<ElePosition> GetPositionInDocAsync(this ElementReference element)
    {
        return await element.GetJs().InvokeAsync<ElePosition>(ElementConstants.GetPositionInDocMethod, element);
    }

    #endregion

    #endregion
}
