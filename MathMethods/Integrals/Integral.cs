using System;
using System.Collections.Generic;
using System.Text;

namespace RandomUsefulThings.Math.Integrals
{
    public static class Integral
    {
        //
        //
        // There are different types of integrals:

        // - DEFINITE INTEGRAL
        //   - Has start and end points.

        // - INDEFINITE INTEGRAL (a.k.a. ANTIDERIVATIVE)
        //   - Doesn't have start and end points.
        //   - Can't be calculated numerically (because its domain is [-inf..inf]).
        //
        //

        // notation for a single integral
        // ∫ f(x) dx

        // notation for a double integral
        // ∫∫ f(x, y) dx dy
        // - the "inner" integral varies `dx`, the "outer" integral varies `dy`, like a nested for-loop with i and j.

        // The process of computing an integral is called integration.
        // The function f(x) is called the integrand, the points a and b are called the limits (or bounds) of integration.

        // The geometric interpretation of a single integral is the (signed) area under the curve defined by the integrated function.
        // The geometric interpretation of a double integral is the (signed) volume under the surface defined by the integrated function.

        // Integrals can be thought of as continuous sums.



        // Integrals can be thought of functions that have a function (the integrand) as a parameter.


        //
        //  Multi-variable integrals:
        //
        // A single integral can only integrate one variable (the variable of integration).
        //
        // To fully integrate a function that takes 2 parameters, you need a double integral, or triple for 3 parameters, and so on.
        // - Each integral integrating over its own variable.
        //
        //

        // If the function being integrated has asymptotes at any point in the integrated interval, the resulting integral is an improper integral.

        // If I have this identity `∫k dx = kx + C`, I can pick any constant of integration C, and it will hold.

        // Any function that claims to be a true inverse of a derivative would have to solve for the original constant of integration.


        //
        static void dummy()
        {
            float t = 0;
            float Δ = 1e-6f;
            Func<float,float> f = null;

            float acc = 0;
            for( float y = 0; y < t; y += y += Δ )
            {
                for( float x = 0; x < y; x += Δ )
                {
                    acc += f( x ) * Δ * Δ;
                }
            }
        }
    }
}