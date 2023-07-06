using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Geometry
{
    public struct Polygon
    {
        public Vector2[] Vertices { get; }

        public Polygon( params Vector2[] vertices )
        {
            if( vertices.Length < 3 )
            {
                throw new ArgumentException( "A polygon must have at least 3 vertices" );
            }
            this.Vertices = vertices;
        }

        public Polygon( IEnumerable<Vector2> vertices )
        {
            Vector2[] verticesArray = vertices.ToArray();
            if( verticesArray.Length < 3 )
            {
                throw new ArgumentException( "A polygon must have at least 3 vertices" );
            }

            this.Vertices = verticesArray;
        }

        //[Obsolete("Unconfirmed")]
        /*public static bool DoLineSegmentIntersectPolygon( Vector2 p1, Vector2 p2, List<Vector2> vertices )
        {
            // Check if either of the points of the line segment is inside the polygon
            if( IsPointInPolygon( p1, vertices ) || IsPointInPolygon( p2, vertices ) )
            {
                return true;
            }

            // Check if the line segment intersects any of the sides of the polygon
            for( int i = 0; i < vertices.Count; i++ )
            {
                Vector2 v1 = vertices[i];
                Vector2 v2 = vertices[(i + 1) % vertices.Count];
                if( LineSegment2D.DoLineSegmentsIntersect( p1, p2, v1, v2 ) )
                {
                    return true;
                }
            }

            return false;
        }*/
    }
}
