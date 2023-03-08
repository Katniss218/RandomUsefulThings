using System;
using System.Drawing;

namespace MathMethods
{
    public class MathMethods
    {
        // Formula for a 2D orbit shape, E is eccentricity, P is the periapsis.
        // x and y are coordinates.
        // (1 - E^2)*x^2 - 2Px + y^2 = 0

        [Obsolete( "Unconfirmed" )]
        public static float Modulo( float a, float b )
        {
            return a - b * (float)Math.Floor( a / b );
        }

        /// <param name="x">The value to sample.</param>
        public static double Sinc( double x )
        {
            // https://en.wikipedia.org/wiki/Sinc_function
            if( x == 0 )
            {
                return 1.0;
            }
            return Math.Sin( x ) / x;
        }

        /// <summary>
        /// Sinc(x) normalized to have a period of 1.
        /// </summary>
        /// <param name="x">The value to sample.</param>
        public static double SincNormalized( double x )
        {
            // https://en.wikipedia.org/wiki/Sinc_function
            if( x == 0 )
            {
                return 1.0;
            }
            return Math.Sin( Math.PI * x ) / (Math.PI * x);
        }

        /// <summary>
        /// Linearly maps a value from one range onto another range.
        /// </summary>
        /// <param name="value">The value from the original range to map.</param>
        /// <param name="inMin">The min value of the original range.</param>
        /// <param name="inMax">The max value of the original range.</param>
        /// <param name="outMin">The min value of the new range.</param>
        /// <param name="outMax">The max value of the new range.</param>
        /// <returns>The value mapped onto the new range.</returns>
        public static float Map( float value, float inMin, float inMax, float outMin, float outMax )
        {
            // This is related to linear interpolation.

            // First shift the value so that the original range now starts at 0.
            // Then multiply the value to map onto the new range.
            // And last, unshift the value so that the new range starts at `outMin`.
            return (value - inMin) / (inMax - inMin) * (outMax - outMin) + outMin;
        }

        public static float LengthSquared( float x, float y )
        {
            // length squared of a vector.
            return (x * x) + (y * y);
        }

        public static float Length( float x, float y )
        {
            // length of a vector.
            return (float)Math.Sqrt( LengthSquared( x, y ) );
        }

        public static float Dot( float v1x, float v1y, float v2x, float v2y )
        {
            // dot product between 2 vectors.
            return (v1x * v2x) + (v1y * v2y);
        }

        /// <summary>
        /// Mirrors the value across the specified point.
        /// </summary>
        public static float Flip( float value, float mirrorPoint )
        {
            return value + 2 * (mirrorPoint - value);
        }

        /// <summary>
        /// Mirrors the value across the midpoint of the specified range.
        /// </summary>
        public static float Flip( float value, float min, float max )
        {
            float range = max - min;
            return max - (value - min) % range;
        }

        /// <summary>
        /// Returns the greatest common divisor of two integers.
        /// </summary>
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

        // least common multiple
        [Obsolete( "Unconfirmed, but by looking at it, it makes sense" )]
        public static int LCM( int a, int b )
        {
            return (a * b) / GCD( a, b );
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

        private static int FigurateRing( int sides, int n )
        {
            // returns the total number of cannonballs that can fit in a "ring" with a given number of sides `sides` and a given number of cannonballs per edge `n`.
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

        /// <summary>
        /// Returns the factorial of a given number (the product of all the natural numbers less than or equal to it).
        /// </summary>
        public static long Factorial( int value )
        {
            if( value < 0 )
            {
                throw new ArgumentException( "Factorial of a negative number is not defined." );
            }
            if( value == 0 )
            {
                return 1;
            }

            long result = 1;
            for( int i = 2; i <= value; i++ ) // could be optimized by a while loop.
            {
                result *= i;
            }
            return result;
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

        /// <summary>
        /// Rounds the value to the nearest multiple of the specified number.
        /// </summary>
        public static float RoundToMultiple( float value, float multiple )
        {
            return multiple * (float)Math.Round( value / multiple );
        }

        /// <summary>
        /// Returns the scale that makes the object appear to have a constant angular size no matter the distance.
        /// </summary>
        /// <param name="fixedSize">Controls the size of the object.</param>
        public static float GetScaleToFixSize( float distance, float fov, float fixedSize )
        {
            float scale = distance * fixedSize * fov;

            return scale;
        }

        /// <summary>
        /// Returns the FoV needed to make an object appear a constant angular size.
        /// </summary>
        /// <param name="fixedSize">Controls the size of the object.</param>
        public static float GetFovToFixSize( float distance, float fixedSize )
        {
            const float SCALE = 1.0f;
            float fov = SCALE / (distance * fixedSize);

            return fov;
        }
    }
}