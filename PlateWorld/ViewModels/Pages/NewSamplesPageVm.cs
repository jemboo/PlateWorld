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
using PlateWorld.ViewModels.Utils;

namespace PlateWorld.ViewModels.Pages
{
    public class NewSamplesPageVm : ObservableObject
    {
        public NewSamplesPageVm(PageVmBundle pageVmBundle)
        {
            PageVmBundle = pageVmBundle;
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
        public PageVmBundle PageVmBundle { get; }

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
            var firstDex = PageVmBundle.SampleStore.AllSamples.Count();
            var ps = ChosenPropertySetVms.Select(vm => vm.PropertySet)
                                         .AllSamples(firstDex);
            PageVmBundle.SampleStore.AddSamples(ps);

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
        public ICommand NavHomeCommand
        {
            get
            {
                return CommandUtils.Disabled;
            }
        }

        #endregion // NavHomeCommand


        #region NavNewPlateCommand

        RelayCommand? _navNewPlateCommand;
        public ICommand NavNewPlateCommand
        {
            get
            {
                if (_navNewPlateCommand == null)
                {
                    _navNewPlateCommand = new RelayCommand(NavNewPlate, () => true);
                }
                return _navNewPlateCommand;
            }
        }

        void NavNewPlate()
        {
            Action action = () =>
                PageVmBundle.ModalNavigationStore.CurrentViewModel =
                            new NewPlatePageVm(PageVmBundle, NavBackCommand);

            PageVmBundle.UndoRedoService.Push(NavBack, action);
        }

        #endregion // NavNewPlateCommand


        #region NavBackCommand

        RelayCommand? _navBackCommand;
        public ICommand NavBackCommand
        {
            get
            {
                if (_navBackCommand == null)
                {
                    _navBackCommand = new RelayCommand(NavBack, () => true);
                }
                return _navBackCommand;
            }
        }

        void NavBack()
        {
            PageVmBundle.ModalNavigationStore.CurrentViewModel = null;
            PageVmBundle.NavigationStore.CurrentViewModel = 
                new NewSamplesPageVm(PageVmBundle);
        }

        #endregion // NavBackCommand


        #region NavAllPlatesCommand

        RelayCommand? _navAllPlatesCommand;
        public ICommand NavAllPlatesCommand
        {
            get
            {
                if (_navAllPlatesCommand == null)
                {
                    _navAllPlatesCommand = new RelayCommand(NavAllPlates, () => true);
                }
                return _navAllPlatesCommand;
            }
        }

        void NavAllPlates()
        {
            Action action = () =>
                PageVmBundle.NavigationStore.CurrentViewModel =
                            new AllPlatesPageVm(PageVmBundle, null);

            PageVmBundle.UndoRedoService.Push(NavBack, action);
        }


        #endregion // NavAllPlatesCommand



        #region NavAddSamplesToPlateCommand

        public ICommand NavAddSamplesToPlateCommand
        {
            get
            {
                return CommandUtils.Disabled;
            }
        }

        #endregion // NavAddSamplesToPlateCommand


        #region NavAllSamplesCommand

        RelayCommand? _navAllSamplesCommand;
        public ICommand? NavAllSamplesCommand
        {
            get
            {
                if (_navAllSamplesCommand == null)
                {
                    _navAllSamplesCommand = new RelayCommand(NavAllSamples, () => true);
                }
                return _navAllSamplesCommand;
            }
        }

        void NavAllSamples()
        {
            Action action = () =>
                 PageVmBundle.NavigationStore.CurrentViewModel =
                        new AllSamplesPageVm(PageVmBundle);

            PageVmBundle.UndoRedoService.Push(NavBack, action);
        }

        #endregion // NavAllSamplesCommand


        #region NavNewSamplesCommand

        public ICommand? NavNewSamplesCommand
        {
            get
            {
                return CommandUtils.Disabled;
            }
        }

        #endregion // NavNewSamplesCommand


    }
}
