using System;
using System.Collections.Generic;
using System.Text;

namespace RandomUsefulThings.Math.Regression
{
    public static class CurveFitting
    {
        public static (double, double) Linear( IEnumerable<(double x, double y)> points )
        {
            // does seem to work.
            int count = 0;
            double
                sumX = 0,
                sumY = 0,
                sumXY = 0,
                sumXX = 0;

            foreach( var point in points )
            {
                sumX += point.x;
                sumY += point.y;
                sumXY += point.x * point.y;
                sumXX += point.x * point.x;
                count++;
            }

            double meanX = sumX / count;
            double meanY = sumY / count;

            double slope = (sumXY - (count * meanX * meanY)) / (sumXX - (count * meanX * meanX));
            double intercept = meanY - (slope * meanX);

            return (slope, intercept);
        }

        public static double[] Quadratic( List<(double x, double y)> dataPoints )
        {
            // does seem to work too. Coeffs in descending order

            int n = dataPoints.Count;
            double sumX = 0, sumXX = 0, sumXXX = 0, sumXXXX = 0, sumY = 0, sumXY = 0, sumXXY = 0;

            foreach( var (x, y) in dataPoints )
            {
                double xx = x * x;
                double xxx = xx * x;
                double xxxx = xxx * x;

                sumX += x;
                sumXX += xx;
                sumXXX += xxx;
                sumXXXX += xxxx;

                sumY += y;
                sumXY += x * y;
                sumXXY += xx * y;
            }

            // Build the coefficient matrix
            double[,] matrix = { { sumXXXX, sumXXX, sumXX }, { sumXXX, sumXX, sumX }, { sumXX, sumX, n } };

            // Build the constant vector
            double[] vector = { sumXXY, sumXY, sumY };

            // Solve the system of equations
            GaussElimination( matrix, vector );

            return vector;
        }

        public static double[] Polynomial( List<(double x, double y)> dataPoints, int degree )
        {
            // the coeffs are in ascending order

            int n = dataPoints.Count;

            // Build the coefficient matrix
            double[,] matrix = new double[degree + 1, degree + 1];
            double[] vector = new double[degree + 1];

            for( int i = 0; i <= degree; i++ )
            {
                for( int j = 0; j <= degree; j++ )
                {
                    matrix[i, j] = 0;
                    for( int k = 0; k < n; k++ )
                    {
                        matrix[i, j] += System.Math.Pow( dataPoints[k].x, i + j );
                    }
                }

                vector[i] = 0;
                for( int k = 0; k < n; k++ )
                {
                    vector[i] += dataPoints[k].y * System.Math.Pow( dataPoints[k].x, i );
                }
            }

            // Solve the system of equations
            GaussElimination( matrix, vector );

            return vector;
        }

        // Function to perform Gauss elimination for solving a system of linear equations
        static void GaussElimination( double[,] matrix, double[] vector )
        {
            // works.

            int n = vector.Length;

            for( int i = 0; i < n - 1; i++ )
            {
                int maxRow = i;
                double maxVal = matrix[i, i];

                // Find the row with the largest pivot
                for( int k = i + 1; k < n; k++ )
                {
                    if( matrix[k, i] > maxVal )
                    {
                        maxVal = matrix[k, i];
                        maxRow = k;
                    }
                }

                // Swap rows to bring the largest pivot to the current row
                if( maxRow != i )
                {
                    for( int j = i; j < n; j++ )
                    {
                        double temp = matrix[i, j];
                        matrix[i, j] = matrix[maxRow, j];
                        matrix[maxRow, j] = temp;
                    }
                    double temp2 = vector[i];
                    vector[i] = vector[maxRow];
                    vector[maxRow] = temp2;
                }

                // Perform row operations to eliminate variables
                for( int k = i + 1; k < n; k++ )
                {
                    double factor = matrix[k, i] / matrix[i, i];
                    for( int j = i; j < n; j++ )
                    {
                        matrix[k, j] -= factor * matrix[i, j];
                    }
                    vector[k] -= factor * vector[i];
                }
            }

            // Back substitution to find the variable values
            for( int i = n - 1; i >= 0; i-- )
            {
                double sum = 0;
                for( int j = i + 1; j < n; j++ )
                {
                    sum += matrix[i, j] * vector[j];
                }
                vector[i] = (vector[i] - sum) / matrix[i, i];
            }
        }
    }
}