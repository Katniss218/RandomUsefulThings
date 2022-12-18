
//---------------------------------------------------------------------------------------------
// Helper Functions
//---------------------------------------------------------------------------------------------

float saturate(float v)
{
    return clamp(v, 0.0, 1.0);
}

float overlayNRM(float x, float y)
{
    if (x < 0.5)
        return 2.0*x*y;
    else
        return 1.0 - 2.0*(1.0 - x)*(1.0 - y);
}


//
//
//

vec3 darken( vec3 s, vec3 d )
{
	return min(s, d);
}

vec3 multiply( vec3 s, vec3 d )
{
	return s * d;
}

vec3 colorBurn( vec3 s, vec3 d )
{
	return 1.0 - (1.0 - d) / s;
}

vec3 linearBurn( vec3 s, vec3 d )
{
	return s + d - 1.0;
}

vec3 darkerColor( vec3 s, vec3 d )
{
	return (s.x + s.y + s.z < d.x + d.y + d.z) ? s : d;
}

vec3 lighten( vec3 s, vec3 d )
{
	return max(s, d);
}

vec3 screen( vec3 s, vec3 d )
{
	return s + d - s * d;
}

vec3 colorDodge( vec3 s, vec3 d )
{
	return d / (1.0 - s);
}

vec3 linearDodge( vec3 s, vec3 d )
{
	return s + d;
}

vec3 lighterColor( vec3 s, vec3 d )
{
	return (s.x + s.y + s.z > d.x + d.y + d.z) ? s : d;
}

float overlay( float s, float d )
{
	return (d < 0.5) ? 2.0 * s * d : 1.0 - 2.0 * (1.0 - s) * (1.0 - d);
}

vec3 overlay( vec3 s, vec3 d )
{
	vec3 c;
	c.x = overlay(s.x,d.x);
	c.y = overlay(s.y,d.y);
	c.z = overlay(s.z,d.z);
	return c;
}

float softLight( float s, float d )
{
	return (s < 0.5) ? d - (1.0 - 2.0 * s) * d * (1.0 - d) 
		: (d < 0.25) ? d + (2.0 * s - 1.0) * d * ((16.0 * d - 12.0) * d + 3.0) // Iq says this 3.0 should be replaced with 4.0, but original author argues 3.0 is correct.
					 : d + (2.0 * s - 1.0) * (sqrt(d) - d);
}

vec3 softLight( vec3 s, vec3 d )
{
	vec3 c;
	c.x = softLight(s.x,d.x);
	c.y = softLight(s.y,d.y);
	c.z = softLight(s.z,d.z);
	return c;
}

float hardLight( float s, float d )
{
	return (s < 0.5) ? 2.0 * s * d : 1.0 - 2.0 * (1.0 - s) * (1.0 - d);
}

vec3 hardLight( vec3 s, vec3 d )
{
	vec3 c;
	c.x = hardLight(s.x,d.x);
	c.y = hardLight(s.y,d.y);
	c.z = hardLight(s.z,d.z);
	return c;
}

float vividLight( float s, float d )
{
	return (s < 0.5) ? 1.0 - (1.0 - d) / (2.0 * s) : d / (2.0 * (1.0 - s));
}

vec3 vividLight( vec3 s, vec3 d )
{
	vec3 c;
	c.x = vividLight(s.x,d.x);
	c.y = vividLight(s.y,d.y);
	c.z = vividLight(s.z,d.z);
	return c;
}

vec3 linearLight( vec3 s, vec3 d )
{
	return 2.0 * s + d - 1.0;
}

float pinLight( float s, float d )
{
	return (2.0 * s - 1.0 > d) ? 2.0 * s - 1.0 : (s < 0.5 * d) ? 2.0 * s : d;
}

vec3 pinLight( vec3 s, vec3 d )
{
	vec3 c;
	c.x = pinLight(s.x,d.x);
	c.y = pinLight(s.y,d.y);
	c.z = pinLight(s.z,d.z);
	return c;
}

vec3 hardMix( vec3 s, vec3 d )
{
	return floor(s + d);
}

vec3 difference( vec3 s, vec3 d )
{
	return abs(d - s);
}

vec3 exclusion( vec3 s, vec3 d )
{
	return s + d - 2.0 * s * d;
}

vec3 subtract( vec3 s, vec3 d )
{
	return s - d;
}

vec3 divide( vec3 s, vec3 d )
{
	return s / d;
}

//	rgb<-->hsv functions by Sam Hocevar
//	http://lolengine.net/blog/2013/07/27/rgb-to-hsv-in-glsl
vec3 rgb2hsv(vec3 c)
{
	vec4 K = vec4(0.0, -1.0 / 3.0, 2.0 / 3.0, -1.0);
	vec4 p = mix(vec4(c.bg, K.wz), vec4(c.gb, K.xy), step(c.b, c.g));
	vec4 q = mix(vec4(p.xyw, c.r), vec4(c.r, p.yzx), step(p.x, c.r));
	
	float d = q.x - min(q.w, q.y);
	float e = 1.0e-10;
	return vec3(abs(q.z + (q.w - q.y) / (6.0 * d + e)), d / (q.x + e), q.x);
}

