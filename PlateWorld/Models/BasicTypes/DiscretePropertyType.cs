using System;
using System.Collections.Generic;
using System.Linq;

namespace PlateWorld.Models.BasicTypes
{
    public class DiscretePropertyType : IDiscretePropertyType
    {
        public DiscretePropertyType(string name, string description,
            DataType dataType, IEnumerable<object> allowedValues)
        {
            Name = name;
            Description = description;
            DataType = dataType;
            _allowedValues = allowedValues.ToList();
        }
        public string Name { get; }
        public DataType DataType { get; }
        public string Description { get; }
        public Func<object, bool> IsValid
        {
            get { return new Func<object, bool>(v => AllowedValues.Contains(v)); }
        }
        public bool IsDiscrete { get { return true; } }

        List<object> _allowedValues;
        public IEnumerable<object> AllowedValues => _allowedValues;
    }
}
