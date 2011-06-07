using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MusicRater
{
    public class Rating
    {
        public Criteria Criteria { get; set; }
        public int Value { get; set; }
        public string Name { get { return Criteria.Name; } }
    }
}
