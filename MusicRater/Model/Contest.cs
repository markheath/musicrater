using System;
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
        public Contest(string fileName)
        {
            FileName = fileName;
            Criteria = new List<Criteria>();
            Tracks = new List<Track>();
        }

        /// <summary>
        /// The Filename that should be used to save this contest within isolated store
        /// </summary>
        public string FileName { get; private set; }

        /// <summary>
        /// The URL to load contest track info from (KVR OSC metadata on archive.org only supported at the moment)
        /// </summary>
        public string LoadUrl { get; set; }

        /// <summary>
        /// The judging criteria for this contest
        /// </summary>
        public List<Criteria> Criteria { get; private set; }

        /// <summary>
        /// The tracks contained within this contest
        /// </summary>
        public List<Track> Tracks { get; private set; }

        /// <summary>
        /// A friendly name for this contest
        /// </summary>
        public string Name { get; set; }
    }
}
