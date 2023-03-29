using System;
using System.Collections.Generic;
using System.Text;

namespace RandomUsefulThings.Math.Integrals
{
    public static class Integral
    {
        // There are different types of integrals. Definite integral being one.

        // Geometrically, a definite integral represents the signed area under a curve between some start and end points (they can be at infinity).

        // Integral can be thought of as a continuous sum.

        // The function f(x) is called the integrand, the points a and b are called the limits (or bounds) of integration.

        // When the limits are omitted, the integral is called an indefinite integral. It's derivative is the original function.

        // The process of computing an integral is called integration.


        // If the function being integrated as asymptotes (intinite values) at any point in the integrated interval, the resulting integral is an improper integral.

        // A single integral can only integrate one variable (variable of integration).
        // To fully integrate a function that takes multiple parameters, one can take e.g. a double integral (a nested integral)
        // - with one integral integrating one variable, and the ohter integrating the other variable.

        // The geometric interpretation of a double integral is a volume under the surface defined by the integrated function.


        public static double EllipticIntegralOfTheFirstKindFunc( double phi, double k )
        {
            // integral of the function: f(phi,k) = 1 / sqrt(1 - k^2 * sin^2(phi))
            // k - modulus
            // phi - variable of integration.

            double sinPhi = System.Math.Sin( phi );

            return 1.0 / System.Math.Sqrt( 1 - (k * k) * (sinPhi * sinPhi) );
        }
    }
}
