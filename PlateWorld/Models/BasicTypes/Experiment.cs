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
            IEnumerable<PropertySet> propertySets)
        {
            Id = id;
            Name = name;
            Description = description;
            PropertySets = propertySets;
        }
        public Guid Id { get; }
        public string Name { get; }
        public string Description { get; }
        public IEnumerable<PropertySet> PropertySets { get; }
    }
}
