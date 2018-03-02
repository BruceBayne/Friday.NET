using System;
using System.Collections.Generic;

namespace Friday.Base.Comparers
{
    public class FuncComparer<T> : IComparer<T>
    {
        private Func<T, T, int> comparer;

        public FuncComparer(Func<T, T, int> comparer)
        {
            this.comparer = comparer;
        }

        public static IComparer<T> Create(Func<T, T, int> comparer)
        {
            return new FuncComparer<T>(comparer);
        }

        public int Compare(T x, T y)
        {
            return comparer(x, y);
        }
    }
}