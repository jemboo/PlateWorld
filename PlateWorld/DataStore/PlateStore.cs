using PlateWorld.Models;
using System;
using System.Linq;
using System.Collections.Generic;

namespace PlateWorld.DataStore
{
    public class PlateStore
    {
        public event Action<Plate> PlateAdded;
        Dictionary<int, Plate> plateDict { get; set; } = new Dictionary<int, Plate>();

        public void AddPlate(Plate plate)
        {
            plateDict[plate.Id] = plate;
            PlateAdded?.Invoke(plate);
        }

        public bool ContainsPlate(int plateId)
        {
            return plateDict.ContainsKey(plateId);
        }

        public bool ContainsPlateName(string plateName)
        {
            return plateDict.Values.Any(p => p.Name == plateName);
        }
        public IEnumerable<Plate> AllPlates 
        {
            get { return plateDict.Values; }
        }

        public bool RemovePlate(Plate plate)
        {
            return plateDict.Remove(plate.Id);
        }

    }
}
