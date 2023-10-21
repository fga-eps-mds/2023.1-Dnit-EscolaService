using System.Collections.Generic;
using System.Linq;

namespace test.Stubs
{
    public static class UtilsExtensions
    {
        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> collection)
        {
            return collection.Select(item => (item, Random.Shared.Next())).OrderBy(i => i.Item2).Select(i => i.item);
        }

        public static List<T> TakeRandom<T>(this IEnumerable<T> collection, bool empty = false)
        {
            return collection.Shuffle().Take((Random.Shared.Next() % collection.Count()) + (empty ? 0 : 1)).ToList();
        }

        public static string Truncate(this string text, int limit) {
            return text[..Math.Min(limit, text.Length)];
        }
    }
}