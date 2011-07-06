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
    public class DummyRatingControlViewModel
    {
        public DummyRatingControlViewModel()
        {
            this.SubRatings = new List<Rating>();
            this.SubRatings.Add(new Rating(new Criteria("Composition")) { Value = 5 });
            this.SubRatings.Add(new Rating(new Criteria("Emotion")) { Value = 7 });
            this.SubRatings.Add(new Rating(new Criteria("Production")) { Value = 10 });
            this.Listens = 15;
            this.Comments = "Nice bass\r\nNeeds more variety";
        }

        public List<Rating> SubRatings { get; private set; }
        public string Comments { get; private set; }
        public string Suggestions { get; private set; }
        public int Listens { get; private set; }

    }
}
