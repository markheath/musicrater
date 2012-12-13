using System;
using System.Linq;

namespace MusicRater
{
    public class CombinedTrackLoader : ITrackLoader
    {
        private readonly ITrackLoader firstTimeLoader;
        private readonly ITrackLoader subsequentLoader;

        public CombinedTrackLoader(ITrackLoader firstTimeLoader, ITrackLoader subsequentLoader)
        {
            this.firstTimeLoader = firstTimeLoader;
            this.subsequentLoader = subsequentLoader;
            subsequentLoader.Loaded += loader_Loaded;
        }

        public void BeginLoad()
        {
            subsequentLoader.BeginLoad();
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
