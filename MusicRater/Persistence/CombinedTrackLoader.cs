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
        private readonly ITrackLoader firstTimeLoader;
        private readonly string fileName;

        public CombinedTrackLoader(ITrackLoader firstTimeLoader, string fileName)
        {
            this.firstTimeLoader = firstTimeLoader;
            this.fileName = fileName;
        }

        public void BeginLoad()
        {
            IsolatedStoreTrackLoader loader = new IsolatedStoreTrackLoader(fileName);
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
                firstTimeLoader.Loaded += (s, args) => RaiseLoadedEvent(args);
                firstTimeLoader.BeginLoad();
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
