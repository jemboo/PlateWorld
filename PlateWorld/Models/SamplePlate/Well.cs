using PlateWorld.Models.BasicTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlateWorld.Models.SamplePlate
{
    public class Well
    {
        public Well(int row, int column, ISample sample = null)
        {
            WellCoords = new WellCoords(row, column);
            Sample = sample;
        }

        public ISample? Sample { get; }

        public WellCoords WellCoords { get; }

    }

    public static class WellExt
    {

        public static Well AddSample(this Well well, ISample? sample)
        {
            return new Well(well.WellCoords.Row, well.WellCoords.Column, sample);
        }
    }
}
