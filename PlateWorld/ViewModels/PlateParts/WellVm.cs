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

        public void Update(WellVm theOld, WellVm theNew)
        {
            WellUpdater.Update(theOld, theNew);
        }

        public void Update(SampleVm theOld, SampleVm theNew)
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
        public bool IsSelected2
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
