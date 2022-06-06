using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PlateWorld.ViewModels.Pages
{
    public class AllSamplesPageVm
    {


        #region NavAllSamplesCommand

        RelayCommand? _navAllSamplesCommand;
        public ICommand? NavAllSamplesCommand
        {
            get
            {
                Action aa = () => { };
                return _navAllSamplesCommand ?? (
                    _navAllSamplesCommand =
                        new RelayCommand(
                                aa,
                                () => false
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
                Action aa = () => { };
                return _navNewSamplesCommand ?? (_navNewSamplesCommand =
                    new RelayCommand(
                            aa,
                            () => false
                            ));
            }
        }

        #endregion // NavNewSamplesCommand


    }
}
