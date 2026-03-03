namespace BQuery;

/// <summary>
/// Window events for the current DI scope.
/// </summary>
public class BqEvents
{
    private static void ReportHandlerException(Exception exception)
    {
        Console.Error.WriteLine($"BQuery window event handler failed: {exception}");
    }

    // Reusable slot type so new window events only need a backing field plus the public declarations.
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

    private readonly EventSlot<double, double> _resize = new();
    private readonly EventSlot<EventArgs> _scroll = new();
    private readonly EventSlot<MouseEventArgs> _mouseOver = new();
    private readonly EventSlot<MouseEventArgs> _mouseOut = new();
    private readonly EventSlot<MouseEventArgs> _contextMenu = new();
    private readonly EventSlot<MouseEventArgs> _mouseDown = new();
    private readonly EventSlot<MouseEventArgs> _mouseUp = new();
    private readonly EventSlot<MouseEventArgs> _mouseMove = new();
    private readonly EventSlot<MouseEventArgs> _dbClick = new();
    private readonly EventSlot<MouseEventArgs> _click = new();
    private readonly EventSlot<EventArgs> _close = new();
    private readonly EventSlot<FocusEventArgs> _focus = new();
    private readonly EventSlot<FocusEventArgs> _blur = new();
    private readonly EventSlot<TouchEventArgs> _touchStart = new();
    private readonly EventSlot<TouchEventArgs> _touchMove = new();
    private readonly EventSlot<TouchEventArgs> _touchEnd = new();
    private readonly EventSlot<TouchEventArgs> _touchCancel = new();
    private readonly EventSlot<KeyboardEventArgs> _keyDown = new();
    private readonly EventSlot<KeyboardEventArgs> _keyPress = new();
    private readonly EventSlot<KeyboardEventArgs> _keyUp = new();

    #region window.onresize

    /// <summary>
    /// Current-scope <c>window.resize</c> event.
    /// The first parameter is the viewport width, and the second parameter is the viewport height.
    /// </summary>
    public event Action<double, double>? OnResize
    {
        add => _resize.Sync += value;
        remove => _resize.Sync -= value;
    }

    /// <summary>
    /// Current-scope asynchronous <c>window.resize</c> event.
    /// The first parameter is the viewport width, and the second parameter is the viewport height.
    /// </summary>
    public event Func<double, double, Task>? OnResizeAsync
    {
        add => _resize.Async += value;
        remove => _resize.Async -= value;
    }

    /// <summary>
    /// Invoked from JavaScript when the current scope receives a <c>window.resize</c> event.
    /// This method is intended for JS interop and should not normally be called directly.
    /// </summary>
    [JSInvokable]
    public Task WindowResize(double width, double height) => _resize.InvokeAsync(width, height);

    #endregion

    #region window.onscroll

    /// <summary>
    /// Current-scope <c>window.scroll</c> event.
    /// </summary>
    public event Action<EventArgs>? OnScroll
    {
        add => _scroll.Sync += value;
        remove => _scroll.Sync -= value;
    }

    /// <summary>
    /// Current-scope asynchronous <c>window.scroll</c> event.
    /// </summary>
    public event Func<EventArgs, Task>? OnScrollAsync
    {
        add => _scroll.Async += value;
        remove => _scroll.Async -= value;
    }

    /// <summary>
    /// Invoked from JavaScript when the current scope receives a <c>window.scroll</c> event.
    /// This method is intended for JS interop and should not normally be called directly.
    /// </summary>
    [JSInvokable]
    public Task WindowScroll(EventArgs e) => _scroll.InvokeAsync(e);

    #endregion

    #region OnMouseOver

    /// <summary>
    /// Current-scope <c>window.mouseover</c> event.
    /// </summary>
    public event Action<MouseEventArgs>? OnMouseOver
    {
        add => _mouseOver.Sync += value;
        remove => _mouseOver.Sync -= value;
    }

