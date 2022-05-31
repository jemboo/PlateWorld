using Microsoft.Toolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace PlateWorld.Mvvm.ViewModels
{
    public interface INavigationBarViewModel : INotifyPropertyChanged, INotifyPropertyChanging
    {
        void RegisterContent(ObservableObject contentViewModel);
    }

    public class LayoutViewModel : ObservableObject
    {
        public INavigationBarViewModel NavigationBarViewModel { get; }
        public ObservableObject ContentViewModel { get; }

        public LayoutViewModel(INavigationBarViewModel navigationBarViewModel, 
                               ObservableObject contentViewModel)
        {
            NavigationBarViewModel = navigationBarViewModel;
            ContentViewModel = contentViewModel;
            NavigationBarViewModel.RegisterContent(ContentViewModel);
        }
    }


}
