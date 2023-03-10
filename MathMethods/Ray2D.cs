using System;
using System.Collections.Generic;
using System.Text;

namespace Geometry
{
    public struct Ray2D
    {
        public Vector2 Origin { get; }
        public Vector2 Direction { get; }

        public Ray2D( Vector2 origin, Vector2 direction )
        {
            this.Origin = origin;
            this.Direction = direction;
        }
    }
}