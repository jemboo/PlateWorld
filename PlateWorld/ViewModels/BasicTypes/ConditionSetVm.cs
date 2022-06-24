using Microsoft.Toolkit.Mvvm.ComponentModel;
using PlateWorld.Models.BasicTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlateWorld.ViewModels.BasicTypes
{
    public class PropertySetVm : ObservableObject
    {
        public PropertySetVm(IPropertySet propertySet)
        {
            PropertySet = propertySet;
        }

        public IPropertySet PropertySet { get; }

        public string Name { get { return PropertySet.Name; } }

        public string Description { get { return PropertySet.Description; } }

        public int PropertyCount { get { return PropertySet.TestValues.Count(); } }
    }
}
