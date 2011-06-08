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

namespace MusicRater
{
    public class IsolatedStoreTrackLoader : ITrackLoader
    {
        public IsolatedStoreTrackLoader()
        {

        }

        public void BeginLoad()
        {
            RatingsRepository repo = new RatingsRepository();
            var tracks = new List<Track>();
            tracks.AddRange(repo.Load());
            KvrTrackLoader.Shuffle(tracks, new Random());
            if (Loaded != null)
            {
                Loaded(this, new LoadedEventArgs() { Tracks = tracks });
            }
        }

        public event EventHandler<LoadedEventArgs> Loaded;
    }
}
