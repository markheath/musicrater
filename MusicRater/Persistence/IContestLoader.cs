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
    public class LoadedEventArgs<T> : EventArgs
    {
        public LoadedEventArgs(T result)
        {
            Result = result;
        }

        public LoadedEventArgs(Exception error)
        {
            Error = error;
        }

        public Exception Error { get; set; }
        public T Result { get; private set; }
    }
}
