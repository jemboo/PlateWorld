using Microsoft.Toolkit.Mvvm.ComponentModel;
using PlateWorld.Models;
using PlateWorld.ViewModels.DragDrop;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;

namespace PlateWorld.ViewModels.PlateParts
{
    public class PlateVm : ObservableObject
    {
        public PlateDragHandler PlateDragHandler { get; set; }
            = new PlateDragHandler();

        public PlateDropHandler PlateDropHandler { get; set; }
            = new PlateDropHandler();
        public PlateVm(Plate plate)
        {
            Plate = plate ?? throw new System.Exception("Plate was null");
            Name = plate.Name;  
            RowCount = Plate.RowCount;
            ColumnCount = Plate.ColumnCount;
            WellVms = Plate.Wells.Select(w => new WellVm(w)).ToList();
            foreach (var w in WellVms)
            {
                w.PropertyChanged += W_PropertyChanged;
            }

            HorizontalMarginVm = new PlateMarginVm(Orientation.Horizontal, RowCount);
            VerticalMarginVm = new PlateMarginVm(Orientation.Vertical, ColumnCount);
        }

        private void W_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {

        }

        string _name;
        public string Name
        {
            get => _name;
            set
            {
                SetProperty(ref _name, value);
            }
        }
        public int RowCount { get; }
        public int ColumnCount { get; }
        public Plate Plate { get; }
        public List<WellVm> WellVms { get; }
        public PlateMarginVm HorizontalMarginVm { get; }
        public PlateMarginVm VerticalMarginVm { get; }

    }

    public static class PlateVmExt
    {
        public static PlateVm ToPlateVm(this Plate plate)
        {
            return new PlateVm(plate);
        }
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
