using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlateWorld.Models.BasicTypes
{
    public class Experiment : IExperiment
    {
        public Experiment(Guid id, string name, string description,
            IEnumerable<ConditionSet> conditionSets)
        {
            Id = id;
            Name = name;
            Description = description;
            ConditionSets = conditionSets;
        }
        public Guid Id { get; }
        public string Name { get; }
        public string Description { get; }
        public IEnumerable<ConditionSet> ConditionSets { get; }
    }
}
