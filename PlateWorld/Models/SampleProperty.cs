using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlateWorld.Models
{
    public enum PropertyType
    {
        Bool,
        Int,
        Float,
        String
    }
    public class SampleProperty
    {
        public SampleProperty(string name, PropertyType propertyType, object value, string description)
        {
            Name = name;
            PropertyType = propertyType;
            Value = value;
            Description = description;
        }

        public string Description { get; }
        public string Name { get; }
        public PropertyType PropertyType { get; }
        public object Value { get; }
    }
}
