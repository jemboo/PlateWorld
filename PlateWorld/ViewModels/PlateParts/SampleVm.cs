﻿using Microsoft.Toolkit.Mvvm.ComponentModel;
using PlateWorld.Models.BasicTypes;
using PlateWorld.Models.SamplePlate;
using PlateWorld.ViewModels.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PlateWorld.ViewModels.PlateParts
{
    [Serializable]
    public class SampleVm : ObservableObject
    {
        public SampleVm(ISample sample)
        {
            Sample = sample;
            _sampleName = sample.Name;
            PlateName = sample.PlateName;
            WellCoords = sample.WellCoords;
            SampleProperties = Sample.SampleProperties.ToList();
        }

        private bool _isSelected;
        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                SetProperty(ref _isSelected, value);
            }
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

        private string? _plateName;
        public string? PlateName
        {
            get => _plateName;
            set
            {
                SetProperty(ref _plateName, value);
            }
        }

        public WellCoords? _wellCoords;
        public WellCoords? WellCoords
        {
            get => _wellCoords;
            set
            {
                SetProperty(ref _wellCoords, value);
                this.OnPropertyChanged(nameof(WellName));
            }
        }
        public string WellName
        {
            get => WellCoords.ToWellName();
        }

        public Guid Id => Sample.Id;

        public bool HasChanges
        { 
            get
            {
                return ((SampleName != Sample.Name) ||
                        (PlateName != Sample.PlateName) ||
                        (WellCoords.ToWellName() != Sample.WellCoords.ToWellName()));
            }
        }

        public List<IProperty> SampleProperties { get; }

        public ISample Sample { get; }

        public static SampleVm Empty =>
            new SampleVm(Models.BasicTypes.Sample.Empty);
    }

    public static class SampleVmExt
    {
        public static SampleVm ToSampleVm(this ISample sample)
        {
            return new SampleVm(sample);
        }

        public static IEnumerable<DataGridColumnInfo> FixedColumnInfo
        {
            get
            {
                yield return new DataGridColumnInfo(
                    binding: "SampleName", 
                    header: "Name");
                yield return new DataGridColumnInfo(
                    binding: "PlateName",
                    header: "Plate");
                yield return new DataGridColumnInfo(
                    binding: "WellName",
                    header: "Well");
            }
        }
    }
}
