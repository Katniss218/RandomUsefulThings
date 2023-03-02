using System;
using System.Linq;

namespace DifferentialEquations
{
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
}
