using Microsoft.Toolkit.Mvvm.Input;
using System;

namespace PlateWorld.ViewModels.Utils
{
    public static class CommandUtils
    {
        public static Action NoAction
        {
            get { return () => { }; }
        }

        public static Func<bool> AlwaysFalse
        {
            get { return () => false; }
        }

        public static RelayCommand Disabled
        {
            get { return new RelayCommand(NoAction, AlwaysFalse); }
        }
    }
}
