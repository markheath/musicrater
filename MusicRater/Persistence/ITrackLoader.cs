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

namespace MusicRater
{
    public interface ITrackLoader
    {
        void BeginLoad();
        event EventHandler<LoadedEventArgs> Loaded;
    }

    public class LoadedEventArgs : EventArgs
    {
        public Exception Error { get; set; }
        public IEnumerable<Track> Tracks { get; set; }
    }
}
