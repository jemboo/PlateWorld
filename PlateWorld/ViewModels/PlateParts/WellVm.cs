using Microsoft.Toolkit.Mvvm.ComponentModel;
using PlateWorld.Models.SamplePlate;
using System;

namespace PlateWorld.ViewModels.PlateParts
{
    [Serializable]
    public class WellVm : ObservableObject
    {
        public WellVm(Well well)
        {
            Well = well;
            Text = well.WellCoords.ToWellName();
            var sample = Well?.Sample;
            if (sample != null)
            {
                SampleVm = sample.ToVm();
            }
        }

        public Well? Well { get; }

        SampleVm? _sampleVm;
        public SampleVm? SampleVm 
        {
            get => _sampleVm;
            set
            {
                SetProperty(ref _sampleVm, value);
                this.OnPropertyChanged("ContainsSample");
            }
        }

        public string Text { get; }

        private bool _isSelected;
        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                SetProperty(ref _isSelected, value);
            }
        }

        public bool ContainsSample
        {
            get => SampleVm != null;
        }
    }
}
