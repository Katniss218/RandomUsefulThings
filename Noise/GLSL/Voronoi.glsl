#include "Hash.glsl"

// The parameter 'w' controls the smoothness
vec2 voronoi(in vec2 x, float w)
{
    vec2 n = floor(x);
    vec2 f = x - n;

	vec2 m = vec2(8.0, 0.0);
    for( int j = -2; j <= 2; j++ )
    {
        for( int i = -2; i <= 2; i++ )
        {
            vec2 g = vec2( float(i),float(j) );
            vec2 o = hash22( n + g );

            // distance to cell		
            float d = length(g - f + o);

            // cell color
            float col = hash11(dot(n + g, vec2(5.9129015,188.73386)));

            // do the smooth min for colors and distances		
            float h = smoothstep( -1.0, 1.0, (m.x-d)/w );
            m.x = mix( m.x, d, h ) - h*(1.0-h)*w/(1.0+3.0*w); // distance
            m.y = mix( m.y, col, h ) - h*(1.0-h)*w/(1.0+3.0*w); // color
        }
    }
	
	return m;
}

// - ret.x (distance) is [0 to 1]
// - ret.y (average of cellId.x and cellId.y)
// Hash function it expects returns a vec2 cellId in [0 to 1].
vec2 voronoi2d(in vec2 position, in float frequency, in float amplitude)
{
    position *= frequency;
    
    vec2 p00 = floor(position);
    vec2 pOff = fract(position);

	vec2 result = vec2(8.0); // setting this to below 1.0 results in clamping of the maximum value.
    
    // To extend into more dimensions, add an extra for-loop and increase the dimensionality of the hash function.
    for (int j = -1; j <= 1; j++)
    {
        for (int i = -1; i <= 1; i++)
        {
            vec2 gridIndex = vec2(float(i), float(j));
            vec2 cellId = hash22(p00 + gridIndex);
            
            // Get the offset to the influence point in each cell.
            vec2 offsetToPoint = gridIndex - pOff + cellId;
            
            // Use distance squared to avoid square-rooting multiple times.
            // The distance is discarded at most 8 times.
            float distanceSquared = dot(offsetToPoint, offsetToPoint);
            
            if(distanceSquared < result.x)
            {
                result = vec2(distanceSquared, (cellId.x + cellId.y) / 2.0);
            }
        }
    }

    return vec2(sqrt(result.x) * amplitude, result.y);
}