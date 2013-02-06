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
            this.Rating = 7;
            this.Listens = 15;
            this.Comments = "Nice bass\r\nNeeds more variety";
        }

        public int Rating { get; private set; }
        public string Comments { get; private set; }
        public int Listens { get; private set; }

    }
}
