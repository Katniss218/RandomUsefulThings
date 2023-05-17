using System;
using System.Collections.Generic;
using System.Text;

namespace RandomUsefulThings.Math.LinearAlgebra.NumericalMethods
{
    public static class SuccessiveOverRelaxation
    {
        // https://ijmttjournal.org/2018/Volume-56/number-4/IJMTT-V56P531.pdf
        // SOR is a variant of Gauss-Seidel.

        // Seems to work on [ 2, 1 ][ 1 ] equation
        //                  [ 1, 1 ][ 2 ]
        public static double[] Solve2( double[,] A, double[] b, double omega, double tolerance = 1e-6, int maxIterations = 1000 )
        {
            // matrix needs to be diagonally dominant (every diagonal element >= any non-diagonal element)
            // https://stackoverflow.com/questions/11719704/projected-gauss-seidel-for-lcp

            int n = b.Length;
            double[] x = new double[n];
            double[] prevX = new double[n];
            double error = double.MaxValue;
            int iterations = 0;

            while( error > tolerance && iterations < maxIterations )
            {
                Array.Copy( x, prevX, n );

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

                    x[i] = (1 - omega) * prevX[i] + (omega / A[i, i]) * sum;
                }

                error = GaussSeidel.CalculateError( x, prevX );
                iterations++;
            }

            if( iterations >= maxIterations )
            {
                throw new Exception( "SOR method did not converge within the specified number of iterations." );
            }

            return x;
        }

        // Seems to work on [ 2, 1 ][ 1 ] equation
        //                  [ 1, 1 ][ 2 ]
        public static double[] SolveFastIsh( double[,] M, double[] b, double relaxation, double tolerance = 1e-6, int maxIterations = 1000 )
        {
            int problemSize = b.Length;
            double[] x = new double[problemSize];
            double[] prevX = new double[problemSize];
            double error = double.MaxValue;
            int i = 0;

            // Perform SOR iterations until convergence or maxIterations is reached
            while( error > tolerance && i < maxIterations )
            {
                // Save the previous solution vector
                Array.Copy( x, prevX, problemSize );

                // Perform one SOR iteration
                for( int row = 0; row < problemSize; row++ )
                {
                    double sigma = 0.0;
                    for( int col = 0; col < problemSize; col++ )
                    {
                        if( col != row )
                        {
                            sigma += M[row, col] * x[col];
                        }
                    }
                    x[row] = (1 - relaxation) * prevX[row] + (relaxation / M[row, row]) * (b[row] - sigma);
                }

                // Compute the error
                error = 0.0;
                for( int index = 0; index < problemSize; index++ )
                {
                    double absError = System.Math.Abs( x[index] - prevX[index] );
                    if( absError > error )
                    {
                        error = absError;
                    }
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