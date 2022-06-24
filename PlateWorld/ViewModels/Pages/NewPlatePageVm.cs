using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using PlateWorld.Models.SamplePlate;
using PlateWorld.Mvvm.Stores;
using System;
using System.Windows.Input;

namespace PlateWorld.ViewModels.Pages
{
    public class NewPlatePageVm : ObservableObject
    {
        NavigationStore NavigationStore { get; }
        ModalNavigationStore ModalNavigationStore { get; }
        public NewPlatePageVm(
            NavigationStore navigationStore, 
            ModalNavigationStore modalNavigationStore,
            DataStore.SampleStore sampleStore,
            DataStore.PlateStore plateStore,
            ICommand cancelCommand)
        {
            NavigationStore = navigationStore;
            ModalNavigationStore = modalNavigationStore;
            CancelCommand = cancelCommand;
            PlateStore = plateStore;
            SampleStore = sampleStore;
            _plateName = "plateName";
            _rowCount = 8;
            _colCount = 12;
            ValidationResult = String.Empty;
        }

        DataStore.SampleStore SampleStore { get; }
        DataStore.PlateStore PlateStore { get; }

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
                    ModalNavigationStore.CurrentViewModel = null;
                    NavigationStore.CurrentViewModel =
                    new HomePageVm(NavigationStore, 
                    ModalNavigationStore, SampleStore, PlateStore);
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

        RelayCommand? _NavNewPlateCommand;
        public ICommand NavNewPlateCommand
        {
            get
            {
                Action aa = () => {
                };
                return _NavNewPlateCommand ?? (_NavNewPlateCommand =
                    new RelayCommand(
                                aa,
                                () => false
                            ));
            }
        }

        #endregion // NavNewPlateCommand


        #region NavAddSamplesToPlateCommand

        RelayCommand? _navAddSamplesToPlateCommand;
        public ICommand NavAddSamplesToPlateCommand
        {
            get
            {
                Action aa = () => { };
                return _navAddSamplesToPlateCommand ?? 
                    (_navAddSamplesToPlateCommand = 
                    new RelayCommand( aa, () => false));
            }
        }

        #endregion // NavAddSamplesToPlateCommand


        #region NavAllPlatesCommand

        RelayCommand? _NavAllPlatesCommand;
        public ICommand NavAllPlatesCommand
        {
            get
            {
                //ModalNavigationStore.CurrentViewModel = null;
                Action aa = () => {
                    ModalNavigationStore.CurrentViewModel = null;
                    NavigationStore.CurrentViewModel =
                    new AllPlatesPageVm(NavigationStore, 
                    ModalNavigationStore, SampleStore, PlateStore, null);
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
                    NavigationStore.CurrentViewModel =
                    new AllSamplesPageVm(NavigationStore,
                    ModalNavigationStore, SampleStore, PlateStore);
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
                    NavigationStore.CurrentViewModel =
                        new NewSamplesPageVm(NavigationStore,
                        ModalNavigationStore, SampleStore, PlateStore);
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
                    PlateStore.AddPlates(new[] { newPlate });

                    ModalNavigationStore.CurrentViewModel = null;
                    NavigationStore.CurrentViewModel = 
                        new AddSamplesToPlatePageVm(
                                NavigationStore, ModalNavigationStore,
                                SampleStore, PlateStore, newPlate);
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
            if (PlateStore.ContainsPlateName(PlateName))
            {
                ValidationResult = $"Plate name is already in use";
                return false;
            }
            ValidationResult = String.Empty;
            return true;
        }
    }

}
