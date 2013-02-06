using System;
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
using MusicRater.Model;

namespace MusicRater.DummyViewModels
{
    public class DummyTrackListControlViewModel
    {
        public DummyTrackListControlViewModel()
        {
            this.Tracks = new List<TrackViewModel>();
            var builder = new DummyTrackBuilder();
            for (int n = 0; n < 20; n++)
            {
                this.Tracks.Add(new TrackViewModel(builder.Build()));
            }
        }
        public List<TrackViewModel> Tracks { get; private set; }
    }

    class DummyTrackBuilder
    {
        private Random random;
        private int track;

        public DummyTrackBuilder()
        {
            random = new Random();
        }

        public Track Build()
        {
            var t = new Track();
            t.Rating = random.Next(20);
            t.Author = "Someone";
            t.Listens = random.Next(10);
            t.Title = "Track " + (++track).ToString();
            return t;
        }
    }
}
