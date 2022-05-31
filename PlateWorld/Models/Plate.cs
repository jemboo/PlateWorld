using System.Collections.Generic;
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
            WellsPerRow = wellsPerRow;
            WellsPerColumn = wellsPerColumn;
            Wells = wells.ToArray();
        }
        public int Id { get; }
        public string Name { get; }

        public int WellsPerRow { get; set; }

        public int WellsPerColumn { get; set; }

        public Well[] Wells { get; set; }

        public int UsedWells
        {
            get
            {
                return Wells.Where(w => w.Sample != null).Count();
            }
        }
    }

    public static class PlateExt
    {
        public static Plate NewName(this Plate plate, string newName)
        {
            return new Plate(id:plate.Id, 
                name: newName, 
                wellsPerRow:plate.WellsPerRow, 
                wellsPerColumn:plate.WellsPerColumn,
                wells: plate.Wells);
        }
        public static Plate NewRowCount(this Plate plate, int newRowCount)
        {
            return new Plate(
                id: plate.Id, 
                name: plate.Name, 
                wellsPerRow: plate.WellsPerRow, 
                wellsPerColumn: plate.WellsPerColumn,
                wells: WellSet(newRowCount, plate.WellsPerColumn).ToArray());
        }
        public static Plate NewColCount(this Plate plate, int newColCount)
        {
            return new Plate(
                id: plate.Id, 
                name: plate.Name, 
                wellsPerRow: plate.WellsPerRow, 
                wellsPerColumn: plate.WellsPerColumn,
                wells: WellSet(plate.WellsPerRow, newColCount).ToArray());
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
                    yield return new Well(row: i + 1, column: j + 1);
                }
            }
        }
    }
}
