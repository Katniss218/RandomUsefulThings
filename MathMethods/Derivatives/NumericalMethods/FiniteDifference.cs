using System;
using System.Collections.Generic;
using System.Text;

namespace RandomUsefulThings.Math.Derivatives.NumericalMethods
{
    public static class FiniteDifference
    {
        // first derivative can be approximated by rise over run of a function.

        // (f(x + deltaX) - f(x)) / deltaX

        public static double FirstDerivative_Forward( Func<double, double> f, double x, double step = 1e-7 )
        {
            // The classic "rise over run" approximation - Forward Difference.
            // Forward Difference = df/dx =~ (f[i + 1] - f[i]) / deltaX  
            return (f( x + step ) - f( x )) / step;
        }

        public static double FirstDerivative_Backwards( Func<double, double> f, double x, double step = 1e-7 )
        {
            // Backwards Difference = df/dx =~ (f[i] - f[i - 1]) / deltaX
            return (f( x ) - f( x - step )) / step;
        }

        public static double FirstDerivative_Central( Func<double, double> f, double x, double step = 1e-7 )
        {
            // Central Difference = df/dx =~ (f[i + 1] - f[i - 1]) / (2*deltaX)
            return (f( x + step ) - f( x - step )) / (2 * step);
        }

        [Obsolete("Unconfirmed")]
        public static double SecondDerivative_Central( Func<double, double> f, double x, double step = 1e-7 )
        {
            double fxph = f( x + step );
            double fx = f( x );
            double fxmh = f( x - step );

            return (fxph - 2 * fx + fxmh) / (step * step);
        }
    }
}
