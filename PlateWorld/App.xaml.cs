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
        PlateStore _plateStore;
        SampleStore _sampleStore;
        public App()
        {
            _navigationStore = new NavigationStore();
            _modalNavigationStore = new ModalNavigationStore();
            _plateStore = new PlateStore();
            _sampleStore = new SampleStore();
            var mainVm = new MainViewModel(_navigationStore, _modalNavigationStore);
            var startingVm = new HomePageVm(_navigationStore, _modalNavigationStore, _sampleStore, _plateStore);
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
