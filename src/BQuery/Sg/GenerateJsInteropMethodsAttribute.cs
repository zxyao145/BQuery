using System;

namespace BQuery;

[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
internal sealed class GenerateJsInteropMethodsAttribute : Attribute
{
    public GenerateJsInteropMethodsAttribute(Type constantsType)
    {
        ConstantsType = constantsType;
    }

    public Type ConstantsType { get; }
}
