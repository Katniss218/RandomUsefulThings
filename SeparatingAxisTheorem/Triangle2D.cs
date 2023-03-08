using System;
using System.Collections.Generic;
using System.Text;

namespace Geometry
{
    public struct Triangle2D
    {
        public Vector2 P1 { get; }
        public Vector2 P2 { get; }
        public Vector2 P3 { get; }

        /// <summary>
        /// Returns the geometric middle of the triangle (the point where it would balance).
        /// </summary>
        public Vector2 Midpoint { get => (P1 + P2 + P3) / 3.0f; }

        public Triangle2D( Vector2 p1, Vector2 p2, Vector2 p3 )
        {
            this.P1 = p1;
            this.P2 = p2;
            this.P3 = p3;
        }

        /// <summary>
        /// Calculates the lengths of each edge in this triangle.
        /// </summary>
        /// <returns>A tuple with the edges. Each edge is guaranteed to be in the same element of the tuple {P1->P2, P2->P3, P3->P1}.</returns>
        public (float a, float b, float c) GetEdgeLengths()
        {
            return (Vector2.Distance( P1, P2 ), Vector2.Distance( P2, P3 ), Vector2.Distance( P3, P1 ));
        }

        /// <summary>
        /// Calculates the area of a triangle with the specified side lengths
        /// </summary>
        public static float Area( float a, float b, float c )
        {
            float halfPerimeter = (a + b + c) / 2;

            return (float)Math.Sqrt( halfPerimeter * (halfPerimeter - a) * (halfPerimeter - b) * (halfPerimeter - c) );

            // there's also this shoelace formula ` Math.Abs(x1*y2 + x2*y3 + x3*y1 - x1*y3 - x2*y1 - x3*y2) * 0.5f` using vertices (x,y 1 through 3) instead of edges.
        }

        /// <summary>
        /// Calculates the area of this triangle.
        /// </summary>
        public float Area()
        {
            (float a, float b, float c) = GetEdgeLengths();

            return Area( a, b, c );
        }

        /// <summary>
        /// Checks if the point is inside this triangle.
        /// </summary>
        /// <returns>True if the point is inside the triangle, false otherwise.</returns>
        public bool ContainsPoint( Vector2 p )
        {
            // Calculate the area of the triangle.
            float totalArea = Area();

            // Calculate the areas of the three triangles formed by the point and the sides of the triangle
            float area1 = new Triangle2D( p, P1, P2 ).Area();
            float area2 = new Triangle2D( p, P2, P3 ).Area();
            float area3 = new Triangle2D( p, P3, P1 ).Area();

            // If the sum of the three triangle areas is equal to the total area, then the point is inside the triangle
            return (Math.Abs( totalArea - (area1 + area2 + area3) ) < 0.00001f);
        }

        /// <summary>
        /// Checks if the point is inside this triangle.
        /// </summary>
        /// <returns>True if the point is inside the triangle, false otherwise.</returns>
        public bool ContainsPointFaster( Vector2 p )
        {
            // Check if the point is inside the triangle using barycentric coordinates
            float alpha = (((P2.Y - P3.Y) * (p.X - P3.X)) + ((P3.X - P2.X) * (p.Y - P3.Y))) /
                          (((P2.Y - P3.Y) * (P1.X - P3.X)) + ((P3.X - P2.X) * (P1.Y - P3.Y)));

            float beta = (((P3.Y - P1.Y) * (p.X - P3.X)) + ((P1.X - P3.X) * (p.Y - P3.Y))) /
                         (((P2.Y - P3.Y) * (P1.X - P3.X)) + ((P3.X - P2.X) * (P1.Y - P3.Y)));

            float gamma = 1.0f - alpha - beta;

            return alpha > 0.0f && beta > 0.0f && gamma > 0.0f;
        }

        public static float InterpolateTriangle( Vector2 p, Vector2 t1, Vector2 t2, Vector2 t3, float value1, float value2, float value3 )
        {
            // Compute the barycentric coordinates of the point p with respect to the triangle (v1, v2, v3)
            Vector3 barycentric = new Triangle2D( t1, t2, t3 ).BarycentricCoordinates( p );

            // Interpolate the values at the vertices using the barycentric coordinates
            return (value1 * barycentric.X) + (value2 * barycentric.Y) + (value3 * barycentric.Z);
        }

        public Vector3 BarycentricCoordinates( Vector2 point )
        {
            // Barycentric coordinates are basically weights for each vertex, telling you how much that vertex contributes to the current point.
            // they're (0.333.., 0.333.., 0.333..) at the center, and increase to (1, 0, 0) as you approach each vertex.
            float areaABC = new Triangle2D( P1, P2, P3 ).Area();
            float areaPBC = new Triangle2D( point, P2, P3 ).Area();
            float areaPCA = new Triangle2D( P1, point, P3 ).Area();

            float barycentricCoord1 = areaPBC / areaABC;
            float barycentricCoord2 = areaPCA / areaABC;
            float barycentricCoord3 = 1 - barycentricCoord1 - barycentricCoord2;

            return new Vector3( barycentricCoord1, barycentricCoord2, barycentricCoord3 );
        }
    }
}
