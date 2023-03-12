using System;
using System.Collections.Generic;
using System.Text;

namespace MathMethods.Graphs
{
    public class DirectionalGraph<TVertex, TEdge>
    {
        private class Vertex
        {
            public TVertex Data;

            // maps each vertex to what it is connected to. Unidirectional, needs 2 entries to connect.
            public List<(int, TEdge)> ConnectsTo;

            public Vertex( TVertex data )
            {
                this.Data = data;
            }
        }

        List<Vertex> Vertices;
        // maps indices in the list to vertices and 2 of those form an edge.
        // triangular-array relation map of type TEdge could be used for bidirectional graphs.

        public DirectionalGraph()
        {

        }

        public int AddVertex( TVertex data )
        {
            Vertices.Add( new Vertex( data ) );
            return Vertices.Count - 1; // return index of the vertex.
        }

        public void Connect( int from, int to, TEdge data )
        {
            Vertices[from].ConnectsTo.Add( (to, data) );
        }

        public void ConnectBiDirectional( int v1, int v2, TEdge data )
        {
            Vertices[v1].ConnectsTo.Add( (v2, data) );
            Vertices[v2].ConnectsTo.Add( (v1, data) );
        }
    }

}
