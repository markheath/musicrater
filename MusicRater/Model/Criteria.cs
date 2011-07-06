using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MusicRater.Model
{
    public class Criteria
    {
        public Criteria(string name, int weight = 10)
        {
            this.Name = name;
            this.Weight = weight;
        }

        public string Name { get; set; }
        public int Weight { get; set; }
    }
}
