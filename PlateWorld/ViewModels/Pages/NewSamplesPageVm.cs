using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using PlateWorld.Mvvm.Stores;
using PlateWorld.ViewModels.BasicTypes;
using System;
using System.Linq;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace PlateWorld.ViewModels.Pages
{
    public class NewSamplesPageVm : ObservableObject
    {
        NavigationStore NavigationStore { get; }
        ModalNavigationStore ModalNavigationStore { get; }

        public NewSamplesPageVm(
                NavigationStore navigationStore,
                ModalNavigationStore modalNavigationStore,
                DataStore.PlateStore? plates)
        {
            NavigationStore = navigationStore;
            ModalNavigationStore = modalNavigationStore;
            PlateStore = plates;
            AllVms = new ObservableCollection<ConditionSetVm>(
                Models.TestData.ConditionSets.TestConditionSets.Select
                 (cs => new ConditionSetVm(cs)));
        }
        DataStore.PlateStore? PlateStore { get; }


        #region AllConditionSets
        public ObservableCollection<ConditionSetVm> AllVms { get; }
            = new ObservableCollection<ConditionSetVm>();

        private ConditionSetVm _selectedVm;
        public ConditionSetVm SelectedVm
        {
            get => _selectedVm;
            set
            {
                SetProperty(ref _selectedVm, value);
            }
        }

        private int _selectedIndex;
        public int SelectedIndex
        {
            get => _selectedIndex;
            set
            {
                SetProperty(ref _selectedIndex, value);
                _addCommand?.NotifyCanExecuteChanged();
                _removeCommand?.NotifyCanExecuteChanged();
            }
        }

        #endregion //AllConditionSets


        #region SelectedConditionSets
        public ObservableCollection<ConditionSetVm> ChosenVms { get; }
            = new ObservableCollection<ConditionSetVm>();

        private ConditionSetVm _selectedChosenVm;
        public ConditionSetVm SelectedChosenVm
        {
            get => _selectedChosenVm;
            set
            {
                SetProperty(ref _selectedChosenVm, value);
            }
        }

        private int _chosenIndex;
        public int ChosenIndex
        {
            get => _chosenIndex;
            set
            {
                SetProperty(ref _chosenIndex, value);
                _addCommand?.NotifyCanExecuteChanged();
                _removeCommand?.NotifyCanExecuteChanged();
            }
        }

        #endregion //SelectedConditionSets



        #region AddCommand

        RelayCommand? _addCommand;
        public ICommand AddCommand
        {
            get
            {
                Action aa = () =>
                {
                    var addVm = SelectedVm;
                    AllVms.Remove(SelectedVm);
                    ChosenVms.Add(addVm);
                };
                return _addCommand ?? (_addCommand =
                    new RelayCommand(
                            aa,
                            () => SelectedIndex >= 0
                            ));
            }
        }

        #endregion // AddCommand


        #region RemoveCommand

        RelayCommand? _removeCommand;
        public ICommand RemoveCommand
        {
            get
            {
                Action aa = () => {
                    var rmVm = SelectedChosenVm;
                    ChosenVms.Remove(SelectedChosenVm);
                    AllVms.Add(rmVm);
                };
                return _removeCommand ?? (_removeCommand =
                    new RelayCommand
                    (
                        aa,
                        () => ChosenIndex >= 0
                    ));
            }
        }

        #endregion // RemoveCommand





        #region MoveUpCommand

        RelayCommand? _moveUpCommand;
        public ICommand MoveUpCommand
        {
            get
            {
                Action aa = () => 
                {
                    AllVms.Move(SelectedIndex, SelectedIndex - 1); ; 
                };
                return _moveUpCommand ?? (_moveUpCommand =
                    new RelayCommand(
                            aa,
                            () => SelectedIndex > 0
                            ));
            }
        }

        #endregion // MoveUpCommand


        #region MoveDownCommand

        RelayCommand? _moveDownCommand;
        public ICommand MoveDownCommand
        {
            get
            {
                Action aa = () => {
                    AllVms.Move(SelectedIndex, SelectedIndex + 1);
                };
                return _moveDownCommand ?? (_moveDownCommand =
                    new RelayCommand
                    (
                        aa,
                        () => SelectedIndex < (AllVms.Count - 1)
                    ));
            }
        }

        #endregion // MoveDownCommand



        #region NavHomeCommand

        RelayCommand? _NavHomeCommand;
        public ICommand NavHomeCommand
        {
            get
            {
                Action aa = () => { };
                return _NavHomeCommand ?? (_NavHomeCommand =
                    new RelayCommand(
                            aa,
                            () => false
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


        #region NewPlateCancelCommand

        RelayCommand? _newPlateCancelCommand;
        public ICommand NewPlateCancelCommand
        {
            get
            {
                Action aa = () => {
                    ModalNavigationStore.CurrentViewModel = null;
                    NavigationStore.CurrentViewModel =
                    new NewSamplesPageVm(NavigationStore,
                    ModalNavigationStore, PlateStore);
                };
                return _newPlateCancelCommand ?? (_newPlateCancelCommand =
                    new RelayCommand(aa, () => true));
            }
        }

        #endregion // NewPlateSubmitCommand


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
                Action aa = () => { };
                return _navNewSamplesCommand ?? (_navNewSamplesCommand =
                    new RelayCommand(
                            aa,
                            () => false
                            ));
            }
        }

        #endregion // NavNewSamplesCommand


    }
}
