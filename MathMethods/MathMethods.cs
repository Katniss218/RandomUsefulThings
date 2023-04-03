using System;
using System.Collections.Generic;
using System.Drawing;

namespace RandomUsefulThings.Math
{
    public static class MathMethods
    {
        // n*(n+1)/2
        [Obsolete( "untested" )]
        public static long SumOfNaturalNumbers( int lastElement )
        {
            // returns the sum of all natural numbers less than or equal to the parameter.

            return (long)lastElement * ((long)lastElement + 1) / 2;
        }


        public static int Sqrt( int x ) // Returns the floor of the square root of the specified integer. Newton-Raphson method.
        {
            double previous = 0;
            double current = x;
            while( (int)previous != (int)current )
            {
                previous = current;
                current = current - ((current * current) - x) / (2 * current);
            }
            return (int)(current - (current % 1));
        }

        // erf(x) for x > 0
        // 1-\ 6\ \frac{1}{2+\left(x+2\right)^{\left(x+2\right)}}
        // error: -0.05

        public static float Modulo( float x, float range ) // correct.
        {
            // Modulo, supports arbitrary floating point numbers in range [float.MinValue..float.MaxValue] for both parameters.
            // Negative values of range wrap into [-range..0]
            // Positive values of range wrap into [0-..range]

            // The modulo operation is equivalent to the bitwise AND when the divisor is a power of 2
            // mod(x, 1) = fract(x)

            return x - range * (float)System.Math.Floor( x / range ); // without the floor it will equal zero at all points.
        }

        public static double Modulo( double x, double range )
        {
            return x - range * System.Math.Floor( x / range );
        }

