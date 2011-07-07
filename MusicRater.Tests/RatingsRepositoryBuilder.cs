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
using System.IO;
using System.Text;
using Moq;

namespace MusicRater.Tests
{
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
}
