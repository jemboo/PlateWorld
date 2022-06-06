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
            WellCoords = new WellCoords(row, column);
            Sample = sample;
        }

        public Sample Sample { get; set; }

        public WellCoords WellCoords { get; set; }
    }

    public static class WellExt
    {

    }
}
