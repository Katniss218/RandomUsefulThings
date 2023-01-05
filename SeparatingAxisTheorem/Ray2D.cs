using System;
using System.Collections.Generic;
using System.Text;

namespace Geometry
{
    public struct Ray2D
    {
        public Vector2 Point { get; }
        public Vector2 Direction { get; }

        public Ray2D( Vector2 point1, Vector2 point2 )
        {
            this.Point = point1;
            this.Direction = Vector2.PointingAt( point1, point2 );
        }
    }
}