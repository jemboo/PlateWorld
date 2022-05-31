using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using PlateWorld.Models;
using PlateWorld.Mvvm.Stores;
using PlateWorld.ViewModels.PlateParts;
using System;
using System.Windows.Input;

namespace PlateWorld.ViewModels.Pages
{
    public class PlateEditorPageVm : ObservableObject
    {
        NavigationStore NavigationStore { get; }
        ModalNavigationStore ModalNavigationStore { get; }
        public PlateEditorPageVm(NavigationStore navigationStore, 
                ModalNavigationStore modalNavigationStore,
                DataStore.Plates? plates, Plate? plate)
        {
            NavigationStore = navigationStore;
            ModalNavigationStore = modalNavigationStore;
            PlateStore = plates;
            Plate = plate;
            PlateVm = new PlateVm(Plate);
            _plateName = plate.Name;
            _validationResult = String.Empty;
        }
        DataStore.Plates? PlateStore { get; }

        Plate? Plate { get; }

        private PlateVm _plateVm;
        public PlateVm PlateVm
        {
            get => _plateVm;
            set
            {
                SetProperty(ref _plateVm, value);
            }
        }


        #region SubmitCommand

        RelayCommand? _cancelCommand;
        public ICommand CancelCommand
        {
            get
            {
                Action aa = () =>
                {
                    NavigationStore.CurrentViewModel = new PlateEditorPageVm(
                        NavigationStore, ModalNavigationStore, PlateStore, Plate);
                };
                return _cancelCommand ??
                       (_cancelCommand = new RelayCommand(aa, () => EditsWereMade()));
            }
        }

        #endregion // SubmitCommand

        bool EditsWereMade()
        {
            return Plate.Name != PlateName;
        }


        private string _plateName;
        public string PlateName
        {
            get => _plateName;
            set
            {
                SetProperty(ref _plateName, value);
                _savePlateCommand?.NotifyCanExecuteChanged();
                _cancelCommand?.NotifyCanExecuteChanged();
                _navigateHomeCommand.NotifyCanExecuteChanged();
                _navigateNewPlateCommand.NotifyCanExecuteChanged();
                _navigatePlateListCommand.NotifyCanExecuteChanged();
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

        #region NewPlateCancelCommand

        RelayCommand? _newPlateCancelCommand;
        public ICommand NewPlateCancelCommand
        {
            get
            {
                Action aa = () => {
                    ModalNavigationStore.CurrentViewModel = null;
                    NavigationStore.CurrentViewModel =
                    new PlateEditorPageVm(NavigationStore,
                    ModalNavigationStore, PlateStore, Plate);
                };
                return _newPlateCancelCommand ?? (_newPlateCancelCommand =
                    new RelayCommand(
                            aa,
                            () => true
                            ));
            }
        }

        #endregion // NewPlateCancelCommand


        #region SavePlateCommand

        RelayCommand? _savePlateCommand;
        public ICommand SavePlateCommand
        {
            get
            {
                Action aa = () =>
                {
                    var newPlate = Plate.NewName(newName: PlateName);
                    PlateStore.data.Remove(Plate.Name);
                    PlateStore.data[newPlate.Name] = newPlate;
                    NavigationStore.CurrentViewModel = new PlateEditorPageVm(
                        NavigationStore, ModalNavigationStore, PlateStore, newPlate);
                };
                return _savePlateCommand ?? 
                       (_savePlateCommand = new RelayCommand(aa, () => Validate() ));
            }
        }

        #endregion // NavigateHomeCommand


        #region NavigateHomeCommand

        RelayCommand? _navigateHomeCommand;
        public ICommand NavigateHomeCommand
        {
            get
            {
                Action aa = () =>
                {
                    NavigationStore.CurrentViewModel =
                    new HomePageVm(NavigationStore, 
                    ModalNavigationStore, PlateStore);
                };
                return _navigateHomeCommand ?? (_navigateHomeCommand =
                    new RelayCommand( aa, () => !EditsWereMade() ));
            }
        }

        #endregion // NavigateHomeCommand


        #region NavigateNewPlateCommand

        RelayCommand? _navigateNewPlateCommand;
        public ICommand NavigateNewPlateCommand
        {
            get
            {
                Action aa = () =>
                {
                    ModalNavigationStore.CurrentViewModel =
                    new NewPlatePageVm(NavigationStore, ModalNavigationStore, 
                                PlateStore, NewPlateCancelCommand);
                };
                return _navigateNewPlateCommand ?? (_navigateNewPlateCommand =
                    new RelayCommand(
                                aa,
                                () => !EditsWereMade()
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
                    (_navigatePlateEditorCommand = new RelayCommand(aa,
                            () => false ));
            }
        }

        #endregion // NavigatePlateEditorCommand


        #region NavigatePlateListCommand

        RelayCommand? _navigatePlateListCommand;
        public ICommand NavigatePlateListCommand
        {
            get
            {
                Action aa = () =>
                {
                    NavigationStore.CurrentViewModel =
                    new PlateListPageVm(NavigationStore, 
                        ModalNavigationStore, PlateStore, Plate);
                };
                return _navigatePlateListCommand ?? (_navigatePlateListCommand =
                    new RelayCommand(
                            aa,
                            () => !EditsWereMade()
                            ));
            }
        }

        #endregion // NavigatePlateListCommand

        bool Validate()
        {
            if(!EditsWereMade()) return false;

            if ((Plate.Name != PlateName) && 
                (PlateStore.data.ContainsKey(PlateName)))
            {
                ValidationResult = $"Plate name is already in use";
                return false;
            }
            ValidationResult = String.Empty;
            return true;
        }


    }
}
