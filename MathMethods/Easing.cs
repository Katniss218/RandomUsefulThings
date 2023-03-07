using System;
using System.Collections.Generic;
using System.Text;

namespace MathMethods
{
    public static class Easing
    {
        /// <summary>
        /// 3rd order smoothstep. Has 1st derivative equal to 0 at `t = edge0` and `t = edge1`.
        /// </summary>
        public static float Smoothstep( float edge0, float edge1, float t )
        {
            // Hermite interpolation?
            float tc = (float)Math.Clamp( (t - edge0) / (edge1 - edge0), 0.0, 1.0 );

            // v = -2t^3 + 3t^2
            return tc * tc * ((tc * -2.0f) + 3.0f);
        }

        public static float InverseSmoothstep( float t )
        {
            return 0.5f - (float)Math.Sin( Math.Asin( 1.0 - 2.0 * t ) / 3.0 ); // edge0 = 0, edge1 = 1.
        }

        /// <summary>
        /// 5th order smoothstep. Has 1st and 2nd derivatives equal to 0 at `t = edge0` and `t = edge1`.
        /// </summary>
        public static float SmootherStep( float edge0, float edge1, float t )
        {
            // Proposed by Ken Perlin.
            float tc = (float)Math.Clamp( (t - edge0) / (edge1 - edge0), 0.0, 1.0 );

            // v = 6t^5 - 15t^4 + 10t^3
            return tc * tc * tc * (tc * ((tc * 6.0f) - 15.0f) + 10.0f);
        }

        /// <summary>
        /// 7th order smoothstep. Has 1st, 2nd, and 3rd derivatives equal to 0 at `t = edge0` and `t = edge1`.
        /// </summary>
        public static float Smoothstep7( float edge0, float edge1, float t )
        {
            float tc = (float)Math.Clamp( (t - edge0) / (edge1 - edge0), 0.0, 1.0 );

            // -20t^7 + 70t^6 - 84t^5 + 35t^4
            return tc * tc * tc * tc * (tc * (tc * ((tc * -20.0f) + 70.0f) - 84.0f) + 35.0f);
        }

    }
}
