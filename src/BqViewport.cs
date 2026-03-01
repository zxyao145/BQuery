namespace BQuery;

public class BqViewport
{
    private readonly IJSRuntime _js;


    private Lazy<Task<IJSObjectReference>> moduleTask;
    private IJSObjectReference _module;

    public BqViewport(IJSRuntime jsRuntime)
    {
        this._js = jsRuntime;
        moduleTask = new(() => jsRuntime.InvokeAsync<IJSObjectReference>(
           "import", JsModuleConstants.MJS).AsTask());
    }

    private async ValueTask<IJSObjectReference> GetModuleAsync()
    {
        if (_module != null)
        {
            return _module;
        }
        var v = await moduleTask.Value;
        _module = await v.InvokeAsync<IJSObjectReference>("getViewport");
        return _module;
    }


    #region width height

    /// <summary>
    /// get viewport width
    /// </summary>
    /// <returns></returns>
    public async Task<double> GetWidthAsync()
    {
        return await (await GetModuleAsync()).InvokeAsync<double>(JsModuleConstants.Viewport.GetWidth);
    }

    /// <summary>
    /// get viewport height
    /// </summary>
    /// <returns></returns>
    public async Task<double> GetHeightAsync()
    {
        return await (await GetModuleAsync()).InvokeAsync<double>(JsModuleConstants.Viewport.GetHeight);
    }

    /// <summary>
    /// get viewport width and height
    /// </summary>
    /// <returns>double array: [width,height]</returns>
    public async Task<double[]> GetWidthAndHeightAsync()
    {
        return await (await GetModuleAsync()).InvokeAsync<double[]>(JsModuleConstants.Viewport.GetWidthAndHeight);
    }

    #endregion

    #region Scroll

    /// <summary>
    /// get viewport scroll width
    /// </summary>
    /// <returns></returns>
    public async Task<double> GetScrollWidthAsync()
    {
        return await (await GetModuleAsync()).InvokeAsync<double>(JsModuleConstants.Viewport.GetScrollWidth);
    }

    /// <summary>
    /// get viewport scroll height
    /// </summary>
    /// <returns></returns>
    public async Task<double> GetScrollHeightAsync()
    {
        return await (await GetModuleAsync()).InvokeAsync<double>(JsModuleConstants.Viewport.GetScrollHeight);
    }

    /// <summary>
    /// get viewport scroll width and height
    /// </summary>
    /// <returns>double array: [width,height]</returns>
    public async Task<double[]> GetScrollWidthAndHeightAsync()
    {
        return await (await GetModuleAsync()).InvokeAsync<double[]>(JsModuleConstants.Viewport.GetScrollWidthAndHeight);
    }

    #endregion

    /// <summary>
    /// get viewport scroll left
    /// </summary>
    /// <returns></returns>
    public async Task<double> GetScrollLeftAsync()
    {
        return await (await GetModuleAsync()).InvokeAsync<double>(JsModuleConstants.Viewport.GetScrollLeft);
    }

    /// <summary>
    /// get viewport scroll top
    /// </summary>
    /// <returns></returns>
    public async Task<double> GetScrollTopAsync()
    {
        return await (await GetModuleAsync()).InvokeAsync<double>(JsModuleConstants.Viewport.GetScrollTop);
    }

    /// <summary>
    /// get viewport scroll left and top
    /// </summary>
    /// <returns>double array: [left, top]</returns>
    public async Task<double[]> GetScrollLeftAndTopAsync()
    {
        return await (await GetModuleAsync()).InvokeAsync<double[]>(JsModuleConstants.Viewport.GetScrollLeftAndTop);
    }
}
