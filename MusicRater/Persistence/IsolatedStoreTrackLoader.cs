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
    public class IsolatedStoreTrackLoader : ITrackLoader
    {
        private readonly IIsolatedStore isoStore;
        private readonly Contest contest;

        public IsolatedStoreTrackLoader(Contest contest, IIsolatedStore isoStore)
        {
            this.contest = contest;
            this.isoStore = isoStore;
        }

        public void BeginLoad()
        {
            using (RatingsRepository repo = new RatingsRepository(isoStore))
            {
                repo.Load(this.contest);
            }
            KvrTrackLoader.Shuffle(this.contest.Tracks, new Random());            
            if (Loaded != null)
            {
                Loaded(this, new LoadedEventArgs() { Tracks = this.contest.Tracks });
            }
        }

        public event EventHandler<LoadedEventArgs> Loaded;
    }
}
