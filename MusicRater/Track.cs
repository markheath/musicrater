using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MusicRater
{
    public class Track
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public string Url { get; set; }
        public int Listens { get; set; }
        public string PositiveComments { get; set; }
        public string Suggestions { get; set; }
        public List<Rating> SubRatings { get; set; }
        public int Rating 
        {
            get
            {
                int max = 0;
                int total = 0;
                foreach (var rating in SubRatings)
                {
                    max += rating.Criteria.Weight * 10;
                    total += rating.Value * rating.Criteria.Weight;
                }
                return (total * 100) / max;
            }
        }
    }
}
