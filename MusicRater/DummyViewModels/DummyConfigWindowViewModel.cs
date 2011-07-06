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
    public class DummyConfigWindowViewModel
    {
        public DummyConfigWindowViewModel()
        {
            this.Criteria = new List<Criteria>();
            this.Criteria.Add(new Criteria("Something", 6));
            this.Criteria.Add(new Criteria("Something Else", 10));
            this.Criteria.Add(new Criteria("A Third Criteria", 2));
            this.Criteria.Add(new Criteria("Unused 1", 0));
            this.Criteria.Add(new Criteria("Unused 2", 2));
        }

        public List<Criteria> Criteria { get; private set; }
    }
}
