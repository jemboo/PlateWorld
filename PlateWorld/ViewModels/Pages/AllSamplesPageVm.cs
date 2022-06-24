using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using PlateWorld.Mvvm.Stores;
using PlateWorld.ViewModels.PlateParts;
using System;
using System.Linq;
using System.Collections.ObjectModel;
using System.Windows.Input;
using PlateWorld.ViewModels.Utils;
using System.Collections.Generic;
using PlateWorld.Models.BasicTypes;

namespace PlateWorld.ViewModels.Pages
{
    public class AllSamplesPageVm : ObservableObject, IUpdater<SampleVm>
    {
        NavigationStore NavigationStore { get; }
        ModalNavigationStore ModalNavigationStore { get; }

        public AllSamplesPageVm(
                NavigationStore navigationStore,
                ModalNavigationStore modalNavigationStore,
                DataStore.SampleStore sampleStore,
                DataStore.PlateStore plates)
        {
            NavigationStore = navigationStore;
            ModalNavigationStore = modalNavigationStore;
            PlateStore = plates;
            SampleStore = sampleStore; 
            if (SampleStore != null)
            {
                _sampleVms = new ObservableCollection<SampleVm>(
                    SampleStore.AllSamples.Select(s => s.ToSampleVm(this)));

                var propTypes = SampleStore.AllSamples.GetPropertySets()
                                           .Select(ps=>ps.PropertyType)
                                           .ToList();

                var fixedCols = SampleVmExt.FixedColumnInfo;
                var extraCols = propTypes.MakeDataGridColumnInfo("SampleProperties");
                ColumnInfo = fixedCols.Concat(extraCols).ToList();
            }
        }

        public void Update(SampleVm entity)
        {
            NeedsSampleUpdate = true;
        }

        public bool NeedsSampleUpdate { get; set; }
        public bool NeedsPlateUpdate { get; set; }

        DataStore.SampleStore SampleStore { get; }
        DataStore.PlateStore PlateStore { get; }

        List<DataGridColumnInfo> _columnInfo;
        public List<DataGridColumnInfo> ColumnInfo
        {
            get => _columnInfo;
            set
            {
                SetProperty(ref _columnInfo, value);
            }
        }


        ObservableCollection<SampleVm> _sampleVms;
        public ObservableCollection<SampleVm> SampleVms
        {
            get { return _sampleVms; }
        }


        #region UpdateCommand

        RelayCommand? _updateCommand;
        public ICommand UpdateCommand
        {
            get
            {
                return _updateCommand ?? (_updateCommand =
                    new RelayCommand(UpdateAction, CanUpdate));
            }
        }

        void UpdateAction()
        {
            NeedsPlateUpdate = false;
            NeedsSampleUpdate = false;
        }

        bool CanUpdate()
        {
            return NeedsPlateUpdate || NeedsSampleUpdate;
        }


        #endregion // UpdateCommand


        #region NavHomeCommand

        RelayCommand? _NavHomeCommand;
        public ICommand NavHomeCommand
        {
            get
            {
                Action aa = () => {
                    NavigationStore.CurrentViewModel =
                    new HomePageVm(NavigationStore,
                    ModalNavigationStore, SampleStore, PlateStore);
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
                    ModalNavigationStore, SampleStore, PlateStore);
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
                Action aa = () => 
                {
                    NavigationStore.CurrentViewModel =
                        new AllPlatesPageVm(NavigationStore,
                        ModalNavigationStore, SampleStore, PlateStore, null);
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
                Action aa = () => 
                {
                    NavigationStore.CurrentViewModel =
                        new NewSamplesPageVm(NavigationStore,
                        ModalNavigationStore, SampleStore, PlateStore);
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
