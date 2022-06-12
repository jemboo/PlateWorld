using Microsoft.Toolkit.Mvvm.ComponentModel;
using PlateWorld.Models.BasicTypes;
using System;

namespace PlateWorld.ViewModels.PlateParts
{
    [Serializable]
    public class SampleVm : ObservableObject
    {
        public SampleVm(ISample sample)
        {
            Sample = sample;
            SampleName = sample.Name;
        }

        private string _sampleName;
        public string SampleName
        {
            get => _sampleName;
            set
            {
                SetProperty(ref _sampleName, value);
            }
        }

        public Guid Id => Sample.Id;

        public bool HasChanges
        {
            get
            {
                return ((SampleName != Sample.Name) ||
                        (SampleName != Sample.Name));
            }
        }

        public ISample Sample { get; }

        public static SampleVm Empty =>
            new SampleVm(Models.BasicTypes.Sample.Empty);
    }

    public static class SampleVmExt
    {
        public static SampleVm ToVm(this ISample sample)
        {
            return new SampleVm(sample);
        }
    }
}
