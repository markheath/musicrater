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
using System.IO;
using System.Xml;
using System.Xml.Linq;

namespace MusicRater
{
    public class MainPageViewModel : ViewModelBase
    {
        private MediaElement me;
        private Track selectedTrack;
        private string errorMessage;

        public MainPageViewModel(MediaElement me)
        {
            this.me = me; // new MediaElement();            
            this.me.BufferingProgressChanged += (s, e) => { this.BufferingProgress = me.BufferingProgress; RaisePropertyChanged("BufferingProgress"); };
            this.me.MediaFailed += (s, e) => this.ErrorMessage = e.ErrorException.Message;
            this.me.MediaEnded += (s, e) => Next();
            WebClient wc = new WebClient();
            Uri trackListUri = new Uri("http://www.archive.org/download/KvrOsc28TyrellN6/KvrOsc28TyrellN6_files.xml", UriKind.Absolute);
            wc.DownloadStringCompleted += new DownloadStringCompletedEventHandler(wc_DownloadStringCompleted);
            wc.DownloadStringAsync(trackListUri);
            this.Tracks = new ObservableCollection<Track>();
            this.PlayCommand = new RelayCommand(() => Play());
            this.PauseCommand = new RelayCommand(() => Pause());
            this.NextCommand = new RelayCommand(() => Next());
            this.PrevCommand = new RelayCommand(() => Prev());
        }

        void wc_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                LoadTrackList(e.Result, "http://www.archive.org/download/KvrOsc28TyrellN6/");
            }
            else
            {
                this.ErrorMessage = e.Error.Message;
            }
        }

        void LoadTrackList(string xml, string prefix)
        {
            XDocument xdoc = XDocument.Parse(xml);
            foreach (var file in xdoc.Element("files").Elements("file"))
            {
                string fileName = file.Attribute("name").Value;
                if (fileName.EndsWith(".mp3", StringComparison.OrdinalIgnoreCase))
                {
                    Track t = new Track();
                    var titleElement = file.Element("title");
                    if (titleElement != null)
                    {
                        string title = file.Element("title").Value;
                        int index = title.IndexOf(" - ");
                        t.Author = index == -1 ? "Unknown" : title.Substring(0, index);
                        t.Title = index == -1 ? title : title.Substring(index + 3);
                    }
                    else
                    {
                        // work it out from the MP3 name
                        string nameOnly = fileName.Substring(0, fileName.Length - 4);
                        int index = nameOnly.IndexOf("-");
                        t.Author = index == -1 ? "Unknown" : nameOnly.Substring(0, index);
                        t.Title = index == -1 ? nameOnly : nameOnly.Substring(index + 3);
                    }
                    t.Url = prefix + fileName;
                    this.Tracks.Add(t);
                }
            }

        }

        private void Play()
        {
            me.Source = new Uri(this.SelectedTrack.Url, UriKind.Absolute);
            me.Play();
        }

        private void Pause()
        {
            me.Pause();
        }

        private void Next()
        {
            int index = Tracks.IndexOf(SelectedTrack);
            index++;
            if (index >= Tracks.Count)
            {
                index=0;
            }
            SelectedTrack = Tracks[index];
        }

        private void Prev()
        {
            int index = Tracks.IndexOf(SelectedTrack);
            index--;
            if (index < 0)
            {
                index = Tracks.Count - 1;
            }
            SelectedTrack = Tracks[index];
        }

        public double BufferingProgress { get; private set; }
        public ObservableCollection<Track> Tracks { get; private set; }
        public ICommand PlayCommand { get; private set; }
        public ICommand PauseCommand { get; private set; }
        public ICommand NextCommand { get; private set; }
        public ICommand PrevCommand { get; private set; }


        public string ErrorMessage
        {
            get
            {
                return errorMessage;
            }
            set
            {
                if (this.errorMessage != value)
                {
                    this.errorMessage = value;
                    RaisePropertyChanged("ErrorMessage");
                }
            }
        }

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