    /// <summary>
    /// Current-scope asynchronous <c>window.mouseover</c> event.
    /// </summary>
    public event Func<MouseEventArgs, Task>? OnMouseOverAsync
    {
        add => _mouseOver.Async += value;
        remove => _mouseOver.Async -= value;
    }

    /// <summary>
    /// Invoked from JavaScript when the current scope receives a <c>window.mouseover</c> event.
    /// This method is intended for JS interop and should not normally be called directly.
    /// </summary>
    [JSInvokable]
    public Task WindowMouseOver(MouseEventArgs e) => _mouseOver.InvokeAsync(e);

    #endregion

    #region OnMouseOut

    /// <summary>
    /// Current-scope <c>window.mouseout</c> event.
    /// </summary>
    public event Action<MouseEventArgs>? OnMouseOut
    {
        add => _mouseOut.Sync += value;
        remove => _mouseOut.Sync -= value;
    }

    /// <summary>
    /// Current-scope asynchronous <c>window.mouseout</c> event.
    /// </summary>
    public event Func<MouseEventArgs, Task>? OnMouseOutAsync
    {
        add => _mouseOut.Async += value;
        remove => _mouseOut.Async -= value;
    }

    /// <summary>
    /// Invoked from JavaScript when the current scope receives a <c>window.mouseout</c> event.
    /// This method is intended for JS interop and should not normally be called directly.
    /// </summary>
    [JSInvokable]
    public Task WindowMouseOut(MouseEventArgs e) => _mouseOut.InvokeAsync(e);

    #endregion

    #region OnContextMenu

    /// <summary>
    /// Current-scope <c>window.contextmenu</c> event.
    /// </summary>
    public event Action<MouseEventArgs>? OnContextMenu
    {
        add => _contextMenu.Sync += value;
        remove => _contextMenu.Sync -= value;
    }

    /// <summary>
    /// Current-scope asynchronous <c>window.contextmenu</c> event.
    /// </summary>
    public event Func<MouseEventArgs, Task>? OnContextMenuAsync
    {
        add => _contextMenu.Async += value;
        remove => _contextMenu.Async -= value;
    }

    /// <summary>
    /// Invoked from JavaScript when the current scope receives a <c>window.contextmenu</c> event.
    /// This method is intended for JS interop and should not normally be called directly.
    /// </summary>
    [JSInvokable]
    public Task WindowContextMenu(MouseEventArgs e) => _contextMenu.InvokeAsync(e);

    #endregion

    #region OnMouseDown

    /// <summary>
    /// Current-scope <c>window.mousedown</c> event.
    /// </summary>
    public event Action<MouseEventArgs>? OnMouseDown
    {
        add => _mouseDown.Sync += value;
        remove => _mouseDown.Sync -= value;
    }

    /// <summary>
    /// Current-scope asynchronous <c>window.mousedown</c> event.
    /// </summary>
    public event Func<MouseEventArgs, Task>? OnMouseDownAsync
    {
        add => _mouseDown.Async += value;
        remove => _mouseDown.Async -= value;
    }

    /// <summary>
    /// Invoked from JavaScript when the current scope receives a <c>window.mousedown</c> event.
    /// This method is intended for JS interop and should not normally be called directly.
    /// </summary>
    [JSInvokable]
    public Task WindowMouseDown(MouseEventArgs e) => _mouseDown.InvokeAsync(e);

    #endregion

    #region OnMouseUp

    /// <summary>
    /// Current-scope <c>window.mouseup</c> event.
    /// </summary>
    public event Action<MouseEventArgs>? OnMouseUp
    {
        add => _mouseUp.Sync += value;
        remove => _mouseUp.Sync -= value;
    }

    /// <summary>
    /// Current-scope asynchronous <c>window.mouseup</c> event.
    /// </summary>
    public event Func<MouseEventArgs, Task>? OnMouseUpAsync
    {
        add => _mouseUp.Async += value;
        remove => _mouseUp.Async -= value;
    }

