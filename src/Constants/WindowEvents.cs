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

    public static readonly WindowEvent All = new WindowEvent("*");
    /// <summary>
    /// OnDoubleClick click is Equals onclick. OnDoubleClick will simultaneously listen to onclick
    /// </summary>
    public static readonly WindowEvent OnDoubleClick = new WindowEvent("click");

    /// <summary>
    /// onclick will simultaneously listen to OnDoubleClick
    /// </summary>
    public static readonly WindowEvent onclick = new WindowEvent("click");
    public static readonly WindowEvent oncontextmenu = new WindowEvent("ctextmenu");
    public static readonly WindowEvent onmousedown = new WindowEvent("mousedown");
    public static readonly WindowEvent onmouseup = new WindowEvent("mouseup");
    public static readonly WindowEvent onmouseover = new WindowEvent("mouseover");
    public static readonly WindowEvent onmouseout = new WindowEvent("mouseout");
    public static readonly WindowEvent onmousemove = new WindowEvent("mousemove");
    public static readonly WindowEvent onresize = new WindowEvent("resize");
    public static readonly WindowEvent onscroll = new WindowEvent("scroll");
    public static readonly WindowEvent onclose = new WindowEvent("close");
    public static readonly WindowEvent onfocus = new WindowEvent("focus");
    public static readonly WindowEvent onblur = new WindowEvent("blur");
    public static readonly WindowEvent ontouchstart = new WindowEvent("touchstart");
    public static readonly WindowEvent ontouchmove = new WindowEvent("touchmove");
    public static readonly WindowEvent ontouchend = new WindowEvent("touchend");
    public static readonly WindowEvent ontouchcancel = new WindowEvent("touchcancel");
    public static readonly WindowEvent onkeydown = new WindowEvent("keydown");

    [Obsolete("Since this event has been deprecated, you should use `beforeinput` or `keydown` instead.")]
    public static readonly WindowEvent onkeypress = new WindowEvent("keypress");
    public static readonly WindowEvent onkeyup = new WindowEvent("keyup");
    #endregion
}

