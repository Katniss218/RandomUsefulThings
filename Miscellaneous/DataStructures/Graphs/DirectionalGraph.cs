using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RandomUsefulThings.Misc.Graphs
{
    public class DirectedHashGraph<TKey, TValue> // instead of using hashsets, we could implement a resizable array ourselves, and manually resize the vertices, and the adjacency list. This would grant better cache locality.
    {
        private class Vertex
        {
            public HashSet<TKey> adjacent; // vertex points towards those.
            public TValue value;


            public Vertex( TValue value )
            {
                this.adjacent = new HashSet<TKey>();
                this.value = value;
            }
        }

        private Dictionary<TKey, Vertex> _data; // TKey can be a string ID for example.

        public DirectedHashGraph()
        {
            _data = new Dictionary<TKey, Vertex>();
        }


        /// Adds a disconnected vertex.
        public void AddVertex( TKey key, TValue value )
        {
            _data.Add( key, new Vertex( value ) );
        }


        public void AddVertex( TKey key, TValue value, IEnumerable<TKey> adjacent )
        {
            Vertex vertex = new Vertex( value );
            foreach( var adj in adjacent )
            {
                if( key.Equals( adj ) )
                    throw new ArgumentException( "A vertex can't be adjacent to itself" );


                vertex.adjacent.Add( adj );
            }
            _data.Add( key, vertex );
        }


        public void AddEdge( TKey from, TKey to )
        {
            if( from.Equals( to ) )
                throw new ArgumentException( "A vertex can't be adjacent to itself" );


            // guard when `to` is not in graph.


            _data[from].adjacent.Add( to ); // HashSet.Add on adjacent won't add if it's already there.
        }

        [Obsolete( "unconfirmed" )]
        public List<TKey> TopologicalSort()
        {
            // Calculate in-degrees for all nodes
            Dictionary<TKey, int> inDegrees = _data.Keys.ToDictionary( key => key, _ => 0 );
            foreach( var vertex in _data.Values )
            {
                foreach( var node in vertex.adjacent )
                {
                    inDegrees[node]++;
                }
            }

            // Enqueue nodes with in-degree 0
            Queue<TKey> zeroDegreeNodes = new Queue<TKey>( inDegrees.Where( pair => pair.Value == 0 ).Select( pair => pair.Key ) );

            List<TKey> result = new List<TKey>();

            // Kahn's algorithm
            while( zeroDegreeNodes.Count > 0 )
            {
                TKey node = zeroDegreeNodes.Dequeue();
                result.Add( node );

                foreach( var neighbor in _data[node].adjacent )
                {
                    inDegrees[neighbor]--;
                    if( inDegrees[neighbor] == 0 )
                    {
                        zeroDegreeNodes.Enqueue( neighbor );
                    }
                }
            }

            if( result.Count != _data.Count )
            {
                // Graph has a cycle, topological sort is not possible
                throw new InvalidOperationException( "The graph has a cycle!" );
            }

            return result;
        }
    }


}
