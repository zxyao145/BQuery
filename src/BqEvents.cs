namespace BQuery;

/// <summary>
/// Window events for the current DI scope.
/// </summary>
public class BqEvents
{
    #region window.onresize

    /// <summary>
    /// Current-scope <c>window.resize</c> event.
    /// The first parameter is the viewport width, and the second parameter is the viewport height.
    /// </summary>
    public event Action<double, double>? OnResize;

    /// <summary>
    /// Current-scope asynchronous <c>window.resize</c> event.
    /// The first parameter is the viewport width, and the second parameter is the viewport height.
    /// </summary>
    public event Func<double, double, Task>? OnResizeAsync;

    /// <summary>
    /// Invoked from JavaScript when the current scope receives a <c>window.resize</c> event.
    /// This method is intended for JS interop and should not normally be called directly.
    /// </summary>
    [JSInvokable]
    public async Task WindowResize(double width, double height)
    {
        OnResize?.Invoke(width, height);
        if (OnResizeAsync != null)
        {
            await OnResizeAsync(width, height);
        }
    }

    #endregion

    #region window.onscroll

    /// <summary>
    /// Current-scope <c>window.scroll</c> event.
    /// </summary>
    public event Action<EventArgs>? OnScroll;

    /// <summary>
    /// Current-scope asynchronous <c>window.scroll</c> event.
    /// </summary>
    public event Func<EventArgs, Task>? OnScrollAsync;

    /// <summary>
    /// Invoked from JavaScript when the current scope receives a <c>window.scroll</c> event.
    /// This method is intended for JS interop and should not normally be called directly.
    /// </summary>
    [JSInvokable]
    public async Task WindowScroll(EventArgs e)
    {
        OnScroll?.Invoke(e);
        if (OnScrollAsync != null)
        {
            await OnScrollAsync(e);
        }
    }

    #endregion

    #region OnMouseOver

    /// <summary>
    /// Current-scope <c>window.mouseover</c> event.
    /// </summary>
    public event Action<MouseEventArgs>? OnMouseOver;

    /// <summary>
    /// Current-scope asynchronous <c>window.mouseover</c> event.
    /// </summary>
    public event Func<MouseEventArgs, Task>? OnMouseOverAsync;

    /// <summary>
    /// Invoked from JavaScript when the current scope receives a <c>window.mouseover</c> event.
    /// This method is intended for JS interop and should not normally be called directly.
    /// </summary>
    [JSInvokable]
    public async Task WindowMouseOver(MouseEventArgs e)
    {
        OnMouseOver?.Invoke(e);
        if (OnMouseOverAsync != null)
        {
            await OnMouseOverAsync(e);
        }
    }

    #endregion

    #region OnMouseOut

    /// <summary>
    /// Current-scope <c>window.mouseout</c> event.
    /// </summary>
    public event Action<MouseEventArgs>? OnMouseOut;

    /// <summary>
    /// Current-scope asynchronous <c>window.mouseout</c> event.
    /// </summary>
    public event Func<MouseEventArgs, Task>? OnMouseOutAsync;

    /// <summary>
    /// Invoked from JavaScript when the current scope receives a <c>window.mouseout</c> event.
    /// This method is intended for JS interop and should not normally be called directly.
    /// </summary>
    [JSInvokable]
    public async Task WindowMouseOut(MouseEventArgs e)
    {
        OnMouseOut?.Invoke(e);
        if (OnMouseOutAsync != null)
        {
            await OnMouseOutAsync(e);
        }
    }

    #endregion

    #region OnContextMenu

    /// <summary>
    /// Current-scope <c>window.contextmenu</c> event.
    /// </summary>
    public event Action<MouseEventArgs>? OnContextMenu;

    /// <summary>
    /// Current-scope asynchronous <c>window.contextmenu</c> event.
    /// </summary>
    public event Func<MouseEventArgs, Task>? OnContextMenuAsync;

    /// <summary>
    /// Invoked from JavaScript when the current scope receives a <c>window.contextmenu</c> event.
    /// This method is intended for JS interop and should not normally be called directly.
    /// </summary>
    [JSInvokable]
    public async Task WindowContextMenu(MouseEventArgs e)
    {
        OnContextMenu?.Invoke(e);
        if (OnContextMenuAsync != null)
        {
            await OnContextMenuAsync(e);
        }
    }

