using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using PlateWorld.Models;
using PlateWorld.Mvvm.Stores;
using PlateWorld.ViewModels.PlateParts;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;


namespace PlateWorld.ViewModels.Pages
{
    public class AllPlatesPageVm : ObservableObject
    {
        NavigationStore NavigationStore { get; }
        ModalNavigationStore ModalNavigationStore { get; }
        public AllPlatesPageVm(NavigationStore navigationStore, 
                               ModalNavigationStore modalNavigationStore,
                               DataStore.PlateStore? plateStore, 
                               Plate? selectedPlate)
        {
            NavigationStore = navigationStore;
            ModalNavigationStore = modalNavigationStore;
            PlateStore = plateStore;

            if(PlateStore != null)
            {
                PlateVms = new ObservableCollection<PlateVm>(
                    PlateStore.AllPlates.Select(p => p.ToPlateVm()));
            }

            if (selectedPlate != null)
            {
                SelectedPlateVm = selectedPlate.ToPlateVm();
                if (selectedPlate.RowCount > 20)
                {
                    Zoom = 1;
                }
                else Zoom = 2;
            }
            else
            {
                SelectedPlateVm = Plate.Empty.ToPlateVm();
                Zoom = 2;
            }
        }


        private string _plateName;
        public string PlateName
        {
            get => _plateName;
            set
            {
                SetProperty(ref _plateName, value);
                _changePlateNameCommand?.NotifyCanExecuteChanged();
            }
        }


        DataStore.PlateStore PlateStore { get; }


        private PlateVm? _selectedPlateVm;
        public PlateVm? SelectedPlateVm
        {
            get => _selectedPlateVm;
            set
            {
                SetProperty(ref _selectedPlateVm, value);
                _navAddSamplesToPlateCommand?.NotifyCanExecuteChanged();
                _deleteCommand?.NotifyCanExecuteChanged();
            }
        }


        public ObservableCollection<PlateVm> PlateVms { get; } 
            = new ObservableCollection<PlateVm>();



        private string _validationResult;
        public string ValidationResult
        {
            get => _validationResult;
            set
            {
                SetProperty(ref _validationResult, value);
            }
        }

        bool Validate()
        {
            if (PlateStore.ContainsPlateName(PlateName))
            {
                ValidationResult = $"Plate name is already in use";
                return false;
            }
            ValidationResult = String.Empty;
            return true;
        }

        private double _zoom;
        public double Zoom
        {
            get => _zoom;
            set
            {
                SetProperty(ref _zoom, value);
            }
        }


        #region DeleteCommand

        RelayCommand? _deleteCommand;
        public ICommand DeleteCommand
        {
            get
            {
                Action aa = () => {
                    var selectedPlate = SelectedPlateVm.Plate;
                    PlateVms.Remove(SelectedPlateVm);
                    PlateStore.RemovePlate(selectedPlate);
                };
                return _deleteCommand ?? (_deleteCommand =
                    new RelayCommand(
                            aa,
                            () => (SelectedPlateVm != null)
                            ));
            }
        }

        #endregion // DeleteCommand


        #region NavHomeCommand

        RelayCommand? _NavHomeCommand;
        public ICommand NavHomeCommand
        {
            get
            {
                Action aa = () => {
                    NavigationStore.CurrentViewModel =
                    new HomePageVm(NavigationStore, 
                    ModalNavigationStore, PlateStore);
                };
                return _NavHomeCommand ?? (_NavHomeCommand = 
                    new RelayCommand(
                            aa,
                            () => true
                            ));
            }
        }

        #endregion // NavHomeCommand


        #region NavAddSamplesToPlateCommand

        RelayCommand _navAddSamplesToPlateCommand;
        public ICommand NavAddSamplesToPlateCommand
        {
            get
            {
                Action aa = () => {
                    NavigationStore.CurrentViewModel =
                        new AddSamplesToPlatePageVm(NavigationStore,
                        ModalNavigationStore, PlateStore, SelectedPlateVm?.Plate);
                };
                return _navAddSamplesToPlateCommand ?? (_navAddSamplesToPlateCommand = 
                    new RelayCommand( aa, () => (SelectedPlateVm != null) ));
            }
        }

