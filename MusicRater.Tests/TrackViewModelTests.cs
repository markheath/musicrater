using System;
using System.Net;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace MusicRater.Tests
{
    [TestClass]
    public class TrackViewModelTests
    {
        [TestMethod]
        public void TrackViewModelShouldReportPropertyChangedForPropertyChanges()
        {
            TrackViewModel tvm = new TrackViewModel(new Track(new Rating[] {}));

            PropertyChangedEventArgs nea = null;
            tvm.PropertyChanged += (s, e) => nea = e;
            tvm.Comments = "Good";
            Assert.IsNotNull(nea);
        }

        [TestMethod]
        public void TrackViewModelShouldReportPropertyChangedForRatingChanges()
        {            
            Rating r = new Rating(new Criteria("Hello"));
            TrackViewModel tvm = new TrackViewModel(new Track(new Rating[] { r }));

            PropertyChangedEventArgs nea = null;
            tvm.PropertyChanged += (s, e) => nea = e;
            tvm.SubRatings.First().Value++;
            Assert.IsNotNull(nea);
        }
    }
}
