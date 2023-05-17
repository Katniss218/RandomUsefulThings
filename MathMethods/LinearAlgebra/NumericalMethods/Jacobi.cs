using System;
using System.Collections.Generic;
using System.Text;

namespace RandomUsefulThings.Math.LinearAlgebra.NumericalMethods
{
    public static class Jacobi
    {
        // Seems to work on [ 2, 1 ][ 1 ] equation
        //                  [ 1, 1 ][ 2 ]
        public static double[] Solve2( double[,] A, double[] b, double tolerance = 1e-6, int maxIterations = 1000 )
        {
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
                            sum -= A[i, j] * prevX[j];
                        }
                    }

                    x[i] = sum / A[i, i];
                }

                error = GaussSeidel.CalculateError( x, prevX );
                iterations++;
            }

            if( iterations >= maxIterations )
            {
                throw new Exception( "Jacobi method did not converge within the specified number of iterations." );
            }

            return x;
        }


        /// <summary>
        /// Calculate the row value using the specified x values from xValues
        /// </summary>
        /// <param name="rowIndex">Number of row, where to substitute x with xValues</param>
        /// <param name="xValues">X values to use</param>
        /// <param name="bMatrix">Left part values</param>
        /// <returns>b - ai1*x1 - ai2*x2 - ai3*x3 ...</returns>
        [Obsolete( "untested" )]
        public static double CombineValues( this Matrix M, int rowIndex, Matrix xValues, Matrix bMatrix )
        {
            // https://github.com/3approx4/Numerical-Methods/blob/master/Numerical-Methods.Algorithms/JacobiMethod.cs

            double result = bMatrix[rowIndex, 0];
            for( int i = 0; i < xValues.Rows; i++ )
            {
                if( i != rowIndex )
                    result -= M[rowIndex, i] * xValues[i, 0];
            }
            result /= M[rowIndex, rowIndex];
            return result;
        }

        /// <summary>
        /// Get the maximal distance between the matrices
        /// </summary>
        /// <param name="compareMatrix">Matrix for comparison</param>
        /// <returns>Maximal absolute distance</returns>
        [Obsolete( "untested" )]
        public static double MaxDelta( this Matrix M, Matrix compareMatrix )
        {
            // https://github.com/3approx4/Numerical-Methods/blob/master/Numerical-Methods.Algorithms/JacobiMethod.cs

            if( M.Cols != compareMatrix.Cols || M.Rows != compareMatrix.Rows )
                return float.NaN;

            double maxDelta = 0;
            for( int i = 0; i < M.Rows; i++ )
            {
                double newDelta = System.Math.Abs( M[i, 0] - compareMatrix[i, 0] );
                maxDelta = newDelta > maxDelta ? newDelta : maxDelta;
            }

            return maxDelta;
        }

        [Obsolete( "untested" )]
        public static Matrix Solve( Matrix A, Matrix B, double epsilon, bool checkDominance = false )
        {
            // matrix needs to be diagonally dominant (every diagonal element >= any non-diagonal element)
            // https://github.com/3approx4/Numerical-Methods/blob/master/Numerical-Methods.Algorithms/JacobiMethod.cs

            // Check if the result is reachable during next iterations ( matrix is diagonally dominant )
            if( checkDominance )
            {
                if( !A.IsDiagonallyDominant() )
                {
                    throw new Exception( "Matrix is not diagonally dominant!" );
                }
            }

            double maxDelta;

            Matrix prevX = new Matrix( B.Rows, B.Cols );

            do
            {
                // Result of current iteration
                Matrix currX = new Matrix( B.Rows, B.Cols );
                // Calculate each X value
                for( int i = 0; i < B.Rows; i++ )
                {
                    // Calculate x based on the row values and x values from previous iteration
                    currX[i, 0] = A.CombineValues( i, prevX, B );
                }

                // Calculate current distance between iteration results
                maxDelta = currX.MaxDelta( prevX );

                // Store the current iteration results as the previous iteration results
                prevX = new Matrix( currX ); // fix of having multiple references to single object (not sure if that's needed with Katniss's implementation that uses a struct, original was a class)
            } while( maxDelta > epsilon ); // Iterate until the required precision is reached
            // return the value of the last iteration
            return prevX;
        }
    }
}