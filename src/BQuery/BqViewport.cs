namespace BQuery;

[GenerateJsInteropMethods(typeof(JsModuleConstants.Viewport))]
public partial class BqViewport
{
    private readonly IJSRuntime _jsRuntime;

    internal BqViewport(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }


    #region width height

    /// <summary>
    /// get viewport width
    /// </summary>
    /// <returns></returns>
    public async Task<double> GetWidthAsync()
    {
        return await _jsRuntime.InvokeAsync<double>(GetWidthMethod);
    }

    /// <summary>
    /// get viewport height
    /// </summary>
    /// <returns></returns>
    public async Task<double> GetHeightAsync()
    {
        return await _jsRuntime.InvokeAsync<double>(GetHeightMethod);
    }

    /// <summary>
    /// get viewport width and height
    /// </summary>
    /// <returns>double array: [width,height]</returns>
    public async Task<double[]> GetWidthAndHeightAsync()
    {
        return await _jsRuntime.InvokeAsync<double[]>(GetWidthAndHeightMethod);
    }

    #endregion

    #region Scroll

    /// <summary>
    /// get viewport scroll width
    /// </summary>
    /// <returns></returns>
    public async Task<double> GetScrollWidthAsync()
    {
        return await _jsRuntime.InvokeAsync<double>(GetScrollWidthMethod);
    }

    /// <summary>
    /// get viewport scroll height
    /// </summary>
    /// <returns></returns>
    public async Task<double> GetScrollHeightAsync()
    {
        return await _jsRuntime.InvokeAsync<double>(GetScrollHeightMethod);
    }

    /// <summary>
    /// get viewport scroll width and height
    /// </summary>
    /// <returns>double array: [width,height]</returns>
    public async Task<double[]> GetScrollWidthAndHeightAsync()
    {
        return await _jsRuntime.InvokeAsync<double[]>(GetScrollWidthAndHeightMethod);
    }

    #endregion

    /// <summary>
    /// get viewport scroll left
    /// </summary>
    /// <returns></returns>
    public async Task<double> GetScrollLeftAsync()
    {
        return await _jsRuntime.InvokeAsync<double>(GetScrollLeftMethod);
    }

    /// <summary>
    /// get viewport scroll top
    /// </summary>
    /// <returns></returns>
    public async Task<double> GetScrollTopAsync()
    {
        return await _jsRuntime.InvokeAsync<double>(GetScrollTopMethod);
    }

    /// <summary>
    /// get viewport scroll left and top
    /// </summary>
    /// <returns>double array: [left, top]</returns>
    public async Task<double[]> GetScrollLeftAndTopAsync()
    {
        return await _jsRuntime.InvokeAsync<double[]>(GetScrollLeftAndTopMethod);
    }

}
