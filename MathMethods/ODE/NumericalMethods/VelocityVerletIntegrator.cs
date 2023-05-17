using System;
using System.Collections.Generic;
using System.Text;

namespace RandomUsefulThings.Math.ODE.NumericalMethods
{
    public class VelocityVerletIntegrator
    {
        float p; // position (1D).
        float v; // velocity (1D).
        float t; // time

        [Obsolete( "Unconfirmed" )]
        public void Integrate( float stepSize, int steps, Func<float, float, float> acceleration )
        {
            // Velocity Verlet, a.k.a. Leapfrog

            // `acceleration(t, v) is a function that calculates the acceleration acting on the body at a given time and position.
            float oldV = v;
            v += (0.5f * stepSize) * acceleration( t, p ); // Initialize velocity at t + dt/2

            for( int i = 0; i < steps; i++ )
            {
                float newV = oldV + stepSize * acceleration( t, p );
                oldV = v;
                v = newV;

                t += stepSize;
                p += v;
            }
        }
    }
}