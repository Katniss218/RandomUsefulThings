using DifferentialEquations;
using Geometry;
using MathMethods;
using System;
using static MathMethods.Thermodynamics;

namespace TestConsole
{
    class Program
    {
        static void Main( string[] args )
        {
            double tempK = Physics.Radioactivity.RadioactiveHeating.CalculatePu238Temperature( 1.0f );
            Miscellaneous.Graphics.ColorRGBA c = new Miscellaneous.Graphics.ColorRGBA( 0, 1, 0 );
            int vint = c.GetIntRGB();

            c = Miscellaneous.Graphics.ColorRGBA.FromIntRGB( vint );

            // 1 is when it approaches 1,0,0 again and wraps around to 0.

            Console.WriteLine( MathMethods.MathMethods.Factorial( 1 ) );
            Console.WriteLine( MathMethods.MathMethods.Factorial( 2 ) );
            Console.WriteLine( MathMethods.MathMethods.Factorial( 3 ) );
            Console.WriteLine( MathMethods.MathMethods.Factorial( 4 ) );
            Console.WriteLine( MathMethods.MathMethods.Factorial( 5 ) );
            Console.WriteLine( MathMethods.MathMethods.Factorial( 6 ) );

            Quaternion q = Quaternion.FromEulerAngles( -1.4f, 0.5f, 1.6f );

            Quaternion qi = q.Inverse();
            Quaternion qb = qi.Inverse();

            Vector3 euler = q.ToEulerAngles();

            // dv/dt = 2t
            // integrator lets us use both t and v for computing the value of dv/dt, even tho we don't need v in our made up equation.
            Func<float, float, float> eq = ( t, v ) => 2 * t; // eq for delta x

            EulerIntegrator e = new EulerIntegrator( 5.0f, 0.0f );

            e.Integrate( 0.01f, 200, eq );

            RK4Integrator rk4 = new RK4Integrator( 5.0f, 0.0f );

            rk4.Integrate( 0.5f, 4, eq );

            // some stuff for testing.

            // Euler
            float x1 = 5;
            float y1 = 5;
            float t1 = 0;

            // range of t = 2

            Func<float, float, float, float> dxdtEqualsSomething = ( t, x, y ) =>
            {
                return 2 * t + 2 * y;
            };

            float stepSize1 = 0.01f;
            int steps1 = 200;
            for( int i = 0; i < steps1; i++ )
            {
                // dx/dt = 2t + 2y;

                float dt = stepSize1;
                float dxdt = dxdtEqualsSomething( t1, x1, y1 );
                float dx = dxdt * dt;
                x1 += dx;
                //y1 ; doesn't change since we don't have a 2nd differential equation in the system of differential equations that can change it.
                t1 += dt;
            }


            Func<float, float[], float[]> eqSystem = ( t, variables ) =>
            {
                // x, y
                // dx/dt = 2t * 2y
                // [0] => dx
                // [1] => dy
                return new float[]
                {
                    2 * t + 2 * variables[1],
                    0
                };
            };

            EulerIntegratorBetter eb = new EulerIntegratorBetter( new[] { 5.0f, 5.0f }, 0.0f );

            eb.Integrate( 0.01f, 200, eqSystem );

            // RK4
            float x2 = 5;
            float y2 = 5;
            float t2 = 0;

            float stepSize2 = 0.5f;
            int steps2 = 4;
            for( int i = 0; i < steps2; i++ )
            {
                // dv/dt = 2t;
                float dt = stepSize2;

                float k1 = stepSize2 * dxdtEqualsSomething( t2, x2, y2 );
                float k2 = stepSize2 * dxdtEqualsSomething( t2 + stepSize2 / 2, x2 + k1 / 2, y2 ); // Seems that it's extended by passing the unchanging (constant) variables "raw". I guess it makes sense.
                float k3 = stepSize2 * dxdtEqualsSomething( t2 + stepSize2 / 2, x2 + k2 / 2, y2 );
                float k4 = stepSize2 * dxdtEqualsSomething( t2 + stepSize2, x2 + k3, y2 );

                float dv = (1.0f / 6.0f) * (k1 + 2 * k2 + 2 * k3 + k4); // RK4 formula

                x2 += dv;
                //y2 ; doesn't change since we don't have a 2nd differential equation in the system of differential equations that can change it.
                t2 += dt;
            }

            RK4IntegratorBetter rk4b = new RK4IntegratorBetter( new[] { 5.0f, 5.0f }, 0.0f );

            rk4b.Integrate( 0.5f, 4, eqSystem );



            Func<float, float[], float[]> eqSystem2 = ( t, variables ) =>
            {
                // x, y
                // dx/dt = 2t * 2y
                // [0] => dx
                // [1] => dy
                return new float[]
                {
                    2 * t + 2 * variables[1],
                    t * variables[0]
                };
            };


            eb = new EulerIntegratorBetter( new[] { 5.0f, 5.0f }, 0.0f );
            eb.Integrate( 0.001f, 2000, eqSystem2 );

            rk4b = new RK4IntegratorBetter( new[] { 5.0f, 5.0f }, 0.0f );
            rk4b.Integrate( 0.02f, 100, eqSystem2 );

            const float gamma = 1.4f;

            Func<float, float[], float[]> nozzleEquations = ( x, vars ) =>
            {
                float rho = vars[0];
                float v = vars[1];
                float p = vars[2];

                float dvdx = (p / rho) - (1 / rho) * nozzleProfile( x, v );
                float drhodx = -rho * nozzleProfile( x, dvdx );
                float dpdx = -gamma * p * nozzleProfile( x, v );

                return new float[] { drhodx, dvdx, dpdx };
            };

            rk4b = new RK4IntegratorBetter( new[] { 1.0f, 100.0f, 10000.0f }, 0.0f );
            rk4b.Integrate( 0.2f, 20, nozzleEquations );

        }

        public static float nozzleProfile( float x, float v )
        {
            if( x < 0.5f )
            {
                return 2.0f;
            }
            else if( x < 1.5f )
            {
                return 2.0f - 0.5f * (x - 0.5f);
            }
            else
            {
                return 1.5f;
            }
        }
    }
}