        #endregion // NavAddSamplesToPlateCommand


        #region EditPlateCancelCommand

        RelayCommand? _editPlateCancelCommand;
        public ICommand EditPlateCancelCommand
        {
            get
            {
                Action aa = () => {
                    ModalNavigationStore.CurrentViewModel = null;
                    NavigationStore.CurrentViewModel =
                    new AllPlatesPageVm(NavigationStore,
                    ModalNavigationStore, PlateStore, SelectedPlateVm?.Plate);
                };
                return _editPlateCancelCommand ?? (_editPlateCancelCommand =
                    new RelayCommand(
                            aa,
                            () => true
                            ));
            }
        }

        #endregion // EditPlateCancelCommand


        #region NavNewPlateCommand

        RelayCommand? _NavNewPlateCommand;
        public ICommand? NavNewPlateCommand
        {
            get
            {
                Action aa = () => {
                    ModalNavigationStore.CurrentViewModel =
                    new NewPlatePageVm(NavigationStore, ModalNavigationStore,
                    PlateStore, NewPlateCancelCommand);
                };
                return _NavNewPlateCommand ?? (_NavNewPlateCommand =
                    new RelayCommand(
                                aa,
                                () => true
                            ));
            }
        }

        #endregion // NavNewPlateCommand


        #region NewPlateCancelCommand

        RelayCommand? _newPlateCancelCommand;
        public ICommand NewPlateCancelCommand
        {
            get
            {
                Action aa = () => {
                    ModalNavigationStore.CurrentViewModel = null;
                    NavigationStore.CurrentViewModel =
                    new AllPlatesPageVm(NavigationStore, 
                    ModalNavigationStore, PlateStore, SelectedPlateVm.Plate);
                };
                return _newPlateCancelCommand ?? (_newPlateCancelCommand =
                    new RelayCommand(
                            aa,
                            () => true
                            ));
            }
        }

        #endregion // NewPlateCancelCommand


        #region NavAllPlatesCommand

        RelayCommand? _NavAllPlatesCommand;
        public ICommand? NavAllPlatesCommand
        {
            get
            {
                Action aa = () => { };
                return _NavAllPlatesCommand ?? (_NavAllPlatesCommand =
                    new RelayCommand(
                            aa,
                            () => false
                            ));
            }
        }

        #endregion // NavAllPlatesCommand


        #region NavAllSamplesCommand

        RelayCommand? _navAllSamplesCommand;
        public ICommand? NavAllSamplesCommand
        {
            get
            {
                Action aa = () => { };
                return _navAllSamplesCommand ?? (_navAllSamplesCommand =
                    new RelayCommand(
                            aa,
                            () => false
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
                Action aa = () => { };
                return _navNewSamplesCommand ?? (_navNewSamplesCommand =
                    new RelayCommand(
                            aa,
                            () => false
                            ));
            }
        }

        #endregion // NavNewSamplesCommand



        #region ChangePlateNameCommand

        RelayCommand? _changePlateNameCommand;
        public ICommand ChangePlateNameCommand
        {
            get
            {
                Action aa = () =>
                {
                    var newPlate = SelectedPlateVm.Plate.NewName(SelectedPlateVm.Name);
                    PlateStore.AddPlate(newPlate);

                    ModalNavigationStore.CurrentViewModel = null;
                    NavigationStore.CurrentViewModel = new AddSamplesToPlatePageVm(
                        NavigationStore, ModalNavigationStore, PlateStore, newPlate);
                };
                return _changePlateNameCommand ??
                       (_changePlateNameCommand = new RelayCommand(aa, () => Validate()));
            }
        }

        #endregion // SubmitCommand

    }
}
