using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using PlateWorld.Models.SamplePlate;
using PlateWorld.Mvvm.Stores;
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


        #region NavHomeCommand

        RelayCommand? _NavHomeCommand;
        public ICommand NavHomeCommand
        {
            get
            {
                Action aa = () => {
                    PageVmBundle.ModalNavigationStore.CurrentViewModel = null;
                    PageVmBundle.NavigationStore.CurrentViewModel =
                    new HomePageVm(PageVmBundle);
                };
                return _NavHomeCommand ?? (_NavHomeCommand =
                    new RelayCommand(
                            aa,
                            () => true
                            ));
            }
        }

        #endregion // NavHomeCommand


        #region NavNewPlateCommand
        public ICommand NavNewPlateCommand
        {
            get
            {
                return CommandUtils.Disabled;
            }
        }

        #endregion // NavNewPlateCommand


        #region NavAddSamplesToPlateCommand

        RelayCommand? _navAddSamplesToPlateCommand;
        public ICommand NavAddSamplesToPlateCommand
        {
            get
            {
                return CommandUtils.Disabled;
            }
        }

        #endregion // NavAddSamplesToPlateCommand


        #region NavAllPlatesCommand

        RelayCommand? _NavAllPlatesCommand;
        public ICommand NavAllPlatesCommand
        {
            get
            {
                Action aa = () => {
                    PageVmBundle.ModalNavigationStore.CurrentViewModel = null;
                    PageVmBundle.NavigationStore.CurrentViewModel =
                        new AllPlatesPageVm(PageVmBundle, null);
                };
                return _NavAllPlatesCommand ?? (_NavAllPlatesCommand =
                    new RelayCommand( aa, () => true ));
            }
        }

        #endregion // NavAllPlatesCommand


        #region NavAllSamplesCommand

        RelayCommand? _navAllSamplesCommand;
        public ICommand? NavAllSamplesCommand
        {
            get
            {
                Action aa = () => {
                    PageVmBundle.NavigationStore.CurrentViewModel =
                    new AllSamplesPageVm(PageVmBundle);
                };
                return _navAllSamplesCommand ?? (_navAllSamplesCommand =
                    new RelayCommand(
                            aa,
                            () => true
                            ));
            }
        }

        #endregion // NavAllSamplesCommand


        #region NavNewSamplesCommand

        RelayCommand? _navNewSamplesCommand;
        public ICommand? NavNewSamplesCommand
        {
            get
            {
                Action aa = () => {
                    PageVmBundle.NavigationStore.CurrentViewModel =
                        new NewSamplesPageVm(PageVmBundle);
                };
                return _navNewSamplesCommand ?? (_navNewSamplesCommand =
                    new RelayCommand(
                            aa,
                            () => true
                            ));
            }
        }

        #endregion // NavNewSamplesCommand


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
                    PageVmBundle.PlateStore.AddPlates(new[] { newPlate });

                    PageVmBundle.ModalNavigationStore.CurrentViewModel = null;
                    PageVmBundle.NavigationStore.CurrentViewModel = 
                        new AddSamplesToPlatePageVm(PageVmBundle, newPlate);
                };
                return _submitCommand ?? 
                       (_submitCommand = new RelayCommand(aa, () => Validate() ));
            }
        }

        #endregion // SubmitCommand



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
