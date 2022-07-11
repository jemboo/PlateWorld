using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using PlateWorld.Mvvm.Stores;
using PlateWorld.ViewModels.Utils;
using System;
using System.Windows.Input;

namespace PlateWorld.ViewModels.Pages
{
    public class HomePageVm : ObservableObject
    {
        public HomePageVm(PageVmBundle pageVmBundle)
        {
            PageVmBundle = pageVmBundle;
        }
        public PageVmBundle PageVmBundle { get; }


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

            PageVmBundle.UndoRedoService.Push(
                NavBack, "Go to Home", 
                action, "Go to Make new Plate");
        }

        #endregion // NavNewPlateCommand


        #region NavAddSamplesToPlateCommand

        public ICommand NavAddSamplesToPlateCommand
        {
            get
            {
                return CommandUtils.Disabled;
            }
        }

        #endregion // NavAddSamplesToPlateCommand


        #region NavBackCommand

        RelayCommand? _navBackCommand;
        public ICommand NavBackCommand
        {
            get
            {
                if(_navBackCommand == null)
                {
                    _navBackCommand =  new RelayCommand(NavBackAndPop, () => true);
                }
                return _navBackCommand;
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

            PageVmBundle.UndoRedoService.Push(
                NavBack, "Go to Home", 
                action, "Go to All Plates");
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
                NavBack, "Go to Home", 
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
            var newSamplesPageVm = new NewSamplesPageVm(PageVmBundle);
            Action action = () =>
                 PageVmBundle.NavigationStore.CurrentViewModel =
                        newSamplesPageVm;

            PageVmBundle.UndoRedoService.Push(
                NavBack, "Go to Home",
                action, "Go to Make new Samples");
        }

        #endregion // NavNewSamplesCommand

    }
}
