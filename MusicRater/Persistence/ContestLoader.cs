using System;
using System.Collections.Generic;
using System.Linq;
using MusicRater.Model;

namespace MusicRater
{
    public class ContestLoader
    {
        private readonly ContestInfo contestInfo;
        private readonly IsolatedStoreContestLoader isoLoader;
        private readonly KvrTrackListLoader kvrLoader;

        public ContestLoader(ContestInfo contestInfo, IIsolatedStore isoStore, Criteria[] defaultCriteria)
        {
            this.contestInfo = contestInfo;
            this.isoLoader = new IsolatedStoreContestLoader(contestInfo.IsoStoreFileName, isoStore);
            this.kvrLoader = new KvrTrackListLoader(contestInfo.TrackListUrl, defaultCriteria);
            
        }

        public void BeginLoad()
        {
            var contest = isoLoader.Load();
            if (contest == null)
            {
                kvrLoader.Loaded += OnTracksLoaded;
                kvrLoader.BeginLoad();
            }
            else
            {
                OnLoaded(new LoadedEventArgs<Contest>(contest));
            }
        }

        void OnTracksLoaded(object sender, LoadedEventArgs<List<Track>> e)
        {
            if (e.Error == null && e.Result != null)
            {
                var contest = new Contest(contestInfo);
                contest.Tracks.AddRange(e.Result);
                OnLoaded(new LoadedEventArgs<Contest>(contest));
            }
            else
            {
                OnLoaded(new LoadedEventArgs<Contest>(e.Error));
            }
        }

        public event EventHandler<LoadedEventArgs<Contest>> Loaded;

        protected virtual void OnLoaded(LoadedEventArgs<Contest> e)
        {
            EventHandler<LoadedEventArgs<Contest>> handler = Loaded;
            if (handler != null) handler(this, e);
        }
    }
}
