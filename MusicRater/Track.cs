using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GalaSoft.MvvmLight;
using System.Diagnostics;
using System.Windows.Media;

namespace MusicRater
{
    public class TrackViewModel : ViewModelBase
    {
        private Track track;

        public TrackViewModel(IEnumerable<Rating> ratings)
        {
            this.track = new Track(ratings);
            foreach (var rating in track.SubRatings)
            {
                rating.PropertyChanged += (s, e) => RaisePropertyChanged("Rating");
            }
        }

        public string Title 
        { 
            get { return track.Title; } 
            set { track.Title = value; } 
        }
        
        public string Author 
        { 
            get { return track.Author; }
            set { track.Author = value; }
        }
        
        public string Url 
        { 
            get { return track.Url; } 
            set { track.Url = value; } 
        }
        
        public string PositiveComments 
        { 
            get { return track.PositiveComments; } 
            set { track.PositiveComments = value; } 
        }

        public string Suggestions
        {
            get { return track.Suggestions; }
            set { track.Suggestions = value; }
        }

        public bool IsExcluded
        {
            get
            {
                return track.IsExcluded;
            }
            set
            {
                if (track.IsExcluded != value)
                {
                    track.IsExcluded = value;
                    RaisePropertyChanged("IsExcluded");
                    RaisePropertyChanged("TextBrush");
                }
            }
        }

        public double Rating
        {
            get { return track.Rating; }
        }

        private readonly static Brush enabledBrush = new SolidColorBrush(Colors.Black);
        private readonly static Brush disabledBrush = new SolidColorBrush(Colors.Gray);

        public Brush TextBrush
        {
            get
            {
                return IsExcluded ? disabledBrush : enabledBrush;
            }
        }

        public int Listens
        {
            get
            {
                return track.Listens;
            }
            set
            {
                if (track.Listens != value)
                {
                    track.Listens = value;
                    RaisePropertyChanged("Listens");
                }
            }
        }

        public IEnumerable<Rating> SubRatings
        {
            get { return track.SubRatings; }
        }
    }

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
                Debug.WriteLine("Total: {0}, Max: {1}", total, max);
                return total / (double)max;
            }
        }
    }
}
