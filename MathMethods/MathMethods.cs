using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace RandomUsefulThings.Math
{
    public static class MathMethods
    {
        // erf(x) for x > 0
        // 1-\ 6\ \frac{1}{2+\left(x+2\right)^{\left(x+2\right)}}
        // error: -0.05

        // curvature = Wedge(f', f'') / Magn(f')^3

        public static double Modulo( double x, double range ) // correct.
        {
            // Modulo, supports arbitrary floating point numbers in range [float.MinValue..float.MaxValue] for both parameters.
            // Negative values of range wrap into [-range..0]
            // Positive values of range wrap into [0-..range]

            // The modulo operation is equivalent to the bitwise AND when the divisor is a power of 2
            // mod(x, 1) = fract(x)

            return x - range * System.Math.Floor( x / range ); // without the floor it will equal zero at all points.
        }

        [Obsolete( "Don't use this. This is buggy because of range issues. For educational purposes only." )]
        public static int Floor( float n )
        {
            // you can technically get the fractional part of a number by incrementing an integer until it's larger than the desired number.
            // - if you don't have access to any sort of function that can return the fractional part of a number.

            // Maybe a better way with repeated division by 10?

            // multiply by 2 until larger, then use binary search to get to the actual value.


            int x = 0; // ignore the fact this returns the integer part.
            if( n < 0 )
            {
                while( x < n )
                {
                    x++;
                }
                return x - 1;
            }
            if( n > 0 )
            {
                while( x > n )
                {
                    x--;
                }
                return x + 1;
            }
            return 0;
        }

        public static int NChoose2( int n )
        {
            return (n * (n - 1)) / 2;
        }

        public static int NChoose4( int n )
        {
            return (n * (n - 1) * (n - 2) * (n - 3)) / /*factorial(4)*/ 24;
        }

        [Obsolete( "untested, but should work I think" )]
        public static int NChooseM( int n, int m )
        {
            int acc = 1;
            for( int i = 0; i < m; i++ )
            {
                acc *= n - i;
            }
            acc /= (int)Factorial( m );
            return acc;
        }

        [Obsolete( "Don't use this. This is buggy because of range issues. For educational purposes only." )]
        public static float Fract( float n )
        {
            return n - (float)Floor( n );
        }

        /// <param name="x">The value to sample.</param>
        public static double Sinc( double x )
        {
            // sin(x) / x
            // https://en.wikipedia.org/wiki/Sinc_function
            if( x == 0 )
            {
                return 1.0;
            }
            return System.Math.Sin( x ) / x;
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
            return System.Math.Sin( System.Math.PI * x ) / (System.Math.PI * x);
        }

        public static T Clamp<T>( T value, T min, T max ) where T : IComparable<T>
        {
            if( value.CompareTo( max ) > 0 )
                return max;
            if( value.CompareTo( min ) < 0 )
                return min;
            return value;
        }

        public static bool IsDivisible( int n, int byN )
        {
            while( byN % 2 == 0 )
            {
                byN /= 2;
            }
            while( byN % 5 == 0 )
            {
                byN /= 5;
            }
            return n % byN == 0;
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
            // Then divide to get normalized range (i.e. [0..1]) and multiply to map onto the new range.
            // And lastly, unshift the value so that the new range starts at `outMin`.
            return (((value - inMin) / (inMax - inMin)) * (outMax - outMin)) + outMin;
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

        public static int Triangular( int n )
        {
            // Returns the sum of all integers >= 1 && <= n
            // sum of first n numbers that are 1 or larger. Also the 3-sided figurate number.

            if( n < 0 )
            {
                throw new ArgumentException( "n must be a non-negative integer." );
            }
            return n * (n + 1) / 2;
        }

        public static int Tetrahedral( int n )
        {
            // Equivalent to:
            // - Triangular(x) on every x in [1..n] (inclusive), sum up the results.

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
        public static long Factorial( int n )
        {
            if( n < 0 )
            {
                throw new ArgumentException( "Factorial of a negative number is not defined." );
            }
            if( n == 0 )
            {
                return 1;
            }

            // Iterative method. It's faster and more stack efficient.
            long result = 1;
            for( ; n >= 2; --n ) // input param `n` is passed by value.
            {
                result *= n;
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
        public static float RoundToMultiple( float value, float multipleOfThis )
        {
            // Rounds the number to the closest integer in space where `multipleOfThis` = 1, then brings it back up to the correct range.
            return multipleOfThis * (float)System.Math.Round( value / multipleOfThis );
        }

        [Obsolete( "Unconfirmed" )]
        public static double GetAngularDiameter( double distance, double radius )
        {
            double rad = System.Math.Atan2( radius, distance );
            return rad;
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
        public static float GetFovToFixSize( float distance, float fixedSize, float scale = 1.0f )
        {
            float fov = scale / (distance * fixedSize);

            return fov;
        }

        [Obsolete( "Unconfirmed" )]
        public static double Round( double x, double b )
        {
            // Rounds the value in the specified number system (base b)
            return System.Math.Round( x / b ) * b;
        }

        public static double BellCurve( double x, double standardDeviation, double midpoint )
        {
            // gaussian distribution.

            const double SqrtTwoPI = 2.50662827463; // sqrt(pi*2)

            double squaredExp = (x - midpoint) / standardDeviation;
            squaredExp *= squaredExp;

            return (1 / (standardDeviation * SqrtTwoPI)) * System.Math.Pow( System.Math.E, -0.5 * squaredExp );
        }

        /// <summary>
        /// Computes binomial coefficients (n choose k).
        /// </summary>
        /// <returns>The number of ways that k elements from a set of n elements can be arranged.</returns>
        [Obsolete( "unconfirmed" )]
        public static int Binomial( int n, int k )
        {
            int result = 1;

            for( int i = 1; i <= k; i++ )
            {
                result *= n - i + 1;
                result /= i;
            }

            return result;
        }
    }
}