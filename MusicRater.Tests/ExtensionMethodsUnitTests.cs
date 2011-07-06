using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MusicRater;
using System.Collections.Generic;

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

        public static void AssertListsAreEqual<T>(IEnumerable<T> first, IEnumerable<T> second)
        {
            bool areEqual = first.Zip(second, (f, s) => EqualityComparer<T>.Default.Equals(f, s)).All((e) => e);
            if (!areEqual)
            {
                string message = String.Format("{0} != {1}",                    
                    string.Join(",", first.Select(s => s.ToString())),
                    string.Join(",", second.Select(s => s.ToString())));
                Assert.IsTrue(areEqual, message);
            }            
        }
    }
}
