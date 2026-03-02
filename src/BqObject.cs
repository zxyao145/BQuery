namespace BQuery;

public class BqObject : IAsyncDisposable
{
    private Lazy<Task<IJSObjectReference>>? moduleTask;
    private IJSObjectReference? _module;
    private readonly DotNetObjectReference<BqEvents> _eventsReference;
    private readonly HashSet<WindowEvent> _registeredEvents = [];
    private readonly IJSRuntime _jsRuntime;
    private bool _disposed;

    public BqObject(IJSRuntime jsRuntime, BqViewport viewport, BqEvents events)
    {
        _jsRuntime = jsRuntime;
        moduleTask = new(() => jsRuntime.InvokeAsync<IJSObjectReference>(
           "import", JsModuleConstants.MJS).AsTask());

        Viewport = viewport;
        Events = events;
        _eventsReference = DotNetObjectReference.Create(events);
    }

    private async ValueTask<IJSObjectReference> GetModuleAsync()
    {
        ObjectDisposedException.ThrowIf(_disposed, this);

        if (_module != null)
        {
            return _module;
        }

        var importedModule = await moduleTask!.Value;
        _module = await importedModule.InvokeAsync<IJSObjectReference>("getBq");
        return _module;
    }

    /// <summary>
    /// Viewport operation
    /// </summary>
    public BqViewport Viewport { get; }

    /// <summary>
    /// Window event hub for the current DI scope.
    /// </summary>
    public BqEvents Events { get; }

    public async Task addWindowEventsListener(params WindowEvent[] events)
    {
        ObjectDisposedException.ThrowIf(_disposed, this);

        var eventList = events
            .Where(static e => !string.IsNullOrWhiteSpace(e.Name))
            .Distinct()
            .ToArray();

        if (eventList.Length == 0)
        {
            return;
        }

        foreach (var windowEvent in eventList)
        {
            _registeredEvents.Add(windowEvent);
        }

        await (await GetModuleAsync())
            .InvokeVoidAsync(
                JsModuleConstants.AddWindowEventsListener,
                eventList,
                _eventsReference);
    }

    public async Task RemoveWindowEventsListener(params WindowEvent[] events)
    {
        if (_disposed)
        {
            return;
        }

        var eventList = events
            .Where(static e => !string.IsNullOrWhiteSpace(e.Name))
            .Distinct()
            .ToArray();

        if (eventList.Length == 0)
        {
            return;
        }

        foreach (var windowEvent in eventList)
        {
            _registeredEvents.Remove(windowEvent);
        }

        await (await GetModuleAsync())
            .InvokeVoidAsync(
                JsModuleConstants.RemoveWindowEventsListener,
                eventList,
                _eventsReference);
    }

    /// <summary>
    /// get browser useragent
    /// </summary>
    public async Task<string> GetUserAgentAsync()
    {
        ObjectDisposedException.ThrowIf(_disposed, this);
        return await (await GetModuleAsync())
            .InvokeAsync<string>(JsModuleConstants.GetUserAgent);
    }

    #region drag

    /// <summary>
    /// bind drag for <param name="element">element</param> 
    /// </summary>
    public async Task BindDragAsync(ElementReference element, DragOptions? options = null)
    {
        ObjectDisposedException.ThrowIf(_disposed, this);

        var method = JsModuleConstants.GetMethod(
            JsModuleConstants.ModuleName,
            JsModuleConstants.Drag.ModuleName,
            JsModuleConstants.Drag.BindDrag
            );
        await _jsRuntime.InvokeVoidAsync(method, element, options);
    }

    /// <summary>
    /// Disposes the JavaScript module reference and unregisters scoped window listeners.
    /// </summary>
    public async ValueTask DisposeAsync()
    {
        if (_disposed)
        {
            return;
        }

        _disposed = true;

        if (_module != null && _registeredEvents.Count > 0)
        {
            await _module.InvokeVoidAsync(
                JsModuleConstants.RemoveWindowEventsListener,
                _registeredEvents.ToArray(),
                _eventsReference);
        }

        _registeredEvents.Clear();
        _eventsReference.Dispose();

        if (_module != null)
        {
            await _module.DisposeAsync();
            _module = null;
        }

        if (moduleTask != null && moduleTask.IsValueCreated)
        {
            var module = await moduleTask.Value;
            await module.DisposeAsync();
            moduleTask = null;
        }
    }

    #endregion
}
