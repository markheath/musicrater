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

namespace MusicRater.DummyViewModels
{
    public class DummyPositionControlViewModel
    {
        public double DownloadProgress { get { return 55; } }
        public double Duration { get { return 265; } }
        public double PlaybackPosition { get { return 101; } }
    }
}
