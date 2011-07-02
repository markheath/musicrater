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

namespace MusicRater.DummyViewModels
{
    public class DummyTrackListControlViewModel
    {
        public DummyTrackListControlViewModel()
        {
            this.Tracks = new List<TrackViewModel>();
            DummyTrackBuilder builder = new DummyTrackBuilder();
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
        private Criteria[] criteria;

        public DummyTrackBuilder()
        {
            random = new Random();
            criteria = new Criteria[3];
            criteria[0] = new Criteria("Sound");
            criteria[1] = new Criteria("Production");
            criteria[2] = new Criteria("Song-Writing");
        }

        public Track Build()
        {
            Track t = new Track(CreateRatings());
            t.Author = "Someone";
            t.Listens = random.Next(10);
            t.Title = "Track " + (++track).ToString();
            return t;
        }

        private IEnumerable<Rating> CreateRatings()
        {
            List<Rating> ratings = new List<Rating>();
            for (int n = 0; n < this.criteria.Length; n++)
            {
                Rating r = new Rating(criteria[n]);
                r.Value = random.Next(10);
                ratings.Add(r);
            }
            return ratings;
        }
    }
}
