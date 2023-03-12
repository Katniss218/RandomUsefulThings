using System;
using System.Collections.Generic;
using System.Text;

namespace RandomUsefulThings.Math.LinearAlgebra.NumericalMethods
{
    public static class GaussSeidel
    {
        [Obsolete( "Unconfirmed" )]
        public static double[] Run( double[][] A, double[] b, double[] x0, int maxIterations, double tolerance )
        {
            // A is a two-dimensional array representing the coefficients of the linear system (matrix form)
            // b is a one-dimensional array representing the constants (vector of constants i.e. the right side)
            // x0 is the initial guess for the solution
            // maxIterations is the maximum number of iterations to perform
            // tolerance is the convergence criterion
            // The method returns an array x representing the solution to the linear system.

            int n = b.Length;
            double[] x = new double[n];
            x0.CopyTo( x, 0 );

            for( int k = 0; k < maxIterations; k++ )
            {
                double maxError = 0.0;
                for( int i = 0; i < n; i++ )
                {
                    double sigma = 0.0;
                    for( int j = 0; j < n; j++ )
                    {
                        if( j != i )
                        {
                            sigma += A[i][j] * x[j];
                        }
                    }
                    double xi = (b[i] - sigma) / A[i][i];
                    double error = System.Math.Abs( xi - x[i] );
                    if( error > maxError )
                    {
                        maxError = error;
                    }
                    x[i] = xi;
                }
                if( maxError < tolerance )
                {
                    return x;
                }
            }

            throw new Exception( "Gauss-Seidel failed to converge" );
        }
    }
}
