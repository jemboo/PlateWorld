using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using PlateWorld.Mvvm.Stores;
using System;
using System.Windows.Input;


namespace PlateWorld.ViewModels.Pages
{
    public class AllSamplesPageVm : ObservableObject
    {
        NavigationStore NavigationStore { get; }
        ModalNavigationStore ModalNavigationStore { get; }

        public AllSamplesPageVm(
                NavigationStore navigationStore,
                ModalNavigationStore modalNavigationStore,
                DataStore.PlateStore? plates)
        {
            NavigationStore = navigationStore;
            ModalNavigationStore = modalNavigationStore;
            PlateStore = plates;
        }
        DataStore.PlateStore? PlateStore { get; }


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


        #region NewPlateCancelCommand

        RelayCommand? _newPlateCancelCommand;
        public ICommand NewPlateCancelCommand
        {
            get
            {
                Action aa = () => {
                    ModalNavigationStore.CurrentViewModel = null;
                    NavigationStore.CurrentViewModel =
                    new AllSamplesPageVm(NavigationStore,
                    ModalNavigationStore, PlateStore);
                };
                return _newPlateCancelCommand ?? (_newPlateCancelCommand =
                    new RelayCommand(aa, () => true));
            }
        }

        #endregion // NewPlateSubmitCommand


        #region NavNewPlateCommand

        RelayCommand? _NavNewPlateCommand;
        public ICommand NavNewPlateCommand
        {
            get
            {
                Action aa = () => {
                    ModalNavigationStore.CurrentViewModel =
                    new NewPlatePageVm(NavigationStore,
                                       ModalNavigationStore,
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


        #region NavAddSamplesToPlateCommand

        RelayCommand? _navAddSamplesToPlateCommand;
        public ICommand NavAddSamplesToPlateCommand
        {
            get
            {
                Action aa = () => { };
                return _navAddSamplesToPlateCommand ??
                    (_navAddSamplesToPlateCommand =
                    new RelayCommand(aa, () => false));
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
                    NavigationStore.CurrentViewModel =
                    new AllPlatesPageVm(NavigationStore,
                    ModalNavigationStore, PlateStore, null);
                };
                return _NavAllPlatesCommand ?? (_NavAllPlatesCommand =
                    new RelayCommand(
                            aa,
                            () => true
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
                return _navAllSamplesCommand ?? (
                    _navAllSamplesCommand =
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
