using System;
using System.Linq;

namespace RandomUsefulThings.Math.DifferentialEquations
{
    // dx/dt = 2 * t
    // this is a differential equation. It describes how the value of one variable changes in respect to other variables.
    // in this case, it's very simple. The change in 'v' over some arbitrary change in 't', is equal to 2 times that change in 't'.

    // *possibly untrue* but a differential equation AFAIK, can only describe how a single variable changes.
    // - To describe the changes in other variables, you need a system of differential equations using those same variables as parameters.

    public class EulerIntegrator // euler integrator for equations with only a single independent variable and nothing else.
    {
        // First order method - halve the step size, halve the error.
        public float v;
        public float t;

        public EulerIntegrator( float v, float t )
        {
            this.v = v;
            this.t = t;
        }

        public static readonly Func<float, float, float> dydt = ( t, v ) =>
        {
            return 2 * t;
        };

        // this seems to work.
        public void Integrate( float stepSize, int steps, Func<float, float, float> equation )
        {
            // equation takes in the right side of the equation and spits out the rate of change of a given variable over dt = 1.0.
            // for dv/dt = 2t       =>  equation = (t,v) => 2 * t;
            // for dv/dt = 2t + a   =>  equation = (t,v,a) => (2 * t) + a;        // needs modified Integrate method with the correct number of parameter in the equation func and a correctly typed 'variable' field.

            for( int i = 0; i < steps; i++ )
            {
                float dv_dt = equation( t, v );
                float dv = dv_dt * stepSize; // multiply both sides by dt (stepSize = dt).

                v += dv;
                t += stepSize;
            }
        }
    }
}