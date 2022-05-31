using PlateWorld.DataStore;
using PlateWorld.Mvvm.Stores;
using PlateWorld.Mvvm.ViewModels;
using PlateWorld.ViewModels;
using PlateWorld.ViewModels.Pages;
using PlateWorld.Views;
using System.Windows;

namespace PlateWorld
{
    public partial class App : Application
    {
        MainWindow _mainWindow; 
        NavigationStore _navigationStore;
        ModalNavigationStore _modalNavigationStore;
        Plates _plates;
        public App()
        {
            _navigationStore = new NavigationStore();
            _modalNavigationStore = new ModalNavigationStore();
            _plates = new Plates(); 
            var mainVm = new MainViewModel(_navigationStore, _modalNavigationStore);
            var startingVm = new HomePageVm(_navigationStore, _modalNavigationStore, _plates);
            _mainWindow = new MainWindow();
            _mainWindow.DataContext = mainVm;
            _navigationStore.CurrentViewModel = startingVm;
            
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            _mainWindow.Show();
        }
    }
}
