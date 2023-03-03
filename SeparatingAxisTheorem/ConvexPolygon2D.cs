using System;
using System.Collections.Generic;

namespace Geometry
{
    public class ConvexPolygon2D
    {
        // A list to store the vertices of the polygon
        private List<Vector2> _vertices;

        // Constructor to initialize the list of vertices
        public ConvexPolygon2D()
        {
            _vertices = new List<Vector2>();
        }

        // Method to add a vertex to the polygon
        public void AddVertex( Vector2 vertex )
        {
            _vertices.Add( vertex );
        }

        // Method to get the vertices of the polygon
        public List<Vector2> GetVertices()
        {
            return _vertices;
        }

        [Obsolete( "Unfconfirmed" )]
        public float GetArea()
        {
            // To calculate the area of a convex polygon, you can use the Shoelace theorem, also known as the surveyor's formula.

            // This theorem states that the area of a polygon can be calculated by taking the sum of the product of the x and y coordinates of each vertex,
            // and then subtracting the sum of the product of the y and x coordinates of each vertex.

            // Get the list of vertices for the polygon
            List<Vector2> vertices = GetVertices();

            // Initialize variables to store the sum of the x and y coordinates of each vertex
            float sum1 = 0;
            float sum2 = 0;

            // Loop through the vertices and calculate the sum of the x and y coordinates
            for( int i = 0; i < vertices.Count; i++ )
            {
                sum1 += vertices[i].X * vertices[(i + 1) % vertices.Count].Y;
                sum2 += vertices[i].Y * vertices[(i + 1) % vertices.Count].X;
            }

            // Calculate the area of the polygon using the Shoelace theorem
            float area = 0.5f * Math.Abs( sum1 - sum2 );

            // Return the calculated area
            return area;
        }

        [Obsolete( "Unconfirmed" )]
        public bool IsColliding( ConvexPolygon2D polygon1, ConvexPolygon2D polygon2 )
        {
            // The separating axis theorem, also known as the SAT theorem, is a mathematical concept that is commonly used in computer graphics and computational geometry. It is a method for determining whether or not two convex polygons are colliding or intersecting.
            // The theorem states that if there is a line or axis that can be drawn to separate the two convex polygons, then the polygons are not colliding. In other words, if it is possible to draw a line or axis that passes through the polygons in such a way that the polygons are on opposite sides of the line, then the polygons are not colliding.
            // The theorem is useful for quickly and efficiently determining whether or not two convex polygons are colliding in a computer program, without having to perform a more complex and time - consuming calculation.This can be particularly useful in video games and other applications where real - time collision detection is important.
            // The theorem can be extended to more than two convex polygons, and can also be used to test for collisions between other types of convex shapes, such as circles and ellipses.

            // Get the list of vertices for the first polygon
            List<Vector2> vertices1 = polygon1.GetVertices();

            // Get the list of vertices for the second polygon
            List<Vector2> vertices2 = polygon2.GetVertices();

            // Loop through the vertices of the first polygon
            for( int i = 0; i < vertices1.Count; i++ )
            {
                // Calculate the edge vector by subtracting the current vertex from the next vertex
                Vector2 edge = vertices1[(i + 1) % vertices1.Count] - vertices1[i];

                // Calculate the normal vector for the edge by rotating it 90 degrees counterclockwise
                Vector2 normal = new Vector2( -edge.Y, edge.X );

                // Normalize the normal vector
                normal = normal.Normalized();

                // Calculate the minimum and maximum dot products for the first polygon
                float min1 = float.MaxValue;
                float max1 = float.MinValue;
                foreach( Vector2 vertex in vertices1 )
                {
                    float dot = Vector2.Dot( vertex, normal );
                    min1 = Math.Min( min1, dot );
                    max1 = Math.Max( max1, dot );
                }

                // Calculate the minimum and maximum dot products for the second polygon
                float min2 = float.MaxValue;
                float max2 = float.MinValue;
                foreach( Vector2 vertex in vertices2 )
                {
                    float dot = Vector2.Dot( vertex, normal );
                    min2 = Math.Min( min2, dot );
                    max2 = Math.Max( max2, dot );
                }

                // If the minimum and maximum dot products for the polygons do not overlap,
                // then there is a separating axis and the polygons are not colliding
                if( min1 > max2 || min2 > max1 )
                {
                    return false;
                }
            }

            // If no separating axis was found, then the polygons are colliding
            return true;
        }

        [Obsolete( "Unconfirmed" )]
        public Vector2 GetMinimumTranslationVector( ConvexPolygon2D polygon1, ConvexPolygon2D polygon2 )
        {
            // One approach to calculating the collision response is to use the separating axis theorem to determine the minimum translation vector (MTV), which is the smallest vector needed to move one of the polygons out of the other polygon.

            // Get the list of vertices for the first polygon
            List<Vector2> vertices1 = polygon1.GetVertices();

            // Get the list of vertices for the second polygon
            List<Vector2> vertices2 = polygon2.GetVertices();

            // Initialize the minimum translation vector
            Vector2 mtv = new Vector2( float.MaxValue, float.MaxValue );

            // Loop through the vertices of the first polygon
            for( int i = 0; i < vertices1.Count; i++ )
            {
                // Calculate the edge vector by subtracting the current vertex from the next vertex
                Vector2 edge = vertices1[(i + 1) % vertices1.Count] - vertices1[i];

                // Calculate the normal vector for the edge by rotating it 90 degrees counterclockwise
                Vector2 normal = new Vector2( -edge.Y, edge.X );

                // Normalize the normal vector
                normal = normal.Normalized();

                // Calculate the minimum and maximum dot products for the first polygon
                float min1 = float.MaxValue;
                float max1 = float.MinValue;
                foreach( Vector2 vertex in vertices1 )
                {
                    float dot = Vector2.Dot( vertex, normal );
                    min1 = Math.Min( min1, dot );
                    max1 = Math.Max( max1, dot );
                }

                // Calculate the minimum and maximum dot products for the second polygon
                float min2 = float.MaxValue;
                float max2 = float.MinValue;
                foreach( Vector2 vertex in vertices2 )
                {
                    float dot = Vector2.Dot( vertex, normal );
                    min2 = Math.Min( min2, dot );
                    max2 = Math.Max( max2, dot );
                }

                // Calculate the overlap between the polygons along the current axis
                float overlap = Math.Min( max1, max2 ) - Math.Max( min1, min2 );

                // If there is no overlap, then there is a separating axis and the polygons are not colliding
                if( overlap <= 0 )
                {
                    return new Vector2( 0, 0 );
                }

                // If the overlap is smaller than the current minimum translation vector,
                // then set the MTV to the overlap along the current axis
                if( overlap < mtv.Length )
                {
                    mtv = normal * overlap;
                }
            }

            // Return the minimum translation vector
            return mtv;
        }
    }
}
