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
    public class DummyMainWindowViewModel
    {
        public DummyMainWindowViewModel()
        {
            this.IsLoading = false;
            this.Duration = 256;
            this.DownloadProgress = 76;
            this.PlaybackPosition = 103;
        }

        public bool IsLoading { get; private set; }
        public double Duration { get; private set; }
        public double DownloadProgress { get; private set; }
        public double PlaybackPosition { get; private set; }
    }
}
