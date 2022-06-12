using System;
using System.Collections.Generic;
using System.Linq;

namespace PlateWorld.Models.BasicTypes
{
    public class ConditionSet : IConditionSet
    {
        public ConditionSet(
            string name,
            string description,
            IPropertyType propertyType,
            IEnumerable<object> testValues)
        {
            Name = name;
            Description = description;
            PropertyType = propertyType;
            _testValues = testValues.ToList();
        }

        public string Name { get; }
        public string Description { get; }
        public IPropertyType PropertyType { get; }

        List<object> _testValues = new List<object>();
        public IEnumerable<object> TestValues => _testValues;
    }
}
