using System;
using System.Collections.Generic;
using System.Text;

namespace RandomUsefulThings.Math.Integrals.NumericalMethods
{
    public class SimpsonRule
    {
        /// <summary>
        /// Computes the integral of the function f, along the interval, using the specified number of slices.
        /// </summary>
        /// <param name="start">The start of the integrated interval.</param>
        /// <param name="end">The end of the integrated interval.</param>
        /// <param name="slices">The number of sub-intervals (slices) to use. Must be a multiple of 2.</param>
        /// <returns></returns>
        [Obsolete( "unconfirmed" )]
        public static float Integrate( Func<float, float> f, float start, float end, int slices )
        {
            // Simpson rule uses quadratic polynomials as the tops of the vertical slices. Usually more accurate than rectangles/trapezoids.
            if( slices % 2 != 0 )
            {
                throw new ArgumentException( "n must be an even number" );
            }

            float h = (end - start) / slices;
            float sum = f( start ) + f( end );

            for( int i = 1; i < slices; i++ )
            {
                float x = start + i * h;
                sum += (i % 2 == 0) ? 2 * f( x ) : 4 * f( x );
            }

            return (h / 3) * sum;
        }
    }
}
