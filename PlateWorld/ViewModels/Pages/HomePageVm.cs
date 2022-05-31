using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using PlateWorld.Mvvm.Stores;
using System;
using System.Windows.Input;

namespace PlateWorld.ViewModels.Pages
{
    public class HomePageVm : ObservableObject
    {
        NavigationStore NavigationStore { get; }
        ModalNavigationStore ModalNavigationStore { get; }
        public HomePageVm(
                NavigationStore navigationStore, 
                ModalNavigationStore modalNavigationStore,
                DataStore.Plates? plates)
        {
            NavigationStore = navigationStore;
            ModalNavigationStore = modalNavigationStore;
            PlateStore = plates;
        }
        DataStore.Plates? PlateStore { get; }

        #region NavigateHomeCommand

        RelayCommand? _navigateHomeCommand;
        public ICommand NavigateHomeCommand
        {
            get
            {
                Action aa = () => { };
                return _navigateHomeCommand ?? (_navigateHomeCommand = 
                    new RelayCommand(
                            aa,
                            () => false
                            ));
            }
        }

        #endregion // NavigateHomeCommand


        #region NavigateNewPlateCommand

        RelayCommand? _navigateNewPlateCommand;
        public ICommand NavigateNewPlateCommand
        {
            get
            {
                Action aa = () => {
                    ModalNavigationStore.CurrentViewModel =
                    new NewPlatePageVm(NavigationStore, 
                                       ModalNavigationStore,
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


        #region NavigatePlateEditorCommand

        RelayCommand? _navigatePlateEditorCommand;
        public ICommand NavigatePlateEditorCommand
        {
            get
            {
                Action aa = () => { };
                return _navigatePlateEditorCommand ?? 
                    (_navigatePlateEditorCommand = 
                    new RelayCommand( aa,
                            () => false
                            ));
            }
        }

        #endregion // NavigatePlateEditorCommand


        #region NewPlateCancelCommand

        RelayCommand? _newPlateCancelCommand;
        public ICommand NewPlateCancelCommand
        {
            get
            {
                Action aa = () => {
                    ModalNavigationStore.CurrentViewModel = null;
                    NavigationStore.CurrentViewModel =
                    new HomePageVm(NavigationStore, 
                    ModalNavigationStore, PlateStore);
                };
                return _newPlateCancelCommand ?? (_newPlateCancelCommand =
                    new RelayCommand( aa, () => true ));
            }
        }

        #endregion // NewPlateSubmitCommand


        #region NavigatePlateListCommand

        RelayCommand? _navigatePlateListCommand;
        public ICommand NavigatePlateListCommand
        {
            get
            {
                Action aa = () => {
                    NavigationStore.CurrentViewModel =
                    new PlateListPageVm(NavigationStore, 
                    ModalNavigationStore, PlateStore, null);
                };
                return _navigatePlateListCommand ?? (_navigatePlateListCommand = 
                    new RelayCommand(
                            aa,
                            () => true
                            ));
            }
        }

        #endregion // NavigatePlateListCommand

    }
}
