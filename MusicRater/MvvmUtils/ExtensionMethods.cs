using System.Collections.Generic;
using System.Linq;

namespace MusicRater
{
    public static class ExtensionMethods
    {
        public static IEnumerable<T> OnceRoundStartingAfter<T>(this IEnumerable<T> list, T startAfter) 
        {
            // can't use != if T can be a struct
            // http://stackoverflow.com/questions/390900/cant-operator-be-applied-to-generic-types-in-c
            return list.SkipWhile(t => !EqualityComparer<T>.Default.Equals(t, startAfter)).Skip(1)
                .Concat(list.TakeWhile(t => !EqualityComparer<T>.Default.Equals(t, startAfter)))
                .Concat(new T[] { startAfter });
        }
    }
}
