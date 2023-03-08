using MathMethods;
using System;
using System.Collections.Generic;
using System.Text;

namespace Geometry
{
    public struct LineSegment2D
    {
        public Vector2 Point1 { get; }
        public Vector2 Point2 { get; }

        public Vector2 Midpoint { get => Vector2.Midpoint( Point1, Point2 ); }

        public float Slope { get => throw new NotImplementedException(); }

        public LineSegment2D( Vector2 p1, Vector2 p2 )
        {
            this.Point1 = p1;
            this.Point2 = p2;
        }

        [Obsolete( "Unconfirmed" )]
        public static bool AreLineSegmentsIntersecting( Vector2 l1_p1, Vector2 l1_p2, Vector2 l2_p1, Vector2 l2_p2, bool shouldIncludeEndPoints )
        {
            //To avoid floating point precision issues we can add a small value
            float epsilon = 0.00001f;

            bool isIntersecting = false;

            float denominator = (l2_p2.Y - l2_p1.Y) * (l1_p2.X - l1_p1.X) - (l2_p2.X - l2_p1.X) * (l1_p2.Y - l1_p1.Y);

            //Make sure the denominator is > 0, if not the lines are parallel
            if( denominator != 0f )
            {
                float u_a = ((l2_p2.X - l2_p1.X) * (l1_p1.Y - l2_p1.Y) - (l2_p2.Y - l2_p1.Y) * (l1_p1.X - l2_p1.X)) / denominator;
                float u_b = ((l1_p2.X - l1_p1.X) * (l1_p1.Y - l2_p1.Y) - (l1_p2.Y - l1_p1.Y) * (l1_p1.X - l2_p1.X)) / denominator;

                //Are the line segments intersecting if the end points are the same
                if( shouldIncludeEndPoints )
                {
                    //Is intersecting if u_a and u_b are between 0 and 1 or exactly 0 or 1
                    if( u_a >= 0f + epsilon && u_a <= 1f - epsilon && u_b >= 0f + epsilon && u_b <= 1f - epsilon )
                    {
                        isIntersecting = true;
                    }
                }
                else
                {
                    //Is intersecting if u_a and u_b are between 0 and 1
                    if( u_a > 0f + epsilon && u_a < 1f - epsilon && u_b > 0f + epsilon && u_b < 1f - epsilon )
                    {
                        isIntersecting = true;
                    }
                }
            }

            return isIntersecting;
        }

        [Obsolete( "Unconfirmed" )]
        public static bool DoLineSegmentsIntersect( LineSegment2D l1, LineSegment2D l2 )
        {
            Vector2 p1 = l1.Point1;
            Vector2 p2 = l1.Point2;
            Vector2 p3 = l2.Point1;
            Vector2 p4 = l2.Point2;

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
            Vector2 v1 = point - Point1;
            Vector2 v2 = Point2 - Point1;

            float t = Vector2.Dot( v1, v2 ) / Vector2.Dot( v2, v2 );

            if( t < 0 )
            {
                return Point1;
            }
            if( t > 1 )
            {
                return Point2;
            }
            return new Vector2( Interpolation.LerpUnclamped( Point1.X, Point2.X, t ), Interpolation.LerpUnclamped( Point1.Y, Point2.Y, t ) );
        }
    }
}