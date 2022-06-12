using System.Collections.Generic;

namespace PlateWorld.Models.BasicTypes
{
    public interface IDiscretePropertyType : IPropertyType
    {
        public IEnumerable<object> AllowedValues { get; }
    }
}
