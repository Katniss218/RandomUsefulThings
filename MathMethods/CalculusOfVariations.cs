using System;
using System.Collections.Generic;
using System.Text;

namespace RandomUsefulThings.Math
{
    class CalculusOfVariations
    {
        // Variational Calculus (Calculus of Variations) is a technique for coming up with differential equations.
        // It transforms a problem written in some way and spits out a differential equation.

        // hmm, an (ODE + an initial value) is a way to define a function.
        // the initial value here is known because it is the boundary condition, I think.

        // it finds a differential equation that minimizes the integral of the function defined by the differential equation?
        // in other words, it finds a function, that has the lowest integral?
        // something like that

        // it finds minima/maxima of a functional. A functional is a function that takes in a function as a parameter.
        // A functional maps *functions* to scalars
        // so you give it a function and it returns a number (its integral?) well it's some property of the function. an integral is indeed a property of a function.
        // whereas a function maps scalars to scalars, or vectors to scalars, etc.



        // take a change in the functional, which is by adding another function of the same variable
        // analogous of taking the change in a function by adding some value to the input and checking how much the output changes.
        // so we add a function (scaled by the degree of variation `S`) to the unknown minimized function, and see how much that changes. (how much the integral changes?)

        // what is the added function though?
        // - the value at `x` = endpoint must be 0


        // differentiate with respect to the constant (degree of variation S), apparently. Because when the variation from optimum goes to 0, we get the optimum.
        // lagrangian is equal to the integrand of the functional


        // f^ (f-hat) is often a notation for a "minimizer of a function f".
    }
}
