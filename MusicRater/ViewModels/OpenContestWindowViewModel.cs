using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using GalaSoft.MvvmLight;
using MusicRater.Model;
using System.Collections.ObjectModel;

namespace MusicRater
{
    public class OpenContestWindowViewModel : ViewModelBase
    {
        public ObservableCollection<ContestInfo> Contests { get; private set; }
        
        public OpenContestWindowViewModel(IIsolatedStore store, IEnumerable<ContestInfo> knownContests)
        {
            this.Contests = new ObservableCollection<ContestInfo>();
            
            var repo = new RatingsRepository(store);
            
            foreach(var fileName in store.GetFileNames("*.xml"))
            {
                var c = repo.Load(fileName);
                this.Contests.Add(new ContestInfo() { IsoStoreFileName = fileName, Name = c.Name, TrackListUrl = c.TrackListUrl});
            }

            AddKnownContests(knownContests);
        }

        private void AddKnownContests(IEnumerable<ContestInfo> knownContests)
        {
            foreach (var contestInfo in knownContests)
            {
                if (Contests.All(c => c.TrackListUrl != contestInfo.TrackListUrl))
                {
                    Contests.Add(contestInfo);
                }
            }
        }
    }
}
