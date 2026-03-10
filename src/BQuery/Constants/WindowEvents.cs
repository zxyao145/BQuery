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
    internal static readonly WindowEvent All = new("*");

    #region Mouse Event

    [WindowEventHandler(typeof(MouseEventArgs))]
    public static readonly WindowEvent OnMouseDown = new("mousedown");
    [WindowEventHandler(typeof(MouseEventArgs))]
    public static readonly WindowEvent OnMouseUp = new("mouseup");

    [WindowEventHandler(typeof(MouseEventArgs))]
    public static readonly WindowEvent OnClick = new("click");
    [WindowEventHandler(typeof(MouseEventArgs))]
    public static readonly WindowEvent OnDblClick = new("dblclick");

    [WindowEventHandler(typeof(MouseEventArgs))]
    public static readonly WindowEvent OnMouseOver = new("mouseover");
    [WindowEventHandler(typeof(MouseEventArgs))]
    public static readonly WindowEvent OnMouseOut = new("mouseout");
    [WindowEventHandler(typeof(MouseEventArgs))]
    public static readonly WindowEvent OnMouseMove = new("mousemove");


    [WindowEventHandler(typeof(MouseEventArgs))]
    public static readonly WindowEvent OnContextMenu = new("contextmenu");

    #endregion

    #region size or state event

    [WindowEventHandler(typeof(ResizeEventArgs))]
    public static readonly WindowEvent OnResize = new("resize");
    [WindowEventHandler(typeof(EventArgs))]
    public static readonly WindowEvent OnScroll = new("scroll");

    #endregion

    #region Focus Event

    [WindowEventHandler(typeof(FocusEventArgs))]
    public static readonly WindowEvent OnFocus = new("focus");
    [WindowEventHandler(typeof(FocusEventArgs))]
    public static readonly WindowEvent OnBlur = new("blur");

    #endregion

    #region Touch Event

    [WindowEventHandler(typeof(TouchEventArgs))]
    public static readonly WindowEvent OnTouchStart = new("touchstart");
    [WindowEventHandler(typeof(TouchEventArgs))]
    public static readonly WindowEvent OnTouchMove = new("touchmove");
    [WindowEventHandler(typeof(TouchEventArgs))]
    public static readonly WindowEvent OnTouchEnd = new("touchend");
    [WindowEventHandler(typeof(TouchEventArgs))]
    public static readonly WindowEvent OnTouchCancel = new("touchcancel");

    #endregion

    #region Keyboard Event

    [WindowEventHandler(typeof(KeyboardEventArgs))]
    public static readonly WindowEvent OnKeyDown = new("keydown");
    [WindowEventHandler(typeof(KeyboardEventArgs))]
    public static readonly WindowEvent OnKeyPress = new("keypress");
    [WindowEventHandler(typeof(KeyboardEventArgs))]
    public static readonly WindowEvent OnKeyUp = new("keyup");
    #endregion

    #endregion
}

