﻿using System;
using System.Linq;
using System.Collections.Generic;
using PlateWorld.Models.SamplePlate;

namespace PlateWorld.DataStore
{
    public class PlateStore
    {
        public event Action<IPlate[]> PlatesAdded;
        public event Action<IPlate[]> PlatesRemoved;
        Dictionary<Guid, IPlate> plateDict { get; set; } 
            = new Dictionary<Guid, IPlate>();

        public void AddPlates(IPlate[] plates)
        {
            foreach (var plate in plates)
            {
                plateDict[plate.Id] = plate;
            }
            PlatesAdded?.Invoke(plates);
        }

        public bool ContainsPlate(Guid plateId)
        {
            return plateDict.ContainsKey(plateId);
        }

        public bool ContainsPlateName(string plateName)
        {
            return plateDict.Values.Any(p => p.Name == plateName);
        }
        public IEnumerable<IPlate> AllPlates 
        {
            get { return plateDict.Values; }
        }

        public bool[] RemovePlates(IPlate[] plates)
        {
            var res = plates.Select(
                plate => plateDict.Remove(plate.Id));
            PlatesRemoved?.Invoke(plates);
            return res.ToArray();
        }

    }
}
