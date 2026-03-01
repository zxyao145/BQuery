namespace BQuery;

/// <summary>
/// window events
/// </summary>
public class BqEvents
{
    public BqEvents()
    {

    }
    
    #region window.onresize

    /// <summary>
    /// window.onresize event
    /// first parameter is width,
    /// second first parameter is height
    /// </summary>
    public event Action<double, double>? OnResize;
    
    /// <summary>
    /// async window.onresize event
    /// first parameter is width,
    /// second first parameter is height, 
    /// </summary>
    public event Func<double, double, Task>? OnResizeAsync;

    internal async Task InvokeOnResize(double width, double height)
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
    /// window.onscroll event
    /// </summary>
    public event Action<EventArgs>? OnScroll;
    
    /// <summary>
    /// async window.onscroll event
    /// </summary>
    public event Func<EventArgs, Task>? OnScrollAsync;
    
    internal async Task InvokeOnScroll(EventArgs e)
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
    /// window event
    /// </summary>
    public event Action<MouseEventArgs>? OnMouseOver;

    /// <summary>
    /// async window event
    /// </summary>
    public event Func<MouseEventArgs, Task>? OnMouseOverAsync;

    internal async Task InvokeOnMouseOver(MouseEventArgs e)
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
    /// window event
    /// </summary>
    public event Action<MouseEventArgs>? OnMouseOut;

    /// <summary>
    /// async window event
    /// </summary>
    public event Func<MouseEventArgs, Task>? OnMouseOutAsync;

    internal async Task InvokeOnMouseOut(MouseEventArgs e)
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
    /// window event
    /// </summary>
    public event Action<MouseEventArgs>? OnContextMenu;

    /// <summary>
    /// async window event
    /// </summary>
    public event Func<MouseEventArgs, Task>? OnContextMenuAsync;

    internal async Task InvokeOnContextMenu(MouseEventArgs e)
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
    /// window event
    /// </summary>
    public event Action<MouseEventArgs>? OnMouseDown;

    /// <summary>
    /// async window event
    /// </summary>
    public event Func<MouseEventArgs, Task>? OnMouseDownAsync;

    internal async Task InvokeOnMouseDown(MouseEventArgs e)
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
    /// window event
    /// </summary>
    public event Action<MouseEventArgs>? OnMouseUp;

    /// <summary>
    /// async window event
    /// </summary>
    public event Func<MouseEventArgs, Task>? OnMouseUpAsync;

    internal async Task InvokeOnMouseUp(MouseEventArgs e)
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
    /// window.mousemove event
    /// </summary>
    public event Action<MouseEventArgs>? OnMouseMove;

    /// <summary>
    /// async window.mousemove event
    /// </summary>
    public event Func<MouseEventArgs, Task>? OnMouseMoveAsync;

    internal async Task InvokeOnMouseMove(MouseEventArgs e)
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
    /// window.ondbClick event
    /// </summary>
    public event Action<MouseEventArgs>? OnDbClick;

    /// <summary>
    /// async window.ondbClick event
    /// </summary>
    public event Func<MouseEventArgs, Task>? OnDbClickAsync;

    internal async Task InvokeOnDbClick(MouseEventArgs e)
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
    /// window.ondbClick event
    /// </summary>
    public event Action<MouseEventArgs>? OnClick;

    /// <summary>
    /// async window.ondbClick event
    /// </summary>
    public event Func<MouseEventArgs, Task>? OnClickAsync;

    internal async Task InvokeOnClick(MouseEventArgs e)
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
    /// 
    /// </summary>
    public event Action<EventArgs>? OnClose;

    /// <summary>
    /// async event
    /// </summary>
    public event Func<EventArgs, Task>? OnCloseAsync;

    internal async Task InvokeOnClose(EventArgs e)
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
    /// 
    /// </summary>
    public event Action<FocusEventArgs>? OnFocus;

    /// <summary>
    /// async event
    /// </summary>
    public event Func<FocusEventArgs, Task>? OnFocusAsync;

    internal async Task InvokeOnFocus(FocusEventArgs e)
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
    /// 
    /// </summary>
    public event Action<FocusEventArgs>? OnBlur;

    /// <summary>
    /// async event
    /// </summary>
    public event Func<FocusEventArgs, Task>? OnBlurAsync;

    internal async Task InvokeOnBlur(FocusEventArgs e)
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
    /// 
    /// </summary>
    public event Action<TouchEventArgs>? OnTouchStart;

    /// <summary>
    /// async event
    /// </summary>
    public event Func<TouchEventArgs, Task>? OnTouchStartAsync;

    internal async Task InvokeOnTouchStart(TouchEventArgs e)
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
    /// 
    /// </summary>
    public event Action<TouchEventArgs>? OnTouchMove;

    /// <summary>
    /// async event
    /// </summary>
    public event Func<TouchEventArgs, Task>? OnTouchMoveAsync;

    internal async Task InvokeOnTouchMove(TouchEventArgs e)
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
    /// 
    /// </summary>
    public event Action<TouchEventArgs>? OnTouchEnd;

    /// <summary>
    /// async event
    /// </summary>
    public event Func<TouchEventArgs, Task>? OnTouchEndAsync;

    internal async Task InvokeOnTouchEnd(TouchEventArgs e)
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
    /// 
    /// </summary>
    public event Action<TouchEventArgs>? OnTouchCancel;

    /// <summary>
    /// async event
    /// </summary>
    public event Func<TouchEventArgs, Task>? OnTouchCancelAsync;

    internal async Task InvokeOnTouchCancel(TouchEventArgs e)
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
    /// 
    /// </summary>
    public event Action<KeyboardEventArgs>? OnKeyDown;

    /// <summary>
    /// async event
    /// </summary>
    public event Func<KeyboardEventArgs, Task>? OnKeyDownAsync;

    internal async Task InvokeOnKeyDown(KeyboardEventArgs e)
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
    /// 
    /// </summary>
    public event Action<KeyboardEventArgs>? OnKeyPress;

    /// <summary>
    /// async event
    /// </summary>
    public event Func<KeyboardEventArgs, Task>? OnKeyPressAsync;

    internal async Task InvokeOnKeyPress(KeyboardEventArgs e)
    {
        OnKeyPress?.Invoke(e);
        OnKeyPressAsync?.Invoke(e);
        if (OnKeyPressAsync != null)
        {
            await OnKeyPressAsync(e);
        }
    }

    #endregion

    #region OnKeyUp

    /// <summary>
    /// 
    /// </summary>
    public event Action<KeyboardEventArgs>? OnKeyUp;

    /// <summary>
    /// async event
    /// </summary>
    public event Func<KeyboardEventArgs, Task>? OnKeyUpAsync;

    internal async Task InvokeOnKeyUp(KeyboardEventArgs e)
    {
        OnKeyUp?.Invoke(e);
        if(OnKeyUpAsync != null)
        {
            await OnKeyUpAsync(e);
        }
    }

    #endregion
}
