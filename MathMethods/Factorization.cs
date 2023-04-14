using System;
using System.Collections.Generic;
using System.Text;

namespace RandomUsefulThings.Math
{
    public static class Factorization
    {
        /// <summary>
        /// Returns the greatest common divisor of two integers.
        /// </summary>
        public static int GCD( int a, int b )
        {
            // Euclid's method.
            while( b != 0 )
            {
                int temp = b;
                b = a % b;
                a = temp;
            }
            return a;
        }

        [Obsolete( "Untested, should work I think" )]
        public static (int gcd, int x, int y) BezotsEuclidGCD( int a, int b )
        {
            //     result[0] is gcd(a, b)
            //     result[1] is x
            //     result[2] is y 

            // Initialize the state variables.
            int s = 0;
            int oldS = 1;

            int t = 1;
            int oldT = 0;

            int r = b;
            int oldR = a;

            while( r != 0 )
            {
                int quotient = oldR / r;

                int temp = r;
                r = oldR - quotient * r;
                oldR = temp;

                temp = s;
                s = oldS - quotient * s;
                oldS = temp;

                temp = t;
                t = oldT - quotient * t;
                oldT = temp;
            }
            return (oldR, oldS, oldT);
        }

        // least common multiple
        [Obsolete( "Unconfirmed, but by looking at it, it makes sense" )]
        public static int LCM( int a, int b )
        {
            return (a * b) / GCD( a, b );
        }

        [Obsolete( "Unconfirmed" )]
        public static bool AreCoprime( int a, int b )
        {
            return GCD( a, b ) == 1;
        }

        [Obsolete( "Unconfirmed" )]
        public static int Totient( int n )
        {
            // Supposed to be Euler's Totient function.
            int result = n;
            for( int p = 2; p * p <= n; p++ )
            {
                if( n % p == 0 )
                {
                    while( n % p == 0 )
                    {
                        n /= p;
                    }
                    result -= result / p;
                }
            }
            if( n > 1 )
            {
                result -= result / n;
            }
            return result;
        }

        [Obsolete( "Unconfirmed" )]
        public static bool IsPrime( int n )
        {
            if( n <= 1 )
                return false;
            if( n <= 3 )
                return true;
            if( n % 2 == 0 || n % 3 == 0 )
                return false;
            for( int i = 5; i * i <= n; i = i + 6 ) // check numbers in [5..sqrt(n)].
            {
                if( n % i == 0 || n % (i + 2) == 0 )
                    return false;
            }
            return true;
        }

        [Obsolete( "Unconfirmed" )]
        public static List<int> GetPrimeFactors( int n )
        {
            // Brute Force.
            List<int> factors = new List<int>();

            for( int i = 2; i <= n; i++ )
            {
                while( n % i == 0 )
                {
                    factors.Add( i );
                    n /= i;
                }
            }

            return factors;
        }

        [Obsolete( "Unconfirmed" )]
        public static List<int> ContinuedFraction( double x )
        {
            // returns the continued fraction ? maybe idk

            List<int> terms = new List<int>();
            int integerPart = (int)x;
            double fractionPart = x - integerPart;
            while( fractionPart != 0 )
            {
                int term = (int)(1 / fractionPart);
                terms.Add( term );
                fractionPart = 1 / fractionPart - term;
            }
            terms.Insert( 0, integerPart );
            return terms;
        }

    }
}
