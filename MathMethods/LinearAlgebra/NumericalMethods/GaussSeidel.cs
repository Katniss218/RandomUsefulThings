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
        public static double[] SolveFast( double[,] A, double[] b, double[] initialGuess = null, double tolerance = 1e-6, int maxIterations = 1000 )
        {
            // matrix needs to be diagonally dominant magnitude of every diagonal element is >= magnitude of any other element in the same row.
            // apparently also has to be symmetric (M == transpose(M)) and positive definite (all eigenvalues are positive).

            // it is possible to turn some matrices into a diagonally dominant form by swapping rows or columns.

            int n = b.Length;
            double[] x = new double[n];
            if( initialGuess != null )
            {
                initialGuess.CopyTo( x, 0 );
            }
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

        internal static double CalculateError( in double[] x, in double[] prevX )
        {
            double maxDiff = 0;

            for( int i = 0; i < x.Length; i++ )
            {
                double diff = System.Math.Abs( x[i] - prevX[i] );

                if( diff > maxDiff )
                {
                    maxDiff = diff;
                }
            }

            return maxDiff;
        }

        // Seems to work on [ 2, 1 ][ 1 ] equation
        //                  [ 1, 1 ][ 2 ]
        public static double[] Solve2( double[,] A, double[] b, double tolerance = 1e-6, int maxIterations = 1000 )
        {
            // works, but ~2x slower than Solve, for some reason.
            int n = b.Length;
            double[] x = new double[n];
            double[] prevX = new double[n];
            double error = double.MaxValue;
            int iterations = 0;

            while( error > tolerance && iterations < maxIterations )
            {
                Array.Copy( x, prevX, n ); // used to calc error

                for( int i = 0; i < n; i++ )
                {
                    double sum = b[i];

                    for( int j = 0; j < n; j++ )
                    {
                        if( j != i )
                        {
                            sum -= A[i, j] * x[j];
                        }
                    }

                    x[i] = sum / A[i, i];
                }

                error = CalculateError( x, prevX );
                iterations++;
            }

            if( iterations >= maxIterations )
            {
                throw new Exception( "Gauss-Seidel method did not converge within the specified number of iterations." );
            }

            return x;
        }
    }
}