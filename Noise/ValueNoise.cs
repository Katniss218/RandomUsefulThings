using MathMethods;
using System;

// GLSL noises
// https://gist.github.com/patriciogonzalezvivo/670c22f3966e662d2f83

namespace Noise
{
    public class ValueNoise
    {
        /* HLSL
        
        float noise( float x, float y, float amp, float freq )
        {    
            x *= freq;
            y *= freq;

            float x0 = floor(x);
            float x1 = x0 + 1.f;
            float y0 = floor(y);
            float y1 = y0 + 1.f;
    
            float v00 = hash(uvec2(x0,y0));
            float v01 = hash(uvec2(x0,y1));
            float v10 = hash(uvec2(x1,y0));
            float v11 = hash(uvec2(x1,y1));
    
    
            float sx = x - x0;
            float sy = y - y0;
            float n0 = mix(v00, v10, sx);
            float n1 = mix(v01, v11, sx);
            float v = mix(n0, n1, sy);

            float result = v * amp;
    
            return result;
        }
        */

        /// <summary>
        /// Returns a single layer of noise at the given coordinates.
        /// </summary>
        public static float ValueAt( float x, float y, float amp, float freq )
        {
            x *= freq;
            y *= freq;

            float x0 = (float)Math.Floor( x );
            float x1 = x0 + 1.0f;
            float y0 = (float)Math.Floor( y );
            float y1 = y0 + 1.0f;

            float v00 = Hash.Get( x0, y0 );
            float v01 = Hash.Get( x0, y1 );
            float v10 = Hash.Get( x1, y0 );
            float v11 = Hash.Get( x1, y1 );


            float sx = x - x0;
            float sy = y - y0;
            float n0 = MathMethods.MathMethods.LerpUnclamped( v00, v10, sx );
            float n1 = MathMethods.MathMethods.LerpUnclamped( v01, v11, sx );
            float v = MathMethods.MathMethods.LerpUnclamped( n0, n1, sy );

            float result = v * amp;

            return result;
        }
    }
}