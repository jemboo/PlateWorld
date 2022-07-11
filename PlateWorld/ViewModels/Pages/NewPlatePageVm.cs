using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using PlateWorld.Models.SamplePlate;
using PlateWorld.ViewModels.Utils;
using System;
using System.Windows.Input;

namespace PlateWorld.ViewModels.Pages
{
    public class NewPlatePageVm : ObservableObject
    {
        public NewPlatePageVm(
                    PageVmBundle pageVmBundle,
                    ICommand cancelCommand)
        {
            PageVmBundle = pageVmBundle;
            CancelCommand = cancelCommand;
            _plateName = "plateName";
            _rowCount = 8;
            _colCount = 12;
            ValidationResult = String.Empty;
        }
        PageVmBundle PageVmBundle { get; }

        #region CancelCommand
        public ICommand? CancelCommand { get; }

        #endregion

        private string _plateName;
        public string PlateName
        {
            get => _plateName;
            set
            {
                SetProperty(ref _plateName, value);
                _submitCommand?.NotifyCanExecuteChanged();
            }
        }

        private int _rowCount;
        public int RowCount
        {
            get => _rowCount;
            set
            {
                SetProperty(ref _rowCount, value);
                _submitCommand?.NotifyCanExecuteChanged();
            }
        }

        private int _colCount;
        public int ColCount
        {
            get => _colCount;
            set
            {
                SetProperty(ref _colCount, value);
                _submitCommand?.NotifyCanExecuteChanged();
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

        #region SubmitCommand

        RelayCommand? _submitCommand;
        public ICommand SubmitCommand
        {
            get
            {
                Action aa = () =>
                {
                    var newPlate = PlateExt.MakePlate(
                            plateName: PlateName,
                            rowCount: RowCount,
                            colCount: ColCount);
                    PageVmBundle.PlateStore.AddPlates(new[] { newPlate } );

                    PageVmBundle.ModalNavigationStore.CurrentViewModel = null;
                    PageVmBundle.NavigationStore.CurrentViewModel = 
                        new AllPlatesPageVm(PageVmBundle, newPlate);
                };
                return _submitCommand ?? 
                      (_submitCommand = new RelayCommand(NewPlateAction, () => Validate() ));
            }
        }

        #endregion // SubmitCommand


        void NewPlateAction()
        {
            var newPlate = PlateExt.MakePlate(
                                    plateName: PlateName,
                                    rowCount: RowCount,
                                    colCount: ColCount);

            PageVmBundle.UndoRedoService.Push(
                () => RemoveNewPlate(newPlate), "remove new plate",
                () => MakeNewPlate(newPlate), "make new plate");
        }

        void MakeNewPlate(Plate plate)
        {
            PageVmBundle.PlateStore.AddPlates(new Plate[] { plate } );
            PageVmBundle.ModalNavigationStore.CurrentViewModel = null;
            PageVmBundle.NavigationStore.CurrentViewModel =
                new AllPlatesPageVm(PageVmBundle, plate);
        }

        void RemoveNewPlate(Plate plate)
        {
            PageVmBundle.PlateStore.RemovePlates(new Plate[] { plate } );
            CancelCommand?.Execute(null);
        }

        bool Validate()
        {
            if ((ColCount > 48) || (ColCount < 4))
            {
                ValidationResult = $"Column count must be between 4 and 48";
                return false;
            }

            if ((RowCount > 48) || (RowCount < 4))
            {
                ValidationResult = $"Row count must be between 4 and 48";
                return false;
            }
            if (PageVmBundle.PlateStore.ContainsPlateName(PlateName))
            {
                ValidationResult = $"Plate name is already in use";
                return false;
            }
            ValidationResult = String.Empty;
            return true;
        }
    }

}
