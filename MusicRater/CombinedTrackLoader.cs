using System;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace MusicRater
{
    public class CombinedTrackLoader : ITrackLoader
    {
        public void BeginLoad()
        {
            IsolatedStoreTrackLoader loader = new IsolatedStoreTrackLoader("tracks.xml");
            loader.Loaded += new EventHandler<LoadedEventArgs>(loader_Loaded);
            loader.BeginLoad();
        }

        void loader_Loaded(object sender, LoadedEventArgs e)
        {
            if (e.Error == null && e.Tracks.Count() > 0)
            {
                RaiseLoadedEvent(e);
            }
            else
            {
                KvrTrackLoader kvrLoader = new KvrTrackLoader("http://www.archive.org/download/KvrOsc28TyrellN6/KvrOsc28TyrellN6_files.xml");
                kvrLoader.Loaded += (s, args) => RaiseLoadedEvent(args);
                kvrLoader.BeginLoad();
            }
        }

        private void RaiseLoadedEvent(LoadedEventArgs e)
        {
            if (Loaded != null)
            {
                Loaded(this, e);
            }
        }

        public event EventHandler<LoadedEventArgs> Loaded;
    }
}
