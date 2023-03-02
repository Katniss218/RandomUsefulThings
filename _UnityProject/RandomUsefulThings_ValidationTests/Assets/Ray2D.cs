using System;
using System.Collections.Generic;
using System.Text;

namespace Geometry
{
    [System.Serializable]
    public struct Ray2D
    {
        public Vector2 Origin;
        public Vector2 Direction;

        public Ray2D( Vector2 origin, Vector2 direction )
        {
            this.Origin = origin;
            this.Direction = direction;
        }
    }
}