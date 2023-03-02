using Geometry;
using System;
using System.Collections.Generic;
using System.Text;

namespace Miscellaneous
{
    public static class RandomEx
    {
        [Obsolete("Unconfirmed")]
        public static Vector2 RandomPointOnCircle( this System.Random rand, Vector2 center, float radius )
        {
            double angle = rand.NextDouble() * 2 * Math.PI;
            double x = center.X + radius * Math.Cos( angle );
            double y = center.Y + radius * Math.Sin( angle );

            return new Vector2( (float)x, (float)y );
        }
    }
}
