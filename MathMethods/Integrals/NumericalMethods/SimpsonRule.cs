using System;
using System.Collections.Generic;
using System.Text;

namespace RandomUsefulThings.Math.Integrals.NumericalMethods
{
    /// <summary>
    /// Simpson Rule is an extension of the rectangle/trapezoid approximation. Instead of rectangles/trapezoids, it uses quadratics to approximate the curve.
    /// </summary>
    public class SimpsonRule
    {
        /// <summary>
        /// Computes the integral of the function f, along the interval, using the specified number of slices.
        /// </summary>
        /// <param name="start">The start of the integrated interval.</param>
        /// <param name="end">The end of the integrated interval.</param>
        /// <param name="slices">The number of sub-intervals (slices) to use. Must be a multiple of 2.</param>
        /// <returns>An approximation of the definite integral of the function f, in the interval [start..end].</returns>
        [Obsolete( "Unconfirmed, it does something kinda fancy to make it faster." )]
        public static double Integrate( Func<double, double> f, double start, double end, int slices )
        {
            // Simpson rule uses quadratic polynomials as the tops of the vertical slices. Usually more accurate than rectangles/trapezoids.
            if( slices % 2 != 0 )
            {
                throw new ArgumentException( "n must be an even number" );
            }

            double step = (end - start) / slices; // deltaX
            double sum = f( start ) + f( end );

            for( int i = 1; i < slices; i++ )
            {
                double x = start + i * step;
                sum += (i % 2 == 0) ? 2 * f( x ) : 4 * f( x );
            }

            return (step / 3) * sum;
        }
    }
}
