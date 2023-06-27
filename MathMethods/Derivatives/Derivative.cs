using System;
using System.Collections.Generic;
using System.Text;

namespace RandomUsefulThings.Math.Derivatives
{
    class Derivative
    {
        // A derivative of a function is the rate of change of that function, as its input value changes.
        // The 2nd derivative is equal to the derivative of the derivative of the function.

        // function: position
        // 1st derivative is: velocity
        // 2nd derivative is: acceleration
        // 3rd derivative is: jerk a.k.a. jolt
        // ...


        // f'/f is called a logarithmic derivative, and is the derivative of a logarithm of a function, but defined for both positive and negative numbers (but not 0).


        public class LagrangeNotation
        {
            // f'(x) is the 1st derivative of the function f(x)
            // f''(x) is the 2nd derivative of the function f(x)
            // and so on.
        }

        public class LeibnizNotation
        {
            // The `d` in `dx` represents an infinitely small change. Analogous to how Δ (delta) represents a finite change.

            // `dx/dt` is the derivative of `x` with respect to `t`.
            // - It can also be thought of as "change in `x` for a given change in `t`".

            // This is useful because it directly shows that the 1st derivative can be approximated numerically using the formula `Δd/Δt`, for sufficiently small values of t.
            // - The `Δt` can be thought of as "normalizing" the `dx`, making it independent of any specific `Δt` that was used to compute it.
            // - To get the `Δx` for a specific `Δt`, we need to multiply it with some value of `Δt`, depending on e.g. timestep.
            ///- Using `Δd/Δt` to approximate the value of a derivative has a name - see <see cref="NumericalMethods.FiniteDifference"/>

            // Higher order:
            // dx/dt = f'(x)
            // d^2x/dt^2 = f''(x)
            // d^nx/dt^n = f'_n(x)   -- number of ' prime symbols equal to n
        }
    }
}