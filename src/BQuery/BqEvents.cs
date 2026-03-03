namespace BQuery;

[GenerateWindowEvents(typeof(WindowEvent))]
/// <summary>
/// Window events for the current DI scope.
/// </summary>
public partial class BqEvents
{
    private static void ReportHandlerException(Exception exception)
    {
        Console.Error.WriteLine($"BQuery window event handler failed: {exception}");
    }

    private sealed class EventSlot<T>
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
            List<Task>? pendingTasks = null;

            if (_syncHandlers is not null)
            {
                foreach (var handler in _syncHandlers.GetInvocationList().Cast<Action<T>>())
                {
                    try
                    {
                        handler(args);
                    }
                    catch (Exception exception)
                    {
                        ReportHandlerException(exception);
                    }
                }
            }

            if (_asyncHandlers is not null)
            {
                pendingTasks = [];
                foreach (var handler in _asyncHandlers.GetInvocationList().Cast<Func<T, Task>>())
                {
                    pendingTasks.Add(InvokeAsyncHandler(handler, args));
                }
            }

            if (pendingTasks is not null)
            {
                await Task.WhenAll(pendingTasks);
            }
        }

        private static async Task InvokeAsyncHandler(Func<T, Task> handler, T args)
        {
            try
            {
                await handler(args);
            }
            catch (Exception exception)
            {
                ReportHandlerException(exception);
            }
        }
    }

    private sealed class EventSlot<T1, T2>
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
            List<Task>? pendingTasks = null;

            if (_syncHandlers is not null)
            {
                foreach (var handler in _syncHandlers.GetInvocationList().Cast<Action<T1, T2>>())
                {
                    try
                    {
                        handler(arg1, arg2);
                    }
                    catch (Exception exception)
                    {
                        ReportHandlerException(exception);
                    }
                }
            }

            if (_asyncHandlers is not null)
            {
                pendingTasks = [];
                foreach (var handler in _asyncHandlers.GetInvocationList().Cast<Func<T1, T2, Task>>())
                {
                    pendingTasks.Add(InvokeAsyncHandler(handler, arg1, arg2));
                }
            }

            if (pendingTasks is not null)
            {
                await Task.WhenAll(pendingTasks);
            }
        }

        private static async Task InvokeAsyncHandler(Func<T1, T2, Task> handler, T1 arg1, T2 arg2)
        {
            try
            {
                await handler(arg1, arg2);
            }
            catch (Exception exception)
            {
                ReportHandlerException(exception);
            }
        }
    }
}
