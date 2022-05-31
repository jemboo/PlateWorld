using Microsoft.Toolkit.Mvvm.ComponentModel;
using System;

namespace PlateWorld.Mvvm.Stores
{
    public class ModalNavigationStore
    {
        private ObservableObject? _currentViewModel;
        public ObservableObject? CurrentViewModel
        {
            get => _currentViewModel;
            set
            {
                _currentViewModel = value;
                OnCurrentViewModelChanged();
            }
        }

        public bool IsOpen => CurrentViewModel != null;

        public event Action? CurrentViewModelChanged;

        public void Close()
        {
            CurrentViewModel = null;
        }

        private void OnCurrentViewModelChanged()
        {
            CurrentViewModelChanged?.Invoke();
        }
    }
}
