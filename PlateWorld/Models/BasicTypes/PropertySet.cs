using System;
using System.Collections.Generic;
using System.Linq;

namespace PlateWorld.Models.BasicTypes
{
    public class PropertySet : IPropertySet
    {
        public PropertySet(
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

    public static class PropertySetExt
    {
        public static IEnumerable<IProperty> AllProperties(this IPropertySet propertySet)
        {
            return propertySet.TestValues.Select(tv =>
                new Property(name: $"{propertySet.PropertyType.Name}_{tv}",
                             propertyType: propertySet.PropertyType,
                             value: tv));
        }

        public static IEnumerable<List<T>> Comby<T>(this List<List<T>> bse, List<T> top)
        {
            foreach(var b in bse)
            {
                foreach(var t in top)
                {
                    var qua = new List<T>(b);
                    qua.Add(t);
                    yield return qua;
                }
            }
        }

        public static List<T> AsList<T>(this T itme)
        {
            var lret = new List<T>();
            lret.Add(itme);
            return lret;
        }

        static IEnumerable<List<T>> _recJoin<T>(List<List<T>> spBase, List<List<T>> llprops)
        {
            if ((llprops == null) || (llprops.Count == 0))
            {
                return spBase;
            }
            var nextProps = llprops.First();
            var remProps = llprops.Skip(1).ToList();
            var newBase = spBase.Comby(nextProps).ToList();
            return _recJoin(newBase, remProps);
        }

        public static IEnumerable<List<T>> powerJoin<T>(this List<List<T>> llprops)
        {
            if (llprops == null)
            {
                return Enumerable.Empty<List<T>>();
            }
            var firstProps = llprops.First().Select(itm => itm.AsList()).ToList();
            var remProps = llprops.Skip(1).ToList();
            return _recJoin(firstProps, remProps);
        }
    }
}
