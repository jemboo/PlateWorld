using Microsoft.Toolkit.Mvvm.ComponentModel;
using PlateWorld.Models;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;

namespace PlateWorld.ViewModels.PlateParts
{
    public class PlateVm : ObservableObject
    {
        public PlateVm(Plate plate)
        {
            Plate = plate;
            WellVms = Plate.Wells.Select(w => new WellVm(w)).ToList();
            HorizontalMarginVm = new PlateMarginVm(Orientation.Horizontal, RowCount);
            VerticalMarginVm = new PlateMarginVm(Orientation.Vertical, ColumnCount);
        }
        public string Name => Plate.Name;
        public int RowCount => Plate.WellsPerColumn;
        public int ColumnCount => Plate.WellsPerRow;
        public Plate Plate { get; }

        public List<WellVm> WellVms { get; }
        public PlateMarginVm HorizontalMarginVm { get; }
        public PlateMarginVm VerticalMarginVm { get; }
    }

    public class PlateVmD : PlateVm
    {
        public PlateVmD() : base(TestPlate())
        {
        }
        static Plate TestPlate()
        {
            return PlateExt.MakePlate(plateName: "Ralph", rowCount: 8, colCount: 12);
        }
    }

}
