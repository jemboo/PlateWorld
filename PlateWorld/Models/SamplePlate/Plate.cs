using System.Collections.Generic;
using System;
using System.Linq;
using PlateWorld.DataStore;

namespace PlateWorld.Models.SamplePlate
{
    public class Plate : IPlate
    {
        public Plate(Guid id, string name, int collumCount,
                     int rowCount, IEnumerable<Well> wells)
        {
            Id = id;
            Name = name;
            ColumnCount = collumCount;
            RowCount = rowCount;
            _wells = wells.ToArray(); // PlateExt.WellSet(rowCt: RowCount, colCt: ColumnCount, sampleStore: SampleStore).ToArray();
        }
        public Guid Id { get; }
        public string Name { get; }
        public int ColumnCount { get; }
        public int RowCount { get; }

        Well[] _wells;
        public IEnumerable<Well> Wells 
        {
            get { return _wells; }
        }

        public int UsedWells
        {
            get
            {
                return Wells.Where(w => w.Sample != null).Count();
            }
        }

        public static Plate Empty
        {
            get { return new Plate(Guid.Empty, string.Empty, 0, 0, Enumerable.Empty<Well>()); }
        }
    }

    public static class PlateExt
    {
        public static Plate Update(this IPlate plate, string newName, IEnumerable<Well> newWells)
        {
            return
                new Plate(  id: plate.Id,
                            name: newName,
                            collumCount: plate.ColumnCount,
                            rowCount: plate.RowCount,
                            newWells);
        }
        public static Plate MakePlate(string plateName, int rowCount, int colCount)
        {
            var ws = PlateExt.WellSet(rowCt: rowCount, colCt: colCount);
            return new Plate(id: Guid.NewGuid(),
                             name: plateName,
                             collumCount: colCount,
                             rowCount: rowCount, ws);
                        }

        public static IEnumerable<Well> WellSet(int rowCt, int colCt)
        {
            for (var i = 0; i < rowCt; i++)
            {
                for (var j = 0; j < colCt; j++)
                {
                    //var samp = (i + j) % 2 == 1 ?
                    //    new Sample(Guid.NewGuid(), "Name", Enumerable.Empty<Property>()) :
                    //    null;
                    yield return new Well(row: i + 1, column: j + 1, sample: null);
                }
            }
        }
    }
}
