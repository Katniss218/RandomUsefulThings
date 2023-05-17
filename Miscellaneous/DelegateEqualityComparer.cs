using System;
using System.Collections.Generic;
using System.Text;

namespace RandomUsefulThings.Misc
{
    public class DelegateEqualityComparer<T> : IEqualityComparer<T>
    {
        Func<T, T, bool> comparer;

        public DelegateEqualityComparer( Func<T, T, bool> comparer )
        {
            this.comparer = comparer;
        }

        public bool Equals( T a, T b )
        {
            return comparer( a, b );
        }

        public int GetHashCode( T a )
        {
            return a.GetHashCode();
        }
    }

}
