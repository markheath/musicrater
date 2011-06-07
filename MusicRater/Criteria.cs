﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MusicRater
{
    public class Criteria
    {
        public Criteria(string name, int weight = 10)
        {
            this.Name = name;
            this.Weight = weight;
        }

        public string Name { get; private set; }
        public int Weight { get; private set; }
    }
}
