using System.Collections.Generic;

namespace PlateWorld.Models.BasicTypes
{
    public interface IPropertySet
    {
        string Name { get; }
        string Description { get; }
        IPropertyType PropertyType { get; }
        IEnumerable<object> TestValues { get; }
    }
}