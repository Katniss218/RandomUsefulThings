using System;
using System.Collections.Generic;
using System.Text;

namespace Geometry
{
    public struct Line2D
    {
        public Vector2 Point { get; }
        public Vector2 Direction { get; }

        /// <summary>
        /// Returns the slope (y / x) of the line.
        /// </summary>
        public float Slope { get => Direction.Y / Direction.X; }

        public Line2D( Vector2 point1, Vector2 point2 )
        {
            this.Point = point1;
            this.Direction = Vector2.PointingAt( point1, point2 );
        }

        public static Line2D FromPointDirection( Vector2 point, Vector2 direction )
        {
            return new Line2D( point, point + direction );
        }

        public static Vector2 ClosestPointOnLine( Vector2 p, Vector2 p1, Vector2 p2 )
        {
            Vector2 line = p2 - p1;
            Vector2 lineToPoint = p - p1;
            float t = Vector2.Dot( lineToPoint, line ) / line.LengthSquared;
            if( t < 0 )
                return p1;
            else if( t > 1 )
                return p2;
            else
                return p1 + t * line;
        }

        [Obsolete( "Unconfirmed" )]
        public bool OnLine( Vector2 point )
        {
            Vector2 pointToVector = Vector2.PointingAt( Point, point );

            return Vector2.Cross( pointToVector, Direction ) == 0;
        }

        [Obsolete( "Unconfirmed" )]
        public static bool LineLineIntersection( Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4, out Vector2 intersection ) // an implementation uses specific points, might be required.
        {
            intersection = Vector2.Zero;

            Vector2 p13 = p1 - p3;
            Vector2 p43 = p4 - p3;
            Vector2 p21 = p2 - p1;

            float d1343 = p13.X * p43.Y - p13.Y * p43.X;
            float d4321 = p43.X * p21.Y - p43.Y * p21.X;
            float d1321 = p13.X * p21.Y - p13.Y * p21.X;
            float d4343 = p43.X * p43.Y - p43.Y * p43.X;
            float d2121 = p21.X * p21.Y - p21.Y * p21.X;

            float denom = d2121 * d4343 - d4321 * d4321;
            if( Math.Abs( denom ) < float.Epsilon )
                return false;

            float numer = d1343 * d4321 - d1321 * d4343;

            float mua = numer / denom;
            float mub = (d1343 + d4321 * (mua)) / d4343; // uhh, not used later????

            intersection = new Vector2(
                p1.X + mua * p21.X,
                p1.Y + mua * p21.Y
                );

            return true;
        }
    }
}
