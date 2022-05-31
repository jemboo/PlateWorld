using PlateWorld.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlateWorld.DataStore
{
    public class Plates
    {
        public Plates() { }
        public Dictionary<string, Plate> data { get; set; } = new Dictionary<string, Plate>();

    }
}