    #endregion

    #region OnMouseDown

    /// <summary>
    /// Current-scope <c>window.mousedown</c> event.
    /// </summary>
    public event Action<MouseEventArgs>? OnMouseDown;

    /// <summary>
    /// Current-scope asynchronous <c>window.mousedown</c> event.
    /// </summary>
    public event Func<MouseEventArgs, Task>? OnMouseDownAsync;

    /// <summary>
    /// Invoked from JavaScript when the current scope receives a <c>window.mousedown</c> event.
    /// This method is intended for JS interop and should not normally be called directly.
    /// </summary>
    [JSInvokable]
    public async Task WindowMouseDown(MouseEventArgs e)
    {
        OnMouseDown?.Invoke(e);
        if (OnMouseDownAsync != null)
        {
            await OnMouseDownAsync(e);
        }
    }

    #endregion

    #region OnMouseUp

    /// <summary>
    /// Current-scope <c>window.mouseup</c> event.
    /// </summary>
    public event Action<MouseEventArgs>? OnMouseUp;

    /// <summary>
    /// Current-scope asynchronous <c>window.mouseup</c> event.
    /// </summary>
    public event Func<MouseEventArgs, Task>? OnMouseUpAsync;

    /// <summary>
    /// Invoked from JavaScript when the current scope receives a <c>window.mouseup</c> event.
    /// This method is intended for JS interop and should not normally be called directly.
    /// </summary>
    [JSInvokable]
    public async Task WindowMouseUp(MouseEventArgs e)
    {
        OnMouseUp?.Invoke(e);
        if (OnMouseUpAsync != null)
        {
            await OnMouseUpAsync(e);
        }
    }

    #endregion

    #region window.OnMouseMove

    /// <summary>
    /// Current-scope <c>window.mousemove</c> event.
    /// </summary>
    public event Action<MouseEventArgs>? OnMouseMove;

    /// <summary>
    /// Current-scope asynchronous <c>window.mousemove</c> event.
    /// </summary>
    public event Func<MouseEventArgs, Task>? OnMouseMoveAsync;

    /// <summary>
    /// Invoked from JavaScript when the current scope receives a <c>window.mousemove</c> event.
    /// This method is intended for JS interop and should not normally be called directly.
    /// </summary>
    [JSInvokable]
    public async Task WindowMouseMove(MouseEventArgs e)
    {
        OnMouseMove?.Invoke(e);
        if (OnMouseMoveAsync != null)
        {
            await OnMouseMoveAsync(e);
        }
    }

    #endregion

    #region OnDbClick

    /// <summary>
    /// Current-scope double-click event synthesized from window click events.
    /// </summary>
    public event Action<MouseEventArgs>? OnDbClick;

    /// <summary>
    /// Current-scope asynchronous double-click event synthesized from window click events.
    /// </summary>
    public event Func<MouseEventArgs, Task>? OnDbClickAsync;

    /// <summary>
    /// Invoked from JavaScript when the current scope receives a synthesized double-click event.
    /// This method is intended for JS interop and should not normally be called directly.
    /// </summary>
    [JSInvokable]
    public async Task WindowDbClick(MouseEventArgs e)
    {
        OnDbClick?.Invoke(e);
        if (OnDbClickAsync != null)
        {
            await OnDbClickAsync(e);
        }
    }

    #endregion

    #region OnClick

    /// <summary>
    /// Current-scope <c>window.click</c> event.
    /// </summary>
    public event Action<MouseEventArgs>? OnClick;

    /// <summary>
    /// Current-scope asynchronous <c>window.click</c> event.
    /// </summary>
    public event Func<MouseEventArgs, Task>? OnClickAsync;

    /// <summary>
    /// Invoked from JavaScript when the current scope receives a <c>window.click</c> event.
    /// This method is intended for JS interop and should not normally be called directly.
    /// </summary>
    [JSInvokable]
    public async Task WindowClick(MouseEventArgs e)
    {
        OnClick?.Invoke(e);
        if (OnClickAsync != null)
        {
            await OnClickAsync(e);
        }
    }

