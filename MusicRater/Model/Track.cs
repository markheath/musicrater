using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GalaSoft.MvvmLight;
using System.Diagnostics;
using System.Windows.Media;

namespace MusicRater.Model
{
    public class Track
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public string Url { get; set; }
        public string Comments { get; set; }
        public bool IsExcluded { get; set; }
        public int Rating { get; set; }
        public int Listens { get; set; }
    }
}
