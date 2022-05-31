using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using PlateWorld.Models;
using PlateWorld.Mvvm.Stores;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;


namespace PlateWorld.ViewModels.Pages
{
    public class PlateListPageVm : ObservableObject
    {
        NavigationStore NavigationStore { get; }
        ModalNavigationStore ModalNavigationStore { get; }
        public PlateListPageVm(NavigationStore navigationStore, 
                               ModalNavigationStore modalNavigationStore,
                               DataStore.Plates? plateStore, Plate? selectedPlate)
        {
            NavigationStore = navigationStore;
            ModalNavigationStore = modalNavigationStore;
            PlateStore = plateStore;
            Plates = new ObservableCollection<Plate>(PlateStore?.data?.Values);
            _selectedPlate = selectedPlate;
        }

        DataStore.Plates? PlateStore { get; }

        private Plate? _selectedPlate;
        public Plate? SelectedPlate
        {
            get => _selectedPlate;
            set
            {
                SetProperty(ref _selectedPlate, value);
                _navigatePlateEditorCommand?.NotifyCanExecuteChanged();
                _deleteCommand?.NotifyCanExecuteChanged();
            }
        }

        public ObservableCollection<Plate> Plates { get; }


        #region DeleteCommand

        RelayCommand? _deleteCommand;
        public ICommand DeleteCommand
        {
            get
            {
                Action aa = () => {
                    var sp = SelectedPlate;
                    Plates.Remove(sp);
                    PlateStore.data.Remove(sp.Name);
                };
                return _deleteCommand ?? (_deleteCommand =
                    new RelayCommand(
                            aa,
                            () => (SelectedPlate != null)
                            ));
            }
        }

        #endregion // DeleteCommand


        #region NavigateHomeCommand

        RelayCommand? _navigateHomeCommand;
        public ICommand NavigateHomeCommand
        {
            get
            {
                Action aa = () => {
                    NavigationStore.CurrentViewModel =
                    new HomePageVm(NavigationStore, 
                    ModalNavigationStore, PlateStore);
                };
                return _navigateHomeCommand ?? (_navigateHomeCommand = 
                    new RelayCommand(
                            aa,
                            () => true
                            ));
            }
        }

        #endregion // NavigateHomeCommand


        #region NavigatePlateEditorCommand

        RelayCommand _navigatePlateEditorCommand;
        public ICommand NavigatePlateEditorCommand
        {
            get
            {
                Action aa = () => {
                    NavigationStore.CurrentViewModel =
                        new PlateEditorPageVm(NavigationStore,
                        ModalNavigationStore, PlateStore, SelectedPlate);
                };
                return _navigatePlateEditorCommand ?? (_navigatePlateEditorCommand = 
                    new RelayCommand(
                            aa,
                            () => (SelectedPlate != null)
                            ));
            }
        }

        #endregion // NavigatePlateEditorCommand


        #region EditPlateCancelCommand

        RelayCommand? _editPlateCancelCommand;
        public ICommand EditPlateCancelCommand
        {
            get
            {
                Action aa = () => {
                    ModalNavigationStore.CurrentViewModel = null;
                    NavigationStore.CurrentViewModel =
                    new PlateListPageVm(NavigationStore,
                    ModalNavigationStore, PlateStore, SelectedPlate);
                };
                return _editPlateCancelCommand ?? (_editPlateCancelCommand =
                    new RelayCommand(
                            aa,
                            () => true
                            ));
            }
        }

        #endregion // NewPlateSubmitCommand


        #region NavigateNewPlateCommand

        RelayCommand? _navigateNewPlateCommand;
        public ICommand? NavigateNewPlateCommand
        {
            get
            {
                Action aa = () => {
                    ModalNavigationStore.CurrentViewModel =
                    new NewPlatePageVm(NavigationStore, ModalNavigationStore,
                    PlateStore, NewPlateCancelCommand);
                };
                return _navigateNewPlateCommand ?? (_navigateNewPlateCommand =
                    new RelayCommand(
                                aa,
                                () => true
                            ));
            }
        }

        #endregion // NavigateNewPlateCommand


        #region NewPlateCancelCommand

        RelayCommand? _newPlateCancelCommand;
        public ICommand NewPlateCancelCommand
        {
            get
            {
                Action aa = () => {
                    ModalNavigationStore.CurrentViewModel = null;
                    NavigationStore.CurrentViewModel =
                    new PlateListPageVm(NavigationStore, 
                    ModalNavigationStore, PlateStore, null);
                };
                return _newPlateCancelCommand ?? (_newPlateCancelCommand =
                    new RelayCommand(
                            aa,
                            () => true
                            ));
            }
        }

        #endregion // NewPlateCancelCommand


        #region NavigatePlateListCommand

        RelayCommand? _navigatePlateListCommand;
        public ICommand? NavigatePlateListCommand
        {
            get
            {
                Action aa = () => { };
                return _navigatePlateListCommand ?? (_navigatePlateListCommand =
                    new RelayCommand(
                            aa,
                            () => false
                            ));
            }
        }

        #endregion // NavigatePlateListCommand

    }
}
