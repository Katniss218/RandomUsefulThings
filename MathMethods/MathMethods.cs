using System;

namespace MathMethods
{
    public class MathMethods
    {
        public static float Lerp( float start, float end, float amount )
        {
            // This function linearly interpolates between two values, start and end, by the amount amount.
            // The amount is typically a value between 0 and 1, where 0 represents the start value and 1 represents the end value.

            return start + (end - start) * amount;
        }

        [Obsolete( "Unconfirmed" )]
        public static float Smoothstep( float edge0, float edge1, float x )
        {
            // Clamp x to the range [edge0, edge1]
            x = Math.Clamp( x, edge0, edge1 );

            // Calculate the smooth step value
            float t = (x - edge0) / (edge1 - edge0);
            return t * t * (3 - 2 * t);
        }

        /*
        // Linear step function
        [Obsolete( "Basically the same as Lerp" )]
        public static float LinearStep( float edge0, float edge1, float x )
        {
            // Clamp x to the range [edge0, edge1]
            x = Math.Max( Math.Min( x, edge1 ), edge0 );

            // Calculate the linear step value
            float t = (x - edge0) / (edge1 - edge0);
            return t;
        }
        */

        // Smoother step function
        [Obsolete( "Unconfirmed" )]
        public static float SmootherStep( float edge0, float edge1, float x )
        {
            // This is a variant of the smooth step function that produces a smoother curve.
            // It is defined as t * t * t * (t * (t * 6 - 15) + 10)

            // Clamp x to the range [edge0, edge1]
            x = Math.Max( Math.Min( x, edge1 ), edge0 );

            // Calculate the smoother step value
            float t = (x - edge0) / (edge1 - edge0);
            return t * t * t * (t * (t * 6 - 15) + 10);
        }

        // Smooth clamp function
        [Obsolete( "Unconfirmed" )]
        public static float SmoothClamp( float edge0, float edge1, float x )
        {
            // This is a function that smoothly clamps a value x to the range [edge0, edge1].
            // It is defined as edge0 + Smoothstep(0, 1, (x - edge0) / (edge1 - edge0)) * (edge1 - edge0).

            // Calculate the smooth clamp value
            float t = (x - edge0) / (edge1 - edge0);
            return edge0 + Smoothstep( 0, 1, t ) * (edge1 - edge0);
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
            return a + Smoothstep( 0, 1, t ) * (b - a);
        }
    }
}