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
}
