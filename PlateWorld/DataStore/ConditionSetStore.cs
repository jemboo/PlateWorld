using System;
using System.Linq;
using System.Collections.Generic;
using PlateWorld.Models.SamplePlate;

namespace PlateWorld.DataStore
{
    public class PropertySetStore
    {
        public event Action<IPlate[]> PropertySetsAdded;
        public event Action<IPlate[]> PropertySetsRemoved;
        Dictionary<Guid, Plate> propertySetDict { get; set; } 
            = new Dictionary<Guid, Plate>();

        public void AddPropertySets(Plate[] propertySets)
        {
            foreach (var propertySet in propertySets)
            {
                propertySetDict[propertySet.Id] = propertySet;
            }
            PropertySetsAdded?.Invoke(propertySets);
        }

        public bool ContainsPropertySet(Guid propertySetId)
        {
            return propertySetDict.ContainsKey(propertySetId);
        }

        public bool ContainsPropertySetName(string propertySetName)
        {
            return propertySetDict.Values.Any(p => p.Name == propertySetName);
        }
        public IEnumerable<Plate> AllPropertySets 
        {
            get { return propertySetDict.Values; }
        }

        public bool[] Remove(IPlate[] propertySets)
        {
            var res = propertySets.Select(
                propertySet => propertySetDict.Remove(propertySet.Id));
            PropertySetsRemoved?.Invoke(propertySets);
            return res.ToArray();
        }

    }
}
