using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using PlateWorld.Models.SamplePlate;
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
                               DataStore.PlateStore plateStore, 
                               IPlate? selectedPlate)
        {
            NavigationStore = navigationStore;
            ModalNavigationStore = modalNavigationStore;
            PlateStore = plateStore;

            if(PlateStore != null)
            {
                PlateVms = new ObservableCollection<PlateVm>(
                    PlateStore.AllPlates.Select(p => p.ToPlateVm(PlateStore)));
            }
            if (selectedPlate == null)
            {
                var firstPlate = PlateStore?.AllPlates.FirstOrDefault();
                if(firstPlate != null)
                {
                    SelectedPlateVm = firstPlate.ToPlateVm(PlateStore);
                }
                else
                {
                    Zoom = 4;
                }
            }
            else
            {
                SelectedPlateVm = selectedPlate.ToPlateVm(PlateStore);
            }

            _navAddSamplesToPlateCommand?.NotifyCanExecuteChanged();
        }

        DataStore.PlateStore? PlateStore { get; }

        private PlateVm _selectedPlateVm;
        public PlateVm SelectedPlateVm
        {
            get => _selectedPlateVm;
            set
            {
                _selectedPlateVm?.UndoChanges();

                SetProperty(ref _selectedPlateVm, value);
                if(value != null)
                {
                    Zoom = value.GoodZoomLevel();
                }
                _navAddSamplesToPlateCommand?.NotifyCanExecuteChanged();
                _deleteCommand?.NotifyCanExecuteChanged();
            }
        }


        private int _selectedIndex;
        public int SelectedIndex
        {
            get => _selectedIndex;
            set
            {
                SetProperty(ref _selectedIndex, value);
            }
        }



        public ObservableCollection<PlateVm> PlateVms { get; } 


        private string _validationResult;
        public string ValidationResult
        {
            get => _validationResult;
            set
            {
                SetProperty(ref _validationResult, value);
            }
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
                return _deleteCommand ?? (_deleteCommand =
                    new RelayCommand( deleteAction, deleteIsEnabled ));
            }
        }

        void deleteAction()
        {
            if (SelectedPlateVm == null) return;
            var selectedPlate = SelectedPlateVm.Plate;
            var curDex = PlateVms.IndexOf(SelectedPlateVm);
            PlateVms.Remove(SelectedPlateVm);
            PlateStore.RemovePlates(new[] { selectedPlate });


            SelectedPlateVm = (curDex < 1) ?
                Plate.Empty.ToPlateVm(PlateStore) :
                PlateVms[curDex - 1];
        }

        bool deleteIsEnabled()
        {
            return SelectedPlateVm != null;
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
                Action aa = () => {
                    NavigationStore.CurrentViewModel =
                    new AllSamplesPageVm(NavigationStore,
                    ModalNavigationStore, PlateStore);
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
                    ModalNavigationStore, PlateStore);
                };
                return _navNewSamplesCommand ?? (_navNewSamplesCommand =
                    new RelayCommand(
                            aa,
                            () => true
                            ));
            }
        }

        #endregion // NavNewSamplesCommand

    }
}
