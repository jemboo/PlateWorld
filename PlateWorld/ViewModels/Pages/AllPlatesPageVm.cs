using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using PlateWorld.Models.SamplePlate;
using PlateWorld.Mvvm.Stores;
using PlateWorld.ViewModels.PlateParts;
using PlateWorld.ViewModels.Utils;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using System.Windows.Threading;

namespace PlateWorld.ViewModels.Pages
{
    public class AllPlatesPageVm : ObservableObject
    {
        public AllPlatesPageVm(PageVmBundle pageVmBundle,
                IPlate? selectedPlate)
        {
            PageVmBundle = pageVmBundle;
            if (PageVmBundle.PlateStore != null)
            {
                PlateVms = new ObservableCollection<PlateVm>(
                    PageVmBundle.PlateStore.AllPlates.Select(
                        p => p.ToPlateVm(PageVmBundle.PlateStore, null)));
            }

            if (selectedPlate == null)
            {
                var firstPlate = PageVmBundle.PlateStore?.AllPlates.FirstOrDefault();
                if (firstPlate != null)
                {
                    SelectedPlateVm = firstPlate.ToPlateVm(PageVmBundle?.PlateStore, null);
                }
                else
                {
                    Zoom = 4;
                }
            }
            else
            {
                SelectedPlateVm = selectedPlate.ToPlateVm(PageVmBundle.PlateStore, null);
            }
            _navAddSamplesToPlateCommand?.NotifyCanExecuteChanged();
        }
        public PageVmBundle PageVmBundle { get; }
        public bool NeedsSampleUpdate { get; set; }
        public bool NeedsPlateUpdate { get; set; }

        private PlateVm _selectedPlateVm;
        public PlateVm SelectedPlateVm
        {
            get => _selectedPlateVm;
            set
            {
                _selectedPlateVm?.UndoChanges();

                SetProperty(ref _selectedPlateVm, value);
                if (value != null)
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

        void NavBack()
        {
            PageVmBundle.ModalNavigationStore.CurrentViewModel = null;
            PageVmBundle.NavigationStore.CurrentViewModel = this;
        }

        void NavBackAndPop()
        {
            PageVmBundle.UndoRedoService.PopUndo();
            NavBack();
        }


        #region DeleteCommand

        RelayCommand? _deleteCommand;
        public ICommand DeleteCommand
        {
            get
            {
                return _deleteCommand ?? (_deleteCommand =
                    new RelayCommand(DoTheDelete, deleteIsEnabled));
            }
        }


        void DoTheDelete()
        {
            var curPlateVm = SelectedPlateVm;
            Action redoAction = () => deleteAction(curPlateVm);
            Action undoAction = () => unDeleteAction(curPlateVm);
            PageVmBundle.UndoRedoService.Push(
                undoAction, $"Undo delete for: {curPlateVm.Name}",
                redoAction, $"Delete plate: {curPlateVm.Name}");
        }

        void deleteAction(PlateVm plateVm)
        {
            var curDex = PlateVms.IndexOf(plateVm);
            var selectedPlate = plateVm.Plate;
            PlateVms.RemoveAt(curDex);
            PageVmBundle.PlateStore.RemovePlates(new[] { selectedPlate });
            if(PlateVms.Count() == 0)
            {
                SelectedPlateVm = PlateVm.Empty;
            }
            else if(curDex==0)
            {
                SelectedPlateVm = PlateVms[0];
            }
            else
            {
                SelectedPlateVm = PlateVms[curDex - 1];
            }
        }

        void unDeleteAction(PlateVm plateVm)
        {
            PageVmBundle.PlateStore.AddPlates(new[] { plateVm.Plate });
            PlateVms.Add(plateVm);
            SelectedPlateVm = plateVm;
        }

        bool deleteIsEnabled()
        {
            if (SelectedPlateVm == null) return false;
            return SelectedPlateVm != PlateVm.Empty;
        }

        #endregion // DeleteCommand


        #region NavHomeCommand

        RelayCommand? _navHomeCommand;
        public ICommand NavHomeCommand
        {
            get
            {
                if (_navHomeCommand == null)
                {
                    _navHomeCommand = new RelayCommand(NavHome, () => true);
                }
                return _navHomeCommand;
            }
        }
        void NavHome()
        {
            Action action = () =>
                    PageVmBundle.NavigationStore.CurrentViewModel =
                                new HomePageVm(PageVmBundle);

            PageVmBundle.UndoRedoService.Push(
                NavBack, "Go to All Plates",
                action, "Go to Home");
        }

        #endregion // NavHomeCommand


        #region NavAddSamplesToPlateCommand

        RelayCommand _navAddSamplesToPlateCommand;
        public ICommand NavAddSamplesToPlateCommand
        {
            get
            {
                return _navAddSamplesToPlateCommand ?? (_navAddSamplesToPlateCommand =
                    new RelayCommand(NavAddSamplesToPlate, CanAddSamplesToPlate));
            }
        }

        void NavAddSamplesToPlate()
        {
            Action action = () =>
                    PageVmBundle.NavigationStore.CurrentViewModel =
                                new AddSamplesToPlatePageVm(PageVmBundle, SelectedPlateVm?.Plate);

            PageVmBundle.UndoRedoService.Push(
                NavBack, "Go to All Plates",
                action, "Go to Add Samples to Plate");
        }


        bool CanAddSamplesToPlate()
        {
            if (SelectedPlateVm == null) return false;
            return SelectedPlateVm != PlateVm.Empty;
        }

        #endregion // NavAddSamplesToPlateCommand


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

            PageVmBundle.UndoRedoService.Push(
                NavBack, "Go to All Plates", 
                action, "Go to Make new Plate");
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
                    _navBackCommand = new RelayCommand(NavBackAndPop, () => true);
                }
                return _navBackCommand;
            }
        }

        #endregion // NavBackCommand


        #region NavAllPlatesCommand

        public ICommand? NavAllPlatesCommand
        {
            get
            {
                return CommandUtils.Disabled;
            }
        }

        #endregion // NavAllPlatesCommand


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

            PageVmBundle.UndoRedoService.Push(
                NavBack, "Go to All Plates", 
                action, "Go to All Samples");
        }

        #endregion // NavAllSamplesCommand


        #region NavNewSamplesCommand

        RelayCommand? _navNewSamplesCommand;
        public ICommand? NavNewSamplesCommand
        {
            get
            {
                if (_navNewSamplesCommand == null)
                {
                    _navNewSamplesCommand = new RelayCommand(NavNewSamples, () => true);
                }
                return _navNewSamplesCommand;
            }
        }

        void NavNewSamples()
        {
            Action action = () =>
                 PageVmBundle.NavigationStore.CurrentViewModel =
                        new NewSamplesPageVm(PageVmBundle);

            PageVmBundle.UndoRedoService.Push(
                NavBack, "Go to All Plates", 
                action, "Go to Make new Samples");
        }

        #endregion // NavNewSamplesCommand


    }
}
