using System.Collections.Generic;
using System;
using PlateWorld.Models.SamplePlate;

namespace PlateWorld.Models.BasicTypes
{
    public interface ISample
    {
        Guid Id { get; }
        string Name { get; }
        string? PlateName { get; set; }
        WellCoords? WellCoords { get; set; }
        IEnumerable<IProperty> SampleProperties { get; }
        IProperty GetProperty(string name);
        //ISample SetProperty(IProperty value);
        //ISample ChangeName(string newName);
    }
}