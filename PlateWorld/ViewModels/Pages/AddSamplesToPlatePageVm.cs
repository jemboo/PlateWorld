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
        NavigationStore NavigationStore { get; }
        ModalNavigationStore ModalNavigationStore { get; }

        public SamplesDragHandler SamplesDragHandler { get; set; } 
            = new SamplesDragHandler();

        public SamplesDropHandler SamplesDropHandler { get; set; } 
            = new SamplesDropHandler();

        public AddSamplesToPlatePageVm(
                NavigationStore navigationStore, 
                ModalNavigationStore modalNavigationStore,
                DataStore.SampleStore sampleStore,
                DataStore.PlateStore plateStore, 
                IPlate? plate)
        {
            NavigationStore = navigationStore;
            ModalNavigationStore = modalNavigationStore;
            PlateStore = plateStore;
            SampleStore = sampleStore;
            if (SampleStore != null)
            {
                _sampleVms = new ObservableCollection<SampleVm>(
                    SampleStore.AllSamples.Select(
                            s => s.ToSampleVm(this)));
                var propTypes = SampleStore.AllSamples.GetPropertySets()
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
                PlateVm = new PlateVm(Plate, PlateStore);
                if (Plate.RowCount > 20)
                {
                    Zoom = 1;
                }
            }
        }

        public void Update(WellVm entity)
        {
            NeedsPlateUpdate = true; 
            NotifyCommands();
        }

        public void Update(SampleVm entity)
        {
            NeedsSampleUpdate = true;
            NotifyCommands();
        }

        public bool NeedsSampleUpdate { get; set; }
        public bool NeedsPlateUpdate { get; set; }
        DataStore.PlateStore PlateStore { get; }
        DataStore.SampleStore SampleStore { get; }

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



        #region NewPlateCancelCommand

        RelayCommand? _newPlateCancelCommand;
        public ICommand NewPlateCancelCommand
        {
            get
            {
                Action aa = () => {
                    ModalNavigationStore.CurrentViewModel = null;
                    NavigationStore.CurrentViewModel =
                    new AddSamplesToPlatePageVm(
                        NavigationStore,
                        ModalNavigationStore,
                        SampleStore, PlateStore, Plate);
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
                (PlateStore.ContainsPlateName(PlateName)))
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
            Reset();
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
                Action aa = () =>
                {
                    NavigationStore.CurrentViewModel =
                    new HomePageVm(NavigationStore, 
                    ModalNavigationStore, SampleStore, PlateStore);
                };
                return _navHomeCommand ?? (_navHomeCommand =
                    new RelayCommand( aa, () => !EditsWereMade() ));
            }
        }

        #endregion // NavHomeCommand


        #region NavNewPlateCommand

        RelayCommand? _navNewPlateCommand;
        public ICommand NavNewPlateCommand
        {
            get
            {
                Action aa = () =>
                {
                    ModalNavigationStore.CurrentViewModel =
                    new NewPlatePageVm(NavigationStore, ModalNavigationStore, 
                                SampleStore,
                                PlateStore, NewPlateCancelCommand);
                };
                return _navNewPlateCommand ?? (_navNewPlateCommand =
                    new RelayCommand(
                                aa,
                                () => !EditsWereMade()
                            ));
            }
        }

        #endregion // NavNewPlateCommand


        #region NavAddSamplesToPlateCommand

        RelayCommand _navAddSamplesToPlateCommand;
        public ICommand NavAddSamplesToPlateCommand
        {
            get
            {
                return _navAddSamplesToPlateCommand ?? (_navAddSamplesToPlateCommand =
                    new RelayCommand(Reset, CanAddSamplesToPlate));
            }
        }

        void Reset()
        {
            NavigationStore.CurrentViewModel =
                new AddSamplesToPlatePageVm(NavigationStore,
                        ModalNavigationStore,
                        SampleStore, PlateStore, Plate);
        }


        bool CanAddSamplesToPlate()
        {
            return false;
        }

        #endregion // NavAddSamplesToPlateCommand


        #region NavAllPlatesCommand

        RelayCommand? _navAllPlatesCommand;
        public ICommand NavAllPlatesCommand
        {
            get
            {
                Action aa = () =>
                {
                    NavigationStore.CurrentViewModel =
                        new AllPlatesPageVm(NavigationStore, 
                            ModalNavigationStore, SampleStore, PlateStore, Plate);
                };
                return _navAllPlatesCommand ?? (_navAllPlatesCommand =
                    new RelayCommand(
                            aa,
                            () => !EditsWereMade()
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
                Action aa = () => {
                    NavigationStore.CurrentViewModel =
                        new AllSamplesPageVm(NavigationStore,
                                ModalNavigationStore,
                                SampleStore, PlateStore);
                };
                return _navAllSamplesCommand ?? (_navAllSamplesCommand =
                    new RelayCommand(
                            aa,
                            () => !EditsWereMade()
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
                Action aa = () => 
                {
                    NavigationStore.CurrentViewModel =
                    new NewSamplesPageVm(NavigationStore,
                    ModalNavigationStore, SampleStore, PlateStore);
                };
                return _navNewSamplesCommand ?? (_navNewSamplesCommand =
                    new RelayCommand(
                            aa,
                            () => !EditsWereMade()
                            ));
            }
        }

        #endregion // NavNewSamplesCommand


    }
}
