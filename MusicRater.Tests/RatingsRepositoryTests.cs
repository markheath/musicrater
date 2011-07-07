using System;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Silverlight.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MusicRater.Model;
using System.IO;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Text;

namespace MusicRater.Tests
{
    [TestClass]
    public class RatingsRepositoryTests
    {
        private RatingsRepositoryBuilder builder;

        [TestInitialize]
        public void SetUp()
        {
            builder = new RatingsRepositoryBuilder();
        }

        [TestMethod]
        public void LoadShouldReturnFalseIfFileNotFound()
        {
            builder.FileExists = false;
            var repo = builder.Build();
            var loaded = repo.Load(new Contest("blah.xml", "http://ignored"));
            Assert.IsFalse(loaded);
        }

        [TestMethod]
        public void LoadShouldPopulateCriteriaOnTheContest()
        {
            builder.Xml = TestData.OneTrackXml;
            var repo = builder.Build();
            var contest = new Contest("blah.xml", "http://ignored");

            var loaded = repo.Load(contest);

            Assert.IsTrue(loaded);
            Assert.AreEqual(3, contest.Criteria.Count, "Criteria Count");
            Assert.AreEqual("Criteria 1", contest.Criteria[0].Name);
            Assert.AreEqual(1, contest.Criteria[0].Weight);
            Assert.AreEqual("Criteria 2", contest.Criteria[1].Name);
            Assert.AreEqual(2, contest.Criteria[1].Weight);
            Assert.AreEqual("Criteria 3", contest.Criteria[2].Name);
            Assert.AreEqual(3, contest.Criteria[2].Weight);
        }

        [TestMethod]
        public void LoadShouldPopulateTracksOnTheContest()
        {
            builder.Xml = TestData.OneTrackXml;
            var repo = builder.Build();
            var contest = new Contest("blah.xml", "http://ignored");

            var loaded = repo.Load(contest);

            Assert.IsTrue(loaded);
            Assert.AreEqual(1, contest.Tracks.Count, "Track Count");
            Track firstTrack = contest.Tracks[0];
            Assert.AreEqual("t", firstTrack.Title);
            Assert.AreEqual("a", firstTrack.Author);
            Assert.AreEqual(false, firstTrack.IsExcluded);
            Assert.AreEqual("u", firstTrack.Url);
            Assert.AreEqual(1, firstTrack.Listens);
        }

        [TestMethod]
        public void LoadShouldPopulateRatingValuesOnTheContest()
        {
            builder.Xml = TestData.OneTrackXml;
            var repo = builder.Build();
            var contest = new Contest("blah.xml", "http://ignored");

            var loaded = repo.Load(contest);

            Assert.IsTrue(loaded);
            List<Rating> subRatings = new List<Rating>(contest.Tracks[0].SubRatings);
            Assert.AreEqual(3, subRatings.Count, "SubRatings Count");
            Assert.AreEqual(5, subRatings[0].Value);
            Assert.AreEqual(6, subRatings[1].Value);
            Assert.AreEqual(7, subRatings[2].Value);
        }

        [TestMethod]
        public void SaveShouldCreateValidXmlFile()
        {
            var repo = builder.Build();
            var contest = new ContestBuilder().Build();
            repo.Save(contest);
            XDocument xdoc = XDocument.Parse(builder.SavedXml);
            Assert.AreEqual(1, xdoc.Elements("Tracks").Count());
        }
    }

    class ContestBuilder
    {
        public string FileName = "blah.xml";
        public string LoadUrl = "http://ignored.com/blah.xml";

        public Contest Build()
        {
            var contest = new Contest(FileName, LoadUrl);

            return contest;
        }
    }

    class RatingsRepositoryBuilder
    {
        public RatingsRepositoryBuilder()
        {
            FileExists = true;
            Xml = "";
            this.outputStream = new IgnoreDisposeStream();
        }

        public bool FileExists { get; set; }
        public string Xml { get; set; }
        public string SavedXml { get { return Encoding.UTF8.GetString(outputStream.GetBuffer(), 0, (int)outputStream.Length); } }
        private MemoryStream outputStream;

        public RatingsRepository Build()
        {
            var iso = new Mock<IIsolatedStore>();
            iso.Setup((x) => x.FileExists(It.IsAny<string>())).Returns(FileExists);
            iso.Setup((x) => x.OpenFile(It.IsAny<string>())).Returns(() => new MemoryStream(System.Text.UTF8Encoding.UTF8.GetBytes(Xml)));
            iso.Setup((x) => x.CreateFile(It.IsAny<string>())).Returns(outputStream);
            return new RatingsRepository(iso.Object);
        }
    }

    class IgnoreDisposeStream : MemoryStream
    {
        protected override void Dispose(bool disposing)
        {
            //base.Dispose(disposing);
        }
    }
}