    /// <summary>
    /// Invoked from JavaScript when the current scope receives a <c>window.mouseup</c> event.
    /// This method is intended for JS interop and should not normally be called directly.
    /// </summary>
    [JSInvokable]
    public Task WindowMouseUp(MouseEventArgs e) => _mouseUp.InvokeAsync(e);

    #endregion

    #region window.OnMouseMove

    /// <summary>
    /// Current-scope <c>window.mousemove</c> event.
    /// </summary>
    public event Action<MouseEventArgs>? OnMouseMove
    {
        add => _mouseMove.Sync += value;
        remove => _mouseMove.Sync -= value;
    }

    /// <summary>
    /// Current-scope asynchronous <c>window.mousemove</c> event.
    /// </summary>
    public event Func<MouseEventArgs, Task>? OnMouseMoveAsync
    {
        add => _mouseMove.Async += value;
        remove => _mouseMove.Async -= value;
    }

    /// <summary>
    /// Invoked from JavaScript when the current scope receives a <c>window.mousemove</c> event.
    /// This method is intended for JS interop and should not normally be called directly.
    /// </summary>
    [JSInvokable]
    public Task WindowMouseMove(MouseEventArgs e) => _mouseMove.InvokeAsync(e);

    #endregion

    #region OnDbClick

    /// <summary>
    /// Current-scope double-click event synthesized from window click events.
    /// </summary>
    public event Action<MouseEventArgs>? OnDbClick
    {
        add => _dbClick.Sync += value;
        remove => _dbClick.Sync -= value;
    }

    /// <summary>
    /// Current-scope asynchronous double-click event synthesized from window click events.
    /// </summary>
    public event Func<MouseEventArgs, Task>? OnDbClickAsync
    {
        add => _dbClick.Async += value;
        remove => _dbClick.Async -= value;
    }

    /// <summary>
    /// Invoked from JavaScript when the current scope receives a synthesized double-click event.
    /// This method is intended for JS interop and should not normally be called directly.
    /// </summary>
    [JSInvokable]
    public Task WindowDbClick(MouseEventArgs e) => _dbClick.InvokeAsync(e);

    #endregion

    #region OnClick

    /// <summary>
    /// Current-scope <c>window.click</c> event.
    /// </summary>
    public event Action<MouseEventArgs>? OnClick
    {
        add => _click.Sync += value;
        remove => _click.Sync -= value;
    }

    /// <summary>
    /// Current-scope asynchronous <c>window.click</c> event.
    /// </summary>
    public event Func<MouseEventArgs, Task>? OnClickAsync
    {
        add => _click.Async += value;
        remove => _click.Async -= value;
    }

    /// <summary>
    /// Invoked from JavaScript when the current scope receives a <c>window.click</c> event.
    /// This method is intended for JS interop and should not normally be called directly.
    /// </summary>
    [JSInvokable]
    public Task WindowClick(MouseEventArgs e) => _click.InvokeAsync(e);

    #endregion

    #region OnClose

    /// <summary>
    /// Current-scope close-like notification forwarded from <c>window.beforeunload</c> or <c>window.pagehide</c>.
    /// </summary>
    public event Action<EventArgs>? OnClose
    {
        add => _close.Sync += value;
        remove => _close.Sync -= value;
    }

    /// <summary>
    /// Current-scope asynchronous close-like notification forwarded from <c>window.beforeunload</c> or <c>window.pagehide</c>.
    /// </summary>
    public event Func<EventArgs, Task>? OnCloseAsync
    {
        add => _close.Async += value;
        remove => _close.Async -= value;
    }

    /// <summary>
    /// Invoked from JavaScript when the current scope receives a close-like page lifecycle event.
    /// This method is intended for JS interop and should not normally be called directly.
    /// </summary>
    [JSInvokable]
    public Task WindowClose(EventArgs e) => _close.InvokeAsync(e);

    #endregion

    #region OnFocus

    /// <summary>
    /// Current-scope <c>window.focus</c> event.
    /// </summary>
    public event Action<FocusEventArgs>? OnFocus
    {
        add => _focus.Sync += value;
        remove => _focus.Sync -= value;
    }

