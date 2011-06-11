using System;
using System.Net;
using System.Linq;
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
using System.Windows.Data;
using System.Collections.Generic;
using System.Windows.Threading;
using System.Diagnostics;

namespace MusicRater
{
    public class MainPageViewModel : ViewModelBase
    {
        private MediaElement me;
        private string errorMessage;
        private ObservableCollection<TrackViewModel> tracksInternal; // this technique from http://www.silverlightplayground.org/post/2009/07/18/Use-CollectionViewSource-effectively-in-MVVM-applications.aspx
        private DispatcherTimer timer;
        private bool dirtyFlag;
        private bool anonymousMode = true;
        private bool isLoading;

        public MainPageViewModel(MediaElement me)
        {
            this.me = me; // new MediaElement();            
            this.me.AutoPlay = false;
            this.me.BufferingProgressChanged += (s, e) => { this.BufferingProgress = me.BufferingProgress; RaisePropertyChanged("BufferingProgress"); };
            this.me.MediaFailed += (s, e) => this.ErrorMessage = e.ErrorException.Message;
            this.me.MediaOpened += me_MediaOpened;
            this.me.MediaEnded += (s, e) => { SelectedTrack.Listens++; Next(); };
            this.me.DownloadProgressChanged += (s, e) => { this.DownloadProgress = me.DownloadProgress * 100; RaisePropertyChanged("DownloadProgress"); };

            this.timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(500);
            timer.Tick += new EventHandler(timer_Tick);
            timer.Start();

            this.tracksInternal = new ObservableCollection<TrackViewModel>();
            this.Tracks = new CollectionViewSource();
            this.Tracks.Source = tracksInternal;
            this.Tracks.View.CurrentChanged += new EventHandler(CurrentSelectionChanged);
            this.PlayCommand = new RelayCommand(() => Play());
            this.PauseCommand = new RelayCommand(() => Pause());
            this.NextCommand = new RelayCommand(() => Next());
            this.PrevCommand = new RelayCommand(() => Prev());
            this.AnonCommand = new RelayCommand(() => Anon());

            ITrackLoader loader = new CombinedTrackLoader();
            loader.Loaded += new EventHandler<LoadedEventArgs>(loader_Loaded);
            this.IsLoading = true;
            loader.BeginLoad();
        }

        void CurrentSelectionChanged(object sender, EventArgs e)
        {
            me.Source = new Uri(this.SelectedTrack.Url, UriKind.Absolute);
            me.Position = TimeSpan.Zero;
            RaisePropertyChanged("PlaybackPosition");
        }

        void loader_Loaded(object sender, LoadedEventArgs e)
        {
            if (e.Error != null)
            {
                this.ErrorMessage = e.Error.Message;
            }
            else
            {
                foreach (var t in e.Tracks)
                {
                    var trackViewModel = new TrackViewModel(t);
                    trackViewModel.AnonymousMode = this.anonymousMode;
                    trackViewModel.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(trackViewModel_PropertyChanged);
                    tracksInternal.Add(trackViewModel);
                }
                this.Tracks.View.MoveCurrentToFirst();
            }
            this.IsLoading = false;
        }

        void trackViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            this.dirtyFlag = true;
        }

        void timer_Tick(object sender, EventArgs e)
        {
            if (me.CurrentState == MediaElementState.Playing)
            {
                this.RaisePropertyChanged("PlaybackPosition");
            }
            if (dirtyFlag == true)
            {
                RatingsRepository repo = new RatingsRepository();
                repo.Save(this.tracksInternal);
                dirtyFlag = false;
            }
        }

        void me_MediaOpened(object sender, RoutedEventArgs e)
        {
            this.Duration = me.NaturalDuration.TimeSpan.TotalSeconds;
            RaisePropertyChanged("Duration");
            this.me.Play();
        }

        public double Duration { get; set; }

        public bool IsLoading
        {
            get
            {
                return this.isLoading;
            }
            set
            {
                if (this.isLoading != value)
                {
                    this.isLoading = value;
                    RaisePropertyChanged("IsLoading");
                }
            }
        }

        public double PlaybackPosition
        {
            get
            {
                return this.me.Position.TotalSeconds;
            }
            set
            {
                // Debug.WriteLine("SET Position {0}", value);
                this.me.Position = TimeSpan.FromSeconds(value);
            }
        }

        private void Play()
        {            
            me.Play();
        }

        private void Pause()
        {
            me.Pause();
        }

        private void Anon()
        {
            this.anonymousMode = !this.anonymousMode;
            foreach (var track in tracksInternal)
            {
                track.AnonymousMode = this.anonymousMode;
            }
        }

        private void Next()
        {
            int originalIndex = Tracks.View.CurrentPosition;
            int index = originalIndex;
            do
            {
                index++;
                if (index >= tracksInternal.Count)
                {
                    index = 0;
                }
                if (!tracksInternal[index].IsExcluded)
                {
                    Tracks.View.MoveCurrentToPosition(index);
                    break;
                }
            } while (index != originalIndex);
        }

        private void Prev()
        {
            int index = Tracks.View.CurrentPosition;
            index--;
            if (index < 0)
            {
                index = tracksInternal.Count - 1;
            }
            Tracks.View.MoveCurrentToPosition(index);
        }

        public double BufferingProgress { get; private set; }
        public double DownloadProgress { get; private set; }
        public CollectionViewSource Tracks { get; private set; }
        public ICommand PlayCommand { get; private set; }
        public ICommand PauseCommand { get; private set; }
        public ICommand NextCommand { get; private set; }
        public ICommand PrevCommand { get; private set; }
        public ICommand AnonCommand { get; private set; }

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

        public TrackViewModel SelectedTrack
        {
            get
            {
                return (TrackViewModel)Tracks.View.CurrentItem;
            }

        }
    }
}
