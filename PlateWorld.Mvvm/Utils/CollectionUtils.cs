using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace PlateWorld.Mvvm.Utils
{
    public static class CollectionUtils
    {
        public static ObservableCollection<T> Replace<T>(
            this ObservableCollection<T> oc, T targ, T replc, Func<T, T,bool> matcher)
        {
            var mtch = oc.Where(m => matcher(targ, m)).FirstOrDefault();
            if (mtch == null) return oc;
            var dex = oc.IndexOf(mtch);
            oc[dex] = replc;
            return oc;
        }
    }
}
