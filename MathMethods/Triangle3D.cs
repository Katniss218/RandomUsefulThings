using System;
using System.Collections.Generic;
using System.Text;

namespace Geometry
{
    public struct Triangle3D
    {
        public Vector3 P1 { get; }
        public Vector3 P2 { get; }
        public Vector3 P3 { get; }

        /// <summary>
        /// Returns the normal vector for this triangle.
        /// </summary>
        // The normal can be calculated by taking the vectors pointing from one vertex, to the 2 other vertices, and computing their cross product,
        //  which gives the vector perpendicular to both.
        public Vector3 Normal { get => Vector3.Cross( Vector3.PointingAt( P1, P2 ), Vector3.PointingAt( P1, P3 ) ); }

        /// <summary>
        /// Returns the geometric middle of the triangle (the point where it would balance).
        /// </summary>
        public Vector3 Midpoint { get => (P1 + P2 + P3) / 3.0f; }

        public Triangle3D( Vector3 p1, Vector3 p2, Vector3 p3 )
        {
            this.P1 = p1;
            this.P2 = p2;
            this.P3 = p3;
        }

        [Obsolete( "incomplete" )]
        public bool RayIntersectsTriangle( Vector3 rayOrigin, Vector3 rayDirection )
        {
            Vector3? planeIntersection = Line3D.LinePlaneIntersection( rayOrigin, rayDirection, this.P1, this.Normal );
            if( planeIntersection == null )
            {
                return false;
            }

            throw new NotImplementedException();
            //return PointInTriangle( planeIntersection.Value );
        }

        public float Area()
        {
            // this is derived from the area for a parallelogram, which is the same, but without the division by 2.
            // area of a triangle in 3D space can be calculated from the formula cross(edge1, edge2) / 2

            Vector3 edge1 = P2 - P1;
            Vector3 edge2 = P3 - P1;
            Vector3 crossProduct = Vector3.Cross( edge1, edge2 );
            float area = crossProduct.Length / 2.0f;
            return area;
        }
    }
}
