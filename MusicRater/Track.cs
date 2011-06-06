using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MusicRater
{
    public class Track
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public string Url { get; set; }
        public int Rating { get; set; }
        public int Listens { get; set; }
        public int Comments { get; set; }
    }
}
