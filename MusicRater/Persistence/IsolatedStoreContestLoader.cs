using System;
using MusicRater.Model;

namespace MusicRater
{
    public class IsolatedStoreContestLoader
    {
        private readonly IIsolatedStore isoStore;
        private readonly string fileName;

        public IsolatedStoreContestLoader(string fileName, IIsolatedStore isoStore)
        {
            this.fileName = fileName;
            this.isoStore = isoStore;
        }

        public Contest Load()
        {
            var repo = new RatingsRepository(isoStore);
            var contest = repo.Load(fileName);
            if (contest != null)
            {
                KvrTrackListLoader.Shuffle(contest.Tracks, new Random());
            }
            return contest;
        }

    }
}
