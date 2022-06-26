﻿using Microsoft.Toolkit.Mvvm.ComponentModel;
using PlateWorld.Models.SamplePlate;
using PlateWorld.ViewModels.DragDrop;
using PlateWorld.ViewModels.Utils;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;

namespace PlateWorld.ViewModels.PlateParts
{
    public class PlateVm : ObservableObject, IUpdater<WellVm>
    {
        public PlateVm(IPlate plate,
                       DataStore.PlateStore plateStore)
        {
            Plate = plate ?? throw new Exception("Plate was null");
            PlateStore = plateStore;
            _name = plate.Name;
            _validationMessage = string.Empty;

            WellVms = new ObservableCollection<WellVm>();
            ResetWellVms();

            HorizontalMarginVm = new PlateMarginVm(Orientation.Horizontal, Plate.ColumnCount);
            VerticalMarginVm = new PlateMarginVm(Orientation.Vertical, Plate.RowCount);
        }

        void ResetWellVms()
        {
            WellVms.Clear();
            foreach (var w in Plate.Wells)
            {
                WellVms.Add(new WellVm(w, Plate.Name, this));
            }
        }

        public void Update(WellVm theOld, WellVm theNew)
        {
            CheckForChanges();
        }

        DataStore.PlateStore PlateStore { get; }

        string _name;
        public string Name
        {
            get => _name;
            set
            {
                SetProperty(ref _name, value);
                ValidateNameChange();
                CheckForChanges();
            }
        }
        public IPlate Plate { get; private set; }
        public ObservableCollection<WellVm> WellVms { get; private set; }
        public PlateMarginVm HorizontalMarginVm { get; }
        public PlateMarginVm VerticalMarginVm { get; }

        void CheckForChanges()
        {
            HasChanges = (Name != Plate.Name) ||
                          WellVms.Any(w => w.HasChanges);
        }

        bool _hasChanges;
        public bool HasChanges
        {
            get => _hasChanges;
            set
            {
                SetProperty(ref _hasChanges, value);
            }
        }

        public void UndoChanges()
        {
            Name = Plate.Name;
            ResetWellVms();
        }

        public static PlateVm Empty =>
            new PlateVm(Models.SamplePlate.Plate.Empty, null);


        #region DragDrop
        public PlateDragHandler PlateDragHandler { get; set; }
            = new PlateDragHandler();

        public PlateDropHandler PlateDropHandler { get; set; }
            = new PlateDropHandler();

        #endregion


        public void SaveChanges()
        {
            var newWells = WellVms.Select(vm => vm.Well.AddSample(vm.SampleVm?.Sample));
            var newPlate = Plate.Update(Name, newWells);
            PlateStore.RemovePlates(new[] { Plate });
            PlateStore.AddPlates(new[] { newPlate });
            Plate = newPlate;
        }

        void ValidateNameChange()
        {
            if (Plate.Name == Name)
            {
                ValidationMessage = String.Empty;
                return;
            }
            if (PlateStore == null)
            {
                ValidationMessage = String.Empty;
                return;
            }
            if (PlateStore.ContainsPlateName(Name))
            {
                ValidationMessage = $"Plate name is already in use";
                return;
            }
        }


        private string _validationMessage;
        public string ValidationMessage
        {
            get => _validationMessage;
            set
            {
                SetProperty(ref _validationMessage, value);
            }
        }

    }

    public static class PlateVmExt
    {
        public static PlateVm ToPlateVm(this IPlate plate,
                                        DataStore.PlateStore plateStore)
        {
            return new PlateVm(plate, plateStore);
        }

        public static int GoodZoomLevel(this PlateVm vm)
        {
            if (vm.Plate.RowCount > 20)
            {
                return 1;
            }
            else return 2;
        }
    }

    public class PlateVmD : PlateVm
    {
        public PlateVmD() : base(TestPlate(), null)
        {
        }
        static Plate TestPlate()
        {
            return PlateExt.MakePlate(plateName: "Ralph", rowCount: 8, colCount: 12);
        }
    }
}