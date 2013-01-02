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
        private readonly MediaElement me;
        private readonly DispatcherTimer timer;
        private readonly IIsolatedStore isoStore;
        private TrackViewModel selectedTrack;
        private bool dirtyFlag;
        private bool isLoading;
        private Contest contest;

        private static readonly List<ContestInfo> knownContests = new List<ContestInfo>()
            {
                new ContestInfo("OSC28.xml", "OSC 28 (Tyrell N6)", "http://www.archive.org/download/KvrOsc28TyrellN6/KvrOsc28TyrellN6_files.xml"),
                new ContestInfo("OSC29.xml", "OSC 29 (String Theory)", "http://www.archive.org/download/KvrOsc29StringTheory/KvrOsc29StringTheory_files.xml"),
                new ContestInfo("OSC30.xml", "OSC 30 (Farbrausch V2)", "http://www.archive.org/download/KvrOsc30FarbrauschV2/KvrOsc30FarbrauschV2_files.xml"),
                new ContestInfo("OSC33.xml", "OSC 33 (Charlatan)", "http://www.archive.org/download/KvrOsc33Charlatan/KvrOsc33Charlatan_files.xml"),
                new ContestInfo("OSC34.xml", "OSC 34 (Sonigen)", "http://www.archive.org/download/KvrOsc34Sonigen/KvrOsc34Sonigen_files.xml"),
                new ContestInfo("OSC35.xml", "OSC 35 (Diva)", "http://www.archive.org/download/KvrOsc35Diva/KvrOsc35Diva_files.xml"),
                new ContestInfo("OSC46.xml", "OSC 46 (Triple Cheese)", "http://www.archive.org/download/KvrOsc46TripleCheese/KvrOsc46TripleCheese_files.xml"),
                new ContestInfo("KVR-OSC-47.xml", "OSC 47 (Bazille)", "http://www.archive.org/download/KvrOsc47Bazille/KvrOsc47Bazille_files.xml"),
            };

        public MainPageViewModel(MediaElement me, IIsolatedStore isolatedStore)
        {
            isoStore = isolatedStore;

            this.me = me;
            this.me.AutoPlay = false;
            this.me.BufferingProgressChanged += (s, e) => { this.BufferingProgress = me.BufferingProgress; RaisePropertyChanged("BufferingProgress"); };
            this.me.MediaFailed += (s, e) => ShowError("Error loading " + me.Source.ToString()); // e.ErrorException.Message
            this.me.MediaOpened += me_MediaOpened;
            this.me.MediaEnded += (s, e) => { SelectedTrack.Listens++; Next(); };
            this.me.DownloadProgressChanged += (s, e) => { this.DownloadProgress = me.DownloadProgress * 100; RaisePropertyChanged("DownloadProgress"); };

            this.timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(500);
            timer.Tick += OnTimerTick;
            timer.Start();

            this.Tracks = new ObservableCollection<TrackViewModel>();
            this.PlayCommand = new RelayCommand(() => this.me.Play());
            this.PauseCommand = new RelayCommand(() => this.me.Pause());
            this.NextCommand = new RelayCommand(() => Next());
            this.PrevCommand = new RelayCommand(() => Prev());
            this.AnonCommand = new AnonymiseCommand(Tracks);
            this.ConfigCommand = new RelayCommand(() => Config());
            this.OpenCommand = new RelayCommand(() => Open());





            /*var contestInfo = new ContestInfo()
                                  {
                                      IsoStoreFileName = "KVR-OSC-46.xml",
                                      TrackListUrl = "http://www.archive.org/download/KvrOsc46TripleCheese/KvrOsc46TripleCheese_files.xml",
                                      Name = "OSC 46 (Triple Cheese)"
                                  };*/
            var contestInfo = new ContestInfo()
                                  {
                                      IsoStoreFileName = "KVR-OSC-47.xml",
                                      TrackListUrl = "http://www.archive.org/download/KvrOsc47Bazille/KvrOsc47Bazille_files.xml",
                                      Name = "OSC 47 (Bazille)"
                                  };
            var defaultCriteria = new[]
                                      {
                                          new Criteria("Songwriting"),
                                          new Criteria("Sound Design"),
                                          new Criteria("Production")
                                      };

            var loader = new ContestLoader(contestInfo, isoStore, defaultCriteria);
            loader.Loaded += OnContestLoaded;
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

        void OnContestLoaded(object sender, LoadedEventArgs<Contest> e)
        {
            if (e.Error != null)
            {
                this.ShowError(e.Error.Message);
            }
            else
            {
                this.contest = e.Result;
                foreach (var t in e.Result.Tracks)
                {
                    var trackViewModel = new TrackViewModel(t);
                    trackViewModel.AnonymousMode = this.AnonCommand.AnonymousMode;
                    trackViewModel.PropertyChanged += (s, args) => this.dirtyFlag = true;
                    this.Tracks.Add(trackViewModel);
                }
                SelectedTrack = Tracks.FirstOrDefault(t => !t.IsExcluded);
            }
            this.IsLoading = false;
        }
        
        void OnTimerTick(object sender, EventArgs e)
        {
            if (me.CurrentState == MediaElementState.Playing)
            {
                this.RaisePropertyChanged("PlaybackPosition");
            }
            if (dirtyFlag)
            {
                var repo = new RatingsRepository(this.isoStore);
                repo.Save(this.contest);
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
                return me.Position.TotalSeconds;
            }
            set
            {
                me.Position = TimeSpan.FromSeconds(value);
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
            var nextTrack = tracks.OnceRoundStartingAfter(SelectedTrack).FirstOrDefault(t => !t.IsExcluded);
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
        public ICommand ConfigCommand { get; private set; }
        public ICommand OpenCommand { get; private set; }
        public AnonymiseCommand AnonCommand { get; private set; }

        private void ShowError(string message)
        {
            var w = new ErrorMessageWindow();
            w.DataContext = new ErrorMessageWindowViewModel() { Message = message };
            w.Show();
        }

        private void Config()
        {
            var w = new ConfigWindow();
            w.DataContext = new ConfigWindowViewModel(contest.Criteria);
            w.Show();
        }

        private void Open()
        {
            var w = new OpenContestWindow();
            w.DataContext = new OpenContestWindowViewModel(isoStore, knownContests);
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
