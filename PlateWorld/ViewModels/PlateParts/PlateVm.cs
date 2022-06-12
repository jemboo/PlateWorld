using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using PlateWorld.Models.SamplePlate;
using PlateWorld.ViewModels.DragDrop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;

namespace PlateWorld.ViewModels.PlateParts
{
    public class PlateVm : ObservableObject
    {
        public PlateVm(IPlate plate,
                        DataStore.PlateStore plateStore)
        {
            Plate = plate ?? throw new Exception("Plate was null");
            PlateStore = plateStore;
            _name = plate.Name;
            _validationResult = string.Empty;
            _changePlateNameCommand = new RelayCommand(DoNameChange, ValidNameChange);
            RowCount = Plate.RowCount;
            ColumnCount = Plate.ColumnCount;
            WellVms = Plate.Wells.Select(w => new WellVm(w)).ToList();
            foreach (var w in WellVms)
            {
                w.PropertyChanged += W_PropertyChanged;
            }

            HorizontalMarginVm = new PlateMarginVm(Orientation.Horizontal, RowCount);
            VerticalMarginVm = new PlateMarginVm(Orientation.Vertical, ColumnCount);
        }

        DataStore.PlateStore PlateStore { get; }

        private void W_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {

        }

        string _name;
        public string Name
        {
            get => _name;
            set
            {
                SetProperty(ref _name, value);
                _changePlateNameCommand?.NotifyCanExecuteChanged();
            }
        }
        public int RowCount { get; }
        public int ColumnCount { get; }
        public IPlate Plate { get; private set; }
        public List<WellVm> WellVms { get; }
        public PlateMarginVm HorizontalMarginVm { get; }
        public PlateMarginVm VerticalMarginVm { get; }
        public bool HasChanges
        {
            get
            {
                return ((Name != Plate.Name) ||
                        (Name != Plate.Name));
            }
        }

        public void UndoChanges()
        {
            Name = Plate.Name;
        }

        public static PlateVm Empty =>
            new PlateVm(Models.SamplePlate.Plate.Empty, null);


        #region DragDrop
        public PlateDragHandler PlateDragHandler { get; set; }
            = new PlateDragHandler();

        public PlateDropHandler PlateDropHandler { get; set; }
            = new PlateDropHandler();

        #endregion

        #region ChangePlateNameCommand

        RelayCommand? _changePlateNameCommand;
        public ICommand ChangePlateNameCommand
        {
            get { return _changePlateNameCommand; }
        }
        void DoNameChange()
        {
            var newPlate = Plate.NewName(Name);
            PlateStore.RemovePlates(new[] { Plate });
            PlateStore.AddPlates(new[] { newPlate });
            Plate = newPlate;
            _changePlateNameCommand?.NotifyCanExecuteChanged();
        }

        bool ValidNameChange()
        {
            if (PlateStore == null)
            {
                ValidationResult = String.Empty;
                return false;
            }
            if (Plate.Name == Name)
            {
                ValidationResult = String.Empty;
                return false;
            }

            if (PlateStore.ContainsPlateName(Name))
            {
                ValidationResult = $"Plate name is already in use";
                return false;
            }
            ValidationResult = String.Empty;
            return true;
        }

        #endregion // ChangePlateNameCommand

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