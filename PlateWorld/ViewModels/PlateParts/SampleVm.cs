using Microsoft.Toolkit.Mvvm.ComponentModel;
using PlateWorld.Models;
using System;

namespace PlateWorld.ViewModels.PlateParts
{
    [Serializable]
    public class SampleVm : ObservableObject
    {
        public SampleVm(Guid id, string sampleName)
        {
            SampleName = sampleName;
            Id = id;
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

        private Guid _id;
        public Guid Id
        {
            get => _id;
            set
            {
                SetProperty(ref _id, value);
            }
        }

    }

    public static class SampleVmExt
    {
        public static SampleVm ToVm(this ISample sample)
        {
            return new SampleVm(sample.Id, sample.Name);
        }
    }
}
