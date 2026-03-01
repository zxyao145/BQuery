namespace BQuery;

public class BqObject:IAsyncDisposable
{
    private Lazy<Task<IJSObjectReference>> moduleTask;
    private IJSObjectReference? _module;
    public BqObject(IJSRuntime jsRuntime, BqViewport viewport)
    {
        moduleTask = new(() => jsRuntime.InvokeAsync<IJSObjectReference>(
           "import", JsModuleConstants.MJS).AsTask());

        Viewport = viewport;
    }


    private async ValueTask<IJSObjectReference> GetModuleAsync()
    {
        if(_module != null)
        {
            return _module;
        }
        var v = await moduleTask.Value;
        _module = await v.InvokeAsync<IJSObjectReference>("getBq");
        return _module;
    }


    /// <summary>
    /// Viewport operation
    /// </summary>
    public BqViewport Viewport { get; private set; }

    public async Task addWindowEventsListener(params WindowEvent[] events)
    {
        if (events.Length == 0)
        {
            return;
        }
        await (await GetModuleAsync())
            .InvokeVoidAsync(JsModuleConstants.AddWindowEventsListener, events);
    }

    /// <summary>
    /// get browser useragent
    /// </summary>
    /// <returns></returns>
    public async Task<string> GetUserAgentAsync()
    {
        return await (await GetModuleAsync())
            .InvokeAsync<string>(JsModuleConstants.GetUserAgent);
    }


    #region drag

    /// <summary>
    /// bind drag for <param name="element">element</param> 
    /// </summary>
    /// <param name="element"></param>
    /// <param name="options"></param>
    /// <returns></returns>
    public async Task BindDragAsync(ElementReference element, DragOptions? options = null)
    {
        var method = JsModuleConstants.GetMethod(
            JsModuleConstants.ModuleName,
            JsModuleConstants.Drag.ModuleName,
            JsModuleConstants.Drag.BindDrag
            );
        await (await GetModuleAsync())
            .InvokeVoidAsync(method, element, options);
    }

    public async ValueTask DisposeAsync()
    {
        if(_module != null) 
        {
            await _module.DisposeAsync();
        }
    }
    #endregion


}
