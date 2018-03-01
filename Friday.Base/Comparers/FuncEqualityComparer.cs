using System;
using System.Collections.Generic;

namespace Friday.Base.Comparers
{
    public class FuncEqualityComparer<T> : IEqualityComparer<T>
    {
        readonly Func<T, T, bool> comparer;
        readonly Func<T, int> hash;

        public FuncEqualityComparer(Func<T, T, bool> comparer)
            : this(comparer,
                t => 0) // NB Cannot assume anything about how e.g., t.GetHashCode() interacts with the comparer's behavior
        {
        }

        public FuncEqualityComparer(Func<T, T, bool> comparer, Func<T, int> hash)
        {
            this.comparer = comparer;
            this.hash = hash;
        }

        public static IEqualityComparer<T> Create(Func<T, T, bool> comparer)
        {
            return new FuncEqualityComparer<T>(comparer);
        }

        public bool Equals(T x, T y)
        {
            return comparer(x, y);
        }

        public int GetHashCode(T obj)
        {
            return hash(obj);
        }
    }
}