using System;
using System.Collections.Generic;
using System.Text;

namespace RandomUsefulThings.Math.Integrals
{
    public static class Integral
    {
        // There are different types of integrals.
        // - Definite integral = has start and end points.
        // - Indefinite integral = doesn't have start and end points. (related to antiderivative)

        // Geometrically, a definite integral represents the signed area under a curve between some start and end points (they can be at infinity).

        // Integral can be thought of as a continuous sum.

        // The function f(x) is called the integrand, the points a and b are called the limits (or bounds) of integration.

        // The process of computing an integral is called integration.


        // If the function being integrated as asymptotes (intinite values) at any point in the integrated interval, the resulting integral is an improper integral.

        // A single integral can only integrate one variable (variable of integration).
        // To fully integrate a function that takes multiple parameters, one can take e.g. a double integral (a nested integral)
        // - with one integral integrating one variable, and the ohter integrating the other variable.

        // The geometric interpretation of a double integral is a volume under the surface defined by the integrated function.

        // If I have this identity `∫k dx = kx + C`, I can pick any constant of integration C, and it will hold.

        // Any function that claims to be a true inverse of a derivative would have to solve for the original constant of integration.
    }
}
