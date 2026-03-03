using System;

namespace BQuery;

[AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
internal sealed class WindowEventHandlerAttribute : Attribute
{
    public WindowEventHandlerAttribute(Type argumentType)
    {
        ArgumentType1 = argumentType;
    }

    public WindowEventHandlerAttribute(Type argumentType1, Type argumentType2)
    {
        ArgumentType1 = argumentType1;
        ArgumentType2 = argumentType2;
    }

    public Type ArgumentType1 { get; }

    public Type? ArgumentType2 { get; }
}
