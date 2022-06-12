using Microsoft.Toolkit.Mvvm.ComponentModel;
using PlateWorld.Models.BasicTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlateWorld.ViewModels.BasicTypes
{
    public class ConditionSetVm : ObservableObject
    {
        public ConditionSetVm(IConditionSet conditionSet)
        {
            ConditionSet = conditionSet;
        }

        public IConditionSet ConditionSet { get; }

        public string Name { get { return ConditionSet.Name; } }

        public string Description { get { return ConditionSet.Description; } }

        public int ConditionCount { get { return ConditionSet.TestValues.Count(); } }
    }
}
