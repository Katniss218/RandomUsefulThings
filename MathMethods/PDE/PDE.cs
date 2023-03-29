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

        discretizing the spatial derivative using forward difference.
        We are approximating the derivative at point i
        ∂u/∂x ≈ (u[i+1] - u[i]) / Δx
        u is property of the grid because it changes over position ∂u/∂x
        first derivative, thus we can use first order scheme.
        for 2nd derivative, we use a quadratic instead of a line.
        */

        // A PDE has to be discretized before it can be computed.
        // This can be done with several methods, dependong on the problem we are trying to solve.

        /*
        ∂p/∂t + ∂p/∂x = 0
        whatever is in the numerators of the derivatives (p) are properties of the grid.
        whatever is in the denominators (t and x) are axes of the complete grid once we completely and fully discretize it.
        */

        public static class FiniteDifferenceMethod
        {
            // Finite difference method approximates the integrals of the variables with finite difference approximations.

            // First order (linear) derivative approximations.

            // Forward Difference = du/dx =~ (u[i + 1] - u[i]) / deltaX  // -- Where `i` is the grid index and deltaX is the grid spacing (gridPosition = i * deltaX).
            public static float Linear_Forward( Func<float, float> u, int i, float deltaX )
                => (u( i + 1 ) - u( i )) / deltaX;

            // Backward Difference = du/dx =~ (u[i] - u[i - 1]) / deltaX
            public static float Linear_Backward( Func<float, float> u, int i, float deltaX )
                => (u( i ) - u( i - 1 )) / deltaX;

            // Central Difference = du/dx =~ (u[i + 1] - u[i - 1]) / 2*deltaX
            public static float Linear_Central( Func<float, float> u, int i, float deltaX )
                => (u( i + 1 ) - u( i - 1 )) / 2 * deltaX;

            // Higher order can be approximated by fitting a higher order polynomial to the curve (by sampling 3 points instead of 2 for a quadratic for example).

            // Converting PDE into ODE using Forward Difference:
            // ∂p/∂t + ∂p/∂x = 0
            // dp[i]/dt + ((p[i + 1] - p[i]) / Δx) =~ 0
            // dp[i]/dt =~ -((p[i + 1] - p[i]) / Δx)
            // One can also discretize it in 2D (both x and time) to get a normal equation to use to calculate the values instead of an ODE.
        }

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
}
