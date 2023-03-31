using RandomUsefulThings.Math.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Text;

namespace RandomUsefulThings.Misc.Graphics
{
    public static class SobelFilter
    {
        [Obsolete("Unconfirmed")]
        public static Matrix Apply( Matrix inputImage )
        {
            // Applies a sobel (edge detect) filter.
            Matrix Kx = new Matrix( new double[,]
                { {-1, 0, 1},
                  {-2, 0, 2},
                  {-1, 0, 1} } );

            Matrix Ky = new Matrix( new double[,]
                { {-1,-2,-1},
                  { 0, 0, 0},
                  { 1, 2, 1} } );

            Matrix outputImage = new Matrix( inputImage.Rows, inputImage.Cols );

            // Convolve the input image with the Kx and Ky kernels
            Matrix gx = Matrix.Convolve( inputImage, Kx );
            Matrix gy = Matrix.Convolve( inputImage, Ky );

            // Compute the magnitude of the gradient at each pixel
            for( int i = 0; i < inputImage.Rows; i++ )
            {
                for( int j = 0; j < inputImage.Cols; j++ )
                {
                    double gxVal = gx[i, j];
                    double gyVal = gy[i, j];

                    double gradientMagnitude = System.Math.Sqrt( gxVal * gxVal + gyVal * gyVal );

                    outputImage[i, j] = gradientMagnitude;
                }
            }

            return outputImage;
        }
    }
}