namespace BQuery;

public class BqObject : IAsyncDisposable
{
    private Lazy<Task<IJSObjectReference>>? moduleTask;
    private IJSObjectReference? _module;
    private readonly DotNetObjectReference<BqEvents> _eventsReference;
    private readonly string _listenerId = Guid.NewGuid().ToString("N");
    private readonly HashSet<WindowEvent> _registeredEvents = [];
    private readonly IJSRuntime _jsRuntime;
    private bool _disposed;

    public BqObject(IJSRuntime jsRuntime, BqEvents events)
    {
        _jsRuntime = jsRuntime;
        moduleTask = new(() => jsRuntime.InvokeAsync<IJSObjectReference>(
           "import", JsModuleConstants.MJS).AsTask());

        Viewport = new BqViewport(GetModuleAsync);
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


    public async Task AddWindowEventListeners(params WindowEvent[] windowEvents)
    {
        ObjectDisposedException.ThrowIf(_disposed, this);

        var eventList = windowEvents
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
                _listenerId,
                _eventsReference);
    }


    public async Task RemoveWindowEventListeners(params WindowEvent[] windowEvents)
    {
        if (_disposed)
        {
            return;
        }

        var eventList = windowEvents
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
                _listenerId,
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
                _listenerId,
                _eventsReference);
        }

        _registeredEvents.Clear();

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

        _eventsReference.Dispose();
    }

    #endregion
}
