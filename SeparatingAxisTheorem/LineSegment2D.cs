using MathMethods;
using System;
using System.Collections.Generic;
using System.Text;

namespace Geometry
{
    public class LineSegment2D
    {
        public Vector2 P1 { get; }
        public Vector2 P2 { get; }

        public LineSegment2D( Vector2 p1, Vector2 p2 )
        {
            this.P1 = p1;
            this.P2 = p2;
        }

        [Obsolete( "Unconfirmed" )]
        public static bool DoLineSegmentsIntersect( Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4 )
        {
            // Calculate the direction vectors of the line segments
            Vector2 v1 = new Vector2( p2.X - p1.X, p2.Y - p1.Y );
            Vector2 v2 = new Vector2( p4.X - p3.X, p4.Y - p3.Y );

            // Check if the line segments are parallel
            if( Vector2.Cross( v1, v2 ) == 0 )
            {
                // The line segments are parallel, but they may still intersect if they are collinear
                if( Vector2.Cross( new Vector2( p3.X - p1.X, p3.Y - p1.Y ), v1 ) == 0 )
                {
                    // The line segments are collinear, check if they overlap
                    if( Math.Max( p1.X, p2.X ) >= Math.Min( p3.X, p4.X ) &&
                        Math.Min( p1.X, p2.X ) <= Math.Max( p3.X, p4.X ) &&
                        Math.Max( p1.Y, p2.Y ) >= Math.Min( p3.Y, p4.Y ) &&
                        Math.Min( p1.Y, p2.Y ) <= Math.Max( p3.Y, p4.Y ) )
                    {
                        return true;
                    }
                }
                return false;
            }

            // Calculate the point of intersection between the lines
            double t = Vector2.Cross( new Vector2( p3.X - p1.X, p3.Y - p1.Y ), v2 ) / Vector2.Cross( v1, v2 );
            double u = Vector2.Cross( new Vector2( p3.X - p1.X, p3.Y - p1.Y ), v1 ) / Vector2.Cross( v1, v2 );

            // Check if the point of intersection lies on both line segments
            return t > 0 && t < 1 && u > 0 && u < 1;
        }

        /// <summary>
        /// Returns a point that is the closest point to a given point, and lies on this line segment.
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        [Obsolete( "Unconfirmed" )]
        public Vector2 ClosestPoint( Vector2 point )
        {
            Vector2 v1 = point - P1;
            Vector2 v2 = P2 - P1;

            float t = Vector2.Dot( v1, v2 ) / Vector2.Dot( v2, v2 );

            if( t < 0 )
            {
                return P1;
            }
            if( t > 1 )
            {
                return P2;
            }
            return new Vector2( MathMethods.MathMethods.LerpUnclamped( P1.X, P2.X, t ), MathMethods.MathMethods.LerpUnclamped( P1.Y, P2.Y, t ) );
        }
    }
}