    #endregion

    #region OnClose

    /// <summary>
    /// Current-scope window close-like event forwarded from JavaScript.
    /// </summary>
    public event Action<EventArgs>? OnClose;

    /// <summary>
    /// Current-scope asynchronous window close-like event forwarded from JavaScript.
    /// </summary>
    public event Func<EventArgs, Task>? OnCloseAsync;

    /// <summary>
    /// Invoked from JavaScript when the current scope receives a close-like event.
    /// This method is intended for JS interop and should not normally be called directly.
    /// </summary>
    [JSInvokable]
    public async Task WindowClose(EventArgs e)
    {
        OnClose?.Invoke(e);
        if (OnCloseAsync != null)
        {
            await OnCloseAsync(e);
        }
    }

    #endregion

    #region OnFocus

    /// <summary>
    /// Current-scope <c>window.focus</c> event.
    /// </summary>
    public event Action<FocusEventArgs>? OnFocus;

    /// <summary>
    /// Current-scope asynchronous <c>window.focus</c> event.
    /// </summary>
    public event Func<FocusEventArgs, Task>? OnFocusAsync;

    /// <summary>
    /// Invoked from JavaScript when the current scope receives a <c>window.focus</c> event.
    /// This method is intended for JS interop and should not normally be called directly.
    /// </summary>
    [JSInvokable]
    public async Task WindowFocus(FocusEventArgs e)
    {
        OnFocus?.Invoke(e);
        if (OnFocusAsync != null)
        {
            await OnFocusAsync(e);
        }
    }

    #endregion

    #region OnBlur

    /// <summary>
    /// Current-scope <c>window.blur</c> event.
    /// </summary>
    public event Action<FocusEventArgs>? OnBlur;

    /// <summary>
    /// Current-scope asynchronous <c>window.blur</c> event.
    /// </summary>
    public event Func<FocusEventArgs, Task>? OnBlurAsync;

    /// <summary>
    /// Invoked from JavaScript when the current scope receives a <c>window.blur</c> event.
    /// This method is intended for JS interop and should not normally be called directly.
    /// </summary>
    [JSInvokable]
    public async Task WindowBlur(FocusEventArgs e)
    {
        OnBlur?.Invoke(e);
        if (OnBlurAsync != null)
        {
            await OnBlurAsync(e);
        }
    }

    #endregion

    #region OnTouchStart

    /// <summary>
    /// Current-scope <c>window.touchstart</c> event.
    /// </summary>
    public event Action<TouchEventArgs>? OnTouchStart;

    /// <summary>
    /// Current-scope asynchronous <c>window.touchstart</c> event.
    /// </summary>
    public event Func<TouchEventArgs, Task>? OnTouchStartAsync;

    /// <summary>
    /// Invoked from JavaScript when the current scope receives a <c>window.touchstart</c> event.
    /// This method is intended for JS interop and should not normally be called directly.
    /// </summary>
    [JSInvokable]
    public async Task WindowTouchStart(TouchEventArgs e)
    {
        OnTouchStart?.Invoke(e);
        if (OnTouchStartAsync != null)
        {
            await OnTouchStartAsync(e);
        }
    }

    #endregion

    #region OnTouchMove

    /// <summary>
    /// Current-scope <c>window.touchmove</c> event.
    /// </summary>
    public event Action<TouchEventArgs>? OnTouchMove;

    /// <summary>
    /// Current-scope asynchronous <c>window.touchmove</c> event.
    /// </summary>
    public event Func<TouchEventArgs, Task>? OnTouchMoveAsync;

    /// <summary>
    /// Invoked from JavaScript when the current scope receives a <c>window.touchmove</c> event.
    /// This method is intended for JS interop and should not normally be called directly.
    /// </summary>
    [JSInvokable]
    public async Task WindowTouchMove(TouchEventArgs e)
    {
        OnTouchMove?.Invoke(e);
        if (OnTouchMoveAsync != null)
        {
            await OnTouchMoveAsync(e);
        }
    }

    #endregion

    #region OnTouchEnd

