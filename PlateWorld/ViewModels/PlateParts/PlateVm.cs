using Microsoft.Toolkit.Mvvm.ComponentModel;
using PlateWorld.Models.SamplePlate;
using PlateWorld.Mvvm.Commands;
using PlateWorld.ViewModels.DragDrop;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;

namespace PlateWorld.ViewModels.PlateParts
{
    public class PlateVm : ObservableObject
    {
        public PlateVm(IPlate plate,
                       DataStore.PlateStore plateStore,
                       UndoRedoService undoRedoService)
        {
            Plate = plate ?? throw new Exception("Plate was null");
            PlateStore = plateStore;
            UndoRedoService = undoRedoService;

            _name = plate.Name;
            _validationResult = string.Empty;

            WellVms = new ObservableCollection<WellVm>();
            ResetWellVms();

            HorizontalMarginVm = new PlateMarginVm(Orientation.Horizontal, Plate.ColumnCount);
            VerticalMarginVm = new PlateMarginVm(Orientation.Vertical, Plate.RowCount);
        }

        UndoRedoService UndoRedoService { get; set; }

        #region AddSampleToWell

        void AddSampleToWell(WellVm oldWellVm, WellVm newWellVm, SampleVm sampleVm)
        {
            Action redoAction = () => MoveSample(oldWellVm, newWellVm, sampleVm);
            Action undoAction = () => MoveSample(newWellVm, oldWellVm, sampleVm);
            UndoRedoService.Push(
                undoAction, $"Move sample",
                redoAction, $"Unmove sample");
        }

        void MoveSample(WellVm oldWellVm, WellVm newWellVm, SampleVm sampleVm)
        {
            if (oldWellVm != null)
            {
                oldWellVm.SampleVm = null;
                sampleVm.WellCoords = null;
                sampleVm.PlateName = String.Empty;
            }
            if (newWellVm != null)
            {
                newWellVm.SampleVm = sampleVm;
                sampleVm.WellCoords = newWellVm.Well.WellCoords;
                sampleVm.PlateName = newWellVm.PlateName;
            }
            CheckForChanges();
        }

        #endregion
        void ResetWellVms()
        {
            WellVms.Clear();
            foreach (var w in Plate.Wells)
            {
                WellVms.Add(new WellVm(w, Plate.Name, AddSampleToWell));
            }
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
            ResetWellVms();
            Name = Plate.Name;
            CheckForChanges();
        }

        public static PlateVm Empty  { get; }
                    = new PlateVm(Models.SamplePlate.Plate.Empty, null, null);

        #region DragDrop
        public PlateDragHandler PlateDragHandler { get; }
            = new PlateDragHandler();

        public PlateDropHandler PlateDropHandler { get; }
            = new PlateDropHandler();

        #endregion

        public void SaveChanges()
        {
            var newWells = WellVms.Select(vm => vm.Well.AddSample(vm.SampleVm?.Sample));
            var newPlate = Plate.Update(Name, newWells);
            PlateStore.RemovePlates(new[] { Plate });
            PlateStore.AddPlates(new[] { newPlate });
            Plate = newPlate;
            CheckForChanges();
        }

        void ValidateNameChange()
        {
            if (Plate.Name == Name)
            {
                ValidationResult = String.Empty;
                return;
            }
            if (PlateStore == null)
            {
                ValidationResult = String.Empty;
                return;
            }
            if (PlateStore.ContainsPlateName(Name))
            {
                ValidationResult = $"Plate name is already in use";
                return;
            }
        }


        private string _validationResult;
        public string ValidationResult
        {
            get => _validationResult;
            set
            {
                SetProperty(ref _validationResult, value);
            }
        }
    }

    public static class PlateVmExt
    {
        public static PlateVm ToPlateVm(this IPlate plate,
                                        DataStore.PlateStore plateStore,
                                        UndoRedoService undoRedoService)
        {
            return new PlateVm(plate, plateStore, undoRedoService);
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
        public PlateVmD() : base(TestPlate(), null, null)
        {
        }
        static Plate TestPlate()
        {
            return PlateExt.MakePlate(plateName: "Ralph", rowCount: 8, colCount: 12);
        }
    }
}