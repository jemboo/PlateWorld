using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlateWorld.ViewModels.Pages
{
    public class Scraps
    {
    }


    //public ObservableCollection<PropertySetVm> PropertySetVms { get; }
    //= new ObservableCollection<PropertySetVm>();


    //private PropertySetVm _selectedPropertySetVm;
    //public PropertySetVm SelectedPropertySetVm
    //{
    //    get => _selectedPropertySetVm;
    //    set
    //    {
    //        SetProperty(ref _selectedPropertySetVm, value);
    //    }
    //}

    //private int _selectedIndex;
    //public int SelectedIndex
    //{
    //    get => _selectedIndex;
    //    set
    //    {
    //        SetProperty(ref _selectedIndex, value);
    //        _moveUpCommand?.NotifyCanExecuteChanged();
    //        _moveDownCommand?.NotifyCanExecuteChanged();
    //    }
    //}


    //#region MoveUpCommand

    //RelayCommand? _moveUpCommand;
    //public ICommand MoveUpCommand
    //{
    //    get
    //    {
    //        Action aa = () =>
    //        {
    //            PropertySetVms.Move(SelectedIndex, SelectedIndex - 1); ;
    //        };
    //        return _moveUpCommand ?? (_moveUpCommand =
    //            new RelayCommand(
    //                    aa,
    //                    () => SelectedIndex > 0
    //                    ));
    //    }
    //}

    //#endregion // MoveUpCommand


    //#region MoveDownCommand

    //RelayCommand? _moveDownCommand;
    //public ICommand MoveDownCommand
    //{
    //    get
    //    {
    //        Action aa = () => {
    //            PropertySetVms.Move(SelectedIndex, SelectedIndex + 1);
    //        };
    //        return _moveDownCommand ?? (_moveDownCommand =
    //            new RelayCommand
    //            (
    //                aa,
    //                () => SelectedIndex < (PropertySetVms.Count - 1)
    //            ));
    //    }
    //}

    //#endregion // MoveDownCommand


}
