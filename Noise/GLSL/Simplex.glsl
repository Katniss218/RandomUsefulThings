/*
		
// Skew constants for n-dimensional simplex noise:
// https://www.youtube.com/watch?v=3LpH9uNQzQ0

Fn = (sqrt(n + 1) - 1) / n
Gn = 1 - (1 / sqrt(n + 1)) / n


sn = p + (p.x + p.y + p.z + ...) * Fn
// sn is used to calculate which skewed unit hypercube cell the input position lies in, and its internal coordinates.
*/

#include "Hash.glsl"

// skew constants for 2d simplex functions
const float F2 = 0.366025404; // (sqrt(3) - 1) / 2;
const float G2 = 0.211324865; // (3 - sqrt(3)) / 6;
    
float simplex2d( in vec2 position )
{
    // 1. find current triangle T and it's three vertices
    // s, s+i1, s+1.0 - absolute skewed (integer) coordinates of T vertices
    // x1, x2, x3 - unskewed coordinates of p relative to each of T vertices

    // calculate s and x
	vec2 s = floor(position + dot(position, vec2(F2))); //- generalizes to higher dimensions
                                                        //vec2 s = floor( position + (position.x + position.y) * F2 ); -- this is interchangeable.
    vec2 x1 = position - s + dot(s, vec2(G2));          //- generalizes to higher dimensions
                                                        //vec2 x1 = position - s + (s.x + s.y) * G2;
    
    // calculate i1
    float e = step(x1.y, x1.x);
    
    vec2 i1 = vec2(e, 1.0 - e);
    
    // x2, x3 kernel summation - contribution from each vertex.
    // unskew the coordinate...
    vec2 x2 = x1 - i1 + G2;
	vec2 x3 = x1 - 1.0 + 2.0 * G2;
    
	// calculate surflet weights
    vec3 w, d;
    w.x = dot(x1, x1);
    w.y = dot(x2, x2);
    w.z = dot(x3, x3);
    
	// w fades from 0.6 at the center of the surflet to 0.0 at the margin. d^2 = 0.5 ensures no discontinuities, d^2 = 0.6 increases visual quality where doscontinuities are not noticeable.
    w = max( 0.6 - w, 0.0 );
    
    // calculate surflet components
    d.x = dot(x1, random2(s + 0.0));
    d.y = dot(x2, random2(s + i1));
    d.z = dot(x3, random2(s + 1.0));
    
    // multiply d by w^4
    w *= w;
    w *= w;
    
    d *= w;
    
    // 3. return the sum of the four surflets
    // Increasing the value in the vec3 increases contrast.
    return dot( d, vec3(52.0) );
}

// skew constants for 3d simplex functions
const float F3 =  0.3333333;
const float G3 =  0.1666667;

// 3d simplex noise - this wants the hash to come in [-0.5..0.5]
float simplex3d(vec3 position)
{
    // 1. find current tetrahedron T and it's four vertices
    // s, s+i1, s+i2, s+1.0 - absolute skewed (integer) coordinates of T vertices
    // x1, x2, x3, x4 - unskewed coordinates of p relative to each of T vertices

    // calculate s and x
    vec3 s = floor(position + dot(position, vec3(F3)));
    vec3 x1 = position - s + dot(s, vec3(G3));

    // calculate i1 and i2
    vec3 e = step(vec3(0.0), x1 - x1.yzx);    
    
                    e.z = min(e.z, 3.0 - dot(e, vec3(1.0)));

    vec3 i1 = e * (1.0 - e.zxy);
    vec3 i2 = 1.0 - e.zxy * (1.0 - e);

    // x2, x3, x4 kernel summation - contribution from each vertex.
    // unskew the coordinate...
    vec3 x2 = x1 - i1 + G3;
    vec3 x3 = x1 - i2 + 2.0 * G3;
    vec3 x4 = x1 - 1.0 + 3.0 * G3;

    // 2. find four surflets and store them in d
    vec4 w, d;

    // calculate surflet weights
    w.x = dot(x1, x1);
    w.y = dot(x2, x2);
    w.z = dot(x3, x3);
    w.w = dot(x4, x4);

    // w fades from 0.6 at the center of the surflet to 0.0 at the margin
    w = max(0.6 - w, 0.0);

    // calculate surflet components
    d.x = dot(x1, random3(s));
    d.y = dot(x2, random3(s + i1));
    d.z = dot(x3, random3(s + i2));
    d.w = dot(x4, random3(s + 1.0));

    // multiply d by w^4
    w *= w;
    w *= w;
    
    d *= w;

    // 3. return the sum of the four surflets
    // Increasing the value in the vec3 increases contrast.
    return dot(d, vec4(52.0));
}

// Cheap, streamlined 3D Simplex noise... of sorts. I cut a few corners, so it's not perfect, but it's
// artifact free and does the job. I gave it a different name, so that it wouldn't be mistaken for
// the real thing.
// 
// "Simplex Noise Demystified," IQ, other "ShaderToy.com" people, etc.
float weirdsimplex3d(vec3 position)
{
    // Skewing the cubic grid, then determining the first vertex and fractional position.
    vec3 i = floor(position + dot(position, vec3(1./3.)) );  position -= i - dot(i, vec3(1./6.));
    
    // Breaking the skewed cube into tetrahedra with partitioning planes, then determining which side of the 
    // intersecting planes the skewed point is on. Ie: Determining which tetrahedron the point is in.
    vec3 i1 = step(position.yzx, position), i2 = max(i1, 1. - i1.zxy); i1 = min(i1, 1. - i1.zxy);    
    
    // Using the above to calculate the other three vertices -- Now we have all four tetrahedral vertices.
    // Technically, these are the vectors from "p" to the vertices, but you know what I mean. :)
    vec3 p1 = position - i1 + 1./6., p2 = position - i2 + 1./3., p3 = position - .5;
  

    // 3D simplex falloff - based on the squared distance from the fractional position "p" within the 
    // tetrahedron to the four vertice points of the tetrahedron. 
    vec4 v = max(.5 - vec4(dot(position, position), dot(p1, p1), dot(p2, p2), dot(p3, p3)), 0.);
    
    // Dotting the fractional position with a random vector, generated for each corner, in order to determine 
    // the weighted contribution distribution... Kind of. Just for the record, you can do a non-gradient, value 
    // version that works almost as well.
    vec4 d = vec4(dot(position, hash33(i)), dot(p1, hash33(i + i1)), dot(p2, hash33(i + i2)), dot(p3, hash33(i + 1.)));
     
     
    // Simplex noise... Not really, but close enough. :)
    //return clamp(dot(d, v*v*v*8.)*1.732 + .5, 0., 1.); // Not sure if clamping is necessary. Might be overkill.
    return dot(d, v*v*v*8.)*1.732 + .5;
}