    /// <summary>
    /// Current-scope asynchronous <c>window.focus</c> event.
    /// </summary>
    public event Func<FocusEventArgs, Task>? OnFocusAsync
    {
        add => _focus.Async += value;
        remove => _focus.Async -= value;
    }

    /// <summary>
    /// Invoked from JavaScript when the current scope receives a <c>window.focus</c> event.
    /// This method is intended for JS interop and should not normally be called directly.
    /// </summary>
    [JSInvokable]
    public Task WindowFocus(FocusEventArgs e) => _focus.InvokeAsync(e);

    #endregion

    #region OnBlur

    /// <summary>
    /// Current-scope <c>window.blur</c> event.
    /// </summary>
    public event Action<FocusEventArgs>? OnBlur
    {
        add => _blur.Sync += value;
        remove => _blur.Sync -= value;
    }

    /// <summary>
    /// Current-scope asynchronous <c>window.blur</c> event.
    /// </summary>
    public event Func<FocusEventArgs, Task>? OnBlurAsync
    {
        add => _blur.Async += value;
        remove => _blur.Async -= value;
    }

    /// <summary>
    /// Invoked from JavaScript when the current scope receives a <c>window.blur</c> event.
    /// This method is intended for JS interop and should not normally be called directly.
    /// </summary>
    [JSInvokable]
    public Task WindowBlur(FocusEventArgs e) => _blur.InvokeAsync(e);

    #endregion

    #region OnTouchStart

    /// <summary>
    /// Current-scope <c>window.touchstart</c> event.
    /// </summary>
    public event Action<TouchEventArgs>? OnTouchStart
    {
        add => _touchStart.Sync += value;
        remove => _touchStart.Sync -= value;
    }

    /// <summary>
    /// Current-scope asynchronous <c>window.touchstart</c> event.
    /// </summary>
    public event Func<TouchEventArgs, Task>? OnTouchStartAsync
    {
        add => _touchStart.Async += value;
        remove => _touchStart.Async -= value;
    }

    /// <summary>
    /// Invoked from JavaScript when the current scope receives a <c>window.touchstart</c> event.
    /// This method is intended for JS interop and should not normally be called directly.
    /// </summary>
    [JSInvokable]
    public Task WindowTouchStart(TouchEventArgs e) => _touchStart.InvokeAsync(e);

    #endregion

    #region OnTouchMove

    /// <summary>
    /// Current-scope <c>window.touchmove</c> event.
    /// </summary>
    public event Action<TouchEventArgs>? OnTouchMove
    {
        add => _touchMove.Sync += value;
        remove => _touchMove.Sync -= value;
    }

    /// <summary>
    /// Current-scope asynchronous <c>window.touchmove</c> event.
    /// </summary>
    public event Func<TouchEventArgs, Task>? OnTouchMoveAsync
    {
        add => _touchMove.Async += value;
        remove => _touchMove.Async -= value;
    }

    /// <summary>
    /// Invoked from JavaScript when the current scope receives a <c>window.touchmove</c> event.
    /// This method is intended for JS interop and should not normally be called directly.
    /// </summary>
    [JSInvokable]
    public Task WindowTouchMove(TouchEventArgs e) => _touchMove.InvokeAsync(e);

    #endregion

    #region OnTouchEnd

    /// <summary>
    /// Current-scope <c>window.touchend</c> event.
    /// </summary>
    public event Action<TouchEventArgs>? OnTouchEnd
    {
        add => _touchEnd.Sync += value;
        remove => _touchEnd.Sync -= value;
    }

    /// <summary>
    /// Current-scope asynchronous <c>window.touchend</c> event.
    /// </summary>
    public event Func<TouchEventArgs, Task>? OnTouchEndAsync
    {
        add => _touchEnd.Async += value;
        remove => _touchEnd.Async -= value;
    }

