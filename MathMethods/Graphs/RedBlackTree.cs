using System;
using System.Collections.Generic;
using System.Text;

namespace MathMethods.Graphs
{
    public static class MyLinq
    {
        [Obsolete( "Untested" )]
        public static IEnumerable<T> DistinctBST<T>( IEnumerable<T> source ) where T : IComparable<T>
        {
            var tree = new RedBlackTree<T>();
            foreach( var item in source )
            {
                if( tree.Contains( item ) )
                {
                    continue;
                }
                tree.Insert( item );
                yield return item;
            }
        }

        // we can also use a HashSet to keep track of unique elements.
        // we can also have a DistinctBy that keeps the specified part in a dictionary<TBy,TEnumeratedFullElement>
        public static IEnumerable<T> Distinct<T>( this IEnumerable<T> source )
        {
            var seen = new HashSet<T>();
            foreach( var item in source )
            {
                if( seen.Add( item ) )
                {
                    yield return item;
                }
            }
        }
        [Obsolete( "Untested" )]
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>( this IEnumerable<TSource> source, Func<TSource, TKey> keySelector )
        {
            var dictionary = new Dictionary<TKey, TSource>();
            foreach( var element in source )
            {
                TKey key = keySelector( element );
                if( !dictionary.ContainsKey( key ) )
                {
                    dictionary.Add( key, element );
                    yield return element;
                }
            }
        }
    }

    [Obsolete( "Unconfirmed" )]
    public class RedBlackTree<T> where T : IComparable<T>
    {
        private enum Color { Red, Black }

        private class Node
        {
            public T Value { get; set; }
            public Node Left { get; set; }
            public Node Right { get; set; }
            public Color Color { get; set; }
        }

        private Node _root;

        public void Insert( T value )
        {
            _root = Insert( _root, value );
            _root.Color = Color.Black;
        }

        private Node Insert( Node node, T value )
        {
            if( node == null )
            {
                return new Node { Value = value, Color = Color.Red };
            }

            int cmp = value.CompareTo( node.Value );
            if( cmp < 0 )
            {
                node.Left = Insert( node.Left, value );
            }
            else if( cmp > 0 )
            {
                node.Right = Insert( node.Right, value );
            }
            else
            {
                // Value already exists in tree
                return node;
            }

            // Maintain red-black tree properties
            if( IsRed( node.Right ) && !IsRed( node.Left ) )
            {
                node = RotateLeft( node );
            }
#warning Possible null hazard (left.left)?
            if( IsRed( node.Left ) && IsRed( node.Left.Left ) )
            {
                node = RotateRight( node );
            }
            if( IsRed( node.Left ) && IsRed( node.Right ) )
            {
                FlipColors( node );
            }

            return node;
        }

        private bool IsRed( Node node )
        {
            return node != null && node.Color == Color.Red;
        }

        private Node RotateLeft( Node node )
        {
            var x = node.Right;
            node.Right = x.Left;
            x.Left = node;
            x.Color = node.Color;
            node.Color = Color.Red;
            return x;
        }

        private Node RotateRight( Node node )
        {
            var x = node.Left;
            node.Left = x.Right;
            x.Right = node;
            x.Color = node.Color;
            node.Color = Color.Red;
            return x;
        }

        private void FlipColors( Node node )
        {
            node.Color = Color.Red;
            node.Left.Color = Color.Black;
            node.Right.Color = Color.Black;
        }

        public bool Contains( T value )
        {
            var node = _root;
            while( node != null )
            {
                int cmp = value.CompareTo( node.Value );
                if( cmp < 0 )
                {
                    node = node.Left;
                }
                else if( cmp > 0 )
                {
                    node = node.Right;
                }
                else
                {
                    return true;
                }
            }
            return false;
        }
    }

}
