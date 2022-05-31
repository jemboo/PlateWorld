using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using PlateWorld.Models;
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
            DataStore.Plates? plateStore,
            ICommand cancelCommand)
        {
            NavigationStore = navigationStore;
            ModalNavigationStore = modalNavigationStore;
            CancelCommand = cancelCommand;
            PlateStore = plateStore;
            _plateName = "plateName";
            _rowCount = 8;
            _colCount = 12;
            ValidationResult = String.Empty;
        }

        DataStore.Plates? PlateStore { get; }

        #region CancelCommand
        public ICommand? CancelCommand { get; }

        #endregion


        #region NavigateHomeCommand

        RelayCommand? _navigateHomeCommand;
        public ICommand NavigateHomeCommand
        {
            get
            {
                Action aa = () => {
                    ModalNavigationStore.CurrentViewModel = null;
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


        #region NavigateNewPlateCommand

        RelayCommand? _navigateNewPlateCommand;
        public ICommand NavigateNewPlateCommand
        {
            get
            {
                Action aa = () => {
                };
                return _navigateNewPlateCommand ?? (_navigateNewPlateCommand =
                    new RelayCommand(
                                aa,
                                () => false
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
                return _navigatePlateEditorCommand ?? (_navigatePlateEditorCommand = new RelayCommand(
                            aa,
                            () => false
                            ));
            }
        }

        #endregion // NavigatePlateEditorCommand


        #region NavigatePlateListCommand

        RelayCommand? _navigatePlateListCommand;
        public ICommand NavigatePlateListCommand
        {
            get
            {
                //ModalNavigationStore.CurrentViewModel = null;
                Action aa = () => {
                    ModalNavigationStore.CurrentViewModel = null;
                    NavigationStore.CurrentViewModel =
                    new PlateListPageVm(NavigationStore, 
                    ModalNavigationStore, PlateStore, null);
                };
                return _navigatePlateListCommand ?? (_navigatePlateListCommand =
                    new RelayCommand( aa, () => true ));
            }
        }

        #endregion // NavigatePlateListCommand


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
                    PlateStore.data[PlateName] = newPlate;

                    ModalNavigationStore.CurrentViewModel = null;
                    NavigationStore.CurrentViewModel = new PlateEditorPageVm(
                        NavigationStore, ModalNavigationStore, PlateStore, newPlate);
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
            if (PlateStore.data.ContainsKey(PlateName))
            {
                ValidationResult = $"Plate name is already in use";
                return false;
            }
            ValidationResult = String.Empty;
            return true;
        }
    }

}
