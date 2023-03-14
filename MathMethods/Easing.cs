using System;
using System.Collections.Generic;
using System.Text;

namespace RandomUsefulThings.Math
{
    public static class Easing
    {
        /// <summary>
        /// Smoothly blends the signal into the desired minimum value.
        /// </summary>
        /// <param name="x">The value to sample.</param>
        /// <param name="threshold">Above this, signal stays unchanged.</param>
        /// <param name="min">At x = 0, returned value = min.</param>
        public static float AlmostIdentity( float x, float threshold, float min )
        {
            // https://iquilezles.org/articles/functions/
            if( x > threshold ) 
                return x;
            float a = 2.0f * min - threshold;
            float b = 2.0f * threshold - 3.0f * min;
            float t = x / threshold;
            return (a * t + b) * t * t + min;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="k">For k = 1: identity curve (straight lerp between 0 and 1), < 1: gain shape, > 1: S-shape.</param>
        /// <returns></returns>
        public static float Gain( float x, float k )
        {
            // https://iquilezles.org/articles/functions/
            float a = 0.5f * (float)System.Math.Pow( 2.0 * ((x < 0.5) ? x : 1.0 - x), k );
            return (x < 0.5f) ? a : 1.0f - a;
        }

        /// <summary>
        /// Maps the interval [0..1] onto an interval [0..1]. The value at x = 0.5 is always 1, and x = 0, x = 1 are always 0.
        /// </summary>
        /// <param name="x">The input value to sample.</param>
        /// <param name="k">Describes how "inflated" the parabola is.</param>
        public static float Parabola( float x, float k )
        {
            // https://iquilezles.org/articles/functions/
            return (float)System.Math.Pow( 4.0 * x * (1.0 - x), k );
        }

        /// <summary>
        /// Quadratic-like falloff, has a value of 1 at x = 0, and falls off to 0 at x = maxPoint.
        /// </summary>
        /// <param name="x">The input value.</param>
        /// <param name="maxPoint">The input value at which the function reaches 0.</param>
        public static float Falloff( float x, float maxPoint )
        {
            // https://iquilezles.org/articles/functions/
            x = 1.0f / ((x + 1.0f) * (x + 1.0f));
            maxPoint = 1.0f / ((maxPoint + 1.0f) * (maxPoint + 1.0f));
            return (x - maxPoint) / (1.0f - maxPoint);
        }

        /// <summary>
        /// 3rd order smoothstep. Has 1st derivative equal to 0 at `t = edge0` and `t = edge1`.
        /// </summary>
        public static float Smoothstep( float edge0, float edge1, float t )
        {
            // Hermite interpolation?
            float tc = (float)System.Math.Clamp( (t - edge0) / (edge1 - edge0), 0.0, 1.0 );

            // v = -2t^3 + 3t^2
            return tc * tc * ((tc * -2.0f) + 3.0f);
        }

        public static float InverseSmoothstep( float t )
        {
            return 0.5f - (float)System.Math.Sin( System.Math.Asin( 1.0 - 2.0 * t ) / 3.0 ); // edge0 = 0, edge1 = 1.
        }

        /// <summary>
        /// 5th order smoothstep. Has 1st and 2nd derivatives equal to 0 at `t = edge0` and `t = edge1`.
        /// </summary>
        public static float SmootherStep( float edge0, float edge1, float t )
        {
            // Proposed by Ken Perlin.
            float tc = (float)System.Math.Clamp( (t - edge0) / (edge1 - edge0), 0.0, 1.0 );

            // v = 6t^5 - 15t^4 + 10t^3
            return tc * tc * tc * (tc * ((tc * 6.0f) - 15.0f) + 10.0f);
        }

        /// <summary>
        /// 7th order smoothstep. Has 1st, 2nd, and 3rd derivatives equal to 0 at `t = edge0` and `t = edge1`.
        /// </summary>
        public static float Smoothstep7( float edge0, float edge1, float t )
        {
            float tc = (float)System.Math.Clamp( (t - edge0) / (edge1 - edge0), 0.0, 1.0 );

            // -20t^7 + 70t^6 - 84t^5 + 35t^4
            return tc * tc * tc * tc * (tc * (tc * ((tc * -20.0f) + 70.0f) - 84.0f) + 35.0f);
        }
    }
}
