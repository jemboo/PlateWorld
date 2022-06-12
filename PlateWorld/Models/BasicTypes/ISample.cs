using System.Collections.Generic;
using System;

namespace PlateWorld.Models.BasicTypes
{
    public interface ISample
    {
        Guid Id { get; }
        string Name { get; }
        IEnumerable<Property> SampleProperties { get; }
        Property GetProperty(string name);
        ISample SetProperty(Property value);
        ISample ChangeName(string newName);
    }
}