using System;
using System.Linq;

namespace RandomUsefulThings.Math.DifferentialEquations
{
    public class RK4Integrator // runge-kutta integrator
    {
        public float v;
        public float t;

        public RK4Integrator( float v, float t )
        {
            this.v = v;
            this.t = t;
        }

        public static readonly Func<float, float, float> dydt = ( t, v ) =>
        {
            return 2 * t;
        };


        public void Integrate( float stepSize, int steps, Func<float, float, float> equation ) // independent variable first, then the rest.
        {
            // this thing works too.
            for( int i = 0; i < steps; i++ )
            {
                float k1 = stepSize * equation( t, v );
                float k2 = stepSize * equation( t + stepSize / 2, v + k1 / 2 ); // If you want to pass more variables, you can just pass them as they are (i.e equation( t + stepSize / 2, x + k1 / 2, y, z, w )). The equation gives the derivative for the 2nd parameter
                float k3 = stepSize * equation( t + stepSize / 2, v + k2 / 2 );
                float k4 = stepSize * equation( t + stepSize, v + k3 );

                float dv = (1.0f / 6.0f) * (k1 + 2 * k2 + 2 * k3 + k4); // RK4 formula

                v += dv;
                t += stepSize;
            }
        }
    }
}