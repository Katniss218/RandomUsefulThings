using System;
using System.Collections.Generic;
using System.Text;

namespace RandomUsefulThings.Math.PDE
{
    public static class PDE
    {
        // a PDE


        /*
        ∂u/∂x + sin(x)u = 0
        ∂u/∂x = -sin(x)u
        "change in u for a given change in position is equal to the negative sin of the position multiplied with the valid of u for that position."
        */

        // A PDE has to be discretized before it can be computed.
        // This can be done with several methods, dependong on the problem we are trying to solve.

        /*
        discretizing the spatial derivative using forward difference.
        We are approximating the derivative at point i
        ∂u/∂x ≈ (u[i+1] - u[i]) / Δx
        u is property of the grid because it changes over position - `∂u/∂x`

        first derivative, thus we can use first order scheme to discretize it.
        for 2nd derivative, we use a quadratic instead of a line, and so on.
        */

        /*
        ∂p/∂t + ∂p/∂x = 0
        whatever is in the numerators of the derivatives (p) are properties of the grid.
        whatever is in the denominators (t and x) are axes of the complete grid once we completely and fully discretize it.
        */

        public static class FiniteDifferenceMethod
        {
            // Finite difference Method approximates the integrals of the variables with Finite Difference approximations of those integrals.
            // It is the same as using Finite Difference to approximate a derivative of a function at given input.

            // First order (linear) derivative approximations.

            // -- Where `i` is the grid index and deltaX is the grid spacing (gridPosition = i * deltaX).
            public static double Order1_Forward( Func<double, double> u, int i, double deltaX )
                => Derivatives.NumericalMethods.FiniteDifference.FirstDerivative_Forward( u, i, deltaX );

            public static double Order1_Backward( Func<double, double> u, int i, double deltaX )
                => Derivatives.NumericalMethods.FiniteDifference.FirstDerivative_Backwards( u, i, deltaX );

            public static double Order1_Central( Func<double, double> u, int i, double deltaX )
                => Derivatives.NumericalMethods.FiniteDifference.FirstDerivative_Central( u, i, deltaX );

            // Higher order can be approximated by fitting a higher order polynomial to the curve (by sampling 3 points instead of 2 for a quadratic for example).

            // Converting PDE into ODE using Forward Difference:
            // ∂p/∂t + ∂p/∂x = 0
            // dp[i]/dt + ((p[i + 1] - p[i]) / Δx) =~ 0
            // dp[i]/dt =~ -((p[i + 1] - p[i]) / Δx)
            // One can also discretize it in 2D (both x and time) to get a normal equation to use to calculate the values instead of an ODE.


            /*
            Boundary conditions:

            "boundary condition" determines the values of the first and last however many points.
            The number of points depends on how many surrounding points we sample to calculate the value of the current point.
            So in a way, it's equivalent to telling the solver what values are "out of bounds", and using a larger grid.
            Since in practice, with boundary conditions we can't use the boundary part of the grid.

            Let's say we have a grid of 10 points, with points 0 and 9 being boundary.
            In this case we sample only points 1..8

            This is analogous to having a grid of 8 points, all of which (0..7) are sampled.
            With "ghost" points -1 and 8 depending on the boundary condition.

            You could also switch to forward/backward difference when there is not enough points near the boundary to approximate using the central difference scheme (i.e. point [i-1] or [i+1] falls out of bounds of the grid).

            The properties / dimensions of the grid doesn't depend on the scheme used for discretizing (forward/central/backward). You could technically mix and match them for every grid point.
            */
        }

        public static class FiniteVolumeMethod
        {
            // Finite volume can be used for fluid dynamics (or really any kind of problem that is equivalent to something flowing between "control volumes").

            // it approximates the continuum using a number of control volumes, which are connected to each other to form an arbitrary graph structure.
            // the edges of that graph represent connections through which stuff can flow.
        }
    }
}
