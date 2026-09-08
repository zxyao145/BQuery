using Microsoft.Extensions.Logging;
using System.Threading;

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
    private readonly SemaphoreSlim _listenerLock = new(1, 1);
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
        await _listenerLock.WaitAsync();
        try
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

            await _jsRuntime.InvokeVoidAsync(
                WindowEventsConstants.AddWindowEventsListenerMethod,
                eventList, _listenerId, _eventsReference);
            _registeredEvents.UnionWith(eventList);
        }
        finally
        {
            _listenerLock.Release();
        }
    }

    /// <summary>
    /// Removes selected listeners. With no arguments, clears all listeners and handlers in this scope.
    /// </summary>
    public async Task RemoveWindowEventListeners(params WindowEvent[] windowEvents)
    {
        await _listenerLock.WaitAsync();
        try
        {
            if (_disposed)
            {
                return;
            }
            if (windowEvents.Length == 0)
            {
                await _jsRuntime.InvokeVoidAsync(
                    WindowEventsConstants.DisposeWindowEventsListenerMethod, _listenerId);
                foreach (var eventSlot in this.EventSlots.Values)
                {
                    eventSlot.Clear();
                }
                _registeredEvents.Clear();
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

            await _jsRuntime.InvokeVoidAsync(
                WindowEventsConstants.RemoveWindowEventsListenerMethod, eventList, _listenerId);
            _registeredEvents.ExceptWith(eventList);
        }
        finally
        {
            _listenerLock.Release();
        }
    }

    public async Task AddWindowEventListener<T>(WindowEvent windowEvent, Func<T, Task> func)
    {
        await _listenerLock.WaitAsync();
        try
        {
            ObjectDisposedException.ThrowIf(_disposed, this);
            var eventSlot = (EventSlot<T>)this.EventSlots[windowEvent];
            eventSlot.Async += func;
            try
            {
                await _jsRuntime.InvokeVoidAsync(
                    WindowEventsConstants.AddWindowEventListenerMethod,
                    windowEvent, _listenerId, _eventsReference);
                _registeredEvents.Add(windowEvent);
            }
            catch
            {
                eventSlot.Async -= func;
                throw;
            }
        }
        finally
        {
            _listenerLock.Release();
        }
    }

    public async Task RemoveWindowEventListener<T>(WindowEvent windowEvent, Func<T, Task> func)
    {
        await _listenerLock.WaitAsync();
        try
        {
            if (_disposed)
            {
                return;
            }
            var eventSlot = (EventSlot<T>)this.EventSlots[windowEvent];
            int previousCount = eventSlot.Count;
            eventSlot.Async -= func;
            if (previousCount > 0 && eventSlot.Count == 0)
            {
                try
                {
                    await _jsRuntime.InvokeVoidAsync(
                        WindowEventsConstants.RemoveWindowEventListenerMethod,
                        windowEvent, _listenerId);
                    _registeredEvents.Remove(windowEvent);
                }
                catch
                {
                    eventSlot.Async += func;
                    throw;
                }
            }
        }
        finally
        {
            _listenerLock.Release();
        }
    }

    /// <summary>
    /// Unregisters scoped window listeners and releases the .NET reference and handlers.
    /// </summary>
    public async ValueTask DisposeAsync()
    {
        if (_disposed)
        {
            return;
        }
        _disposed = true;

        // Wait for in-flight registrations before releasing the callback reference.
        await _listenerLock.WaitAsync();
        try
        {
            if (_registeredEvents.Count > 0)
            {
                await _jsRuntime.InvokeVoidAsync(
                    WindowEventsConstants.DisposeWindowEventsListenerMethod, _listenerId);
            }
        }
        catch (Exception exception) when (CanIgnoreDisposeInteropException(exception))
        {
        }
        finally
        {
            _registeredEvents.Clear();
            foreach (var eventSlot in this.EventSlots.Values)
            {
                eventSlot.Clear();
            }
            _eventsReference.Dispose();
            _listenerLock.Release();
        }
    }


    internal abstract class EventSlot
    {
        public abstract int Count { get; }

        public abstract void Clear();
    }

    internal sealed class EventSlot<T>
        : EventSlot
    {
        private Action<T>? _syncHandlers;
        private Func<T, Task>? _asyncHandlers;


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
                    pendingTasks.Add(handler(args));
                }

                await Task.WhenAll(pendingTasks);
            }
        }


        public override int Count => (_syncHandlers?.GetInvocationList().Length ?? 0)
            + (_asyncHandlers?.GetInvocationList().Length ?? 0);

        public override void Clear()
        {
            _syncHandlers = null;
            _asyncHandlers = null;
        }
    }
}
