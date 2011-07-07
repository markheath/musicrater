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
using MusicRater.Model;

namespace MusicRater.Tests
{
    class TrackBuilder
    {
        public Track BuildWithTitle(string title)
        {
            var track = new Track(new Rating[] { });
            track.Title = title;
            track.Author = "Author";
            track.Url = "http://ignored.com/track.mp3";
            track.Comments = "";
            return track;
        }
    }
}
