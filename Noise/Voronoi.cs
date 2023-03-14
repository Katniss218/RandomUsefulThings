using RandomUsefulThings.Math;
using System;
using System.Collections.Generic;
using System.Text;

namespace Noise
{
    public static class Voronoi
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="smoothness">[0..1]</param>
        public static (float grayscale, float distance) ValueAt( float x, float y, float smoothness )
        {
            float nx = (float)System.Math.Floor( x );
            float ny = (float)System.Math.Floor( y );
            float fx = x - nx;
            float fy = y = ny;

            float grayscale = 0.0f;
            float distance = 8.0f;
            for( int gx = -2; gx <= 2; gx++ )
            {
                for( int gy = -2; gy <= 2; gy++ )
                {
                    float ngx = nx + gx;
                    float ngy = ny + gy;
                    float ox = Hash.Hash2( ngx, ngy ).x;
                    float oy = Hash.Hash2( ngx, ngy ).y;

                    // distance to cell		
                    float d = MathMethods.Length( gy - fx + ox, gx - fy - oy );

                    // cell color
                    float value = Hash.Hash1( MathMethods.Dot( ngx, ngy, 5.9129015f, 188.73386f ) ); // the type of hash is important

                    // do the smooth min for colors and distances
                    float h = Easing.Smoothstep( -1.0f, 1.0f, (distance - d) / smoothness );
                    distance = Interpolation.Lerp( distance, d, h ) - h * (1.0f - h) * smoothness / (1.0f + 3.0f * smoothness); // distance
                    grayscale = Interpolation.Lerp( grayscale, value, h ) - h * (1.0f - h) * smoothness / (1.0f + 3.0f * smoothness); // color
                }
            }

            return (grayscale, distance);
        }
    }
}