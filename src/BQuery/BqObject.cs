using static BQuery.JsModuleConstants;

namespace BQuery;

public partial class BqObject : IAsyncDisposable
{
    private readonly DotNetObjectReference<BqEvents> _eventsReference;
    private readonly string _listenerId = Guid.NewGuid().ToString("N");
    private readonly HashSet<WindowEvent> _registeredEvents = [];
    private readonly Dictionary<WindowEvent, int> _registeredEventCounter = [];

    private readonly IJSRuntime _jsRuntime;
    private bool _disposed;

    public BqObject(IJSRuntime jsRuntime, BqEvents events)
    {
        this. _jsRuntime = jsRuntime;

        Viewport = new BqViewport(jsRuntime);
        Drag = new BqDrag(jsRuntime);
        WindowEvents = events;

        _eventsReference = DotNetObjectReference.Create(events);
    }

    #region modules

    /// <summary>
    /// Viewport operation
    /// </summary>
    public BqViewport Viewport { get; }

    public BqDrag Drag { get; }


    /// <summary>
    /// Window event hub for the current DI scope.
    /// </summary>
    public BqEvents WindowEvents { get; }


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
            WindowEventsConstants.AddWindowEventsListenerMethod,
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
            WindowEventsConstants.RemoveWindowEventsListenerMethod,
            eventList,
            _listenerId,
            _eventsReference);
    }

    public async Task AddWindowEventListener(WindowEvent windowEvent, Func<Object, Task> func)
    {
        if (!_registeredEventCounter.ContainsKey(windowEvent))
        {
            _registeredEventCounter[windowEvent] = 0;
        }
        _registeredEventCounter[windowEvent] += 1;


        if (_registeredEventCounter[windowEvent] == 1)
        {
            await _jsRuntime.InvokeVoidAsync(
                WindowEventsConstants.AddWindowEventListenerMethod,
                windowEvent,
                _listenerId,
                _eventsReference
                );
        }
    }

    public async Task RemoveWindowEventListener(WindowEvent windowEvent, Func<Object, Task> func)
    {
        if (!_registeredEventCounter.ContainsKey(windowEvent))
        {
            return;
        }
        _registeredEventCounter[windowEvent] -= 1;


        if (_registeredEventCounter[windowEvent] == 0)
        {
            await _jsRuntime.InvokeVoidAsync(
                WindowEventsConstants.RemoveWindowEventListenerMethod,
                windowEvent,
                _listenerId
            );
        }
    }


    #endregion


    /// <summary>
    /// get browser useragent
    /// </summary>
    public async Task<string> GetUserAgentAsync()
    {
        ObjectDisposedException.ThrowIf(_disposed, this);
        return await _jsRuntime.InvokeAsync<string>(NavigatorConstants.GetUserAgentMethod);
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

        try
        {
            if (_registeredEvents.Count > 0)
            {
                await _jsRuntime.InvokeVoidAsync(
                    WindowEventsConstants.RemoveWindowEventsListenerMethod,
                    _registeredEvents.ToArray(),
                    _listenerId,
                    _eventsReference);
            }
        }
        catch (Exception exception) when (CanIgnoreDisposeInteropException(exception))
        {
        }

        _registeredEvents.Clear();
        _eventsReference.Dispose();
    }

    private static bool CanIgnoreDisposeInteropException(Exception exception)
    {
        return exception is JSDisconnectedException
            or ObjectDisposedException
            or TaskCanceledException;
    }
}
