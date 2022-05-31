﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlateWorld.Models
{
    public class Sample
    {
        public Sample(int id, string name)
        {
            Id = id;
            Name = name;
        }
        public int Id { get; }

        public string Name { get; }

    }
}
