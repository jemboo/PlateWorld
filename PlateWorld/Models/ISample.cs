using System.Collections.Generic;
using System;
namespace PlateWorld.Models
{
    public interface ISample
    {
        Guid Id { get; }
        string Name { get; }
        IEnumerable<SampleProperty> SampleProperties { get; }
        SampleProperty? GetProperty(string name);
    }
}