vec3 hsv2rgb(vec3 c)
{
	vec4 K = vec4(1.0, 2.0 / 3.0, 1.0 / 3.0, 3.0);
	vec3 p = abs(fract(c.xxx + K.xyz) * 6.0 - K.www);
	return c.z * mix(K.xxx, clamp(p - K.xxx, 0.0, 1.0), c.y);
}

vec3 hue( vec3 s, vec3 d )
{
	d = rgb2hsv(d);
	d.x = rgb2hsv(s).x;
	return hsv2rgb(d);
}

vec3 color( vec3 s, vec3 d )
{
	s = rgb2hsv(s);
	s.z = rgb2hsv(d).z;
	return hsv2rgb(s);
}

vec3 saturation( vec3 s, vec3 d )
{
	d = rgb2hsv(d);
	d.y = rgb2hsv(s).y;
	return hsv2rgb(d);
}

vec3 luminosity( vec3 s, vec3 d )
{
	float dLum = dot(d, vec3(0.3, 0.59, 0.11));
	float sLum = dot(s, vec3(0.3, 0.59, 0.11));
	float lum = sLum - dLum;
	vec3 c = d + lum;
	float minC = min(min(c.x, c.y), c.z);
	float maxC = max(max(c.x, c.y), c.z);
	if(minC < 0.0) return sLum + ((c - sLum) * sLum) / (sLum - minC);
	else if(maxC > 1.0) return sLum + ((c - sLum) * (1.0 - sLum)) / (maxC - sLum);
	else return c;
}

//---------------------------------------------------------------------------------------------
// Normal Blending Techniques
//---------------------------------------------------------------------------------------------

// https://blog.selfshadow.com/publications/blending-in-detail/
// Method	SM3.0 ALU Instruction cost
// Linear	5
// Overlay	9
// PD	    7
// Whiteout	7
// UDN  	5
// RNM *	8
// Unity	8

// * This includes normalization. If it turns out that you don’t need it, then RNM is 6 ALU instructions.

// RNM blending.
vec3 NormalBlend_RNM(vec3 n1, vec3 n2)
{
    // Unpack (see article on why it's not just n*2-1)
	n1 = n1 * vec3( 2,  2, 2) + vec3(-1, -1,  0);
    n2 = n2 * vec3(-2, -2, 2) + vec3( 1,  1, -1);
    
    // Blend
    return n1 * dot(n1, n2) / n1.z - n2;
}

// RNM (Reoriented Normal Mapping) blending - already unpacked.
vec3 NormalBlend_UnpackedRNM(vec3 n1, vec3 n2)
{
	n1 += vec3(0, 0, 1);
	n2 *= vec3(-1, -1, 1);
	
    return n1 * dot(n1, n2) / n1.z - n2;
}

// Partial Derivatives blending.
vec3 NormalBlend_PartialDerivatives(vec3 n1, vec3 n2)
{	
    // Unpack from [0 to 1] to [-1 to 1]
	n1 = n1 * 2.0 - 1.0;
    n2 = n2 * 2.0 - 1.0;
    
    return normalize(vec3(n1.xy * n2.z + n2.xy * n1.z, n1.z * n2.z));
}

// Whiteout blending.
vec3 NormalBlend_Whiteout(vec3 n1, vec3 n2)
{
    // Unpack from [0 to 1] to [-1 to 1]
	n1 = n1 * 2.0 - 1.0;
    n2 = n2 * 2.0 - 1.0;
    
	return normalize(vec3(n1.xy + n2.xy, n1.z * n2.z));    
}

// UDN Unreal Developer Network) blending.
vec3 NormalBlend_UDN(vec3 n1, vec3 n2)
{
    // Unpack from [0 to 1] to [-1 to 1]
	n1 = n1 * 2.0 - 1.0;
    n2 = n2 * 2.0 - 1.0;    
    
	return normalize(vec3(n1.xy + n2.xy, n1.z));
}

// Unity blending.
vec3 NormalBlend_Unity(vec3 n1, vec3 n2)
{
    // Unpack from [0 to 1] to [-1 to 1]
	n1 = n1 * 2.0 - 1.0;
    n2 = n2 * 2.0 - 1.0;
    
    mat3 nBasis = mat3(vec3(n1.z, n1.y, -n1.x), // +90 degree rotation around y axis
        			   vec3(n1.x, n1.z, -n1.y), // -90 degree rotation around x axis
        			   vec3(n1.x, n1.y,  n1.z));
	
    return normalize((n2.x * nBasis[0]) + (n2.y * nBasis[1]) + (n2.z * nBasis[2]));
}

// Linear Blending - incorrect but I'll include it anyway.
vec3 NormalBlend_Linear(vec3 n1, vec3 n2)
{
    // Unpack from [0 to 1] to [-1 to 1]
	n1 = n1 * 2.0 - 1.0;
    n2 = n2 * 2.0 - 1.0;
    
	return normalize(n1 + n2);    
}

// Overlay - incorrect but I'll include it anyway.
vec3 NormalBlend_Overlay(vec3 n1, vec3 n2)
{
    vec3 n;
    n.x = overlayNRM(n1.x, n2.x);
    n.y = overlayNRM(n1.y, n2.y);
    n.z = overlayNRM(n1.z, n2.z);

    return normalize(n*2.0 - 1.0);
}