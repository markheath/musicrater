using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Xml.Linq;
using MusicRater.Model;

namespace MusicRater
{
    public class KvrTrackLoader : ITrackLoader
    {
        public event EventHandler<LoadedEventArgs> Loaded;
        private readonly Contest contest;

        public KvrTrackLoader(Contest contest)
        {
            this.contest = contest;            
        }

        public void BeginLoad()
        {
            WebClient wc = new WebClient();
            Uri uri = new Uri(this.contest.LoadUrl, UriKind.Absolute);
            wc.DownloadStringCompleted += new DownloadStringCompletedEventHandler(wc_DownloadStringCompleted);
            wc.DownloadStringAsync(uri);
        }

        void wc_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                string path = this.contest.LoadUrl.Substring(0,this.contest.LoadUrl.LastIndexOf('/')+1);
                LoadTrackList(e.Result, path);
            }
            else
            {
                RaiseLoadedEvent(new LoadedEventArgs() { Error = e.Error });
            }
        }

        void LoadTrackList(string xml, string prefix)
        {
            this.contest.Criteria.Add(new Criteria("Song Writing"));
            this.contest.Criteria.Add(new Criteria("Sounds"));
            this.contest.Criteria.Add(new Criteria("Production"));
            XDocument xdoc = XDocument.Parse(xml);
            foreach (var file in xdoc.Element("files").Elements("file"))
            {
                string fileName = file.Attribute("name").Value;
                if (fileName.EndsWith(".mp3", StringComparison.OrdinalIgnoreCase))
                {
                    var t = new Track(from c in this.contest.Criteria select new Rating(c));
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
                        string nameOnly = fileName.Substring(0, fileName.Length - 4);
                        int index = nameOnly.IndexOf("-");
                        t.Author = index == -1 ? "Unknown" : nameOnly.Substring(0, index);
                        t.Title = index == -1 ? nameOnly : nameOnly.Substring(index + 1);
                    }
                    t.Url = prefix + fileName;
                    this.contest.Tracks.Add(t);
                }
            }
            Shuffle(this.contest.Tracks, new Random());
            RaiseLoadedEvent(new LoadedEventArgs() { Tracks = this.contest.Tracks });
        }

        public void RaiseLoadedEvent(LoadedEventArgs args)
        {
            if (Loaded != null)
            {
                Loaded(this, args);
            }
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
    }
}
