using System;
using System.Collections.Generic;
using System.Text;

namespace Geometry
{
    public struct Line2D
    {
        public Vector2 Point { get; }
        public Vector2 Direction { get; }

        public float Slope { get => y / x ?}

        public Line2D( Vector2 point1, Vector2 point2 )
        {
            this.Point = point1;
            this.Direction = Vector2.PointingAt( point1, point2 );
        }

        public static Line2D FromPointDirection( Vector2 point, Vector2 direction )
        {
            return new Line2D( point, point + direction );
        }

        [Obsolete( "Unconfirmed" )]
        public bool OnLine( Vector2 point )
        {
            Vector2 pointToVector = Vector2.PointingAt( Point, point );

            return Vector2.Cross( pointToVector, Direction ) == 0;
        }
    }
}
