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
    public interface IContestLoader
    {
        void BeginLoad();
        event EventHandler<ContestLoadedEventArgs> Loaded;
    }

    public class ContestLoadedEventArgs : EventArgs
    {
        public ContestLoadedEventArgs(Contest contest)
        {
            Contest = contest;
        }

        public ContestLoadedEventArgs(Exception error)
        {
            Error = error;
        }

        public Exception Error { get; set; }
        public Contest Contest { get; private set; }
    }
}
