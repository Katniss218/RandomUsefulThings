using System;
using System.Collections.Generic;
using System.Text;

namespace Noise
{
    public static class Voronoi
    {
        /*  HLSL
        
        float hash1( float n ) { return fract(sin(n)*43758.5453); }
        vec2  hash2( vec2  p ) { p = vec2( dot(p,vec2(127.1,311.7)), dot(p,vec2(269.5,183.3)) ); return fract(sin(p)*43758.5453); }

        // The parameter w controls the smoothness
        vec2 voronoi( in vec2 x, float w )
        {
            vec2 n = floor( x );
            vec2 f = x - n;

	        vec2 m = vec2( 8.0, 0.0 );
            for( int j=-2; j<=2; j++ )
            {
                for( int i=-2; i<=2; i++ )
                {
                    vec2 g = vec2( float(i),float(j) );
                    vec2 o = hash2( n + g );

                    // distance to cell		
                    float d = length(g - f + o);

                    // cell color
                    float col = hash1(dot(n + g, vec2(5.9129015,188.73386)));
                    // in linear space

                    // do the smooth min for colors and distances		
                    float h = smoothstep( -1.0, 1.0, (m.x-d)/w );
                    m.x = mix( m.x, d, h ) - h*(1.0-h)*w/(1.0+3.0*w); // distance
                    m.y = mix( m.y, col, h ) - h*(1.0-h)*w/(1.0+3.0*w); // color
                }
            }
	
	        return m;
        }

        // return distance, and cell id
        vec2 voronoi( in vec2 x )
        {
            vec2 n = floor( x );
            vec2 f = fract( x );

	        vec3 m = vec3( 8.0 );
            for( int j=-1; j<=1; j++ )
            {
                for( int i=-1; i<=1; i++ )
                {
                    vec2 gridCell = vec2( float(i), float(j) );
                    vec2 cellId = hash2( n + gridCell );
                    vec2 r = gridCell - f + cellId;
                    float d = dot( r, r );
            
                    if( d < m.x )
                        m = vec3( d, cellId );
                }
            }

            return vec2( sqrt(m.x), (m.y + m.z)/2.f );
        }
        */

        /// <summary>
        /// 
        /// </summary>
        /// <param name="smoothness">[0..1]</param>
        public static (float grayscale, float distance) ValueAt( float x, float y )
        {
            float nx = (float)Math.Floor( x );
            float ny = (float)Math.Floor( y );
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
                    float d = MathMethods.MathMethods.Length( gy - fx + ox, gx - fy - oy );

                    // cell color
                    float value = Hash.Hash1( MathMethods.MathMethods.Dot( ngx, ngy, 5.9129015f, 188.73386f ) ); // the type of hash is important

                    // do the smooth min for colors and distances
                    float h = MathMethods.MathMethods.Smoothstep( -1.0f, 1.0f, (distance - d) / smoothness );
                    distance = MathMethods.MathMethods.Lerp( distance, d, h ) - h * (1.0f - h) * smoothness / (1.0f + 3.0f * smoothness); // distance
                    grayscale = MathMethods.MathMethods.Lerp( grayscale, value, h ) - h * (1.0f - h) * smoothness / (1.0f + 3.0f * smoothness); // color
                }
            }

            return (grayscale, distance);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="smoothness">[0..1]</param>
        public static (float grayscale, float distance) ValueAt( float x, float y, float smoothness )
        {
            float nx = (float)Math.Floor( x );
            float ny = (float)Math.Floor( y );
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
                    float d = MathMethods.MathMethods.Length( gy - fx + ox, gx - fy - oy );

                    // cell color
                    float value = Hash.Hash1( MathMethods.MathMethods.Dot( ngx, ngy, 5.9129015f, 188.73386f ) ); // the type of hash is important

                    // do the smooth min for colors and distances
                    float h = MathMethods.MathMethods.Smoothstep( -1.0f, 1.0f, (distance - d) / smoothness );
                    distance = MathMethods.MathMethods.Lerp( distance, d, h ) - h * (1.0f - h) * smoothness / (1.0f + 3.0f * smoothness); // distance
                    grayscale = MathMethods.MathMethods.Lerp( grayscale, value, h ) - h * (1.0f - h) * smoothness / (1.0f + 3.0f * smoothness); // color
                }
            }

            return (grayscale, distance);
        }
    }
}