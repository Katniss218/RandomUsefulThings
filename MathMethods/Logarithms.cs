using System;
using System.Collections.Generic;
using System.Text;

namespace RandomUsefulThings.Math
{
    public static class Logarithms
    {
        /// <summary>
        /// Base of the natural logarithm.
        /// </summary>
        public static double E = 2.7182818284590452353602874713526624977572470936999;

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
            int powerAdjust = 0;
            while( x > 1.0 )
            {
                x /= E;
                powerAdjust++;
            }
            while( x < .25 )
            {
                x *= E;
                powerAdjust--;
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

            // Combine the result with the powerAdjust value and return
            return t + powerAdjust;
        }
    }
}