using System;
using System.Collections.Generic;
using System.Net;
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
        public ObservableCollection<Contest> Contests { get; private set; }
        

        public OpenContestWindowViewModel(IIsolatedStore store)
        {
            this.Contests = new ObservableCollection<Contest>();
            
            var repo = new RatingsRepository(store);
            
            foreach(var fileName in store.GetFileNames("*.xml"))
            {
                repo.Load(fileName);

            }

            AddKnownContests();
        }

        private void AddKnownContests()
        {
            foreach (var contestUrl in knownContests)
            {
                
            }
        }


        private static readonly List<string> knownContests = new List<string>()
            {
                "http://www.archive.org/download/KvrOsc28TyrellN6/KvrOsc28TyrellN6_files.xml",
                "http://www.archive.org/download/KvrOsc29StringTheory/KvrOsc29StringTheory_files.xml",
                "http://www.archive.org/download/KvrOsc30FarbrauschV2/KvrOsc30FarbrauschV2_files.xml",
                "http://www.archive.org/download/KvrOsc33Charlatan/KvrOsc33Charlatan_files.xml",
                "http://www.archive.org/download/KvrOsc34Sonigen/KvrOsc34Sonigen_files.xml",
                "http://www.archive.org/download/KvrOsc35Diva/KvrOsc35Diva_files.xml",
            };

    }



}
