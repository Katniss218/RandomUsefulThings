using System;
using System.Collections.Generic;
using System.Text;

namespace RandomUsefulThings.ODE
{
    public static class ODE
    {
        // An ODE always has only one output.
        public static Func<double, double, double> f1 = ( t, x ) => -x * t; // dx/dt = -x * t
        public static Func<double, double, double> f2 = ( t, x ) => 5.0 * t; // dx/dt = velocity * t, here velocity is constant 5.0, but it can be an output of a different ODE in the system of ODEs

        // When more than 1 variable is changing, you need a system of ODEs to model them.
        // - example:
        // - - You have a car, the car has 3 variables, position p, velocity v, mass m. The following could be your equations:
        // - - dm/dt = -1 / t       -- change in mass is constant over time. It decreases linearly. WARNING, it will be negative if you run the simulation long enough.
        // - - dv/dt = 1 / m / t    -- change in velocity depends on force (const.) and mass.
        // - - dp/dt = v / t        -- change in position depends on velocity.
        //   It's important to normalize them in respect to the variable t.
        /// <summary>
        /// A representation of a system of ODEs.
        /// </summary>
        public static Func<double, double[], double[]> F = ( t, input ) =>
        {
            return new double[]
            {
                // Note that neither of the variables depends on the position. They absolutely could, they just don't.
                -1 / t,                 // output[0] => dm/dt, input[0] => m
                1 / input[0] / t,       // output[1] => dv/dt, input[1] => v
                input[1] / t            // output[2] => dp/dt, input[2] => p
            };
        };

        // First order ODEs

        // Analytic view:
        // - representation is a function.
        // - solution is an elementary function or a combination of them.
        // Geometric view:
        // - reprsentation is a Direction Field.
        // - solution is an Integral Curve.

        // Direction field is a field of slope values.
        // Integral curve is a curve that goes through the direction field, and at every point its slope is equal to the value of the direction field.
        // - the integral curve DOESN'T pass through every point in the field.
        // - thus, the direction field has to be continuous.
        // - two integral curves can't cross at an angle. They can't touch either.


        // the differential equation can give you the next value in the direction field for the corresponding initial value, at infinitely small step.
        // - it actually gives the change in value, so you multiply by timestep and add that to the initial value.
        // then the value of the field at that new point will be different. Thus the next step will move in the new direction.
        // this is how euler/RK4 approximates the actual curve, as well as why using smaller step gives better results.


        // Symplectic integrators preserve some quantity.
        // They can be advanced forward, and then back and arrive at the same position.
    }
}
