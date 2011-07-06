using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MusicRater;
using System.Collections.Generic;
using MusicRater.Model;

namespace MusicRater.Tests
{
    [TestClass]
    public class ExtensionMethodsUnitTests
    {
        [TestMethod]
        public void TestOnceRoundStartingAfter()
        {
            int[] numbers = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            int[] newList = numbers.OnceRoundStartingAfter(5).ToArray();
            AssertListsAreEqual(newList, new int[] { 6, 7, 8, 9, 10, 1, 2, 3, 4, 5 });
        }

        [TestMethod]
        public void OnceRoundStartingAfterWithExclusions()
        {
            Rating[] ratings = new Rating[] { };
            var first = new Track(ratings) { IsExcluded = true, Title="Track 1"};
            var second = new Track(ratings) { Title="Track 2"};
            var third = new Track(ratings) { Title="Track 3"};
            var fourth = new Track(ratings) { IsExcluded = true, Title = "Track 4" };
            List<Track> tracks = new List<Track>();
            tracks.AddRange(new Track[] { first, second, third, fourth });

            var newTracks = new List<Track>(tracks.OnceRoundStartingAfter(third));

            var next = tracks.OnceRoundStartingAfter(third).Where(t => !t.IsExcluded).FirstOrDefault();
            Assert.AreSame(second, next);
        }

        [TestMethod]
        public void ListsWithDifferentItemsAreNotEqual()
        {
            Assert.IsFalse(AreListsEqual(new int[] { 1, 2, 3 }, new int[] { 1, 2, 4 }));
        }

        [TestMethod]
        public void ListsWithDifferentLengthsAreNotEqual()
        {
            Assert.IsFalse(AreListsEqual(new int[] { 1, 2, 3 }, new int[] { 1, 2 }));
        }

        private static bool AreListsEqual<T>(IEnumerable<T> first, IEnumerable<T> second)
        {
            var firstList = first.ToList();
            var secondList = second.ToList();
            return (firstList.Count == secondList.Count) && firstList.Zip(secondList, (f, s) => EqualityComparer<T>.Default.Equals(f, s)).All((e) => e);
        }

        public static void AssertListsAreEqual<T>(IEnumerable<T> first, IEnumerable<T> second)
        {            
            if (!AreListsEqual(first,second))
            {
                string message = String.Format("{0} != {1}",                    
                    string.Join(",", first.Select(s => s.ToString())),
                    string.Join(",", second.Select(s => s.ToString())));
                Assert.IsTrue(false, message);
            }            
        }
    }
}
