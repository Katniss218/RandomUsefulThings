#include "Hash.glsl"

float noise2d(vec2 position, float amp, float freq)
{
    position *= freq;
    
    vec2 pOff = fract(position);
    
    // Grid positions.
    vec2 p00 = floor(position);
    vec2 p11 = p00 + vec2(1.0);
    vec2 p01 = vec2(p00.x, p11.y);
    vec2 p10 = vec2(p11.x, p00.y);
    
    // Random values at each grid point.
    float v00 = hash12(p00);
    float v01 = hash12(p01);
    float v10 = hash12(p10);
    float v11 = hash12(p11);
    
    // Mix the random values together.
    float n0 = mix(v00, v10, pOff.x);
    float n1 = mix(v01, v11, pOff.x);
    float v = mix(n0, n1, pOff.y);
        
    return v * amp;
}