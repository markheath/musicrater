using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MusicRater.ViewModels;
using MusicRater.Model;

namespace MusicRater
{
    public class MainPageViewModel : ViewModelBase
    {
        private MediaElement me;        
        private TrackViewModel selectedTrack;
        private DispatcherTimer timer;
        private bool dirtyFlag;
        private bool isLoading;
        private Contest contest;
        
        public MainPageViewModel(MediaElement me)
        {
            this.me = me;
            this.me.AutoPlay = false;
            this.me.BufferingProgressChanged += (s, e) => { this.BufferingProgress = me.BufferingProgress; RaisePropertyChanged("BufferingProgress"); };
            this.me.MediaFailed += (s, e) => this.ShowError("Error loading " + me.Source.ToString()); // e.ErrorException.Message
            this.me.MediaOpened += me_MediaOpened;
            this.me.MediaEnded += (s, e) => { SelectedTrack.Listens++; Next(); };
            this.me.DownloadProgressChanged += (s, e) => { this.DownloadProgress = me.DownloadProgress * 100; RaisePropertyChanged("DownloadProgress"); };

            this.timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(500);
            timer.Tick += new EventHandler(timer_Tick);
            timer.Start();

            this.Tracks = new ObservableCollection<TrackViewModel>();
            this.PlayCommand = new RelayCommand(() => me.Play());
            this.PauseCommand = new RelayCommand(() => me.Pause());
            this.NextCommand = new RelayCommand(() => Next());
            this.PrevCommand = new RelayCommand(() => Prev());
            this.AnonCommand = new AnonymiseCommand(this.Tracks);

            //"http://www.archive.org/download/KvrOsc28TyrellN6/KvrOsc28TyrellN6_files.xml"            
            this.contest = new Contest("KVR-OSC-29.xml", "http://www.archive.org/download/KvrOsc29StringTheory/KvrOsc29StringTheory_files.xml");

            var kvrLoader = new KvrTrackLoader(this.contest);
            var isoLoader = new IsolatedStoreTrackLoader(this.contest, new IsolatedStore());
            ITrackLoader loader = new CombinedTrackLoader(kvrLoader, isoLoader);
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
                this.ShowError(e.Error.Message);
            }
            else
            {
                foreach (var t in e.Tracks)
                {
                    var trackViewModel = new TrackViewModel(t);
                    trackViewModel.AnonymousMode = this.AnonCommand.AnonymousMode;
                    trackViewModel.PropertyChanged += (s, args) => this.dirtyFlag = true;
                    this.Tracks.Add(trackViewModel);
                }
                this.SelectedTrack = this.Tracks.Where(t => !t.IsExcluded).FirstOrDefault();
            }
            this.IsLoading = false;
        }
        
        void timer_Tick(object sender, EventArgs e)
        {
            if (me.CurrentState == MediaElementState.Playing)
            {
                this.RaisePropertyChanged("PlaybackPosition");
            }
            if (dirtyFlag == true)
            {
                using (RatingsRepository repo = new RatingsRepository(new IsolatedStore()))
                { 
                    repo.Save(this.contest);
                }
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

        private void ShowError(string message)
        {
            ErrorMessageWindow w = new ErrorMessageWindow();
            w.DataContext = new ErrorMessageWindowViewModel() { Message = message };            
            w.Show();
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

        public class ErrorMessageWindowViewModel
        {
            public string Message { get; set; }
        }
    }
}
