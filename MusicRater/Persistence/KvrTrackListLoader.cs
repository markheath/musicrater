using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Xml.Linq;
using MusicRater.Model;

namespace MusicRater
{
    public class KvrTrackListLoader
    {
        public event EventHandler<LoadedEventArgs<List<Track>>> Loaded;

        private readonly string loadUrl;

        public KvrTrackListLoader(string loadUrl)
        {
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
                OnLoaded(new LoadedEventArgs<List<Track>>(e.Error));
            }
        }

        void LoadTrackList(string xml, string prefix)
        {
            var tracks = new List<Track>();
            var xdoc = XDocument.Parse(xml);
            foreach (var file in xdoc.Element("files").Elements("file"))
            {
                string audioFileName = file.Attribute("name").Value;
                if (audioFileName.EndsWith(".mp3", StringComparison.OrdinalIgnoreCase))
                {
                    var t = new Track();
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
                    tracks.Add(t);
                }
            }
            Shuffle(tracks, new Random());
            OnLoaded(new LoadedEventArgs<List<Track>> (tracks));
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


        protected virtual void OnLoaded(LoadedEventArgs<List<Track>>  e)
        {
            EventHandler<LoadedEventArgs<List<Track>>> handler = Loaded;
            if (handler != null) handler(this, e);
        }
    }

}
