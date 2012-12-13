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
        
        public OpenContestWindowViewModel(IIsolatedStore store)
        {
            this.Contests = new ObservableCollection<ContestInfo>();
            
            var repo = new RatingsRepository(store);
            
            foreach(var fileName in store.GetFileNames("*.xml"))
            {
                var c = repo.Load(fileName);
                this.Contests.Add(new ContestInfo() { IsoStoreFileName = fileName, Name = c.Name, TrackListUrl = c.TrackListUrl});
            }

            AddKnownContests();
        }

        private void AddKnownContests()
        {
            foreach (var contestInfo in knownContests)
            {
                if (Contests.All(c => c.TrackListUrl != contestInfo.TrackListUrl))
                {
                    Contests.Add(contestInfo);
                }
            }
        }


        private static readonly List<ContestInfo> knownContests = new List<ContestInfo>()
            {
                new ContestInfo() { IsoStoreFileName = "OSC28.xml", Name="OSC 28 (Tyrell N6)", TrackListUrl="http://www.archive.org/download/KvrOsc28TyrellN6/KvrOsc28TyrellN6_files.xml" },
                new ContestInfo() { IsoStoreFileName = "OSC29.xml", Name="OSC 29 (String Theory)", TrackListUrl="http://www.archive.org/download/KvrOsc29StringTheory/KvrOsc29StringTheory_files.xml" },
                new ContestInfo() { IsoStoreFileName = "OSC30.xml", Name="OSC 30 (Farbrausch V2)", TrackListUrl="http://www.archive.org/download/KvrOsc30FarbrauschV2/KvrOsc30FarbrauschV2_files.xml" },
                new ContestInfo() { IsoStoreFileName = "OSC33.xml", Name="OSC 33 (Charlatan)", TrackListUrl="http://www.archive.org/download/KvrOsc33Charlatan/KvrOsc33Charlatan_files.xml" },
                new ContestInfo() { IsoStoreFileName = "OSC34.xml", Name="OSC 34 (Sonigen)", TrackListUrl="http://www.archive.org/download/KvrOsc34Sonigen/KvrOsc34Sonigen_files.xml" },
                new ContestInfo() { IsoStoreFileName = "OSC35.xml", Name="OSC 35 (Diva)", TrackListUrl="http://www.archive.org/download/KvrOsc35Diva/KvrOsc35Diva_files.xml" },
            };

    }



}
