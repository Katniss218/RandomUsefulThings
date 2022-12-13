using System;
using System.Drawing;

namespace MathMethods
{
    public class MathMethods
    {
        public static float Lerp( float from, float to, float t )
        {
            // This function linearly interpolates between two values, start and end, by the amount amount.
            // The amount is typically a value between 0 and 1, where 0 represents the start value and 1 represents the end value.

            return Math.Clamp( from + (to - from) * t, from, to );
        }

        public static double Map( double value, double inMin, double inMax, double outMin, double outMax )
        {
            return (value - inMin) / (inMax - inMin) * (outMax - outMin) + outMin;
        }

        public static float LerpUnclamped( float from, float to, float t )
        {
            return from + (to - from) * t;
        }

        public static double LerpUnclamped( double from, double to, double t )
        {
            return from + (to - from) * t;
        }

        // returns where in between the 2 values the current value is (returns the t factor in linear interpolation).
        // inverse lerp kind of.
        public static double InverseLerp( double value, double min, double max )
        {
            return (value - min) / (max - min);
        }

        // mirrors the value across the midpoint of the specified range.
        public static double Flip( double value, double min, double max )
        {
            double range = max - min;
            return max - (value - min) % range;
        }

        // greatest common divisor
        public static int GCD( int a, int b )
        {
            while( b != 0 )
            {
                int temp = b;
                b = a % b;
                a = temp;
            }
            return a;
        }

        // sum of first n numbers that are 1 or larger. Also the 3-sided figurate number.
        public static int Triangular( int n )
        {
            if( n < 0 )
            {
                throw new ArgumentException( "n must be a non-negative integer." );
            }
            return n * (n + 1) / 2;
        }

        // sum of Triangular called on numbers [1-n]
        public static int Tetrahedral( int n )
        {
            if( n < 0 )
            {
                throw new ArgumentException( "n must be a non-negative integer." );
            }
            return n * (n + 1) * (n + 2) / 6;
        }

        // returns the total number of cannonballs that can fit in an "n-ring" with a given number of sides and a given number of cannonballs per edge.
        private static int FigurateRing( int sides, int n )
        {
            return n == 1 ? 1 : sides * (n - 1);
        }

        /// <summary>
        /// Calculates figurate numbers. E.g. Triangular, Square, Pentagonal, Hexagonal, etc. numbers.
        /// </summary>
        /// <returns>Returns the n-th figurate number with the given number of sides.</returns>
        public static int Figurate( int sides, int n )
        {
            // subtract the top 2 edges of each future ring, except the n-th one.
            int nminus1 = n - 1;
            int acc = -(nminus1 * nminus1); // n * n = sum of odd natural numbers smaller than n. Each ring increases by 2.
            // Accumulate the rings.
            for( ; n >= 1; n-- )
            {
                acc += FigurateRing( sides, n );
            }
            return acc;
        }

        public static int SumOfDigits( int n )
        {
            if( n < 0 )
            {
                n = -n;
            }

            int sum = 0;
            while( n > 0 )
            {
                int digit = n % 10;
                sum += digit;
                n /= 10;
            }
            return sum;
        }

        [Obsolete( "Unconfirmed" )]
        public static double Hermite( double from, double to, double t )
        {
            double clampedT = Math.Clamp( t, 0, 1 );
            return LerpUnclamped( from, to, clampedT * clampedT * (3 - 2 * clampedT) );
        }

        [Obsolete( "Unconfirmed" )]
        public static float Smoothstep( float edge0, float edge1, float x )
        {
            // Clamp x to the range [edge0, edge1]
            x = Math.Clamp( x, edge0, edge1 );

            // Calculate the smooth step value
            return LerpUnclamped( edge0, edge1, x * x * (3 - 2 * x) );
        }

        public static Color LerpColor( Color from, Color to, double t )
        {
            double r = LerpUnclamped( from.R, to.R, t );
            double g = LerpUnclamped( from.G, to.G, t );
            double b = LerpUnclamped( from.B, to.B, t );
            double a = LerpUnclamped( from.A, to.A, t );
            return Color.FromArgb( (int)a, (int)r, (int)g, (int)b );
        }
        /*
        public static Color SmoothStepColor( Color from, Color to, double t )
        {
            double r = SmoothStep( from.R, to.R, t );
            double g = SmoothStep( from.G, to.G, t );
            double b = SmoothStep( from.B, to.B, t );
            double a = SmoothStep( from.A, to.A, t );
            return Color.FromArgb( (int)a, (int)r, (int)g, (int)b );
        }
        */

        public delegate double EasingFunction( double t );

        [Obsolete( "Unconfirmed" )]
        public static double Interpolate( double from, double to, double t, EasingFunction easingFunction )
        {
            double clampedT = Math.Clamp( t, 0, 1 );
            return LerpUnclamped( from, to, easingFunction( clampedT ) );
        }

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