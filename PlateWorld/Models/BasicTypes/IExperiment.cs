using System;
using System.Collections.Generic;

namespace PlateWorld.Models.BasicTypes
{
    public interface IExperiment
    {
        IEnumerable<ConditionSet> ConditionSets { get; }
        string Description { get; }
        Guid Id { get; }
        string Name { get; }
    }
}