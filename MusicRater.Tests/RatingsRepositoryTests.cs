﻿using System;
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

namespace MusicRater.Tests
{
    [TestClass]
    public class RatingsRepositoryTests
    {
        [TestMethod]
        public void ShouldNotThrowAnExceptionIfFileNotFound()
        {
            var iso = new Mock<IIsolatedStore>();
            iso.Setup((x) => x.FileExists(It.IsAny<string>())).Returns(false);
            RatingsRepository rr = new RatingsRepository(iso.Object);
            var r = rr.Load();
            Assert.AreEqual(0, r.Count());
        }
    }
}