using System;
using System.Collections.Generic;
using System.Text;

namespace RandomUsefulThings.Math.Integrals.NumericalMethods
{
    public static class TrapezoidalRule
    {
        public static double Integrate( Func<double, double> f, double start, double end, int slices )
        {
            // This does some fancy stuff. It's accurate, but I don't know why it works.
            double step = (end - start) / slices; // deltaInput
            double totalHeight = (f( start ) + f( end )) / 2.0;
            // start with the average of start and end points.

            for( int i = 1; i < slices; i++ ) // don't include the first and last point.
            {
                double x = start + i * step;
                totalHeight += f( x ); // Sum up the heights of all the trapezoids (idk why this gives the sum, but it does).
            }
            return step * totalHeight; // convert to area.
        }

        public static double Integrate2( Func<double, double> f, double start, double end, int slices )
        {
            double step = (end - start) / slices; // deltaInput
            double sum = 0.0;

            double oldX = start;
            for( int i = 1; i <= slices; i++ )
            {
                double x = start + i * step;
                double trapezoid = ((f( x ) + f( oldX )) / 2) * step; // Plain old slow explicit trapezoid.
                sum += trapezoid;
                oldX = x;
            }

            return sum;
        }

        // There is also a rectangle method, which can either be forward, central, or backward (kind of analogous to 1st order derivatives, BUT ONLY KIND OF).
        // - Central has half-width slices at the start and end.
        // forward rectangle is left riemann sum
        // backward rectangle is right riemann sum
        // central rectangle is central riemann sum


        // Trapezoid method only has one variant, because its forward/central/backwards would be equivalent, just with different slice count/positions.
    }
}
