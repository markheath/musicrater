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
using System.IO.IsolatedStorage;
using System.IO;
using System.Xml.Linq;
using System.Linq;

namespace MusicRater
{
    public class RatingsRepository : IDisposable
    {
        private readonly IIsolatedStore isolatedStore;

        public RatingsRepository(IIsolatedStore isolatedStore)
        {
            this.isolatedStore = isolatedStore;
        }

        public void Save(IEnumerable<TrackViewModel> tracks, string fileName)
        {
            var trackNodes = from t in tracks
                             select new XElement("Track",
                                     new XAttribute("Author", t.Author),
                                     new XAttribute("Title", t.Title),
                                     new XAttribute("Url", t.Url),
                                     new XAttribute("Listens", t.Listens),
                                     new XAttribute("IsExcluded", t.IsExcluded),
                                     new XElement("Comments", t.Comments),
                                     new XElement("SubRatings",
                                         from s in t.SubRatings
                                         select new XElement("SubRating",
                                             new XAttribute("Name", s.Name),
                                             new XAttribute("Weight", s.Criteria.Weight),
                                             new XAttribute("Value", s.Value))
                                  )
                         );

            var xelement = new XElement("Tracks", trackNodes);
            using (var outWriter = new StreamWriter(isolatedStore.CreateFile(fileName)))
            {
                outWriter.Write(xelement.ToString());
            }
        }

        public IEnumerable<Track> Load(string fileName)
        {
            var criteria = new Dictionary<string, Criteria>();
            if (!isolatedStore.FileExists(fileName))
            {
                yield break;
            }
            using (var reader = isolatedStore.OpenFile(fileName))
            {
                XDocument doc = XDocument.Load(reader);
                foreach (var trackNode in doc.Element("Tracks").Elements("Track"))
                {
                    yield return CreateTrackFromNode(trackNode, criteria);
                }
            }
        }

        private Track CreateTrackFromNode(XElement trackNode, Dictionary<string, Criteria> criteriaDictionary)
        {
            var ratings = new List<Rating>();
            // read the sub-ratings first
            foreach (var node in trackNode.Element("SubRatings").Elements("SubRating"))
            {
                string criteriaName = node.Attribute("Name").Value;
                Criteria c = null;
                if (!criteriaDictionary.TryGetValue(criteriaName, out c))
                {
                    c = new Criteria(criteriaName, Int32.Parse(node.Attribute("Weight").Value));
                    criteriaDictionary.Add(criteriaName, c);
                }
                Rating r = new Rating(c);
                r.Value = Int32.Parse(node.Attribute("Value").Value);
                ratings.Add(r);
            }
            Track t = new Track(ratings);
            t.Author = trackNode.Attribute("Author").Value;
            t.Title = trackNode.Attribute("Title").Value;
            t.Url = trackNode.Attribute("Url").Value;
            t.Listens = Int32.Parse(trackNode.Attribute("Listens").Value);
            t.IsExcluded = Boolean.Parse(trackNode.Attribute("IsExcluded").Value);
            if (trackNode.Element("PositiveComments") != null)
            {
                // load in legacy comments
                t.Comments = trackNode.Element("PositiveComments").Value + "\r\n" +
                    trackNode.Element("Suggestions").Value;
            }
            else if (trackNode.Element("Comments") != null)
            {
                t.Comments = trackNode.Element("Comments").Value;
            }
            return t;
        }

        public void Dispose()
        {
            this.isolatedStore.Dispose();
        }
    }
}
