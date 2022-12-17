
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