using System.Collections.Generic;
using System;
using System.Linq;

namespace PlateWorld.Models
{
    public class Plate
    {
        public Plate(int id, string name,
                     int wellsPerRow,
                     int wellsPerColumn,
                     IEnumerable<Well> wells)
        {
            Id = id;
            Name = name;
            ColumnCount = wellsPerRow;
            RowCount = wellsPerColumn;
            Wells = PlateExt.WellSet(ColumnCount, RowCount).ToArray();
        }
        public int Id { get; }
        public string Name { get; }

        public int ColumnCount { get; set; }

        public int RowCount { get; set; }

        public Well[] Wells { get; set; }

        public int UsedWells
        {
            get
            {
                return Wells.Where(w => w.Sample != null).Count();
            }
        }

        public static Plate Empty
        {
            get { return new Plate(0, string.Empty, 0, 0, new List<Well>());  }
        }
    }

    public static class PlateExt
    {
        public static Plate NewName(this Plate plate, string newName)
        {
            return new Plate(id:plate.Id, 
                name: newName, 
                wellsPerRow:plate.ColumnCount, 
                wellsPerColumn:plate.RowCount,
                wells: plate.Wells);
        }
        public static Plate NewRowCount(this Plate plate, int newRowCount)
        {
            return new Plate(
                id: plate.Id, 
                name: plate.Name, 
                wellsPerRow: plate.ColumnCount, 
                wellsPerColumn: plate.RowCount,
                wells: WellSet(newRowCount, plate.RowCount).ToArray());
        }
        public static Plate NewColCount(this Plate plate, int newColCount)
        {
            return new Plate(
                id: plate.Id, 
                name: plate.Name, 
                wellsPerRow: plate.ColumnCount, 
                wellsPerColumn: plate.RowCount,
                wells: WellSet(plate.ColumnCount, newColCount).ToArray());
        }

        public static Plate MakePlate(string plateName, int rowCount, int colCount)
        {
            var rndy = new System.Random();
            return new Plate(
                id: rndy.Next(),
                name: plateName,
                wellsPerRow: rowCount,
                wellsPerColumn: colCount,
                wells: WellSet(rowCount, colCount).ToArray());
        }

        public static IEnumerable<Well> WellSet(int rows, int cols)
        {
            for(var i=0; i< rows; i++)
            {
                for(var j=0; j< cols; j++)
                {
                    var samp = (((i + j) % 2) == 1) ? 
                        new Sample(Guid.NewGuid(), "Name", Enumerable.Empty<SampleProperty>()) : 
                        null;
                    yield return new Well(row: i + 1, column: j + 1, sample: samp);
                }
            }
        }
    }
}
