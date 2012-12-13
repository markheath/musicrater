using System;
using System.Linq;

namespace MusicRater
{
    public class CombinedContestLoader : IContestLoader
    {
        private readonly IContestLoader firstTimeLoader;
        private readonly IContestLoader subsequentLoader;

        public CombinedContestLoader(IContestLoader firstTimeLoader, IContestLoader subsequentLoader)
        {
            this.firstTimeLoader = firstTimeLoader;
            this.subsequentLoader = subsequentLoader;
            subsequentLoader.Loaded += loader_Loaded;
        }

        public void BeginLoad()
        {
            subsequentLoader.BeginLoad();
        }

        void loader_Loaded(object sender, ContestLoadedEventArgs e)
        {
            if (e.Error == null && e.Contest != null)
            {
                RaiseLoadedEvent(e);
            }
            else
            {
                firstTimeLoader.Loaded += (s, args) => RaiseLoadedEvent(args);
                firstTimeLoader.BeginLoad();
            }
        }

        private void RaiseLoadedEvent(ContestLoadedEventArgs e)
        {
            if (Loaded != null)
            {
                Loaded(this, e);
            }
        }

        public event EventHandler<ContestLoadedEventArgs> Loaded;
    }
}
