using PlateWorld.Models.TestData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlateWorld.Models
{
    public class Sample : ISample
    {
        public Sample(Guid id, string name, IEnumerable<SampleProperty> sampleProperties)
        {
            Id = id;
            Name = name;
            _propertyDict = new Dictionary<string, SampleProperty>();
            foreach (var item in sampleProperties)
            {
                _propertyDict[item.Name] = item;
            }
        }
        public Guid Id { get; }
        public string Name { get; }
        Dictionary<string, SampleProperty> _propertyDict { get; }
        public IEnumerable<SampleProperty> SampleProperties
        {
            get { return _propertyDict.Values; }
        }

        public SampleProperty? GetProperty(string name)
        {
            if (_propertyDict.ContainsKey(name))
            {
                return _propertyDict[name];
            }
            return null;
        }
    }

    public static class SampleExt
    {
        public static Sample FromAtc(this AnimalThemedContainer atc, string sampleName)
        {
            return new Sample(atc.Id, sampleName, atc.GetSampleProperties());
        }

    }
}
