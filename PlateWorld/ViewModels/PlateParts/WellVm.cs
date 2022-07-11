using Microsoft.Toolkit.Mvvm.ComponentModel;
using PlateWorld.Models.SamplePlate;
using PlateWorld.ViewModels.Utils;
using System;
using System.ComponentModel;

namespace PlateWorld.ViewModels.PlateParts
{
    [Serializable]
    public class WellVm : ObservableObject
    {
        public WellVm(Well well, string plateName,
                      Action<WellVm, WellVm, SampleVm> updato)
        {
            Well = well;
            Text = well.WellCoords.ToWellName();
            _updato = updato;
            var sample = Well?.Sample;
            if (sample != null)
            {
                SampleVm = sample.ToSampleVm();
            }
            PlateName = plateName;
        }

        public bool HasChanges
        {
            get
            {
                return SampleVm?.Sample != Well.Sample;
            }
        }

        Action<WellVm, WellVm, SampleVm> _updato;
        public Action<WellVm, WellVm, SampleVm> Updato
        {
            get { return _updato; }
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
                if (_sampleVm != null)
                {
                    _sampleVm.PropertyChanged -= _sampleVm_PropertyChanged;
                }
                SetProperty(ref _sampleVm, value);
                if (_sampleVm != null)
                {
                    _sampleVm.PropertyChanged += _sampleVm_PropertyChanged;
                }
                OnPropertyChanged("ContainsSample");
            }
        }

        private void _sampleVm_PropertyChanged(
            object? sender, PropertyChangedEventArgs e)
        {
            var sampleVm = sender as SampleVm;
            if (sampleVm == null) return;
            if(e.PropertyName == "IsSelected")
            {
                IsSelected = sampleVm.IsSelected;
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
                if(SampleVm != null)
                {
                    SampleVm.IsSelected = value;
                }
            }
        }

        public bool ContainsSample
        {
            get => SampleVm != null;
        }
    }
}
