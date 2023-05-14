using System;
using System.Collections.Generic;
using System.Text;

namespace RandomUsefulThings.Math.LinearAlgebra.NumericalMethods
{
    // other potentially useful stuff: https://github.com/parvezmrobin/Numerical-Methods/tree/master/Numerical%20Methods

    public static class GaussSeidel
    {
        // Seems to work on [ 2, 1 ][ 1 ] equation
        //                  [ 1, 1 ][ 2 ]
        public static double[] Solve( double[,] A, double[] b, double[] initialGuess, double tolerance = 0.0001, int maxIterations = 1000 )
        {
            // matrix needs to be diagonally dominant (every diagonal element >= any non-diagonal element)
            // apparently also has to be symmetric (M == transpose(M)) and positive definite (all eigenvalues are positive).

            int n = b.Length;
            double[] x = new double[n];
            initialGuess.CopyTo( x, 0 );
            int i = 0;
            double error = tolerance + 1;

            while( i < maxIterations && error > tolerance )
            {
                error = 0;
                for( int j = 0; j < n; j++ )
                {
                    double sum = 0;
                    for( int k = 0; k < n; k++ )
                    {
                        if( k != j )
                        {
                            sum += A[j, k] * x[k];
                        }
                    }
                    double xiNew = (b[j] - sum) / A[j, j];
                    error += System.Math.Abs( xiNew - x[j] );
                    x[j] = xiNew;
                }
                i++;
            }
            if( error > tolerance )
            {
                throw new Exception( "Failed to converge within maximum iterations." );
            }
            return x;
        }
    }
}