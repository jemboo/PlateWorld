using PlateWorld.Models.SamplePlate;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PlateWorld.Models.BasicTypes
{
    public class Sample : ISample
    {
        public Sample(Guid id, string name,
                      IEnumerable<IProperty> sampleProperties)
        {
            Id = id;
            Name = name;
            _propertyDict = new Dictionary<string, IProperty>();
            foreach (var item in sampleProperties)
            {
                _propertyDict[item.Name] = item;
            }
        }
        public Guid Id { get; }
        public string Name { get; }
        public string? PlateName { get; set; }
        public WellCoords? WellCoords { get; set; }
        Dictionary<string, IProperty> _propertyDict { get; }
        public IEnumerable<IProperty> SampleProperties
        {
            get { return _propertyDict.Values; }
        }
        public IProperty? GetProperty(string name)
        {
            if (_propertyDict.ContainsKey(name))
            {
                return _propertyDict[name];
            }
            return null;
        }

        //public ISample SetProperty(IProperty value)
        //{
        //    if (value == null) return this;

        //    return new Sample(
        //        id: Id,
        //        name: Name,
        //        sampleProperties: _propertyDict.Values.Append(value));
        //}

        //public ISample ChangeName(string newName)
        //{
        //    if (string.IsNullOrEmpty(newName)) return this;

        //    return new Sample(
        //        id: Id,
        //        name: newName,
        //        sampleProperties: _propertyDict.Values);
        //}

        public static Sample Empty
        {
            get
            {
                return new Sample(Guid.Empty, "<none>", Enumerable.Empty<IProperty>());
            }
        }
    }

    public static class SampleExt
    {
        public static IEnumerable<ISample> AllSamples(this IEnumerable<IPropertySet> propertySets,
            int firstIndex)
        {
            var pwrSpec = propertySets.Select(ps => ps.AllProperties().ToList()).ToList();
            var pwrList = pwrSpec.powerJoin();
            var dex = firstIndex;
            foreach(var spL in pwrList)
            {
                var name = $"Sample {dex++}";
                yield return new Sample(Guid.NewGuid(), name, spL);
            }
        }

        public static IEnumerable<IPropertySet> GetPropertySets(this IEnumerable<ISample> samples)
        {

            var pByPt = samples.SelectMany(s => s.SampleProperties)
                             .GroupBy(sp => sp.PropertyType).ToList();

            var pvsByPt = pByPt.Select(gp =>
                    {
                        var bug = gp.GroupBy(b => b.Value).Select(b => b.Key).ToList();
                        return new Tuple<IPropertyType, List<object>>(gp.Key, bug);
                    }).ToList();
            return pvsByPt.Select(b => new PropertySet("", "", b.Item1, b.Item2));
        }

    }
}
