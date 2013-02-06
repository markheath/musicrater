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
                                     new XAttribute("Rating", t.Rating),
                                     new XAttribute("IsExcluded", t.IsExcluded),
                                     new XElement("Comments", t.Comments)
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
                    var track = CreateTrackFromNode(trackNode);
                    contest.Tracks.Add(track);
                }

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

        private Track CreateTrackFromNode(XElement trackNode)
        {
            var t = new Track();
            t.Author = trackNode.Attribute("Author").Value;
            t.Title = trackNode.Attribute("Title").Value;
            t.Url = trackNode.Attribute("Url").Value;
            var ratingAttribute = trackNode.Attribute("Rating");
            if (ratingAttribute != null) t.Rating = Int32.Parse(ratingAttribute.Value);
            t.Listens = Int32.Parse(trackNode.Attribute("Listens").Value);
            t.IsExcluded = Boolean.Parse(trackNode.Attribute("IsExcluded").Value);
            var commentsAttribute = trackNode.Element("Comments");
            if (commentsAttribute != null) t.Comments = commentsAttribute.Value;
            return t;
        }
    }
}
