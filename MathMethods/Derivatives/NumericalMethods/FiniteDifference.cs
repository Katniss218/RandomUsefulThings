using System;
using System.Collections.Generic;
using System.Text;

namespace RandomUsefulThings.Math.Derivatives.NumericalMethods
{
    public static class FiniteDifference
    {
        // first derivative can be approximated by rise over run of a function.
        // (f(x + delta_x) - f(x)) / delta_x

        /// <summary>
        /// The classic "rise over run" approximation.
        /// </summary>
        /// <param name="f">The function `f(x)` to evaluate.</param>
        /// <param name="x">The value of `x` to evaluate.</param>
        /// <param name="step">The change `Δx` between the 2 inputs to the evaluated function.</param>
        /// <returns>The approximation of the derivative.</returns>
        public static double FirstDerivative_Forward( Func<double, double> f, double x, double step = 1e-7 )
        {
            // Forward Difference = df/dx =~ (f[i + 1] - f[i]) / Δx  
            return (f( x + step ) - f( x )) / step;
        }

        /// <summary>
        /// The classic "rise over run" approximation, but it goes backwards.
        /// </summary>
        /// <param name="f">The function `f(x)` to evaluate.</param>
        /// <param name="x">The value of `x` to evaluate.</param>
        /// <param name="step">The change `Δx` between the 2 inputs to the evaluated function.</param>
        /// <returns>The approximation of the derivative.</returns>
        public static double FirstDerivative_Backwards( Func<double, double> f, double x, double step = 1e-7 )
        {
            // Backwards Difference = df/dx =~ (f[i] - f[i - 1]) / Δx
            return (f( x ) - f( x - step )) / step;
        }

        /// <summary>
        /// The classic "rise over run" approximation, but it goes both backwards and forwards equally.
        /// </summary>
        /// <param name="f">The function `f(x)` to evaluate.</param>
        /// <param name="x">The value of `x` to evaluate.</param>
        /// <param name="step">The change `Δx` between <paramref name="x"/> and the 2 inputs to the evaluated function.</param>
        /// <returns>The approximation of the derivative.</returns>
        public static double FirstDerivative_Central( Func<double, double> f, double x, double step = 1e-7 )
        {
            // Central Difference = df/dx =~ (f[i + 1] - f[i - 1]) / (2*Δx)
            return (f( x + step ) - f( x - step )) / (2 * step);
        }

        // Higher order derivatives can be approximated by:
        // - evaluating a number of inputs around point of interest, then
        // - fitting an order-n polynomial to them, then
        // - computing the polynomial's n-th derivative at the point of interest.
        // Derivatives of polynomials can be computed easily.

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