    /// <summary>
    /// Current-scope <c>window.touchend</c> event.
    /// </summary>
    public event Action<TouchEventArgs>? OnTouchEnd;

    /// <summary>
    /// Current-scope asynchronous <c>window.touchend</c> event.
    /// </summary>
    public event Func<TouchEventArgs, Task>? OnTouchEndAsync;

    /// <summary>
    /// Invoked from JavaScript when the current scope receives a <c>window.touchend</c> event.
    /// This method is intended for JS interop and should not normally be called directly.
    /// </summary>
    [JSInvokable]
    public async Task WindowTouchEnd(TouchEventArgs e)
    {
        OnTouchEnd?.Invoke(e);
        if (OnTouchEndAsync != null)
        {
            await OnTouchEndAsync(e);
        }
    }

    #endregion

    #region OnTouchCancel

    /// <summary>
    /// Current-scope <c>window.touchcancel</c> event.
    /// </summary>
    public event Action<TouchEventArgs>? OnTouchCancel;

    /// <summary>
    /// Current-scope asynchronous <c>window.touchcancel</c> event.
    /// </summary>
    public event Func<TouchEventArgs, Task>? OnTouchCancelAsync;

    /// <summary>
    /// Invoked from JavaScript when the current scope receives a <c>window.touchcancel</c> event.
    /// This method is intended for JS interop and should not normally be called directly.
    /// </summary>
    [JSInvokable]
    public async Task WindowTouchCancel(TouchEventArgs e)
    {
        OnTouchCancel?.Invoke(e);
        if (OnTouchCancelAsync != null)
        {
            await OnTouchCancelAsync(e);
        }
    }

    #endregion

    #region OnKeyDown

    /// <summary>
    /// Current-scope <c>window.keydown</c> event.
    /// </summary>
    public event Action<KeyboardEventArgs>? OnKeyDown;

    /// <summary>
    /// Current-scope asynchronous <c>window.keydown</c> event.
    /// </summary>
    public event Func<KeyboardEventArgs, Task>? OnKeyDownAsync;

    /// <summary>
    /// Invoked from JavaScript when the current scope receives a <c>window.keydown</c> event.
    /// This method is intended for JS interop and should not normally be called directly.
    /// </summary>
    [JSInvokable]
    public async Task WindowKeyDown(KeyboardEventArgs e)
    {
        OnKeyDown?.Invoke(e);
        if (OnKeyDownAsync != null)
        {
            await OnKeyDownAsync(e);
        }
    }

    #endregion

    #region OnKeyPress

    /// <summary>
    /// Current-scope <c>window.keypress</c> event.
    /// </summary>
    public event Action<KeyboardEventArgs>? OnKeyPress;

    /// <summary>
    /// Current-scope asynchronous <c>window.keypress</c> event.
    /// </summary>
    public event Func<KeyboardEventArgs, Task>? OnKeyPressAsync;

    /// <summary>
    /// Invoked from JavaScript when the current scope receives a <c>window.keypress</c> event.
    /// This method is intended for JS interop and should not normally be called directly.
    /// </summary>
    [JSInvokable]
    public async Task WindowKeyPress(KeyboardEventArgs e)
    {
        OnKeyPress?.Invoke(e);
        if (OnKeyPressAsync != null)
        {
            await OnKeyPressAsync(e);
        }
    }

    #endregion

    #region OnKeyUp

    /// <summary>
    /// Current-scope <c>window.keyup</c> event.
    /// </summary>
    public event Action<KeyboardEventArgs>? OnKeyUp;

    /// <summary>
    /// Current-scope asynchronous <c>window.keyup</c> event.
    /// </summary>
    public event Func<KeyboardEventArgs, Task>? OnKeyUpAsync;

    /// <summary>
    /// Invoked from JavaScript when the current scope receives a <c>window.keyup</c> event.
    /// This method is intended for JS interop and should not normally be called directly.
    /// </summary>
    [JSInvokable]
    public async Task WindowKeyUp(KeyboardEventArgs e)
    {
        OnKeyUp?.Invoke(e);
        if (OnKeyUpAsync != null)
        {
            await OnKeyUpAsync(e);
        }
    }

    #endregion
}
