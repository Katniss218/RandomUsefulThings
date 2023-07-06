using System;
using System.Collections.Generic;
using System.Text;

namespace RandomUsefulThings.Math
{
    public static class FourierTransform
    {
        /// <summary>
        /// This computes an in-place complex-to-complex FFT 
        /// x and y are the real and imaginary arrays of 2^m points.
        /// </summary>
        public static void FFT( bool forward, int m, (float X, float Y)[] data )
        {
            // Calculate the number of points
            int n = 1;
            for( int i = 0; i < m; i++ )
            {
                n *= 2; // TODO - this can be done without a loop.
            }

            // Do the bit reversal
            int i2 = n >> 1;
            int j2Acc = 0;
            for( int i = 0; i < n - 1; i++ )
            {
                if( i < j2Acc )
                {
                    float tx = data[i].X;
                    float ty = data[i].Y;
                    data[i].X = data[j2Acc].X;
                    data[i].Y = data[j2Acc].Y;
                    data[j2Acc].X = tx;
                    data[j2Acc].Y = ty;
                }

                int k = i2;
                while( k <= j2Acc )
                {
                    j2Acc -= k;
                    k >>= 1;
                }

                j2Acc += k;
            }

            // Compute the FFT 
            float c1 = -1.0f;
            float c2 = 0.0f;
            int l2Acc = 1;
            for( int l = 0; l < m; l++ )
            {
                int l1 = l2Acc;
                l2Acc <<= 1;
                float u1 = 1.0f;
                float u2 = 0.0f;
                for( int j = 0; j < l1; j++ )
                {
                    for( int i = j; i < n; i += l2Acc )
                    {
                        int i1 = i + l1;
                        float t1 = u1 * data[i1].X - u2 * data[i1].Y;
                        float t2 = u1 * data[i1].Y + u2 * data[i1].X;
                        data[i1].X = data[i].X - t1;
                        data[i1].Y = data[i].Y - t2;
                        data[i].X += t1;
                        data[i].Y += t2;
                    }
                    float z = (u1 * c1) - (u2 * c2);
                    u2 = (u1 * c2) + (u2 * c1);
                    u1 = z;
                }

                if( forward )
                {
                    c2 = -((float)System.Math.Sqrt( (1.0f - c1) / 2.0f ));
                }
                else
                {
                    c2 = (float)System.Math.Sqrt( (1.0f - c1) / 2.0f );
                }

                c1 = (float)System.Math.Sqrt( (1.0f + c1) / 2.0f );
            }

            // Scaling for forward transform 
            if( forward )
            {
                for( int i = 0; i < n; i++ )
                {
                    data[i].X /= n;
                    data[i].Y /= n;
                }
            }
        }
    }
}
