namespace BQuery;

[GenerateWindowEvents(typeof(WindowEvent))]
/// <summary>
/// Window events for the current DI scope.
/// </summary>
public partial class BqEvents
{
    private abstract class EventSlot
    {
    }

    private sealed class EventSlot<T>
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

    private sealed class EventSlot<T1, T2>
        : EventSlot
    {
        private Action<T1, T2>? _syncHandlers;
        private Func<T1, T2, Task>? _asyncHandlers;

        public event Action<T1, T2>? Sync
        {
            add => _syncHandlers += value;
            remove => _syncHandlers -= value;
        }

        public event Func<T1, T2, Task>? Async
        {
            add => _asyncHandlers += value;
            remove => _asyncHandlers -= value;
        }

        public async Task InvokeAsync(T1 arg1, T2 arg2)
        {
            if (_syncHandlers is not null)
            {
                foreach (var handler in _syncHandlers.GetInvocationList().Cast<Action<T1, T2>>())
                {
                    handler(arg1, arg2);
                }
            }

            if (_asyncHandlers is not null)
            {
                List<Task>? pendingTasks = [];

                foreach (var handler in _asyncHandlers.GetInvocationList().Cast<Func<T1, T2, Task>>())
                {
                    pendingTasks.Add(InvokeAsyncHandler(handler, arg1, arg2));
                }

                await Task.WhenAll(pendingTasks);
            }
        }

        private static async Task InvokeAsyncHandler(Func<T1, T2, Task> handler, T1 arg1, T2 arg2)
        {
            await handler(arg1, arg2);
        }
    }
}
