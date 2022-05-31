using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlateWorld.Models
{
    public class Well
    {
        public Well(int row, int column, Sample sample = null)
        {
            Row = row;
            Column = column;
            Sample = sample;
        }

        public Sample Sample { get; }
        public int Row { get; }
        public int Column { get; }

    }

    public static class WellExt
    {

    }
}
