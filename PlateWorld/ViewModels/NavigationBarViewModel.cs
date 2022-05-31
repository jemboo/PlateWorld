using Microsoft.Toolkit.Mvvm.ComponentModel;
using PlateWorld.Mvvm.ViewModels;
using System.ComponentModel;
using System.Windows.Input;

namespace PlateWorld.ViewModels
{
    public class NavigationBarViewModel : INavigationBarViewModel
    {
        public ICommand? NavigatePlateEditorCommand { get; }
        public ICommand? NavigateNewPlateCommand { get; }
        public ICommand? NavigatePlateListCommand { get; }
        public ICommand? NavigateHomeCommand { get; }


        public NavigationBarViewModel()
            //INavigationService plateEditorNavigationService,
            //INavigationService newPlateNavigationService,
            //INavigationService plateListNavigationService,
            //INavigationService homeNavigationService)
        {
            //NavigatePlateEditorCommand = new NavigateCommand(plateEditorNavigationService);
            //NavigateNewPlateCommand = new NavigateCommand(newPlateNavigationService);
            //NavigatePlateListCommand = new NavigateCommand(plateListNavigationService);
            //NavigateHomeCommand = new NavigateCommand(homeNavigationService);
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public event PropertyChangingEventHandler? PropertyChanging;

        public void RegisterContent(ObservableObject contentViewModel)
        {
            //throw new System.NotImplementedException();
        }
    }
}
