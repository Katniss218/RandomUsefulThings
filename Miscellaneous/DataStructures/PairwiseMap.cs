using System;
using System.Collections.Generic;
using System.Text;

namespace RandomUsefulThings.Misc.DataStructures
{
    public class PairwiseMap<TKey, TValue>
    {
        public struct Pair
        {
            // a pair of objects that is equal regardless of the order.


            public TKey left, right;

            public Pair( TKey left, TKey right )
            {
                this.left = left;
                this.right = right;
            }


            public override int GetHashCode()
            {
                return left.GetHashCode() ^ right.GetHashCode(); // Bitwise XOR is symmetric.
            }


            public bool Equals( Pair other )
            {
                return (left.Equals( other.left ) && right.Equals( other.right ))
                    || (left.Equals( other.right ) && right.Equals( other.left ));
            }


            public override bool Equals( object obj )
            {
                if( obj is Pair other )
                    return Equals( other );


                return false;
            }
        }


        private Dictionary<Pair, TValue> _relations;

        public PairwiseMap()
        {
            _relations = new Dictionary<Pair, TValue>();
        }


        public void Add( TKey v1, TKey v2, TValue value )
        {
            // Add guard
            _relations.Add( new Pair( v1, v2 ), value );
        }


        public bool TryGetValue( TKey v1, TKey v2, out TValue value )
        {
            return _relations.TryGetValue( new Pair( v1, v2 ), out value );
        }
    }
}