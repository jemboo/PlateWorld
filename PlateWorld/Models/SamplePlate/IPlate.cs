using System;
using System.Collections.Generic;

namespace PlateWorld.Models.SamplePlate
{
    public interface IPlate
    {
        int ColumnCount { get; }
        Guid Id { get; }
        string Name { get; }
        int RowCount { get; }
        int UsedWells { get; }
        IEnumerable<Well> Wells { get; }
    }
}