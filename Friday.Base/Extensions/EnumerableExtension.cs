using System.Collections.Generic;
using System.Linq;

namespace Friday.Base.Extensions
{
    public static class EnumerableExtension
    {
        public static T TakeRandom<T>(this IEnumerable<T> source)
        {

            return source.TakeRandom(1).FirstOrDefault();
        }



        public static IEnumerable<T> SymmetricExcept<T>(this IEnumerable<T> seq1,
            IEnumerable<T> seq2, IEqualityComparer<T> equalityComparer)
        {
            var hashSet = new HashSet<T>(seq1, equalityComparer);
            hashSet.SymmetricExceptWith(seq2);
            return hashSet;
        }


        public static IEnumerable<T> SymmetricExcept<T>(this IEnumerable<T> seq1,
            IEnumerable<T> seq2)
        {

            return SymmetricExcept(seq1, seq2, equalityComparer: null);
        }



        public static IList<T> Replace<T>(this IList<T> list, T search, T replace) where T : class
        {
            T tmp = list.First(t => t == search);
            var indexA = list.IndexOf(tmp);
            list[indexA] = replace;

            return list;


        }




        public static IList<T> Swap<T>(this IList<T> list, T a, T b) where T : class
        {
            T tmp = list.First(t => t == a);
            T z = list.First(t => t == b);
            var indexA = list.IndexOf(tmp);
            var indexB = list.IndexOf(z);
            return Swap(list, indexA, indexB);


        }

        public static IList<T> Swap<T>(this IList<T> list, int indexA, int indexB)
        {
            T tmp = list[indexA];
            list[indexA] = list[indexB];
            list[indexB] = tmp;
            return list;
        }

        public static IEnumerable<T> TakeRandom<T>(this IEnumerable<T> source, int count)
        {
            return source.Shuffle().Take(count);
        }




        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source)
        {
            // return source.OrderBy(x => Guid.NewGuid());
            return source.OrderBy(x => FridayRandom.GetNext());
        }
    }
}
