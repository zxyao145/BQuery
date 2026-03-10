using Microsoft.Extensions.Logging;

namespace BQuery;

[GenerateWindowEvents(typeof(WindowEvent))]
/// <summary>
/// Window events for the current DI scope.
/// </summary>
public partial class BqEvents : IAsyncDisposable
{

    private readonly IJSRuntime _jsRuntime;
    private readonly ILogger<BqEvents> _logger;
    private readonly DotNetObjectReference<BqEvents> _eventsReference;
    private readonly string _listenerId = Guid.NewGuid().ToString("N");
    private readonly HashSet<WindowEvent> _registeredEvents = [];
    private bool _disposed;


    public BqEvents(IJSRuntime jsRuntime, ILogger<BqEvents> logger)
    {
        _jsRuntime = jsRuntime;
        _logger = logger;
        _eventsReference = DotNetObjectReference.Create(this);
    }


    private static bool CanIgnoreDisposeInteropException(Exception exception)
    {
        return exception is JSDisconnectedException
            or ObjectDisposedException
            or TaskCanceledException;
    }



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
            _eventsReference
            );
    }


    public async Task RemoveWindowEventListeners(params WindowEvent[] windowEvents)
    {
        if (_disposed)
        {
            return;
        }

        if (windowEvents.Length == 0)
        {
            await _jsRuntime.InvokeVoidAsync(
                    WindowEventsConstants.DisposeWindowEventsListenerMethod,
                    _listenerId);
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
            _listenerId);
    }

    public async Task AddWindowEventListener<T>(WindowEvent windowEvent, Func<T, Task> func)
    {
        var eventSlot = this.EventSlots[windowEvent];
        ((EventSlot<T>)eventSlot).Async += func;

        if (eventSlot.Count == 1)
        {
            _registeredEvents.Add(windowEvent);
            await _jsRuntime.InvokeVoidAsync(
                WindowEventsConstants.AddWindowEventListenerMethod,
                windowEvent,
                _listenerId,
                _eventsReference
                );
        }
    }

    public async Task RemoveWindowEventListener<T>(WindowEvent windowEvent, Func<T, Task> func)
    {
        var eventSlot = this.EventSlots[windowEvent];
        ((EventSlot<T>)eventSlot).Async -= func;

        if (eventSlot.Count == 0)
        {
            _registeredEvents.Remove(windowEvent);
            await _jsRuntime.InvokeVoidAsync(
                WindowEventsConstants.RemoveWindowEventListenerMethod,
                windowEvent,
                _listenerId
            );
        }
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
                    WindowEventsConstants.DisposeWindowEventsListenerMethod,
                    _listenerId);
            }
        }
        catch (Exception exception) when (CanIgnoreDisposeInteropException(exception))
        {
        }

        _registeredEvents.Clear();
        _eventsReference.Dispose();
    }




    internal abstract class EventSlot
    {
        public abstract int Count { get; }
    }

    internal sealed class EventSlot<T>
        : EventSlot
    {
        private Action<T>? _syncHandlers;
        private Func<T, Task>? _asyncHandlers;

        public override int Count => (_syncHandlers?.GetInvocationList().Length ?? 0) +
            (_asyncHandlers?.GetInvocationList().Length ?? 0);

        public event Action<T>? Sync
        {
            add => _syncHandlers += value;
            remove => _syncHandlers -= value;
        }

        public event Func<T, Task>? Async
        {
            add => _asyncHandlers += value;
            remove => _asyncHandlers -= value;
        }

        public async Task InvokeAsync(T args)
        {
            if (_syncHandlers is not null)
            {
                foreach (var handler in _syncHandlers.GetInvocationList().Cast<Action<T>>())
                {
                    handler(args);
                }
            }

            if (_asyncHandlers is not null)
            {
                List<Task>? pendingTasks = [];

                foreach (var handler in _asyncHandlers.GetInvocationList().Cast<Func<T, Task>>())
                {
                    pendingTasks.Add(InvokeAsyncHandler(handler, args));
                }

                await Task.WhenAll(pendingTasks);
            }
        }

        private static async Task InvokeAsyncHandler(Func<T, Task> handler, T args)
        {
            await handler(args);
        }
    }
}
