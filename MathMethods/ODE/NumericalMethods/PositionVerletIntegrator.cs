using System;
using System.Collections.Generic;
using System.Text;

namespace RandomUsefulThings.Math.ODE.NumericalMethods
{
    public class PositionVerletIntegrator
    {
        float p; // position (1D).
        float pOld; // previous position (1D).
        float t; // time

        [Obsolete( "Unconfirmed" )]
        public void Integrate( float stepSize, int steps, Func<float, float, float> acceleration )
        {
            // Position Verlet integration

            // `acceleration(t, p) is a function that calculates the acceleration acting on the body at a given time and position.
            float a; // acceleration
            p += (stepSize * stepSize) * acceleration( t, p ); // Update position at t + dt

            for( int i = 0; i < steps; i++ )
            {
                a = acceleration( t, p );

                float temp = p;
                p = (2f * p) - pOld + ((stepSize * stepSize) * a); // Update position
                pOld = temp;

                t += stepSize;
            }
        }
    }
}