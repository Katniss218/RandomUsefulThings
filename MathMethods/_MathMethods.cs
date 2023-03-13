﻿using System;
using System.Drawing;

namespace MathMethods
{
    public class MathMethods
    {
        public static int Sqrt( int x ) // Returns the floor of the square root of the specified integer. Newton-Raphson method.
        {
            double Prev = 0;
            double Cur = x;
            while( (int)Prev != (int)Cur )
            {
                Prev = Cur;
                Cur = Cur - ((Cur * Cur) - x) / (2 * Cur);
            }
            return (int)(Cur - (Cur % 1));
        }

        // erf(x) for x > 0
        // 1-\ 6\ \frac{1}{2+\left(x+2\right)^{\left(x+2\right)}}
        // error: -0.05

        public static float Modulo( float x, float range ) // correct.
        {
            // Modulo, supports arbitrary floating point numbers in range [float.MinValue..float.MaxValue] for both parameters.
            // Negative values of range wrap into [-range..0]
            // Positive values of range wrap into [0-..range]

            return x - range * (float)Math.Floor( x / range );
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

        public static T Clamp<T>( T value, T min, T max ) where T : IComparable<T>
        {
            if( value.CompareTo( max ) > 0 )
                return max;
            if( value.CompareTo( min ) < 0 )
                return min;
            return value;
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
            // Then divide to get normalized range and multiply to map onto the new range.
            // And lastly, unshift the value so that the new range starts at `outMin`.
            return (((value - inMin) / (inMax - inMin)) * (outMax - outMin)) + outMin;
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

        [Obsolete( "untested" )]
        public static int MySqrt( int x ) // floor of the square root of the specified integer.
        {
            // Newton method.
            long res = x;
            while( res * res > x )
                res = (res + x / res) / 2;
            return (int)res;
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
        public static float RoundToMultiple( float value, float multipleOfThis )
        {
            // Rounds the number to the closest integer in the space where `multipleOfThis` = 1, then brings it back up to the correct range.
            return multipleOfThis * (float)Math.Round( value / multipleOfThis );
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

        /// <summary>
        /// The circle constant.
        /// </summary>
        public static double PI = 3.1415926535897932384626433832795028841971693993751;

        /// <summary>
        /// One degree in radians.
        /// </summary>
        public static double DegToRad = PI / 180.0; // 0.017453292519943295769236907684886127134428718885417

        /// <summary>
        /// One radian in degrees.
        /// </summary>
        public static double RadToDeg = 180.0 / PI; // 57.295779513082320876798154814105170332405472466564 
    }
}