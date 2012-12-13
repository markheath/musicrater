using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using System.Linq;
using MusicRater.Model;

namespace MusicRater
{
    public class RatingsRepository
    {
        private readonly IIsolatedStore isolatedStore;

        public RatingsRepository(IIsolatedStore isolatedStore)
        {
            this.isolatedStore = isolatedStore;
        }

        public IEnumerable<string> GetContestFiles()
        {
            return isolatedStore.GetFileNames("*.xml");
        }

        public void Save(Contest contest)
        {
            var trackNodes = from t in contest.Tracks
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
            
            var tracksElement = new XElement("Tracks", trackNodes);
            var contestElement = new XElement("Contest",
                                              new XElement("TrackListUrl", contest.TrackListUrl),
                                              new XElement("FileName", contest.FileName),
                                              new XElement("Name", contest.Name),
                                              tracksElement);
            using (var outWriter = new StreamWriter(isolatedStore.CreateFile(contest.FileName)))
            {
                outWriter.Write(contestElement.ToString());
            }
        }

        public Contest Load(string fileName)
        {
            var criteria = new Dictionary<string, Criteria>();
            if (!isolatedStore.FileExists(fileName))
            {
                return null;
            }
            using (var reader = isolatedStore.OpenFile(fileName))
            {
                var contestInfo = new ContestInfo() {IsoStoreFileName = fileName};
                var contest = new Contest(contestInfo);
                var doc = XDocument.Load(reader);
                foreach (var trackNode in doc.Element("Contest").Element("Tracks").Elements("Track"))
                {
                    var track = CreateTrackFromNode(trackNode, criteria);
                    contest.Tracks.Add(track);
                }
                contest.Criteria.AddRange(criteria.Values);

                var contestElement = doc.Element("Contest");
                if (contestElement != null)
                {

                    var loadUrlElement = doc.Element("TrackListUrl");
                    if (loadUrlElement != null)
                        contestInfo.TrackListUrl = loadUrlElement.Value;

                    var nameElement = doc.Element("Name");
                    if (nameElement != null)
                        contestInfo.Name = nameElement.Value;
                }
                return contest;
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
                var r = new Rating(c);
                r.Value = Int32.Parse(node.Attribute("Value").Value);
                ratings.Add(r);
            }
            var t = new Track(ratings);
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
    }
}
