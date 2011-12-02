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
using GalaSoft.MvvmLight;
using MusicRater.Model;
using System.Collections.ObjectModel;

namespace MusicRater
{
    public class OpenContestWindowViewModel : ViewModelBase
    {
        public ObservableCollection<string> ContestFiles { get; private set; }
        
        public OpenContestWindowViewModel(IIsolatedStore store)
        {
            this.ContestFiles = new ObservableCollection<string>();
            
            foreach(var fileName in store.GetFileNames("*.xml"))
            {
                this.ContestFiles.Add(fileName);
            }
        }

    }
}
