using System;
using System.Collections.Generic;
using System.Text;

namespace MathMethods
{
    class Trig
    {
        // Taylor series approximations:
        // sin(x) = x - x^3/3! + x^5/5! - x^7/7! + x^9/9! - ...
        // arcsin(x) = x + (1/2)x^3/3 + (1/2)(3/4)x^5/5 + (1/2)(3/4)(5/6)x^7/7 + ...
        // also close to x - 1, arcsin(x) = π/2 - √(1-x^2) - (1/6) (1-x^2)^(3/2)

        // cos(x) = 1 - x^2/2! + x^4/4! - x^6/6! + x^8/8! - ...
        // arccos(x) = π/2 - arcsin(x)

        // tan(x) = x + x^3/3 + 2x^5/15 + 17x^7/315 + 62x^9/2835 + ...
        // tan(x) = sin(x) / cos(x)
        // arctan(x) = x - x^3/3 + x^5/5 - x^7/7 + x^9/9 - ...

        // can be approximated with just +-*/% by precomputing factorials and using multiplication for powers.

        // CORDIC algorithm can be used to calculate approximate values of trig functions.

        public static double Sin( double x ) // seems to work on x = pi and x = half pi
        {
            const int NUM_TERMS = 7;

            x %= System.Math.PI; // shorten the range needed since sin is periodic.

            double result = 0;
            double term = x;
            int sign = 1;
            int factorial = 1;

            for( int i = 0; i < NUM_TERMS; i++ )
            {
                result += sign * term;
                term = term * x * x / ((i * 2 + 2) * (i * 2 + 3));
                sign *= -1;
                factorial *= (i * 2 + 2) * (i * 2 + 3);
            }

            return result;
        }

        [Obsolete( "kinda works?? not very accurate" )]
        public static double Asin( double x )
        {
            double y = System.Math.Abs( x );
            double angle = 0;
            double factor = 1;
            double poweroftwo = 1;

            for( int i = 0; i < 15; i++ )
            {
                double temp = y - factor * poweroftwo;
                if( temp >= 0 )
                {
                    y = temp;
                    angle += factor;
                }
                factor /= 2;
                poweroftwo /= System.Math.Sqrt( 1 + poweroftwo * poweroftwo );
            }

            if( x < 0 )
            {
                angle = -angle;
            }

            return angle;
        }

        public static double Cos( double x ) // seems to work.
        {
            const float NUM_TERMS = 7;
            double result = 1;
            double term = 1;
            int sign = -1;

            for( int i = 2; i < NUM_TERMS; i += 2 )
            {
                term = term * x * x / ((i - 1) * i);
                result += sign * term;
                sign *= -1;
            }

            return result;
        }

        [Obsolete( "this is wrong because asin is wrong" )]
        public static double Acos( double x )
        {
            return System.Math.PI / 2 - Asin( x );
        }

        [Obsolete( "this is wrong" )]
        public static double Tan( double x )
        {
            double result = 0;
            double term = x;
            double xSquared = x * x;
            int sign = 1;

            for( int i = 1; i < 1000; i++ )
            {
                result += sign * term;
                term = term * xSquared * 2 / ((i * 2 + 1) * (xSquared + (i * 2 + 1)));
                sign *= -1;
            }

            return result;
        }

        [Obsolete( "not really right either" )]
        public static double Arctan( double x )
        {
            const int NUM_TERMS = 70;

            double result = 0;
            double term = x;
            int sign = 1;

            for( int i = 0; i < NUM_TERMS; i++ )
            {
                result += sign * term;
                term = term * x * x / ((i * 2 + 3) * (i * 2 + 2));
                sign *= -1;
            }

            return result;
        }

        [Obsolete( "wrong" )]
        public static double Log( double x, double b )
        {
            return Log( x ) / Log( b );
        }

        [Obsolete( "wrong" )]
        public static double Log( double x ) // natural log
        {
            // apparently ln(x) = 2 * ( (x-1) - (1/3)((x-1)^2) + (1/5)((x-1)^3) - (1/7)((x-1)^4) + ... )
            // also apparently, ln(x) = ((-1)^n-1) * ((x-1)^n)/(n)
            // also for any positive x, a is a large number, the larger the better, ln(x) ~= (a*x)^(1/a) − a

            // also apparently log(x,b) = (ln x)/(ln b) = sum(k=0 to infinity) ((x-1)/(x+1))^(2k+1)/(2k+1)

            double result = 0;
            double term = (x - 1) / x;
            double power = term;

            for( int i = 1; i <= 1000; i += 2 )
            {
                result += power / i;
                power *= term * term;
            }

            return 2 * result;
        }

        public static float Sqrt( float x )
        {
            // newton-raphson
            // x_n+1 = x_n/2 + x/2x_n
            float current = x * 0.375f;
            for( int i = 0; i < 10; i++ ) // the bigger it is, the more iterations you need.
            {
                current = (current + (x / current)) * 0.5f;
            }
            return current;

            // faster converging methods to approximate the square root of x.
            // One such method is the Halley's method, an iterative method that uses the formula:
            // x_n + 1 = x_n - (2x_n ^ 3 - 3x_n ^ 2 * x)/ (6x_n ^ 2 - 6x_n* x +x ^ 2)

            // Another even faster method is the Householder's method, an iterative method that uses the formula:
            // x_n + 1 = x_n + 2(x - x_n ^ 2) / (2x_n ^ 2 - x)

            /*
            initial_guess = 2**(n//2) * 2**((n%2)*0.5)

            where "n" is the integer closest to the input value "x" that is greater than or equal to zero.
            This formula computes an initial guess by first computing the integer part of the square root of "x" (i.e., "n//2")
            and then adding half a unit if "x" is odd (i.e., "(n%2)0.5"), and finally multiplying by 2 raised to the power of half the number
            of bits used to represent the floating-point values (i.e., "2*(-k/2)").

            This initial guess is based on the observation that the square root of a number "x" is roughly
            2 raised to the power of half the number of bits used to represent the floating-point values
            (i.e., "2**(-k/2)"), where "k" is the number of bits used. 
            This observation can be used to compute an initial guess that is close to the actual square root and
            that can help speed up the convergence of the Newton-Raphson method.
            */

        }
        [Obsolete( "unconfirmed" )]
        static double InitialGuess( double x )
        {
            if( x == 0 )
                return 0;
            int n = (int)x;
            int k = 0;
            while( n > 0 )
            {
                n >>= 1;
                k++;
            }
            double guess = 1 << ((k >> 1) + (k % 2 == 1 ? 1 : 0));
            return guess;
        }

        // n*(n+1)/2
        [Obsolete( "untested" )]
        public static long SumOfNaturalNumbers( int lastElement )
        {
            // returns the sum of all natural numbers less than or equal to the parameter.

            return (long)lastElement * ((long)lastElement + 1) / 2;
        }

    }
}
