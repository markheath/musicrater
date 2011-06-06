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
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace MusicRater
{
    public class MainPageViewModel : ViewModelBase
    {
        private MediaElement me;
        private Track selectedTrack;

        public MainPageViewModel(MediaElement me)
        {
            this.me = me; // new MediaElement();            
            this.me.BufferingProgressChanged += (s, e) => { this.BufferingProgress = me.BufferingProgress; RaisePropertyChanged("BufferingProgress"); };
            this.Tracks = new ObservableCollection<Track>();
            this.Tracks.Add(new Track()
            {
                Url = "http://www.archive.org/download/KvrOsc28TyrellN6/Acronym-Dryrell.mp3",
                Title = "Dryrell",
                Author = "Acronym"
            });
            this.Tracks.Add(new Track()
            {
                Url = "http://www.archive.org/download/KvrOsc28TyrellN6/Acronym-Radium.mp3",
                Title = "Radium",
                Author = "Acronym"
            });
            this.PlayCommand = new RelayCommand(() => Play());
        }

        private void Play()
        {
            me.Source = new Uri(this.SelectedTrack.Url, UriKind.Absolute);
                        
            me.Play();
        }

        public double BufferingProgress { get; private set; }
        public ObservableCollection<Track> Tracks { get; private set; }
        public ICommand PlayCommand { get; private set; }
        public Track SelectedTrack
        {
            get
            {
                return selectedTrack;
            }
            set
            {
                if (selectedTrack != value)
                {
                    selectedTrack = value;
                    RaisePropertyChanged("SelectedTrack");
                }
            }
        }
    }
}
