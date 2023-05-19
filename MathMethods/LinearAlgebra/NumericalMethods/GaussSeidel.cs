using System;
using System.Collections.Generic;
using System.Text;

namespace RandomUsefulThings.Math.LinearAlgebra.NumericalMethods
{
    // other potentially useful stuff: https://github.com/parvezmrobin/Numerical-Methods/tree/master/Numerical%20Methods

    public static class GaussSeidel
    {
        public static double[] SolveFast( double[,] A, double[] b, double tolerance = 1e-6, int maxIterations = 1000 )
        {
            int n = b.Length;
            double[] x = new double[n];
            double[] prevX = new double[n];
            double[] invDiagonal = new double[n];
            double error = double.MaxValue;
            int iterations = 0;

            for( int i = 0; i < n; i++ )
            {
                invDiagonal[i] = 1.0 / A[i, i];
            }

            while( error > tolerance && iterations < maxIterations )
            {
                for( int i = 0; i < n; i++ )
                {
                    double oldX = x[i];
                    double sum = b[i];

                    for( int j = 0; j < i; j++ )
                    {
                        sum -= A[i, j] * x[j];
                    }

                    for( int j = i + 1; j < n; j++ )
                    {
                        sum -= A[i, j] * x[j];
                    }

                    x[i] = sum * invDiagonal[i];
                    prevX[i] = oldX;
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