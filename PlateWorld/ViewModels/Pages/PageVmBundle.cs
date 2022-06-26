using PlateWorld.DataStore;
using PlateWorld.Mvvm.Commands;
using PlateWorld.Mvvm.Stores;

namespace PlateWorld.ViewModels.Pages
{
    public class PageVmBundle
    {
        public PageVmBundle()
        {
            NavigationStore = new NavigationStore();
            ModalNavigationStore = new ModalNavigationStore();
            PlateStore = new PlateStore();
            SampleStore = new SampleStore();
            UndoRedoService = new UndoRedoService();
        }
        public NavigationStore NavigationStore { get; }
        public ModalNavigationStore ModalNavigationStore { get; }
        public PlateStore PlateStore { get; }
        public SampleStore SampleStore { get; }
        public UndoRedoService UndoRedoService { get; }

    }
}
