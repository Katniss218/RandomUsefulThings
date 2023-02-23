using System;
using System.Linq;

namespace DifferentialEquations
{
    // dx/dt = 2 * t
    // this is a differential equation. It describes how the value of one variable changes in respect to other variables.
    // in this case, it's very simple. The change in 'v' over some arbitrary change in 't', is equal to 2 times that change in 't'.

    // *possibly untrue* but a differential equation AFAIK, can only descrime how a single variable changes.
    // - To describe the changes in other variables, you need a system of differential equations using those same variables as parameters.


    public class EulerIntegrator // euler integrator for equations with only a single independent variable and nothing else.
    {
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

    public class EulerIntegratorBetter // euler integrator for systems of differential equations
    {
        public float[] _variables;
        public float t;

        public EulerIntegratorBetter( float[] initialVariables, float t )
        {
            this._variables = initialVariables.ToArray();
            this.t = t;
        }

        public static readonly Func<float, float[], float[]> x = ( t, variables ) =>
        {
            // t, x, y
            // dx/dt = 1t * 2y
            // [0] => dx
            // [1] => dy
            return new float[]
            {
                2 * t + 2 * variables[1],
                0
            };
        };

        public void Integrate( float stepSize, int steps, Func<float, float[], float[]> systemOfEquations )
        {
            // equation takes in the right side of the equation and spits out the rate of change of a given variable over dt = 1.0.
            // for dv/dt = 2t       =>  equation = (t,v) => 2 * t;
            // for dv/dt = 2t + a   =>  equation = (t,v,a) => (2 * t) + a;        // needs modified Integrate method with the correct number of parameter in the equation func and a correctly typed 'variable' field.

            for( int i = 0; i < steps; i++ )
            {
                float[] dvar = systemOfEquations( t, _variables );
                MultiplyInPlace( ref dvar, stepSize ); // multiply both sides by dt (stepSize = dt).

                AddInPlace( ref _variables, dvar );
                t += stepSize;
            }
        }

        private static void MultiplyInPlace( ref float[] vector, float scalar )
        {
            for( int i = 0; i < vector.Length; i++ )
            {
                vector[i] = vector[i] * scalar;
            }
        }

        private static void AddInPlace( ref float[] vector, float[] addend )
        {
            if( vector.Length != addend.Length )
            {
                throw new InvalidOperationException( "Vector and addend must be the same length" );
            }

            for( int i = 0; i < vector.Length; i++ )
            {
                vector[i] = vector[i] + addend[i];
            }
        }
    }

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

    public class RK4IntegratorBetter
    {
        public float[] _variables;
        public float t;

        public RK4IntegratorBetter( float[] initialVariables, float t )
        {
            this._variables = initialVariables.ToArray();
            this.t = t;
        }

        public static readonly Func<float, float[], float[]> x = ( t, variables ) =>
        {
            // t, x, y
            // dx/dt = 1t * 2y
            // [0] => dx
            // [1] => dy
            return new float[]
            {
                2 * t + 2 * variables[1],
                0
            };
        };

        public void Integrate( float stepSize, int steps, Func<float, float[], float[]> systemOfEquations )
        {
            for( int i = 0; i < steps; i++ )
            {
                float dt = stepSize;

                // it calculates the k-coefficients, then it calculates the parameters using the formula `variables[] + k1[] / 2` which it passes to the system of differential equations.

                float[] k1 = systemOfEquations( t, _variables );
                MultiplyInPlace( ref k1, stepSize );

                float[] k1i = Multiply( k1, 0.5f );
                AddInPlace( ref k1i, _variables ); // k1i should == variables[] + k1[] / 2

                float[] k2 = systemOfEquations( t + stepSize / 2, k1i );
                MultiplyInPlace( ref k2, stepSize );

                float[] k2i = Multiply( k2, 0.5f );
                AddInPlace( ref k2i, _variables ); // k2i should == variables[] + k2[] / 2

                float[] k3 = systemOfEquations( t + stepSize / 2, k2i );
                MultiplyInPlace( ref k3, stepSize );

                float[] k3i = Copy( k3 );
                AddInPlace( ref k3i, _variables ); // k3i should == variables[] + k3[]

                float[] k4 = systemOfEquations( t + stepSize, k3i );
                MultiplyInPlace( ref k4, stepSize );

                for( int j = 0; j < _variables.Length; j++ )
                {
                    _variables[j] += (1.0f / 6.0f) * (k1[j] + 2 * k2[j] + 2 * k3[j] + k4[j]); // RK4 formula
                }

                t += dt;
            }
        }

        private static float[] Multiply( float[] vector, float scalar )
        {
            float[] scaledVector = new float[vector.Length];
            for( int i = 0; i < vector.Length; i++ )
            {
                scaledVector[i] = vector[i] * scalar;
            }
            return scaledVector;
        }

        private static float[] Copy( float[] vector )
        {
            float[] scaledVector = new float[vector.Length];
            for( int i = 0; i < vector.Length; i++ )
            {
                scaledVector[i] = vector[i];
            }
            return scaledVector;
        }

        private static void MultiplyInPlace( ref float[] vector, float scalar )
        {
            for( int i = 0; i < vector.Length; i++ )
            {
                vector[i] = vector[i] * scalar;
            }
        }

        private static void AddInPlace( ref float[] vector, float[] addend )
        {
            if( vector.Length != addend.Length )
            {
                throw new InvalidOperationException( "Vector and addend must be the same length" );
            }

            for( int i = 0; i < vector.Length; i++ )
            {
                vector[i] = vector[i] + addend[i];
            }
        }
    }
}
