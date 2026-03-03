namespace BQuery;

public class BqObject : IAsyncDisposable
{
    private readonly DotNetObjectReference<BqEvents> _eventsReference;
    private readonly string _listenerId = Guid.NewGuid().ToString("N");
    private readonly HashSet<WindowEvent> _registeredEvents = [];
    private readonly IJSRuntime _jsRuntime;
    private bool _disposed;

    public BqObject(IJSRuntime jsRuntime, BqEvents events)
    {
        _jsRuntime = jsRuntime;
        Viewport = new BqViewport(jsRuntime);
        Events = events;
        _eventsReference = DotNetObjectReference.Create(events);
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

        await _jsRuntime.InvokeVoidAsync(
            JsModuleConstants.GetMethod(
                JsModuleConstants.ModuleName,
                JsModuleConstants.AddWindowEventsListener),
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

        await _jsRuntime.InvokeVoidAsync(
            JsModuleConstants.GetMethod(
                JsModuleConstants.ModuleName,
                JsModuleConstants.RemoveWindowEventsListener),
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
        return await _jsRuntime.InvokeAsync<string>(
            JsModuleConstants.GetMethod(
                JsModuleConstants.ModuleName,
                JsModuleConstants.GetUserAgent));
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

        if (_registeredEvents.Count > 0)
        {
            await _jsRuntime.InvokeVoidAsync(
                JsModuleConstants.GetMethod(
                    JsModuleConstants.ModuleName,
                    JsModuleConstants.RemoveWindowEventsListener),
                _registeredEvents.ToArray(),
                _listenerId,
                _eventsReference);
        }

        _registeredEvents.Clear();
        _eventsReference.Dispose();
    }

    #endregion
}
