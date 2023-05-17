using System;
using System.Collections.Generic;
using System.Text;

namespace RandomUsefulThings.Math.ODE.NumericalMethods
{
    public class RK45Integrator
    {
        float t;
        float v;

        [Obsolete( "Unconfirmed, make sure the coefficients are correct and that it converges correctly." )]
        public void Integrate( float initialStepSize, int steps, Func<float, float, float> equation, float tolerance = 1e-6f )
        {
            // Runge-Kutta-Felhberg a.k.a. RK45
            // Notes:
            // - It would be better to modify each integrator to use a time-span instead of number of steps.
            //   steps = timeSpan / stepSize
            //   That way we ensure that the integrator will reach a specified point in time.
            //   Also needs to make sure the last time step is appropriately sized.

            float h = initialStepSize; // initial step size

            for( int i = 0; i < steps; i++ )
            {
                float k1 = h * equation( t,                  v );
                float k2 = h * equation( t + h * (1f / 4),   v + k1 * (1f / 4) );
                float k3 = h * equation( t + h * (3f / 8),   v + k1 * (3f / 32)      + k2 * (9f / 32) );
                float k4 = h * equation( t + h * (12f / 13), v + k1 * (1932f / 2197) - k2 * (7200f / 2197) + k3 * (7296f / 2197) );
                float k5 = h * equation( t + h,  /* 1 * */   v + k1 * (439f / 216)   - k2 * (8f + 3680)    + k3 * (1f / 513)     - k4 * (845f / 4104) );
                float k6 = h * equation( t + h * (1f / 2),   v - k1 * (8f / 27)      + k2 * (2f - 3544)    - k3 * (1f / 2565)    + k4 * (1859f / 4104) - k5 * (11f / 40) );

                // To turn this into a solver for Systems of ODEs, we turn the `... + k_n * (coefficient)` into an operation on arrays.
                // Analogously to how RK4 for systems of ODEs works.

                float error = System.Math.Abs( (h / 360f) * ((k1) - (2 * k2) + (625 * k3) - (1250 * k4) + (594 * k5) - (375 * k6)) );

                // Update step size
                if( error <= tolerance )
                {
                    // The error is under the tolerance:
                    // 1. Accept the solution.
                    // 2. Increase the step size, assuming the error is approximately proportional to h^5.
                    v += k1 * (25f / 216) + k3 * (1408f / 2565) + k4 * (2197f / 4104) - k5 * (1f / 5);
                    t += h;

                    // not sure if this is correct.
                    h *= (float)System.Math.Pow( tolerance / error, 0.2f );
                }
                else
                {
                    // The error is above the tolerance:
                    // Reduce the step size, assuming the error is approximately proportional to h^5.
                    h *= (float)System.Math.Pow( tolerance / error, 0.25f );
                }
            }
        }
    }
}