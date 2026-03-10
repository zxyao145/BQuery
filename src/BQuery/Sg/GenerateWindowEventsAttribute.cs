using System;

namespace BQuery;

[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
internal sealed class GenerateWindowEventsAttribute : Attribute
{
    public GenerateWindowEventsAttribute(Type constantsType)
    {
        ConstantsType = constantsType;
    }

    public Type ConstantsType { get; }
}
