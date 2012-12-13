using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Xml.Linq;
using MusicRater.Model;

namespace MusicRater
{
    public class KvrContestLoader : IContestLoader
    {
        public event EventHandler<ContestLoadedEventArgs> Loaded;

        private readonly string fileName;
        private readonly string loadUrl;

        public KvrContestLoader(string fileName, string loadUrl)
        {
            this.fileName = fileName;
            this.loadUrl = loadUrl;
        }

        public void BeginLoad()
        {
            var wc = new WebClient();
            Uri uri = new Uri(loadUrl, UriKind.Absolute);
            wc.DownloadStringCompleted += wc_DownloadStringCompleted;
            wc.DownloadStringAsync(uri);
        }

        void wc_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                string path = loadUrl.Substring(0,loadUrl.LastIndexOf('/')+1);
                LoadTrackList(e.Result, path);
            }
            else
            {
                OnLoaded(new ContestLoadedEventArgs(e.Error));
            }
        }

        void LoadTrackList(string xml, string prefix)
        {
            var contest = new Contest(fileName);
            contest.Criteria.Add(new Criteria("Song Writing"));
            contest.Criteria.Add(new Criteria("Sounds"));
            contest.Criteria.Add(new Criteria("Production"));
            XDocument xdoc = XDocument.Parse(xml);
            foreach (var file in xdoc.Element("files").Elements("file"))
            {
                string audioFileName = file.Attribute("name").Value;
                if (audioFileName.EndsWith(".mp3", StringComparison.OrdinalIgnoreCase))
                {
                    var t = new Track(from c in contest.Criteria select new Rating(c));
                    var titleElement = file.Element("title");
                    if (titleElement != null)
                    {
                        string title = file.Element("title").Value;
                        string sep = " - ";
                        int index = title.IndexOf(sep);
                        if (index == -1)
                        {
                            sep = "-";
                            index = title.IndexOf(sep);
                        }
                        t.Author = index == -1 ? "Unknown" : title.Substring(0, index);
                        t.Title = index == -1 ? title : title.Substring(index + sep.Length);
                        t.Title = t.Title.Trim();
                    }
                    else
                    {
                        // work it out from the MP3 name
                        string nameOnly = audioFileName.Substring(0, audioFileName.Length - 4);
                        int index = nameOnly.IndexOf("-");
                        t.Author = index == -1 ? "Unknown" : nameOnly.Substring(0, index);
                        t.Title = index == -1 ? nameOnly : nameOnly.Substring(index + 1);
                    }
                    t.Url = prefix + audioFileName;
                    contest.Tracks.Add(t);
                }
            }
            Shuffle(contest.Tracks, new Random());
            OnLoaded(new ContestLoadedEventArgs(contest));
        }

        public static void Shuffle<T>(IList<T> list, Random rng)
        {
            // Note i > 0 to avoid final pointless iteration
            for (int i = list.Count - 1; i > 0; i--)
            {
                // Swap element "i" with a random earlier element it (or itself)
                int swapIndex = rng.Next(i + 1);
                T tmp = list[i];
                list[i] = list[swapIndex];
                list[swapIndex] = tmp;
            }
        }


        protected virtual void OnLoaded(ContestLoadedEventArgs e)
        {
            EventHandler<ContestLoadedEventArgs> handler = Loaded;
            if (handler != null) handler(this, e);
        }
    }
}
