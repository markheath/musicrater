﻿using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;
using GalaSoft.MvvmLight;

namespace MusicRater
{
    public class TrackViewModel : ViewModelBase
    {
        private Track track;

        public TrackViewModel(Track track)
        {
            this.track = track;
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
            set
            {
                if (track.PositiveComments != value)
                {
                    track.PositiveComments = value;
                    RaisePropertyChanged("PositiveComments");
                }
            }

        }

        public string Suggestions
        {
            get { return track.Suggestions; }
            set
            {
                if (track.Suggestions != value)
                {
                    track.Suggestions = value;
                    RaisePropertyChanged("Suggestions");
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
}