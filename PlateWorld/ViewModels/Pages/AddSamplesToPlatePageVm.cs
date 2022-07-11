using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.Xaml.Behaviors;
using PlateWorld.Models.BasicTypes;
using PlateWorld.Models.SamplePlate;
using PlateWorld.ViewModels.DragDrop;
using PlateWorld.ViewModels.PlateParts;
using PlateWorld.ViewModels.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PlateWorld.ViewModels.Pages
{
    public class AddSamplesToPlatePageVm : ObservableObject
    {
        public SamplesDragHandler SamplesDragHandler { get; } 
            = new SamplesDragHandler();

        public SamplesDropHandler SamplesDropHandler { get; } 
            = new SamplesDropHandler();

        public AddSamplesToPlatePageVm(PageVmBundle pageVmBundle,
                                       IPlate plate)
        {
            PageVmBundle = pageVmBundle;
            if (PageVmBundle.SampleStore != null)
            {
                _sampleVms = new ObservableCollection<SampleVm>(
                    PageVmBundle.SampleStore.AllSamples.Select(
                            s => s.ToSampleVm()));
                var propTypes = PageVmBundle.SampleStore.AllSamples.GetPropertySets()
                                            .Select(ps => ps.PropertyType)
                                            .ToList();
                var fixedCols = SampleVmExt.FixedColumnInfo;
                var extraCols = propTypes.MakeDataGridColumnInfo("SampleProperties");
                ColumnInfo = fixedCols.Concat(extraCols).ToList();
            }

            Plate = plate;
            _validationResult = String.Empty;

            Zoom = 2;
            if (Plate != null)
            {
                PlateVm = new PlateVm(Plate, PageVmBundle.PlateStore, 
                    PageVmBundle.UndoRedoService);
                if (Plate.RowCount > 20)
                {
                    Zoom = 1;
                }
            }
        }

        public PageVmBundle PageVmBundle { get; }

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

        PlateVm? _plateVm;
        public PlateVm? PlateVm
        {
            get => _plateVm;
            set
            {
                if (_plateVm != null)
                {
                    _plateVm.PropertyChanged -= _plateVm_PropertyChanged;
                }
                SetProperty(ref _plateVm, value);
                if (_plateVm != null)
                {
                    _plateVm.PropertyChanged += _plateVm_PropertyChanged;
                }
            }
        }

        private void _plateVm_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            NotifyCommands();
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

        void NavBack()
        {
            PageVmBundle.ModalNavigationStore.CurrentViewModel = null;
            PageVmBundle.NavigationStore.CurrentViewModel =
                new AddSamplesToPlatePageVm(PageVmBundle, Plate);
        }

        void NavBackAndPop()
        {
            PageVmBundle.UndoRedoService.PopUndo();
            NavBack();
        }

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


        #region SavePlateCommand

        RelayCommand? _savePlateCommand;
        public ICommand SavePlateCommand
        {
            get
            {
                if(_savePlateCommand == null)
                {
                    _savePlateCommand = new RelayCommand(SaveThePlate, CanSavePlate);
                }
                return _savePlateCommand;
            }
        }

        void SaveThePlate()
        {
            PlateVm.SaveChanges();
            NotifyCommands();
        }

        bool CanSavePlate()
        {
            return PlateVm.HasChanges;
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
            return PlateVm.HasChanges;
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
                    _navHomeCommand = new RelayCommand(NavHome, CanNavHome);
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
                NavBack, "Go to Add Samples to Plate", 
                action, "Go to Home");
        }

        bool CanNavHome()
        {
            return PlateVm.HasChanges;
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
                    _navNewPlateCommand = new RelayCommand(NavNewPlate, CanNavNewPlate);
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
                NavBack, "Go to Add Samples to Plate", 
                action, "Go to Make new Plate" );
        }

        bool CanNavNewPlate()
        {
            return PlateVm.HasChanges;
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
                    _navAllPlatesCommand = new RelayCommand(
                        NavAllPlates, CanNavAllPlates);
                }
                return _navAllPlatesCommand;
            }
        }

        void NavAllPlates()
        {
            Action action = () =>
                PageVmBundle.NavigationStore.CurrentViewModel =
                            new AllPlatesPageVm(PageVmBundle, null);

            PageVmBundle.UndoRedoService.Push(
                NavBack, "Go to Add Samples to Plate", 
                action, "Go to All Plates");
        }

        bool CanNavAllPlates()
        {
            return PlateVm.HasChanges;
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
                    _navAllSamplesCommand = new RelayCommand(
                        NavAllSamples, CanNavAllSamples);
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
                NavBack, "Go to Add Samples to Plate", 
                action, "Go to All Samples");
        }

        bool CanNavAllSamples()
        {
            return PlateVm.HasChanges;
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
                    _navNewSamplesCommand = new RelayCommand(NavNewSamples, CanNavNewSamples);
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
                NavBack, "Go to Add Samples to Plate", 
                action, "Go to Make new Samples");
        }

        bool CanNavNewSamples()
        {
            return PlateVm.HasChanges;
        }

        #endregion // NavNewSamplesCommand


    }
}




public class DataGridSelectedItemsBlendBehavior : Behavior<DataGrid>
{
    public static readonly DependencyProperty SelectedItemsProperty =
        DependencyProperty.Register("SelectedItems", typeof(object),
        typeof(DataGridSelectedItemsBlendBehavior),
        new FrameworkPropertyMetadata(null) { BindsTwoWayByDefault = true });

    public IList SelectedItems
    {
        get { return (IList)GetValue(SelectedItemsProperty); }
        set { SetValue(SelectedItemsProperty, value); }
    }

    protected override void OnAttached()
    {
        base.OnAttached();
        this.AssociatedObject.SelectionChanged += OnSelectionChanged;
    }

    protected override void OnDetaching()
    {
        base.OnDetaching();
        if (this.AssociatedObject != null)
            this.AssociatedObject.SelectionChanged -= OnSelectionChanged;
    }

    private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (SelectedItems != null)
        {
            foreach (var selectedItem in e.AddedItems)
                if (!SelectedItems.Contains(selectedItem))
                    SelectedItems.Add(selectedItem);

            foreach (var unselectedItem in e.RemovedItems)
                SelectedItems.Remove(unselectedItem);
        }
    }
}