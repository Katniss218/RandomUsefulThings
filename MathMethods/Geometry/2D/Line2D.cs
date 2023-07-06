using System;
using System.Collections.Generic;
using System.Text;

namespace Geometry
{
    public struct Line2D
    {
        /// <summary>
        /// The point that defines the position of the line in 2D space.
        /// </summary>
        public Vector2 Point { get; }

        /// <summary>
        /// The direction of the line. Note that this may not necessarily be normalized.
        /// </summary>
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

        public Vector2 LerpAlong( float t )
        {
            // If the line represents an edge, and the Direction is not normalized,
            // then each 1.0 step in t will increase in multiples of direction.
            // I know it's basic, but it's an important property, useful e.g. for clamping line intersections to only part of the line between 2 points.
            return Point + Direction * t;
        }

        public static Line2D FromPointDirection( Vector2 point, Vector2 direction )
        {
            return new Line2D( point, direction );
        }

        public static Vector2 ClosestPointOnLine( Vector2 p, Vector2 p1, Vector2 p2 ) // works.
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

        public bool OnLine( Vector2 point ) // works.
        {
            Vector2 pointToVector = Vector2.PointingAt( Point, point );

            return Vector2.Cross( pointToVector, Direction ) == 0;
        }

        public static Vector2? LineLineIntersection( Line2D line1, Line2D line2 ) // this works.
        {
            Vector2 point1 = line1.Point;
            Vector2 point2 = line2.Point;

            Vector2 direction1 = line1.Direction;
            Vector2 direction2 = line2.Direction;

            float determinant = direction1.X * direction2.Y - direction1.Y * direction2.X;

            if( determinant == 0 )
            {
                // Lines are parallel, no intersection
                return null;
            }

            float t1 = (direction2.X * (point1.Y - point2.Y) - direction2.Y * (point1.X - point2.X)) / determinant;

            Vector2 intersection = point1 + t1 * direction1;

            return intersection;
        }
    }
}
