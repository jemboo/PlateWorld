using System;

namespace PlateWorld.Models.BasicTypes
{
    public interface IPropertyType
    {
        string Name { get; }
        DataType DataType { get; }
        string Description { get; }
        Func<object, bool> IsValid { get; }
        bool IsDiscrete { get; }
    }
}
