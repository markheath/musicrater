using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GalaSoft.MvvmLight;
using System.Diagnostics;

namespace MusicRater
{
    public class Track : ViewModelBase
    {
        private int listens;

        public Track(IEnumerable<Rating> ratings)
        {
            this.SubRatings = new List<Rating>(ratings);
            foreach (var rating in ratings)
            {
                rating.PropertyChanged += (s, e) => { Debug.WriteLine("RATING CHANGE"); RaisePropertyChanged("Rating"); };
            }
        }

        public string Title { get; set; }
        public string Author { get; set; }
        public string Url { get; set; }
        public string PositiveComments { get; set; }
        public string Suggestions { get; set; }

        public IEnumerable<Rating> SubRatings { get; private set; }

        public int Listens
        {
            get
            {
                return listens;
            }
            set
            {
                if (listens != value)
                {
                    listens = value;
                    RaisePropertyChanged("Listens");
                }
            }
        }


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
                Debug.WriteLine("Total: {0}, Max: {1}", total, max);
                return total / (double)max;
            }
        }
    }
}
