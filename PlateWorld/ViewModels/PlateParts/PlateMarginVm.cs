using Microsoft.Toolkit.Mvvm.ComponentModel;
using PlateWorld.Models;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;

namespace PlateWorld.ViewModels.PlateParts
{
    public class PlateMarginVm : ObservableObject
    {
        public PlateMarginVm(Orientation orientation, int count)
        {
            Orientation = orientation;
            Count = count;
            Labels = (Orientation == Orientation.Vertical) ?
                Enumerable.Range(1, count).Select(i => i.ColumnIndexToSymbol()).ToList() :
                Enumerable.Range(1, count).Select(i => i.ToString()).ToList();
                
        }
        public Orientation Orientation { get; set; }    
        public List<string> Labels { get; }
        public int Count { get; set; }
    }

    public class PlateMarginVmD : PlateMarginVm
    {
        public PlateMarginVmD() : base(Orientation.Horizontal, 12)
        {
        }
    }
}
