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

namespace MusicRater.Model
{
    /// <summary>
    /// Holds the information about a given contest
    /// </summary>
    public class Contest
    {
        private readonly ContestInfo contestInfo;

        public Contest(ContestInfo contestInfo)
        {
            this.contestInfo = contestInfo;
            Tracks = new List<Track>();
        }

        /// <summary>
        /// The Filename that should be used to save this contest within isolated store
        /// </summary>
        public string FileName { get { return contestInfo.IsoStoreFileName; } }

        /// <summary>
        /// The URL to load contest track info from (KVR OSC metadata on archive.org only supported at the moment)
        /// </summary>
        public string TrackListUrl { get { return contestInfo.TrackListUrl; } }

        /// <summary>
        /// The tracks contained within this contest
        /// </summary>
        public List<Track> Tracks { get; private set; }

        /// <summary>
        /// A friendly name for this contest
        /// </summary>
        public string Name { get { return contestInfo.Name; } }
    }
}
