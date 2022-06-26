using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using PlateWorld.Models.BasicTypes;
using PlateWorld.ViewModels.PlateParts;
using PlateWorld.ViewModels.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace PlateWorld.ViewModels.Pages
{
    public class AllSamplesPageVm : ObservableObject, IUpdater<SampleVm>
    {
        public AllSamplesPageVm(PageVmBundle pageVmBundle)
        {
            PageVmBundle = pageVmBundle;
            if (PageVmBundle.SampleStore != null)
            {
                _sampleVms = new ObservableCollection<SampleVm>(
                    PageVmBundle.SampleStore.AllSamples.Select(s => s.ToSampleVm(this)));

                var propTypes = PageVmBundle.SampleStore.AllSamples.GetPropertySets()
                                            .Select(ps=>ps.PropertyType)
                                            .ToList();

                var fixedCols = SampleVmExt.FixedColumnInfo;
                var extraCols = propTypes.MakeDataGridColumnInfo("SampleProperties");
                ColumnInfo = fixedCols.Concat(extraCols).ToList();
            }
        }

        public PageVmBundle PageVmBundle { get; }
        public void Update(SampleVm theOld, SampleVm theNew)
        {
            NeedsSampleUpdate = true;
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

            PageVmBundle.UndoRedoService.Push(NavBack, action);
        }

        #endregion // NavHomeCommand


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
            PageVmBundle.NavigationStore.CurrentViewModel 
                = new AllSamplesPageVm(PageVmBundle);
        }

        #endregion // NavBackCommand


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


        #region NavAddSamplesToPlateCommand

        RelayCommand? _navAddSamplesToPlateCommand;
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


        #region NavAllSamplesCommand

        public ICommand? NavAllSamplesCommand
        {
            get
            {
                return CommandUtils.Disabled;
            }
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

            PageVmBundle.UndoRedoService.Push(NavBack, action);
        }

        #endregion // NavNewSamplesCommand

    }
}
