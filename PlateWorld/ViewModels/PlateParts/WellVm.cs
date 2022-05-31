using Microsoft.Toolkit.Mvvm.ComponentModel;
using PlateWorld.Models;

namespace PlateWorld.ViewModels.PlateParts
{
    public class WellVm : ObservableObject
    {
        public WellVm(Well well)
        {
            Well = well;
            WellCoords = new WellCoords(Well.Row, Well.Column);
            Text = WellCoords.ToWellName();
        }

        public Well Well { get; }

        public WellCoords WellCoords { get; }
        public string Text { get; }
    }
}
