
// https://www.shadertoy.com/view/Xtl3zf
// texture sampler v5.
vec3 textureNoTile( in vec2 position )
{
    float k = texture( NOISE_TEXTURE, 0.005 * position ).x; // cheap (cache friendly) lookup to a noise texture.
    
    float l = k * 8.0;
    float f = fract(l);
    
    //float ia = floor(l); // my method
    //float ib = ia + 1.0;
    
    // suslik: Hi iq. I have improved your approach a little bit by making sure that uv discontinuities are always hidden in samples with 0 weight, so explicit mipmap calculation is not needed: https://www.shadertoy.com/view/WdVGWG
    // This same approach can also be used for other types of interpolation to avoid mipmap seams: https://www.shadertoy.com/view/tsVGRd
    float ia = floor(l + 0.5); // suslik's method (see comments)
    float ib = floor(l);
    f = min(f, 1.0 - f) * 2.0; 
    
    vec2 offa = sin(vec2(3.0, 7.0) * ia); // can replace with any other hash
    vec2 offb = sin(vec2(3.0, 7.0) * ib); // can replace with any other hash

    //vec2 duvdx = dFdx( x );
    //vec2 duvdy = dFdy( x );
    
    //vec3 cola = textureGrad( iChannel0, x + v*offa, duvdx, duvdy ).xyz;
    vec3 cola = texture( TEXTURE, position + offa ).xyz;
    //vec3 colb = textureGrad( iChannel0, x + v*offb, duvdx, duvdy ).xyz;
    vec3 colb = texture( TEXTURE, position + offb ).xyz;
    
    vec3 v = cola - colb;
    return mix( cola, colb, smoothstep(0.2, 0.8, f - 0.1 * (v.x + v.y + v.z)) );
}