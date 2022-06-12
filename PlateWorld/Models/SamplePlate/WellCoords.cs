using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PlateWorld.Models.SamplePlate
{
    public struct WellCoords
    {
        public WellCoords(int row, int column)
        {
            Row = row;
            Column = column;
        }

        public int Row { get; }
        public int Column { get; }

    }

    public static class WellCoordsExt
    {
        public static string ToWellName(this WellCoords wc)
        {
            return $"{wc.Row.ColumnIndexToSymbol()}{wc.Column}";
        }

        public static WellCoords ToWellCoords(this string wellName)
        {
            var coords = wellName.SplitCoordName();
            return new WellCoords(row: coords.Item1, column: coords.Item2);
        }

        public static string ColumnIndexToSymbol(this int columnNumber)
        {
            string columnName = "";

            while (columnNumber > 0)
            {
                int modulo = (columnNumber - 1) % 26;
                columnName = Convert.ToChar('A' + modulo) + columnName;
                columnNumber = (columnNumber - modulo) / 26;
            }
            return columnName;
        }
        public static int ColumnSymbolToIndex(this string columnSymbol)
        {
            if (string.IsNullOrEmpty(columnSymbol)) throw new ArgumentNullException("columnSymbol");

            columnSymbol = columnSymbol.ToUpperInvariant();

            int sum = 0;

            for (int i = 0; i < columnSymbol.Length; i++)
            {
                sum *= 26;
                sum += columnSymbol[i] - 'A' + 1;
            }

            return sum;
        }

        // returns the (row, collumn) indexes
        public static Tuple<int, int> SplitCoordName(this string cordName)
        {
            try
            {
                var pcs = Regex.Split(cordName, @"[^A-Z0-9]+|(?<=[A-Z])(?=[0-9])|(?<=[0-9])(?=[A-Z])");
                if (pcs.Length != 2)
                {
                    throw new Exception("invalid well coord name");
                }
                return new Tuple<int, int>(int.Parse(pcs[1]), pcs[0].ColumnSymbolToIndex());
            }
            catch
            {
                throw;
            }
        }

    }

}
