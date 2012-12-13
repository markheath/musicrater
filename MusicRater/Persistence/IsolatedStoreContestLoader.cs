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
    public class IsolatedStoreContestLoader : IContestLoader
    {
        private readonly IIsolatedStore isoStore;
        private readonly string fileName;

        public IsolatedStoreContestLoader(string fileName, IIsolatedStore isoStore)
        {
            this.fileName = fileName;
            this.isoStore = isoStore;
        }

        public void BeginLoad()
        {
            var repo = new RatingsRepository(isoStore);
            var contest = repo.Load(this.fileName);
            if (contest != null)
            {
                KvrContestLoader.Shuffle(contest.Tracks, new Random());
            }
            OnLoaded(new ContestLoadedEventArgs(contest));
        }

        public event EventHandler<ContestLoadedEventArgs> Loaded;

        protected virtual void OnLoaded(ContestLoadedEventArgs e)
        {
            EventHandler<ContestLoadedEventArgs> handler = Loaded;
            if (handler != null) handler(this, e);
        }
    }
}
