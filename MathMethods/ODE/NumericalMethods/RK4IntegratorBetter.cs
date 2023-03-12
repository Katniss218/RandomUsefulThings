using System;
using System.Linq;

namespace RandomUsefulThings.Math.DifferentialEquations
{
    // RK2
    // calculate the slope at `current` time.
    // calculate slope at the `current + timestep` time.
    // average the 2 slopes. This is the new slope.

    // number of evaluations of the slope = the order of the integrator.


    public class RK4IntegratorBetter
    {

        // Fourth order method - half the step size, 1/16th the error.
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
