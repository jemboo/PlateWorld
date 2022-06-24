using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using PlateWorld.Mvvm.Stores;
using PlateWorld.ViewModels.BasicTypes;
using System;
using System.Linq;
using System.Collections.ObjectModel;
using System.Windows.Input;
using PlateWorld.Models.BasicTypes;
using System.Threading.Tasks;

namespace PlateWorld.ViewModels.Pages
{
    public class NewSamplesPageVm : ObservableObject
    {
        NavigationStore NavigationStore { get; }
        ModalNavigationStore ModalNavigationStore { get; }

        public NewSamplesPageVm(
                NavigationStore navigationStore,
                ModalNavigationStore modalNavigationStore,
                DataStore.SampleStore sampleStore,
                DataStore.PlateStore plates)
        {
            NavigationStore = navigationStore;
            ModalNavigationStore = modalNavigationStore;
            SampleStore = sampleStore;
            PlateStore = plates;
            AllPropertySetVms = new ObservableCollection<PropertySetVm>(
                Models.TestData._PropertySets.TestPropertySets.Select
                 (cs => new PropertySetVm(cs)));
            _selectedVm = AllPropertySetVms.FirstOrDefault();
            _selectedChosenVm = null;
            _addCommand = new RelayCommand(AddAction, CanAdd);
            _removeCommand = new RelayCommand(RemoveAction, CanRemove);
            _createSamplesCommand = new RelayCommand(CreateSamplesAction, CanCreateSamples);

            if (AllPropertySetVms.Count() > 0)
            {
                SelectedIndex = 0;
            }
        }

        DataStore.SampleStore SampleStore { get; }
        DataStore.PlateStore PlateStore { get; }


        #region AllPropertySets
        public ObservableCollection<PropertySetVm> AllPropertySetVms { get; }
            = new ObservableCollection<PropertySetVm>();

        private PropertySetVm? _selectedVm;
        public PropertySetVm? SelectedVm
        {
            get => _selectedVm;
            set
            {
                SetProperty(ref _selectedVm, value);
            }
        }

        private int _selectedIndex = -1;
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

        #endregion //AllPropertySets


        #region SelectedPropertySets
        public ObservableCollection<PropertySetVm> ChosenPropertySetVms { get; }
            = new ObservableCollection<PropertySetVm>();

        private PropertySetVm? _selectedChosenVm;
        public PropertySetVm? SelectedChosenVm
        {
            get => _selectedChosenVm;
            set
            {
                SetProperty(ref _selectedChosenVm, value);
            }
        }

        private int _chosenIndex = -1;
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

        #endregion //SelectedPropertySets


        #region AddCommand

        RelayCommand _addCommand;
        public ICommand AddCommand
        {
            get
            {
                return _addCommand;
            }
        }

        void AddAction()
        {
            if (SelectedVm == null) return;
            var addVm = SelectedVm;
            AllPropertySetVms.Remove(SelectedVm);
            ChosenPropertySetVms.Add(addVm);
            _addCommand?.NotifyCanExecuteChanged();
            _removeCommand?.NotifyCanExecuteChanged();
            _createSamplesCommand?.NotifyCanExecuteChanged();
            SampleCount = getSampleCount();
        }

        bool CanAdd()
        {
            return SelectedIndex >= 0;
        }

        #endregion // AddCommand


        #region RemoveCommand

        RelayCommand _removeCommand;
        public ICommand RemoveCommand
        {
            get
            {
                return _removeCommand;
            }
        }

        void RemoveAction()
        {
            var rmVm = SelectedChosenVm;
            ChosenPropertySetVms.Remove(SelectedChosenVm);
            AllPropertySetVms.Add(rmVm);
            _addCommand?.NotifyCanExecuteChanged();
            _removeCommand?.NotifyCanExecuteChanged();
            _createSamplesCommand?.NotifyCanExecuteChanged();
            SampleCount = getSampleCount();
        }

        bool CanRemove()
        {
            return ChosenIndex >= 0;
        }

        #endregion // RemoveCommand


        #region CreateSamplesCommand

        RelayCommand _createSamplesCommand;
        public ICommand CreateSamplesCommand
        {
            get
            {
                return _createSamplesCommand;
            }
        }

        async void CreateSamplesAction()
        {
            AllReadyRunning = true;

            StatusMessage = "Making samples ...";


            await MakeSamples();

            StatusMessage = "Done";
            await Task.Delay(1000);
            StatusMessage = "";

            AllReadyRunning = false;
        }

        async Task MakeSamples()
        {
            var firstDex = SampleStore.AllSamples.Count();
            var ps = ChosenPropertySetVms.Select(vm => vm.PropertySet)
                                         .AllSamples(firstDex);
            SampleStore.AddSamples(ps);

            await Task.Delay(1000);

        }

        private bool allReadyRunning;
        bool AllReadyRunning 
        { 
            get => allReadyRunning;
            set
            {
                if (allReadyRunning != value)
                {
                    allReadyRunning = value;
                    _createSamplesCommand.NotifyCanExecuteChanged();
                }
            }
        }

        private string _statusMessage;
        public string StatusMessage
        {
            get => _statusMessage;
            set
            {
                SetProperty(ref _statusMessage, value);
            }
        }

        bool CanCreateSamples()
        {
            return (!AllReadyRunning) && ChosenPropertySetVms.Count > 0;
        }

        #endregion // CreateSamplesCommand


        int getSampleCount()
        {
            return ChosenPropertySetVms.Select(vm => vm.PropertyCount)
                                       .Aggregate((a, x) => a * x);
        }

        private int _sampleCount;
        public int SampleCount
        {
            get => _sampleCount;
            set
            {
                SetProperty(ref _sampleCount, value);
            }
        }


        #region MoveUpCommand

        RelayCommand? _moveUpCommand;
        public ICommand MoveUpCommand
        {
            get
            {
                Action aa = () =>
                {
                    AllPropertySetVms.Move(SelectedIndex, SelectedIndex - 1); ;
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
                Action aa = () =>
                {
                    AllPropertySetVms.Move(SelectedIndex, SelectedIndex + 1);
                };
                return _moveDownCommand ?? (_moveDownCommand =
                    new RelayCommand
                    (
                        aa,
                        () => SelectedIndex < (AllPropertySetVms.Count - 1)
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
                Action aa = () =>
                {
                    ModalNavigationStore.CurrentViewModel =
                    new NewPlatePageVm(NavigationStore,
                                       ModalNavigationStore,
                                       SampleStore,
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
                Action aa = () =>
                {
                    NavigationStore.CurrentViewModel =
                        new AllPlatesPageVm(NavigationStore,
                            ModalNavigationStore,
                            SampleStore, PlateStore, null);
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
                Action aa = () =>
                {
                    ModalNavigationStore.CurrentViewModel = null;
                    NavigationStore.CurrentViewModel =
                        new NewSamplesPageVm(
                            NavigationStore,
                            ModalNavigationStore, SampleStore, PlateStore);
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
                Action aa = () =>
                {
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
