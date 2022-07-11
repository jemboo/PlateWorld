using PlateWorld.Mvvm.ViewModels;
using PlateWorld.ViewModels.Pages;
using PlateWorld.Views;
using System.Windows;

namespace PlateWorld
{
    public partial class App : Application
    {
        MainWindow _mainWindow;
        PageVmBundle _pageVmBundle;
        public App()
        {
            _pageVmBundle = new PageVmBundle();  
            var mainVm = new MainViewModel(_pageVmBundle.NavigationStore, _pageVmBundle.ModalNavigationStore);
            var startingVm = new HomePageVm(_pageVmBundle);
            _mainWindow = new MainWindow();
            _mainWindow.DataContext = mainVm;
            _pageVmBundle.NavigationStore.CurrentViewModel = startingVm;
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            _mainWindow.Show();
        }
    }
}
