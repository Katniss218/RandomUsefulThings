using System;
using System.Collections.Generic;
using System.Text;

namespace RandomUsefulThings.Math.ODE.NumericalMethods
{
    public class ForestRuthIntegrator
    {
        float p; // position (1D).
        float v; // velocity (1D).
        float t; // time

        [Obsolete("Unconfirmed")]
        public void Integrate( float stepSize, int steps, Func<float, float, float> acceleration )
        {
            float[] c = { 0.6756035959798288f, -0.17560359597982883f, 0.6756035959798288f };

            for( int i = 0; i < steps; i++ )
            {
                for( int j = 0; j < c.Length; j++ )
                {
                    float dt = c[j] * stepSize;
                    p += 0.5f * dt * v;
                    v += dt * acceleration( t, p );
                    p += 0.5f * dt * v;
                    t += dt;
                }
            }
        }
    }
}