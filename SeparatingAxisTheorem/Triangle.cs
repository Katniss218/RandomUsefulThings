using Transformations;
using System;
using System.Collections.Generic;
using System.Text;
using Transformations;

namespace Geometry
{
    public struct Triangle
    {
        [Obsolete("Unconfirmed")]
        // A function that calculates the area of a triangle given the lengths of its sides
        public static float Area( float a, float b, float c )
        {
            if( a <= 0 || b <= 0 || c <= 0 )
            {
                throw new ArgumentException( "Sides must be positive numbers." );
            }

            float s = (a + b + c) / 2;

            return (float)Math.Sqrt( s * (s - a) * (s - b) * (s - c) );
        }

        [Obsolete( "Unconfirmed" )]
        // Returns the area of a triangle given its three vertices
        public float Area( Vector2 p1, Vector2 p2, Vector2 p3 )
        {
            float a = Vector2.Distance( p1, p2 );
            float b = Vector2.Distance( p2, p3 );
            float c = Vector2.Distance( p3, p1 );
            return Area( a, b, c );
        }

        [Obsolete( "Unconfirmed" )]
        // Returns true if the point is inside the triangle, false otherwise
        public bool PointInTriangle( Vector2 p, Vector2 p1, Vector2 p2, Vector2 p3 )
        {
            // Calculate the area of the triangle.
            float totalArea = Area( p1, p2, p3 );

            // Calculate the areas of the three triangles formed by the point and the sides of the triangle
            float area1 = Area( p, p1, p2 );
            float area2 = Area( p, p2, p3 );
            float area3 = Area( p, p3, p1 );

            // If the sum of the three triangle areas is equal to the total area, then the point is inside the triangle
            return (totalArea == area1 + area2 + area3);
        }

        [Obsolete( "Unconfirmed" )]
        public static bool RayIntersectsTriangle( Vector3 rayOrigin, Vector3 rayDirection, Vector3 v1, Vector3 v2, Vector3 v3 )
        {
            // Compute the plane normal and the intersection point of the ray and the plane
            Vector3 planeNormal = Vector3.Cross( v2 - v1, v3 - v1 );
            float t = Vector3.Dot( planeNormal, v1 - rayOrigin ) / Vector3.Dot( planeNormal, rayDirection );
            Vector3 intersectionPoint = rayOrigin + t * rayDirection;

            // Check if the intersection point is inside the triangle
            return PointInTriangle( intersectionPoint, v1, v2, v3 );
        }

        [Obsolete( "Unconfirmed" )]
        public static bool PointInTriangle( Vector3 point, Vector3 v1, Vector3 v2, Vector3 v3 )
        {
            // same as the other method.

            // Compute the barycentric coordinates of the point
            float u = Vector3.Dot( Vector3.Cross( v2 - v1, point - v1 ), Vector3.Cross( v3 - v1, v2 - v1 ) ) / Vector3.Dot( Vector3.Cross( v3 - v1, v2 - v1 ), Vector3.Cross( v3 - v1, v2 - v1 ) );
            float v = Vector3.Dot( Vector3.Cross( v3 - v2, point - v2 ), Vector3.Cross( v1 - v2, v3 - v2 ) ) / Vector3.Dot( Vector3.Cross( v1 - v2, v3 - v2 ), Vector3.Cross( v1 - v2, v3 - v2 ) );
            float w = 1 - u - v;

            // Check if the point is inside the triangle (using a small tolerance value to account for floating point errors)
            const float tolerance = 0.001f;
            return u >= -tolerance && v >= -tolerance && w >= -tolerance;
        }

        [Obsolete( "Unconfirmed" )]
        public static bool IsPointInTriangle( Vector2 p, Vector2 t1, Vector2 t2, Vector2 t3 )
        {
            // Check if the point is inside the triangle using barycentric coordinates
            float alpha = (((t2.Y - t3.Y) * (p.X - t3.X)) + ((t3.X - t2.X) * (p.Y - t3.Y))) /
                           (((t2.Y - t3.Y) * (t1.X - t3.X)) + ((t3.X - t2.X) * (t1.Y - t3.Y)));

            float beta = (((t3.Y - t1.Y) * (p.X - t3.X)) + ((t1.X - t3.X) * (p.Y - t3.Y))) /
                          (((t2.Y - t3.Y) * (t1.X - t3.X)) + ((t3.X - t2.X) * (t1.Y - t3.Y)));

            float gamma = 1.0f - alpha - beta;

            return alpha > 0.0f && beta > 0.0f && gamma > 0.0f;
        }

        [Obsolete( "Unconfirmed - Barycentric" )]
        public static float InterpolateTriangle( Vector2 p, Vector2 t1, Vector2 t2, Vector2 t3, float value1, float value2, float value3 )
        {
            // Compute the barycentric coordinates of the point p with respect to the triangle (v1, v2, v3)
            var barycentric = Barycentric( p, t1, t2, t3 );

            // Interpolate the values at the vertices using the barycentric coordinates
            return (value1 * barycentric.X) + (value2 * barycentric.Y) + (value3 * barycentric.Z);
        }

        [Obsolete( "Unconfirmed" )]
        private static Vector3 Barycentric( Vector2 p, Vector2 t1, Vector2 t2, Vector2 t3 )
        {
            // barycentric coordinates (combination of weights of vertices).
            var s = (t2.Y * t3.X) - (t2.X * t3.Y) + ((t3.Y - t2.Y) * p.X) + ((t2.X - t3.X) * p.Y);
            var t = (t1.X * t3.Y) - (t1.Y * t3.X) + ((t1.Y - t3.Y) * p.X) + ((t3.X - t1.X) * p.Y);

            if( Math.Abs( s ) < float.Epsilon )
            {
                s = 0;
            }

            if( Math.Abs( t ) < float.Epsilon )
            {
                t = 0;
            }

            var barycentric = new Vector3( (float)(1 - (s + t)), (float)s, (float)t );
            return barycentric;
        }
    }
}
