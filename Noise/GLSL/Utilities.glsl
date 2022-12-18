
// outputs average color.
float avg( vec2 vmin, vec2 vmax, float stepSize )
{
    vec2 avg = vec2(0.0);
    vec2 stepCount = floor((vmax - vmin) / stepSize);
    
    avg.x = 0.0;
    for (float x = vmin.x; x <= vmax.x; x += stepSize)
    {
        avg.y = 0.0;
        for (float y = vmin.y; y <= vmax.y; y += stepSize)
        {
            avg.y += _TEST_METHOD_(vec2(x, y));
        }
        avg.y /= stepCount.y;
        avg.x += avg.y;
    }
    
    avg.x /= stepCount.x;
    
    return avg.x;
}


// noise distribution shader.
// https://www.shadertoy.com/view/4ssXRX


// height to normal.

//#define heightMap iChannel0
//#define heightMapResolution iChannelResolution[0]
//#define normalStrength 10.0
//#define textureOffset 1.0
//#define pixelToTexelRatio (iResolution.xy/heightMapResolution.xy)

vec2 stdNormalMap(in vec2 uv) 
{
    float height = texture(heightMap, uv).r;
    return -vec2(dFdx(height), dFdy(height)) * pixelToTexelRatio;
}

vec2 texNormalMap(in vec2 uv)
{
    vec2 s = 1.0/heightMapResolution.xy;
    
    float p = texture(heightMap, uv).x;
    float h1 = texture(heightMap, uv + s * vec2(textureOffset,0)).x;
    float v1 = texture(heightMap, uv + s * vec2(0,textureOffset)).x;
       
   	return (p - vec2(h1, v1));
}