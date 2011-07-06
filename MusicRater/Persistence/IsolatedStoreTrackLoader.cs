﻿using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;

namespace MusicRater
{
    public class IsolatedStoreTrackLoader : ITrackLoader
    {
        public string FileName { get; private set; }

        public IsolatedStoreTrackLoader(string fileName)
        {
            this.FileName = fileName;
        }

        public void BeginLoad()
        {
            var tracks = new List<Track>();
            using (RatingsRepository repo = new RatingsRepository(new IsolatedStore()))
            { 
                tracks.AddRange(repo.Load(this.FileName));
            }
            KvrTrackLoader.Shuffle(tracks, new Random());
            if (Loaded != null)
            {
                Loaded(this, new LoadedEventArgs() { Tracks = tracks });
            }
        }

        public event EventHandler<LoadedEventArgs> Loaded;
    }
}