        [Obsolete( "Don't use this. This is buggy because of range issues. For educational purposes only." )]
        public static int Floor( float n )
        {
            // you can technically get the fractional part of a number by incrementing an integer until it's larger than the desired number.
            // - if you don't have access to any sort of function that can return the fractional part of a number.

            // Maybe a better way with repeated division by 10?
            // Maybe could also work with binary search starting at sign(n) * int.MaxValue/2

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

        [Obsolete( "Don't use this. This is buggy because of range issues. For educational purposes only." )]
        public static float Fract( float n )
        {
            return n = (float)Floor( n );
        }

        [Obsolete( "unconfirmed" )]
        public static float CustomPow( float x, float y )
        {
            if( x == 0 || x == 1 || y == 1 )
                return x;

            if( y == 0 )
                return 1;

            if( y < 0 )
            {
                x = 1 / x;
                y = -y;
            }

            float result = 1;
            while( y > 0 )
            {
                if( y % 2 == 1 )
                    result *= x;

                y /= 2;
                x *= x;
            }

            return result;
        }

        /// <param name="x">The value to sample.</param>
        public static double Sinc( double x )
        {
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
            return (float)System.Math.Sqrt( LengthSquared( x, y ) );
        }

        public static float Dot( float v1x, float v1y, float v2x, float v2y )
        {
            // dot product between 2 vectors.
            return (v1x * v2x) + (v1y * v2y);
        }

        // 1 * 10^n => 1 followed by n zeroes.
        // 1.5 * 10^n => 15 followed by `n - number of decimal digits` zeroes. if number of decimal digits is greater or equal to the power of 10, the decimal point will still exist.

        // 1 * 10^-n => 0.000...01, where total number of zeroes (including leading zero) = n.

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

        [Obsolete( "Unconfirmed" )]
        public static List<int> PrimeFactors( int n )
        {
            List<int> factors = new List<int>();
            int i = 2;
            while( n > 1 )
            {
                while( n % i == 0 )
                {
                    factors.Add( i );
                    n /= i;
                }
                i++;
            }
            return factors;
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

        [Obsolete("Unconfirmed")]
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
            for( ; value >= 2; --value )
            {
                result *= value;
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

        [Obsolete("Unconfirmed")]
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

        [Obsolete("Unconfirmed")]
        public static double Exp( double x, double b )
        {
            // exp but with arbitrary base b.
            return System.Math.Exp( b * System.Math.Log( x ) );
        }

        [Obsolete( "Unconfirmed" )]
        public static double Round( double x, double b )
        {
            // Rounds the value in the specified number system (base b)
            return System.Math.Round( x / b ) * b;
        }

        /// <summary>
        /// Computes e raised to the power of x.
        /// </summary>
        public static double Exp( double x ) // Reasonably accurate around x < 10.
        {
            // exp(x+y) = exp(x)*exp(y)
            // so we can precompute a table with values for the integer part (the full 64-bit precision only has range x in [-745..710]).
            // The fractional part is also only important near 0.

            double[] ExpAdjustment = new double[256]
            {
                1.040389835,
                1.039159306,
                1.037945888,
                1.036749401,
                1.035569671,
                1.034406528,
                1.033259801,
                1.032129324,
                1.031014933,
                1.029916467,
                1.028833767,
                1.027766676,
                1.02671504,
                1.025678708,
                1.02465753,
                1.023651359,
                1.022660049,
                1.021683458,
                1.020721446,
                1.019773873,
                1.018840604,
                1.017921503,
                1.017016438,
                1.016125279,
                1.015247897,
                1.014384165,
                1.013533958,
                1.012697153,
                1.011873629,
                1.011063266,
                1.010265947,
                1.009481555,
                1.008709975,
                1.007951096,
                1.007204805,
                1.006470993,
                1.005749552,
                1.005040376,
                1.004343358,
                1.003658397,
                1.002985389,
                1.002324233,
                1.001674831,
                1.001037085,
                1.000410897,
                0.999796173,
                0.999192819,
                0.998600742,
                0.998019851,
                0.997450055,
                0.996891266,
                0.996343396,
                0.995806358,
                0.995280068,
                0.99476444,
                0.994259393,
                0.993764844,
                0.993280711,
                0.992806917,
                0.992343381,
                0.991890026,
                0.991446776,
                0.991013555,
                0.990590289,
                0.990176903,
                0.989773325,
                0.989379484,
                0.988995309,
                0.988620729,
                0.988255677,
                0.987900083,
                0.987553882,
                0.987217006,
                0.98688939,
                0.98657097,
                0.986261682,
                0.985961463,
                0.985670251,
                0.985387985,
                0.985114604,
                0.984850048,
                0.984594259,
                0.984347178,
                0.984108748,
                0.983878911,
                0.983657613,
                0.983444797,
                0.983240409,
                0.983044394,
                0.982856701,
                0.982677276,
                0.982506066,
                0.982343022,
                0.982188091,
                0.982041225,
                0.981902373,
                0.981771487,
                0.981648519,
                0.981533421,
                0.981426146,
                0.981326648,
                0.98123488,
                0.981150798,
                0.981074356,
                0.981005511,
                0.980944219,
                0.980890437,
                0.980844122,
                0.980805232,
                0.980773726,
                0.980749562,
                0.9807327,
                0.9807231,
                0.980720722,
                0.980725528,
                0.980737478,
                0.980756534,
                0.98078266,
                0.980815817,
                0.980855968,
                0.980903079,
                0.980955475,
                0.981017942,
                0.981085714,
                0.981160303,
                0.981241675,
                0.981329796,
                0.981424634,
                0.981526154,
                0.981634325,
                0.981749114,
                0.981870489,
                0.981998419,
                0.982132873,
                0.98227382,
                0.982421229,
                0.982575072,
                0.982735318,
                0.982901937,
                0.983074902,
                0.983254183,
                0.983439752,
                0.983631582,
                0.983829644,
                0.984033912,
                0.984244358,
                0.984460956,
                0.984683681,
                0.984912505,
                0.985147403,
                0.985388349,
                0.98563532,
                0.98588829,
                0.986147234,
                0.986412128,
                0.986682949,
                0.986959673,
                0.987242277,
                0.987530737,
                0.987825031,
                0.988125136,
                0.98843103,
                0.988742691,
                0.989060098,
                0.989383229,
                0.989712063,
                0.990046579,
                0.990386756,
                0.990732574,
                0.991084012,
                0.991441052,
                0.991803672,
                0.992171854,
                0.992545578,
                0.992924825,
                0.993309578,
                0.993699816,
                0.994095522,
                0.994496677,
                0.994903265,
                0.995315266,
                0.995732665,
                0.996155442,
                0.996583582,
                0.997017068,
                0.997455883,
                0.99790001,
                0.998349434,
                0.998804138,
                0.999264107,
                0.999729325,
                1.000199776,
                1.000675446,
                1.001156319,
                1.001642381,
                1.002133617,
                1.002630011,
                1.003131551,
                1.003638222,
                1.00415001,
                1.004666901,
                1.005188881,
                1.005715938,
                1.006248058,
                1.006785227,
                1.007327434,
                1.007874665,
                1.008426907,
                1.008984149,
                1.009546377,
                1.010113581,
                1.010685747,
                1.011262865,
                1.011844922,
                1.012431907,
                1.013023808,
                1.013620615,
                1.014222317,
                1.014828902,
                1.01544036,
                1.016056681,
                1.016677853,
                1.017303866,
                1.017934711,
                1.018570378,
                1.019210855,
                1.019856135,
                1.020506206,
                1.02116106,
                1.021820687,
                1.022485078,
                1.023154224,
                1.023828116,
                1.024506745,
                1.025190103,
                1.02587818,
                1.026570969,
                1.027268461,
                1.027970647,
                1.02867752,
                1.029389072,
                1.030114973,
                1.030826088,
                1.03155163,
                1.032281819,
                1.03301665,
                1.033756114,
                1.034500204,
                1.035248913,
                1.036002235,
                1.036760162,
                1.037522688,
                1.038289806,
                1.039061509,
                1.039837792,
                1.040618648
            };

            long tmp = (long)(1512775 * x + 1072632447);
            int index = (int)(tmp >> 12) & 0xFF;
            return BitConverter.Int64BitsToDouble( tmp << 32 ) * ExpAdjustment[index];
        }

        public static double ExpTaylor( double x )
        {
            // maclaurin expansion for e^x
            // e^x =~ 1 + x + (x^2)/2! + (x^3)/3! + (x^4)/4! + ...

            // taylor series, 7 terms. Accurate in [-2.4..2.4]
            return (362880 + x * (362880 + x * (181440 + x * (60480 + x * (15120 + x * (3024 + x * (504 + x * (72 + x * (9 + x))))))))) * 2.75573192e-6;
        }

        public static double ExpFunky( double x ) // slower but seems to be more accurate than BitConverter exp.
        {
            double s = 1;
            double a = 1;
            double y = 1;
            double x1 = x / 200;

            for( int k = 1; k <= 200; k++ )
            {
                a /= k;
                y *= x1;
                s += a * y;
            }

            s = System.Math.Pow( s, 200 ); // s to 200 power (integer, can be done without Math.Pow).
            return s;
        }

        /// <summary>
        /// Reasonably accurate.
        /// </summary>
        public static double Log( double x )
        {
            // All logarithms are proportional. Having a function approximation for one base, we already know all bases.
            // log10(x) = log2(x) / log2(10)
            // all logarithms have a root at x=1

            const double E = 2.718281828459045235;
            const int ITERATIONS = 20; // 20 default.

            if( x <= 0 )
            {
                return double.NaN;
            }

            // Confine x to a sensible range
            int power_adjust = 0;
            while( x > 1.0 )
            {
                x /= E;
                power_adjust++;
            }
            while( x < .25 )
            {
                x *= E;
                power_adjust--;
            }

            // Now use the Taylor series to calculate the logarithm
            x -= 1.0;
            double t = 0.0, s = 1.0, z = x;
            for( int k = 1; k <= ITERATIONS; k++ )
            {
                t += z * s / k;
                z *= x;
                s = -s;
            }

            // Combine the result with the power_adjust value and return
            return t + power_adjust;
        }

        public static double BellCurve( double x, double standardDeviation, double midpoint )
        {
            // gaussian distribution.

            const double SqrtTwoPI = 2.50662827463; // sqrt(pi*2)

            double squaredExp = (x - midpoint) / standardDeviation;
            squaredExp *= squaredExp;

            return (1 / (standardDeviation * SqrtTwoPI)) * System.Math.Pow( System.Math.E, -0.5 * squaredExp );
        }

        public static float Sqrt( float x )
        {
            if( x < 0 ) // about 10x slower than Math.Sqrt for large numbers.
            {
                return float.NaN;
            }

            // newton-raphson
            // x_n+1 = x_n/2 + x/2x_n
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
            //float current = InitialGuess(x);
            const int ITERATIONS = 10;
            // Newton-Raphson
            for( int i = 0; i < ITERATIONS; i++ )
            {
                current = (current + (x / current)) * 0.5f;
            }
            // Halley's method
            // x_n + 1 = x_n - (2x_n ^ 3 - 3x_n ^ 2 * x)/ (6x_n ^ 2 - 6x_n* x +x ^ 2)
            // Householder's method:
            // x_n + 1 = x_n + 2(x - x_n ^ 2) / (2x_n ^ 2 - x)
            return current;
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


        /// <summary>
        /// The circle constant.
        /// </summary>
        public static double PI = 3.1415926535897932384626433832795028841971693993751;

        /// <summary>
        /// Base of the natural logarithm.
        /// </summary>
        public static double E = 2.7182818284590452353602874713526624977572470936999;

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