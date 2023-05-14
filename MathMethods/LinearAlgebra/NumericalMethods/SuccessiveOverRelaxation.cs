using System;
using System.Collections.Generic;
using System.Text;

namespace RandomUsefulThings.Math.LinearAlgebra.NumericalMethods
{
    public static class SuccessiveOverRelaxation
    {
        [Obsolete( "Unconfirmed, seems buggy??? I'm not sure" )]
        public static double[] Solve( double[,] M, double[] b, double relaxation, int maxIterations = 100 )
        {
            // matrix needs to be diagonally dominant (every diagonal element >= any non-diagonal element)
            // https://stackoverflow.com/questions/11719704/projected-gauss-seidel-for-lcp

            // Validation omitted
            var x = b;
            double delta;

            // Gauss-Seidel with Successive OverRelaxation Solver
            for( int k = 0; k < maxIterations; ++k )
            {
                for( int i = 0; i < b.Length; ++i )
                {
                    delta = 0.0f;

                    for( int j = 0; j < i; ++j )
                        delta += M[i, j] * x[j];
                    for( int j = i + 1; j < b.Length; ++j )
                        delta += M[i, j] * x[j];

                    delta = (b[i] - delta) / M[i, i];
                    x[i] += relaxation * (delta - x[i]);
                }
            }

            return x;
        }

        [Obsolete( "Unconfirmed, untested" )]
        public static double[] Solve2( double[,] M, double[] b, double relaxation, double tolerance = 0.0001, int maxIterations = 100 )
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