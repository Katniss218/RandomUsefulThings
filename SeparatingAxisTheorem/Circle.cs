using System;
using System.Collections.Generic;
using System.Text;
using Transformations;

namespace Geometry
{
    public struct Circle
    {
        public Vector2 Center { get; }

        public float Radius { get; }

        public float Diameter { get => Radius * 2; }

        public Circle( Vector2 center, float radius )
        {
            this.Center = center;
            this.Radius = radius;
        }

        public Vector2 ClosestPointOnCircle( Vector2 point )
        {
            Vector2 direction = new Vector2( Center, point ).Normalized();

            return Center + (direction * Radius);
        }
    }
}
