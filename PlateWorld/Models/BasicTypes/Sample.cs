using System;
using System.Collections.Generic;
using System.Linq;

namespace PlateWorld.Models.BasicTypes
{
    public class Sample : ISample
    {
        public Sample(Guid id, string name,
                      IEnumerable<Property> sampleProperties)
        {
            Id = id;
            Name = name;
            _propertyDict = new Dictionary<string, Property>();
            foreach (var item in sampleProperties)
            {
                _propertyDict[item.Name] = item;
            }
        }
        public Guid Id { get; }
        public string Name { get; }
        Dictionary<string, Property> _propertyDict { get; }
        public IEnumerable<Property> SampleProperties
        {
            get { return _propertyDict.Values; }
        }
        public Property? GetProperty(string name)
        {
            if (_propertyDict.ContainsKey(name))
            {
                return _propertyDict[name];
            }
            return null;
        }

        public ISample SetProperty(Property value)
        {
            if (value == null) return this;

            return new Sample(
                id: Id,
                name: Name,
                sampleProperties: _propertyDict.Values.Append(value));
        }

        public ISample ChangeName(string newName)
        {
            if (string.IsNullOrEmpty(newName)) return this;

            return new Sample(
                id: Id,
                name: newName,
                sampleProperties: _propertyDict.Values);
        }

        public static Sample Empty
        {
            get
            {
                return new Sample(
                    Guid.Empty,
                    "<none>",
                    Enumerable.Empty<Property>());
            }
        }
    }

    public static class SampleExt
    {

    }
}
