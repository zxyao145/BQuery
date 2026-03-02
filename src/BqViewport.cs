namespace BQuery;

public class BqViewport
{
    private readonly Func<ValueTask<IJSObjectReference>> _moduleAccessor;
    private static readonly string GetWidthMethod = JsModuleConstants.GetMethod(
        JsModuleConstants.Viewport.ModuleName,
        JsModuleConstants.Viewport.GetWidth);
    private static readonly string GetHeightMethod = JsModuleConstants.GetMethod(
        JsModuleConstants.Viewport.ModuleName,
        JsModuleConstants.Viewport.GetHeight);
    private static readonly string GetWidthAndHeightMethod = JsModuleConstants.GetMethod(
        JsModuleConstants.Viewport.ModuleName,
        JsModuleConstants.Viewport.GetWidthAndHeight);
    private static readonly string GetScrollWidthMethod = JsModuleConstants.GetMethod(
        JsModuleConstants.Viewport.ModuleName,
        JsModuleConstants.Viewport.GetScrollWidth);
    private static readonly string GetScrollHeightMethod = JsModuleConstants.GetMethod(
        JsModuleConstants.Viewport.ModuleName,
        JsModuleConstants.Viewport.GetScrollHeight);
    private static readonly string GetScrollWidthAndHeightMethod = JsModuleConstants.GetMethod(
        JsModuleConstants.Viewport.ModuleName,
        JsModuleConstants.Viewport.GetScrollWidthAndHeight);
    private static readonly string GetScrollLeftMethod = JsModuleConstants.GetMethod(
        JsModuleConstants.Viewport.ModuleName,
        JsModuleConstants.Viewport.GetScrollLeft);
    private static readonly string GetScrollTopMethod = JsModuleConstants.GetMethod(
        JsModuleConstants.Viewport.ModuleName,
        JsModuleConstants.Viewport.GetScrollTop);
    private static readonly string GetScrollLeftAndTopMethod = JsModuleConstants.GetMethod(
        JsModuleConstants.Viewport.ModuleName,
        JsModuleConstants.Viewport.GetScrollLeftAndTop);

    internal BqViewport(Func<ValueTask<IJSObjectReference>> moduleAccessor)
    {
        _moduleAccessor = moduleAccessor;
    }

    private ValueTask<IJSObjectReference> GetModuleAsync()
    {
        return _moduleAccessor();
    }


    #region width height

    /// <summary>
    /// get viewport width
    /// </summary>
    /// <returns></returns>
    public async Task<double> GetWidthAsync()
    {
        return await (await GetModuleAsync()).InvokeAsync<double>(GetWidthMethod);
    }

    /// <summary>
    /// get viewport height
    /// </summary>
    /// <returns></returns>
    public async Task<double> GetHeightAsync()
    {
        return await (await GetModuleAsync()).InvokeAsync<double>(GetHeightMethod);
    }

    /// <summary>
    /// get viewport width and height
    /// </summary>
    /// <returns>double array: [width,height]</returns>
    public async Task<double[]> GetWidthAndHeightAsync()
    {
        return await (await GetModuleAsync()).InvokeAsync<double[]>(GetWidthAndHeightMethod);
    }

    #endregion

    #region Scroll

    /// <summary>
    /// get viewport scroll width
    /// </summary>
    /// <returns></returns>
    public async Task<double> GetScrollWidthAsync()
    {
        return await (await GetModuleAsync()).InvokeAsync<double>(GetScrollWidthMethod);
    }

    /// <summary>
    /// get viewport scroll height
    /// </summary>
    /// <returns></returns>
    public async Task<double> GetScrollHeightAsync()
    {
        return await (await GetModuleAsync()).InvokeAsync<double>(GetScrollHeightMethod);
    }

    /// <summary>
    /// get viewport scroll width and height
    /// </summary>
    /// <returns>double array: [width,height]</returns>
    public async Task<double[]> GetScrollWidthAndHeightAsync()
    {
        return await (await GetModuleAsync()).InvokeAsync<double[]>(GetScrollWidthAndHeightMethod);
    }

    #endregion

    /// <summary>
    /// get viewport scroll left
    /// </summary>
    /// <returns></returns>
    public async Task<double> GetScrollLeftAsync()
    {
        return await (await GetModuleAsync()).InvokeAsync<double>(GetScrollLeftMethod);
    }

    /// <summary>
    /// get viewport scroll top
    /// </summary>
    /// <returns></returns>
    public async Task<double> GetScrollTopAsync()
    {
        return await (await GetModuleAsync()).InvokeAsync<double>(GetScrollTopMethod);
    }

    /// <summary>
    /// get viewport scroll left and top
    /// </summary>
    /// <returns>double array: [left, top]</returns>
    public async Task<double[]> GetScrollLeftAndTopAsync()
    {
        return await (await GetModuleAsync()).InvokeAsync<double[]>(GetScrollLeftAndTopMethod);
    }

}
