using System.Diagnostics.CodeAnalysis;

namespace BQuery;

public readonly struct WindowEvent : IEquatable<WindowEvent>
{
    public string Name { get; }

    public WindowEvent(string name)
    {
        Name = name;
    }

    public static bool operator ==(WindowEvent left, WindowEvent right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(WindowEvent left, WindowEvent right)
    {
        return !(left == right);
    }

    public override bool Equals([NotNullWhen(true)] object? obj)
    {
        if (obj is WindowEvent other)
        {
            return Equals(other);
        }

        return false;
    }

    public bool Equals(WindowEvent other)
    {
        return string.Equals(Name, other.Name, StringComparison.OrdinalIgnoreCase);
    }

    public override int GetHashCode()
    {
        return StringComparer.OrdinalIgnoreCase.GetHashCode(Name);
    }

    public override string ToString()
    {
        return Name;
    }

    #region events

    public static readonly WindowEvent All = new("*");
    /// <summary>
    /// Double-click is synthesized from click events.
    /// </summary>
    [WindowEventHandler(typeof(MouseEventArgs))]
    public static readonly WindowEvent OnDoubleClick = new("click");

    /// <summary>
    /// Click will also drive the synthesized double-click event.
    /// </summary>
    [WindowEventHandler(typeof(MouseEventArgs))]
    public static readonly WindowEvent OnClick = new("click");
    [WindowEventHandler(typeof(MouseEventArgs))]
    public static readonly WindowEvent OnContextMenu = new("contextmenu");
    [WindowEventHandler(typeof(MouseEventArgs))]
    public static readonly WindowEvent OnMouseDown = new("mousedown");
    [WindowEventHandler(typeof(MouseEventArgs))]
    public static readonly WindowEvent OnMouseUp = new("mouseup");
    [WindowEventHandler(typeof(MouseEventArgs))]
    public static readonly WindowEvent OnMouseOver = new("mouseover");
    [WindowEventHandler(typeof(MouseEventArgs))]
    public static readonly WindowEvent OnMouseOut = new("mouseout");
    [WindowEventHandler(typeof(MouseEventArgs))]
    public static readonly WindowEvent OnMouseMove = new("mousemove");
    [WindowEventHandler(typeof(double), typeof(double))]
    public static readonly WindowEvent OnResize = new("resize");
    [WindowEventHandler(typeof(EventArgs))]
    public static readonly WindowEvent OnScroll = new("scroll");
    /// <summary>
    /// Exposed as a close-like event and implemented via <c>window.beforeunload</c> and <c>window.pagehide</c>.
    /// </summary>
    [WindowEventHandler(typeof(EventArgs))]
    public static readonly WindowEvent OnClose = new("close");
    [WindowEventHandler(typeof(FocusEventArgs))]
    public static readonly WindowEvent OnFocus = new("focus");
    [WindowEventHandler(typeof(FocusEventArgs))]
    public static readonly WindowEvent OnBlur = new("blur");
    [WindowEventHandler(typeof(TouchEventArgs))]
    public static readonly WindowEvent OnTouchStart = new("touchstart");
    [WindowEventHandler(typeof(TouchEventArgs))]
    public static readonly WindowEvent OnTouchMove = new("touchmove");
    [WindowEventHandler(typeof(TouchEventArgs))]
    public static readonly WindowEvent OnTouchEnd = new("touchend");
    [WindowEventHandler(typeof(TouchEventArgs))]
    public static readonly WindowEvent OnTouchCancel = new("touchcancel");
    [WindowEventHandler(typeof(KeyboardEventArgs))]
    public static readonly WindowEvent OnKeyDown = new("keydown");
    [WindowEventHandler(typeof(KeyboardEventArgs))]
    public static readonly WindowEvent OnKeyPress = new("keypress");
    [WindowEventHandler(typeof(KeyboardEventArgs))]
    public static readonly WindowEvent OnKeyUp = new("keyup");

    #endregion
}

