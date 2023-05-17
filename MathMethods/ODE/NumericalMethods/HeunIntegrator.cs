using System;
using System.Collections.Generic;
using System.Text;

namespace RandomUsefulThings.Math.ODE.NumericalMethods
{
    public class HeunIntegrator
    {
        // a.k.a. Improved/Modified Euler, a.k.a. Explicit Trapezoidal Rule, a.k.a. RK2.

        public float v;
        public float t;

        public HeunIntegrator( float v, float t )
        {
            this.v = v;
            this.t = t;
        }

        public void Integrate( float stepSize, int steps, Func<float, float, float> equation )
        {
            for( int i = 0; i < steps; i++ )
            {
                float k1 = stepSize * equation( t, v );
                float k2 = stepSize * equation( t + stepSize, v + k1 );

                v += (k1 + k2) / 2; // (0.5f * k1) + (0.5f * k2)
                t += stepSize;
            }
        }

        public void IntegrateRalston( float stepSize, int steps, Func<float, float, float> equation )
        {
            for( int i = 0; i < steps; i++ )
            {
                float k1 = stepSize * equation( t, v );
                float k2 = stepSize * equation( t + (2f / 3) * stepSize, v + (2f / 3) * k1 );

                v += (0.25f * k1) + (0.75f * k2);
                t += stepSize;
            }
        }
    }
}