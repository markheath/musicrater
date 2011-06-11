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
using System.Collections;
using System.Collections.Generic;

namespace MusicRater.ViewModels
{
    public class AnonymiseCommand : ICommand
    {
        private IEnumerable<TrackViewModel> tracks;

        public AnonymiseCommand(IEnumerable<TrackViewModel> tracks)
        {
            this.AnonymousMode = true;
            this.tracks = tracks;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged = (s,e) => {};

        public void Execute(object parameter)
        {
            this.AnonymousMode = !this.AnonymousMode;
            foreach (var track in tracks)
            {
                track.AnonymousMode = this.AnonymousMode;
            }
        }

        public bool AnonymousMode { get; private set; }
    }
}
