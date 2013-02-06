using System.Windows.Media;
using GalaSoft.MvvmLight;
using MusicRater.Model;

namespace MusicRater
{
    public class TrackViewModel : ViewModelBase
    {
        private readonly Track track;

        public TrackViewModel(Track track)
        {
            this.track = track;
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

        public string DisplayAuthor
        {
            get { return this.AnonymousMode ? "Anonymous" : track.Author; }
        }

        public string Url
        {
            get { return track.Url; }
            set { track.Url = value; }
        }

        public string Comments
        {
            get { return track.Comments; }
            set
            {
                if (track.Comments != value)
                {
                    track.Comments = value;
                    RaisePropertyChanged("Comments");
                }
            }

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

        private readonly static Brush enabledBrush = new SolidColorBrush(Colors.Black);
        private readonly static Brush disabledBrush = new SolidColorBrush(Colors.Gray);

        public Brush TextBrush
        {
            get
            {
                return IsExcluded ? disabledBrush : enabledBrush;
            }
        }

        public int Rating
        {
            get
            {
                return track.Rating;
            }
            set
            {
                if (track.Rating != value)
                {
                    track.Rating = value;
                    RaisePropertyChanged("Rating");
                }
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

        private bool anonymousMode;
        public bool AnonymousMode 
        {
            get { return anonymousMode; }
            set
            {
                this.anonymousMode = value;
                RaisePropertyChanged("DisplayAuthor");
            }
        }
    }
}
