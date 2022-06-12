using System.Collections.Generic;

namespace PlateWorld.Models.BasicTypes
{
    public interface IConditionSet
    {
        string Name { get; }
        string Description { get; }
        IPropertyType PropertyType { get; }
        IEnumerable<object> TestValues { get; }
    }
}