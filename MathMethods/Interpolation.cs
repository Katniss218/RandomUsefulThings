using System;
using System.Collections.Generic;
using System.Text;

namespace MathMethods
{
    public static class Interpolation
    {
        /// <summary>
        /// Linear interpolation without clamping the output values.
        /// </summary>
        public static float LerpUnclamped( float from, float to, float t )
        {
            // Linear interpolation is the simplest and most canonical interpolation.
            // It is defined as      `v = (to * t) + (from * (1 - t))`
            // Can be simplified to  `v = from + (to - from) * t`

            // Lerp is a C0 continuous function, meaning its not differentiable
            //   (only it's "0th derivative" (the func itself) is continuous, thus 0).

            // By not clamping, we "extend" the line connecting the 2 points (from and to) infinitely in both directions.
            return from + (to - from) * t;
        }
        public static double LerpUnclamped( double from, double to, double t )
        {
            return from + (to - from) * t;
        }

        /// <summary>
        /// Linear interpolation with clamped output values.
        /// </summary>
        public static float Lerp( float from, float to, float t )
        {
            return Math.Clamp( LerpUnclamped( from, to, t ), from, to );
        }
        public static double Lerp( double from, double to, double t )
        {
            return Math.Clamp( LerpUnclamped( from, to, t ), from, to );
        }

        // returns where in between the 2 values the current value is (returns the t factor in linear interpolation).
        // inverse lerp kind of.
        public static float InverseLerp( float value, float from, float to )
        {
            return (value - from) / (to - from);
        }

        public delegate float EasingFunction( float t );

        [Obsolete( "Unconfirmed" )]
        public static double Interpolate( float from, float to, float t, EasingFunction easingFunction )
        {
            // lerp with an easing function applied to the value of t.
            float clampedT = Math.Clamp( t, 0, 1 );
            return LerpUnclamped( from, to, easingFunction( clampedT ) );
        }

        // Smooth clamp function
        [Obsolete( "Unconfirmed" )]
        public static float SmoothClamp( float edge0, float edge1, float x )
        {
            // This is a function that smoothly clamps a value x to the range [edge0, edge1].
            // It is defined as edge0 + Smoothstep(0, 1, (x - edge0) / (edge1 - edge0)) * (edge1 - edge0).

            // Calculate the smooth clamp value
            float t = (x - edge0) / (edge1 - edge0);
            return edge0 + Easing.Smoothstep( 0, 1, t ) * (edge1 - edge0);
        }

        // Smooth interpolation function
        [Obsolete( "Unconfirmed" )]
        public static float SmoothInterpolation( float a, float b, float t )
        {
            // This is a function that smoothly interpolates between two values a and b using a smooth step function.
            // It is defined as a + Smoothstep(0, 1, t) * (b - a), where t is the interpolation value such that 0 <= t <= 1.

            // Clamp t to the range [0, 1]
            t = Math.Max( Math.Min( t, 1 ), 0 );

            // Calculate the smooth interpolation value
            return a + Easing.Smoothstep( 0, 1, t ) * (b - a);
        }

        // Spherical linear interpolation (slerp) can be used to e.g.:
        // - interpolate points on the surface of a sphere.
        // - interpolate direction vectors (it's equivalent to points on a unit sphere).
        // - interpolate quaternions.
    }
}
