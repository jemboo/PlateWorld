using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlateWorld.ViewModels.Utils
{
    public interface IUpdater<T>
    {
        void Update(T entity);
    }

    public class DontUpdate<T> : IUpdater<T>
    {
        public void Update(T entity)
        {

        }
    }
}
