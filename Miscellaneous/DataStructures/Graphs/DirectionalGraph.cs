using System;
using System.Collections.Generic;
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
    }


}
