using Microsoft.Toolkit.Mvvm.ComponentModel;
using PlateWorld.Models.SamplePlate;
using PlateWorld.ViewModels.Utils;
using System;

namespace PlateWorld.ViewModels.PlateParts
{
    [Serializable]
    public class WellVm : ObservableObject, IUpdater<WellVm>, 
        IUpdater<SampleVm>
    {
        public WellVm(Well well, string plateName,
                      IUpdater<WellVm> wellUpdater)
        {
            Well = well;
            Text = well.WellCoords.ToWellName();
            var sample = Well?.Sample;
            if (sample != null)
            {
                SampleVm = sample.ToSampleVm(this);
            }
            PlateName = plateName;
            WellUpdater = wellUpdater;
        }

        public bool HasChanges
        {
            get
            {
                return SampleVm?.Sample != Well.Sample;
            }
        }

        IUpdater<WellVm> WellUpdater { get; }

        public void Update(WellVm entity)
        {
            WellUpdater.Update(entity);
        }

        public void Update(SampleVm entity)
        {

        }

        public bool NeedsSampleUpdate { get; set; }
        public bool NeedsPlateUpdate { get; set; }


        public Well Well { get; }

        public string PlateName { get; }

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
