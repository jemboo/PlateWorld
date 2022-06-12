using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using PlateWorld.Models.SamplePlate;
using PlateWorld.Mvvm.Stores;
using PlateWorld.ViewModels.DragDrop;
using PlateWorld.ViewModels.PlateParts;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace PlateWorld.ViewModels.Pages
{
    public class AddSamplesToPlatePageVm : ObservableObject
    {
        NavigationStore NavigationStore { get; }
        ModalNavigationStore ModalNavigationStore { get; }

        public SamplesDragHandler SamplesDragHandler { get; set; } 
            = new SamplesDragHandler();

        public SamplesDropHandler SamplesDropHandler { get; set; } 
            = new SamplesDropHandler();

        public AddSamplesToPlatePageVm(NavigationStore navigationStore, 
                ModalNavigationStore modalNavigationStore,
                DataStore.PlateStore plateStore, IPlate? plate)
        {
            NavigationStore = navigationStore;
            ModalNavigationStore = modalNavigationStore;
            PlateStore = plateStore;
            Plate = plate;
            _plateName = plate.Name;
            _validationResult = String.Empty;

            if (Plate != null)
            {
                PlateVm = new PlateVm(Plate, PlateStore);
                if (plate.RowCount > 20)
                {
                    Zoom = 1;
                }
                else Zoom = 2;
            }
            else
            {
                Zoom = 2;
            }

            //var samples = AnimalThemedContainerExt.MakeSamples(123, 100)
            //                .ToArray();
            //var dex = 1;
            //var sampleVms = samples.Select(s => s.FromAtc($"Sample {dex++}")
            //                .ToVm());
            //_sampleVms = new ObservableCollection<SampleVm>(sampleVms);
        }
        DataStore.PlateStore PlateStore { get; }

        IPlate? Plate { get; }

        private PlateVm _plateVm;
        public PlateVm PlateVm
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

        #region SubmitCommand

        RelayCommand? _cancelCommand;
        public ICommand CancelCommand
        {
            get
            {
                Action aa = () =>
                {
                    NavigationStore.CurrentViewModel = new AddSamplesToPlatePageVm(
                        NavigationStore, ModalNavigationStore, PlateStore, Plate);
                };
                return _cancelCommand ??
                       (_cancelCommand = new RelayCommand(aa, () => EditsWereMade()));
            }
        }

        #endregion // SubmitCommand

        bool EditsWereMade()
        {
            return Plate.Name != PlateName;
        }


        private string _plateName;
        public string PlateName
        {
            get => _plateName;
            set
            {
                SetProperty(ref _plateName, value);
                _savePlateCommand?.NotifyCanExecuteChanged();
                _cancelCommand?.NotifyCanExecuteChanged();
                _NavHomeCommand.NotifyCanExecuteChanged();
                _NavNewPlateCommand.NotifyCanExecuteChanged();
                _NavAllPlatesCommand.NotifyCanExecuteChanged();
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

        #region NewPlateCancelCommand

        RelayCommand? _newPlateCancelCommand;
        public ICommand NewPlateCancelCommand
        {
            get
            {
                Action aa = () => {
                    ModalNavigationStore.CurrentViewModel = null;
                    NavigationStore.CurrentViewModel =
                    new AddSamplesToPlatePageVm(NavigationStore,
                    ModalNavigationStore, PlateStore, Plate);
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
                Action aa = () =>
                {
                    var newPlate = Plate.NewName(newName: PlateName);
                    PlateStore.AddPlates(new[] { newPlate });
                    NavigationStore.CurrentViewModel = new AddSamplesToPlatePageVm(
                        NavigationStore, ModalNavigationStore, PlateStore, newPlate);
                };
                return _savePlateCommand ?? 
                       (_savePlateCommand = new RelayCommand(aa, () => Validate() ));
            }
        }

        #endregion // NavHomeCommand


        #region NavHomeCommand

        RelayCommand? _NavHomeCommand;
        public ICommand NavHomeCommand
        {
            get
            {
                Action aa = () =>
                {
                    NavigationStore.CurrentViewModel =
                    new HomePageVm(NavigationStore, 
                    ModalNavigationStore, PlateStore);
                };
                return _NavHomeCommand ?? (_NavHomeCommand =
                    new RelayCommand( aa, () => !EditsWereMade() ));
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
                    new NewPlatePageVm(NavigationStore, ModalNavigationStore, 
                                PlateStore, NewPlateCancelCommand);
                };
                return _NavNewPlateCommand ?? (_NavNewPlateCommand =
                    new RelayCommand(
                                aa,
                                () => !EditsWereMade()
                            ));
            }
        }

        #endregion // NavNewPlateCommand


        #region NavAddSamplesToPlateCommand

        RelayCommand? _NavAddSamplesToPlateCommand;
        public ICommand NavAddSamplesToPlateCommand
        {
            get
            {
                Action aa = () => { };
                return _NavAddSamplesToPlateCommand ?? 
                    (_NavAddSamplesToPlateCommand = new RelayCommand(aa,
                            () => false ));
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
                        ModalNavigationStore, PlateStore, Plate);
                };
                return _NavAllPlatesCommand ?? (_NavAllPlatesCommand =
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
                Action aa = () => {
                    NavigationStore.CurrentViewModel =
                    new NewSamplesPageVm(NavigationStore,
                    ModalNavigationStore, PlateStore);
                };
                return _navNewSamplesCommand ?? (_navNewSamplesCommand =
                    new RelayCommand(
                            aa,
                            () => true
                            ));
            }
        }

        #endregion // NavNewSamplesCommand



        bool Validate()
        {
            if(!EditsWereMade()) return false;

            if ((Plate.Name != PlateName) && 
                (PlateStore.ContainsPlateName(PlateName)))
            {
                ValidationResult = $"Plate name is already in use";
                return false;
            }
            ValidationResult = String.Empty;
            return true;
        }


    }
}
