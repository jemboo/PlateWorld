using System;
using System.Linq;
using System.Collections.Generic;
using PlateWorld.Models.SamplePlate;

namespace PlateWorld.DataStore
{
    public class ConditionSetStore
    {
        public event Action<IPlate[]> ConditionSetsAdded;
        public event Action<IPlate[]> ConditionSetsRemoved;
        Dictionary<Guid, Plate> conditionSetDict { get; set; } 
            = new Dictionary<Guid, Plate>();

        public void AddConditionSets(Plate[] conditionSets)
        {
            foreach (var conditionSet in conditionSets)
            {
                conditionSetDict[conditionSet.Id] = conditionSet;
            }
            ConditionSetsAdded?.Invoke(conditionSets);
        }

        public bool ContainsConditionSet(Guid conditionSetId)
        {
            return conditionSetDict.ContainsKey(conditionSetId);
        }

        public bool ContainsConditionSetName(string conditionSetName)
        {
            return conditionSetDict.Values.Any(p => p.Name == conditionSetName);
        }
        public IEnumerable<Plate> AllConditionSets 
        {
            get { return conditionSetDict.Values; }
        }

        public bool[] Remove(IPlate[] conditionSets)
        {
            var res = conditionSets.Select(
                conditionSet => conditionSetDict.Remove(conditionSet.Id));
            ConditionSetsRemoved?.Invoke(conditionSets);
            return res.ToArray();
        }

    }
}
