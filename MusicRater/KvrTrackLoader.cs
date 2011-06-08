using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Xml.Linq;

namespace MusicRater
{
    public class LoadedEventArgs : EventArgs
    {
        public Exception Error { get; set; }
        public IEnumerable<Track> Tracks { get; set; }
    }

    public class KvrTrackLoader
    {
        public event EventHandler<LoadedEventArgs> Loaded;

        public KvrTrackLoader()
        {
            WebClient wc = new WebClient();
            Uri trackListUri = new Uri("http://www.archive.org/download/KvrOsc28TyrellN6/KvrOsc28TyrellN6_files.xml", UriKind.Absolute);
            wc.DownloadStringCompleted += new DownloadStringCompletedEventHandler(wc_DownloadStringCompleted);
            wc.DownloadStringAsync(trackListUri);
        }

        void wc_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                LoadTrackList(e.Result, "http://www.archive.org/download/KvrOsc28TyrellN6/");
            }
            else
            {
                RaiseLoadedEvent(new LoadedEventArgs() { Error = e.Error });
            }
        }

        void LoadTrackList(string xml, string prefix)
        {
            var tracks = new List<Track>();
            var criteria = new List<Criteria>();
            criteria.Add(new Criteria("Song Writing"));
            criteria.Add(new Criteria("Sounds"));
            criteria.Add(new Criteria("Production"));
            XDocument xdoc = XDocument.Parse(xml);
            foreach (var file in xdoc.Element("files").Elements("file"))
            {
                string fileName = file.Attribute("name").Value;
                if (fileName.EndsWith(".mp3", StringComparison.OrdinalIgnoreCase))
                {
                    var t = new Track(from c in criteria select new Rating(c));
                    var titleElement = file.Element("title");
                    if (titleElement != null)
                    {
                        string title = file.Element("title").Value;
                        int index = title.IndexOf(" - ");
                        if (index == -1) index = title.IndexOf("-");
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
                    tracks.Add(t);
                }
            }
            Shuffle(tracks, new Random());
            RaiseLoadedEvent(new LoadedEventArgs() { Tracks = tracks });
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
