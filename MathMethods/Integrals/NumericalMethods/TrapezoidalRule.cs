using System;
using System.Collections.Generic;
using System.Text;

namespace RandomUsefulThings.Math.Integrals.NumericalMethods
{
    public static class TrapezoidalRule
    {
        [Obsolete( "Unconfirmed, it does something kinda fancy to make it faster." )]
        public static double Integrate( Func<double, double> f, double start, double end, int slices )
        {
            double step = (end - start) / slices; // deltaInput
            double sum = (f( start ) + f( end )) / 2.0;

            for( int i = 1; i < slices; i++ )
            {
                double x = start + i * step;
                sum += f( x );
            }
            return step * sum;
        }

        // There is also a rectangle method, which can either be forward, central, or backwards.
        // - Central has half-width slices at the start and end.

        // Trapezoid method only has one variant, because its forward/central/backwards would be equivalent, just with different slice count/positions.
    }
}