    /// <summary>
    /// Invoked from JavaScript when the current scope receives a <c>window.touchend</c> event.
    /// This method is intended for JS interop and should not normally be called directly.
    /// </summary>
    [JSInvokable]
    public Task WindowTouchEnd(TouchEventArgs e) => _touchEnd.InvokeAsync(e);

    #endregion

    #region OnTouchCancel

    /// <summary>
    /// Current-scope <c>window.touchcancel</c> event.
    /// </summary>
    public event Action<TouchEventArgs>? OnTouchCancel
    {
        add => _touchCancel.Sync += value;
        remove => _touchCancel.Sync -= value;
    }

    /// <summary>
    /// Current-scope asynchronous <c>window.touchcancel</c> event.
    /// </summary>
    public event Func<TouchEventArgs, Task>? OnTouchCancelAsync
    {
        add => _touchCancel.Async += value;
        remove => _touchCancel.Async -= value;
    }

    /// <summary>
    /// Invoked from JavaScript when the current scope receives a <c>window.touchcancel</c> event.
    /// This method is intended for JS interop and should not normally be called directly.
    /// </summary>
    [JSInvokable]
    public Task WindowTouchCancel(TouchEventArgs e) => _touchCancel.InvokeAsync(e);

    #endregion

    #region OnKeyDown

    /// <summary>
    /// Current-scope <c>window.keydown</c> event.
    /// </summary>
    public event Action<KeyboardEventArgs>? OnKeyDown
    {
        add => _keyDown.Sync += value;
        remove => _keyDown.Sync -= value;
    }

    /// <summary>
    /// Current-scope asynchronous <c>window.keydown</c> event.
    /// </summary>
    public event Func<KeyboardEventArgs, Task>? OnKeyDownAsync
    {
        add => _keyDown.Async += value;
        remove => _keyDown.Async -= value;
    }

    /// <summary>
    /// Invoked from JavaScript when the current scope receives a <c>window.keydown</c> event.
    /// This method is intended for JS interop and should not normally be called directly.
    /// </summary>
    [JSInvokable]
    public Task WindowKeyDown(KeyboardEventArgs e) => _keyDown.InvokeAsync(e);

    #endregion

    #region OnKeyPress

    /// <summary>
    /// Current-scope <c>window.keypress</c> event.
    /// </summary>
    public event Action<KeyboardEventArgs>? OnKeyPress
    {
        add => _keyPress.Sync += value;
        remove => _keyPress.Sync -= value;
    }

    /// <summary>
    /// Current-scope asynchronous <c>window.keypress</c> event.
    /// </summary>
    public event Func<KeyboardEventArgs, Task>? OnKeyPressAsync
    {
        add => _keyPress.Async += value;
        remove => _keyPress.Async -= value;
    }

    /// <summary>
    /// Invoked from JavaScript when the current scope receives a <c>window.keypress</c> event.
    /// This method is intended for JS interop and should not normally be called directly.
    /// </summary>
    [JSInvokable]
    public Task WindowKeyPress(KeyboardEventArgs e) => _keyPress.InvokeAsync(e);

    #endregion

    #region OnKeyUp

    /// <summary>
    /// Current-scope <c>window.keyup</c> event.
    /// </summary>
    public event Action<KeyboardEventArgs>? OnKeyUp
    {
        add => _keyUp.Sync += value;
        remove => _keyUp.Sync -= value;
    }

    /// <summary>
    /// Current-scope asynchronous <c>window.keyup</c> event.
    /// </summary>
    public event Func<KeyboardEventArgs, Task>? OnKeyUpAsync
    {
        add => _keyUp.Async += value;
        remove => _keyUp.Async -= value;
    }

    /// <summary>
    /// Invoked from JavaScript when the current scope receives a <c>window.keyup</c> event.
    /// This method is intended for JS interop and should not normally be called directly.
    /// </summary>
    [JSInvokable]
    public Task WindowKeyUp(KeyboardEventArgs e) => _keyUp.InvokeAsync(e);

    #endregion
}
