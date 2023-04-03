using RandomUsefulThings.Math.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Text;

namespace RandomUsefulThings.Misc.Graphics
{
    public static class HighpassFilter
    {
       /* [Obsolete( "Unconfirmed" )]
        public static Matrix Apply( Matrix input, double radius )
        {
            int kernelSize = (int)System.Math.Round( radius * 3.0 ) * 2 + 1;

            Matrix kernel = new Matrix( NormalizeKernel( CreateGaussianKernel( kernelSize, radius )) );

            Matrix blurred = Matrix.Convolve( input, kernel );

            // Lowpass should be without the subtraction (just blur) I think.
            Matrix output = Matrix.SubtractElementwise( input, blurred );

            return output;
        }*/

        [Obsolete( "Unconfirmed" )]
        // Helper method to create a Gaussian kernel with the specified kernel size and standard deviation
        private static double[,] CreateGaussianKernel( int kernelSize, double sigma )
        {
            double[,] kernel = new double[kernelSize, kernelSize];

            int center = kernelSize / 2;

            // Compute the kernel weights based on the Gaussian distribution
            for( int i = 0; i < kernelSize; i++ )
            {
                for( int j = 0; j < kernelSize; j++ )
                {
                    double x = i - center;
                    double y = j - center;
                    kernel[i, j] = System.Math.Exp( -(x * x + y * y) / (2 * sigma * sigma) );
                }
            }

            return kernel;
        }

        [Obsolete( "Unconfirmed" )]
        // Helper method to normalize a kernel so that all weights sum to 1
        private static double[,] NormalizeKernel( double[,] kernel )
        {
            // Compute the sum of all kernel weights
            double sum = 0.0;
            for( int i = 0; i < kernel.GetLength( 0 ); i++ )
            {
                for( int j = 0; j < kernel.GetLength( 1 ); j++ )
                {
                    sum += kernel[i, j];
                }
            }

            // Normalize the kernel weights by dividing each weight by the sum
            for( int i = 0; i < kernel.GetLength( 0 ); i++ )
            {
                for( int j = 0; j < kernel.GetLength( 1 ); j++ )
                {
                    kernel[i, j] /= sum;
                }
            }

            return kernel;
        }
    }
}
