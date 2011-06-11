using System;
using System.Linq;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MusicRater.ViewModels;
using System.Collections.Generic;

namespace MusicRater
{
    public class MainPageViewModel : ViewModelBase
    {
        private MediaElement me;
        private string errorMessage;
        private TrackViewModel selectedTrack;
        private DispatcherTimer timer;
        private bool dirtyFlag;
        private bool isLoading;

        public MainPageViewModel(MediaElement me)
        {
            this.me = me;
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

            this.Tracks = new ObservableCollection<TrackViewModel>();
            this.PlayCommand = new RelayCommand(() => Play());
            this.PauseCommand = new RelayCommand(() => Pause());
            this.NextCommand = new RelayCommand(() => Next());
            this.PrevCommand = new RelayCommand(() => Prev());
            this.AnonCommand = new AnonymiseCommand(this.Tracks);

            ITrackLoader loader = new CombinedTrackLoader();
            loader.Loaded += new EventHandler<LoadedEventArgs>(loader_Loaded);
            this.IsLoading = true;
            loader.BeginLoad();
        }

        void CurrentSelectionChanged()
        {
            me.Source = new Uri(this.SelectedTrack.Url, UriKind.Absolute);
            me.Position = TimeSpan.Zero;
            DownloadProgress = 0;
            RaisePropertyChanged("PlaybackPosition");
            RaisePropertyChanged("DownloadProgress");
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
                    trackViewModel.AnonymousMode = this.AnonCommand.AnonymousMode;
                    trackViewModel.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(trackViewModel_PropertyChanged);
                    this.Tracks.Add(trackViewModel);
                }
                this.SelectedTrack = this.Tracks.FirstOrDefault();
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
                repo.Save(this.Tracks);
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

        private void Next()
        {
            SelectNext(this.Tracks);
        }

        private void Prev()
        {
            SelectNext(this.Tracks.Reverse());
        }

        private void SelectNext(IEnumerable<TrackViewModel> tracks)
        {
            var nextTrack = tracks.OnceRoundStartingAfter(SelectedTrack).Where(t => !t.IsExcluded).FirstOrDefault();
            if (nextTrack != null)
            {
                SelectedTrack = nextTrack;
            }
        }

        public double BufferingProgress { get; private set; }
        public double DownloadProgress { get; private set; }
        public ObservableCollection<TrackViewModel> Tracks { get; private set; }
        public ICommand PlayCommand { get; private set; }
        public ICommand PauseCommand { get; private set; }
        public ICommand NextCommand { get; private set; }
        public ICommand PrevCommand { get; private set; }
        public AnonymiseCommand AnonCommand { get; private set; }

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
                return selectedTrack;
            }
            set
            {
                if (selectedTrack != value)
                {
                    selectedTrack = value;
                    RaisePropertyChanged("SelectedTrack");
                    CurrentSelectionChanged();
                }
            }
        }
    }
}
