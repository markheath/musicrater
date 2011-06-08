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

namespace MusicRater.Tests
{
    [TestClass]
    public class RatingsRepositoryTests
    {
        [TestMethod]
        public void NoExceptionIfNotFound()
        {
            RatingsRepository rr = new RatingsRepository();
            var r = rr.Load();
            Assert.AreEqual(0, r.Count());
        }
    }
}