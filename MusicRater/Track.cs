using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GalaSoft.MvvmLight;
using System.Diagnostics;
using System.Windows.Media;

namespace MusicRater
{
    public class Track
    {
        public Track(IEnumerable<Rating> ratings)
        {
            this.SubRatings = new List<Rating>(ratings);
        }

        public string Title { get; set; }
        public string Author { get; set; }
        public string Url { get; set; }
        public string PositiveComments { get; set; }
        public string Suggestions { get; set; }
        public bool IsExcluded { get; set; }
        public IEnumerable<Rating> SubRatings { get; private set; }
        public int Listens { get; set; }

        public double Rating
        {
            get
            {
                int max = 0;
                int total = 0;
                foreach (var rating in SubRatings)
                {
                    max += 10 * rating.Criteria.Weight;
                    total += rating.Value * rating.Criteria.Weight;
                }
                //Debug.WriteLine("Total: {0}, Max: {1}", total, max);
                return total * 100.0 / max;
            }
        }
    }
}
