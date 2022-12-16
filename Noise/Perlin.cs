using System;
using System.Collections.Generic;
using System.Text;

namespace Noise
{
    class Perlin
    {
        /*  GLSL
        
        // Smoothing function to make sure that the lerps blend into each other nicely.
        float fade(float t)
        {
            return t*t*t*(t*(t*6.0 - 15.0) + 10.0);
        }

        float perlin2d(vec2 position, float frequency, float amplitude)
        {
            position *= frequency;
    
            vec2 pOff = fract(position);
    
            vec2 p00 = floor(position);
            vec2 p11 = p00 + vec2(1.0);
    
            vec2 p01 = vec2(p00.x, p11.y);
            vec2 p10 = vec2(p11.x, p00.y);
    
    
            vec2 v00 = random2(p00);
            vec2 v01 = random2(p01);
            vec2 v10 = random2(p10);
            vec2 v11 = random2(p11);
    
            vec2 d00 = p00 - position;
            vec2 d01 = p01 - position;
            vec2 d10 = p10 - position;
            vec2 d11 = p11 - position;
    
    
            float dp00 = dot(d00, v00);
            float dp01 = dot(d01, v01);
            float dp10 = dot(d10, v10);
            float dp11 = dot(d11, v11);
    
    
            float n0 = mix(dp00, dp10, fade(pOff.x));
            float n1 = mix(dp01, dp11, fade(pOff.x));
            float v = mix(n0, n1, fade(pOff.y));
    
            return v * amplitude;
        }
        */
    }
}
