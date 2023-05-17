using System;
using System.Collections.Generic;
using System.Text;

namespace RandomUsefulThings.Math.ODE.NumericalMethods
{
    public class MidpointIntegrator
    {
        public float v;
        public float t;

        public MidpointIntegrator( float v, float t )
        {
            this.v = v;
            this.t = t;
        }

        [Obsolete("Unconfirmed")]
        public void Integrate( float stepSize, int steps, Func<float, float, float> equation )
        {
            for( int i = 0; i < steps; i++ )
            {
                float k1 = stepSize * equation( t, v );
                float k2 = stepSize * equation( t + stepSize / 2, v + k1 / 2 );

                v += k2;
                t += stepSize;
            }
        }
    }
}