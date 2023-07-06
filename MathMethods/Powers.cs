using System;
using System.Collections.Generic;
using System.Text;

namespace RandomUsefulThings.Math
{
    public static class Powers
    {
        // 1 * 10^n => 1 followed by n zeroes.
        // 1.5 * 10^n => 15 followed by `n - number of decimal digits` zeroes. if number of decimal digits is greater or equal to the power of 10, the decimal point will still exist.

        // 1 * 10^-n => 0.000...01, where total number of zeroes (including leading zero) = n.

        public static float FastPow( float a, int power )
        {
            // Fast Pow, or "Exponentiation by squaring" algorithm.
            // This could also work with addition, producing multiplication, as well as normal exponentiation, producing tetration.

            if( a == 0 || a == 1 || power == 1 )
                return a;

            if( power == 0 )
                return 1;

            // `a raised to negative power` == `1/a raised to positive power`.
            if( power < 0 )
            {
                a = 1 / a;
                power = -power;
            }

            // Iterative version (it's faster).
            float result = 1;
            while( power > 0 )
            {
                // x^y = (x^2)^(y/2) if y is even
                // x^y = x*(x^2)^(y/2) if y is odd
                if( power % 2 != 0 ) // make the odd case into the even case.
                {
                    power--;
                    result *= a;
                }

                power /= 2;
                a *= a;
            }

            return result;
        }

        public static int SqrtInt( int x ) // Returns the floor of the square root of the specified integer.
        {
            //  Newton-Raphson method.
            double previous = 0;
            double current = x;
            while( (int)previous != (int)current )
            {
                previous = current;
                current = current - ((current * current) - x) / (2 * current);
            }
            return (int)(current - (current % 1));
        }

        public static int SqrtIntFast( int x ) // floor of the square root of the specified integer.
        {
            // Newton-Raphson method.
            // This is faster than the other SqrtInt (tested for numbers [0..10000]).
            long current = x;
            while( current * current > x ) // current is greater than the square root of x.
            {
                current = (current + x / current) / 2;
            }
            return (int)current;
        }

        public static float Sqrt( float x )
        {
            if( x < 0 )
            {
                return float.NaN;
            }

            // Initialize current to the initial guess.
            float current;
            if( x < 10000 )
            {
                current = x * 0.375f;
            }
            else
            {
                int n = (int)x;
                int k = 0;
                while( n > 0 )
                {
                    n >>= 1;
                    k++;
                }
                current = 1 << ((k >> 1) + (k % 2 == 1 ? 1 : 0));
            }

            const int ITERATIONS = 10;
            // Newton-Raphson
            for( int i = 0; i < ITERATIONS; i++ )
            {
                // newton-raphson
                // x_n+1 = x_n/2 + x/2x_n
                current = (current + (x / current)) * 0.5f;
            }
            /*for( int i = 0; i < ITERATIONS; i++ )
            {
                // Halley's method
                // x_n+1 = x_n - (2x_n ^ 3 - 3x_n ^ 2 * x)/ (6x_n ^ 2 - 6x_n* x +x ^ 2)
                current = current - (2 * (current * current * current) - 3 * (current * current) * x) / (6 * (current * current) - 6 * current * x + x * x);
            }
            for( int i = 0; i < ITERATIONS; i++ )
            {
                // Householder's method:
                // x_n+1 = x_n + 2(x - x_n ^ 2) / (2x_n ^ 2 - x)
                current = current + (2 * (x - (current * current))) / (2 * (current * current) - x);
            }*/
            return current;
        }

    }
}
