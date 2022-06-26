using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using PlateWorld.Models.BasicTypes;
using PlateWorld.Models.SamplePlate;
using PlateWorld.Mvvm.Stores;
using PlateWorld.ViewModels.DragDrop;
using PlateWorld.ViewModels.PlateParts;
using PlateWorld.ViewModels.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace PlateWorld.ViewModels.Pages
{
    public class AddSamplesToPlatePageVm : ObservableObject, 
                    IUpdater<SampleVm>, IUpdater<WellVm>
    {
        public SamplesDragHandler SamplesDragHandler { get; set; } 
            = new SamplesDragHandler();

        public SamplesDropHandler SamplesDropHandler { get; set; } 
            = new SamplesDropHandler();

        public AddSamplesToPlatePageVm(
                PageVmBundle pageVmBundle,
                IPlate? plate)
        {
            PageVmBundle = pageVmBundle;
            if (PageVmBundle.SampleStore != null)
            {
                _sampleVms = new ObservableCollection<SampleVm>(
                    PageVmBundle.SampleStore.AllSamples.Select(
                            s => s.ToSampleVm(this)));
                var propTypes = PageVmBundle.SampleStore.AllSamples.GetPropertySets()
                                           .Select(ps => ps.PropertyType)
                                           .ToList();
                var fixedCols = SampleVmExt.FixedColumnInfo;
                var extraCols = propTypes.MakeDataGridColumnInfo("SampleProperties");
                ColumnInfo = fixedCols.Concat(extraCols).ToList();

            }
            Plate = plate;
            _plateName = (plate == null) ? String.Empty : plate.Name;
            _validationResult = String.Empty;

            Zoom = 2;
            if (Plate != null)
            {
                PlateVm = new PlateVm(Plate, PageVmBundle.PlateStore);
                if (Plate.RowCount > 20)
                {
                    Zoom = 1;
                }
            }
        }
        public PageVmBundle PageVmBundle { get; }

        public void Update(WellVm theOld, WellVm theNew)
        {
            NeedsPlateUpdate = true; 
            NotifyCommands();
        }

        public void Update(SampleVm theOld, SampleVm theNew)
        {
            NeedsSampleUpdate = true;
            NotifyCommands();
        }

        public bool NeedsSampleUpdate { get; set; }
        public bool NeedsPlateUpdate { get; set; }

        List<DataGridColumnInfo> _columnInfo;
        public List<DataGridColumnInfo> ColumnInfo
        {
            get => _columnInfo;
            set
            {
                SetProperty(ref _columnInfo, value);
            }
        }


        IPlate? Plate { get; }

        private PlateVm? _plateVm;
        public PlateVm? PlateVm
        {
            get => _plateVm;
            set
            {
                SetProperty(ref _plateVm, value);
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


        ObservableCollection<SampleVm> _sampleVms;
        public ObservableCollection<SampleVm> SampleVms
        {
            get { return _sampleVms; }
        }

        private string _plateName;
        public string PlateName
        {
            get => _plateName;
            set
            {
                SetProperty(ref _plateName, value);
                NotifyCommands();
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

        void NotifyCommands()
        {
            _savePlateCommand?.NotifyCanExecuteChanged();
            _clearChangesCommand?.NotifyCanExecuteChanged();
            _navHomeCommand?.NotifyCanExecuteChanged();
            _navNewPlateCommand?.NotifyCanExecuteChanged();
            _navAddSamplesToPlateCommand?.NotifyCanExecuteChanged();
            _navAllPlatesCommand?.NotifyCanExecuteChanged();
            _navAllSamplesCommand?.NotifyCanExecuteChanged();
            _navNewSamplesCommand?.NotifyCanExecuteChanged();
        }


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
                new AddSamplesToPlatePageVm(PageVmBundle, Plate);
        }

        #endregion // NavBackCommand


        #region SavePlateCommand

        RelayCommand? _savePlateCommand;
        public ICommand SavePlateCommand
        {
            get
            {
                return _savePlateCommand ?? 
                       (_savePlateCommand = new RelayCommand(SaveThePlate, Validate));
            }
        }

        void SaveThePlate()
        {
            //var newPlate = Plate?.NewName(newName: PlateName);
            //PlateStore.AddPlates(new[] { newPlate });

            NeedsSampleUpdate = false;
            NeedsPlateUpdate = false;
            NotifyCommands();
        }

        bool EditsWereMade()
        {
            return (Plate?.Name != PlateName) ||
                    NeedsSampleUpdate ||
                    NeedsPlateUpdate;
        }

        bool Validate()
        {
            if (!EditsWereMade()) return false;

            if ((Plate?.Name != PlateName) &&
                (PageVmBundle.PlateStore.ContainsPlateName(PlateName)))
            {
                ValidationResult = $"Plate name is already in use";
                return false;
            }
            ValidationResult = String.Empty;
            return true;
        }

        #endregion // SavePlateCommand


        #region ClearChangesCommand

        RelayCommand? _clearChangesCommand;
        public ICommand ClearChangesCommand
        {
            get
            {
                return _clearChangesCommand ??
                       (_clearChangesCommand = new RelayCommand(ClearChanges, CanClearChanges));
            }
        }

        void ClearChanges()
        {
            //Reset();
        }

        bool CanClearChanges()
        {
            var res = Validate();
            return res;
        }
        #endregion // ClearChangesCommand


        #region NavHomeCommand

        RelayCommand? _navHomeCommand;
        public ICommand NavHomeCommand
        {
            get
            {
                if (_navHomeCommand == null)
                {
                    _navHomeCommand = new RelayCommand(NavHome, () => !EditsWereMade());
                }
                return _navHomeCommand;
            }
        }
        void NavHome()
        {
            Action action = () =>
                    PageVmBundle.NavigationStore.CurrentViewModel =
                                new HomePageVm(PageVmBundle);

            PageVmBundle.UndoRedoService.Push(NavBack, action);
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
                    _navNewPlateCommand = new RelayCommand(NavNewPlate, () => !EditsWereMade());
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


        #region NavAddSamplesToPlateCommand

        RelayCommand _navAddSamplesToPlateCommand;
        public ICommand NavAddSamplesToPlateCommand
        {
            get
            {
                return CommandUtils.Disabled;
            }
        }

        #endregion // NavAddSamplesToPlateCommand


        #region NavAllPlatesCommand

        RelayCommand? _navAllPlatesCommand;
        public ICommand NavAllPlatesCommand
        {
            get
            {
                if (_navAllPlatesCommand == null)
                {
                    _navAllPlatesCommand = new RelayCommand(NavAllPlates, () => !EditsWereMade());
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


        #region NavAllSamplesCommand

        RelayCommand? _navAllSamplesCommand;
        public ICommand? NavAllSamplesCommand
        {
            get
            {
                if (_navAllSamplesCommand == null)
                {
                    _navAllSamplesCommand = new RelayCommand(NavAllSamples, () => !EditsWereMade());
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

        RelayCommand? _navNewSamplesCommand;
        public ICommand? NavNewSamplesCommand
        {
            get
            {
                if (_navNewSamplesCommand == null)
                {
                    _navNewSamplesCommand = new RelayCommand(NavNewSamples, () => !EditsWereMade());
                }
                return _navNewSamplesCommand;
            }
        }

        void NavNewSamples()
        {
            Action action = () =>
                 PageVmBundle.NavigationStore.CurrentViewModel =
                        new NewSamplesPageVm(PageVmBundle);

            PageVmBundle.UndoRedoService.Push(NavBack, action);
        }
        #endregion // NavNewSamplesCommand


    }
}
