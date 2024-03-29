﻿
// - ret uniformly distributed in [0 to 1] with average at 0.5
float nrand( in vec2 position )
{
    return fract( sin( dot( position, vec2( 12.9898, 78.233 ) ) ) * 43758.5453 );
}


//https://www.shadertoy.com/view/MsV3z3
float hash21_2DWeyl( in ivec2 position )
{
    int x = 0x3504f333 * position.x * position.x + position.y;
    int y = 0xf1bbcdcb * position.y * position.y + position.x;

    return float( x * y ) * (2.0 / 8589934592.0) + 0.5;
}

//https://www.shadertoy.com/view/4tXyWN
float hash21_IQ3( in uvec2 position )
{
    uvec2 q = 1103515245U * ((position >> 1U) ^ (position.yx));
    uint n = 1103515245U * ((q.x) ^ (q.y >> 3U));

    return float( n ) * (1.0 / float( 0xffffffffU ));
}

// @#@#@#@#@#@#@#@#@#@#@#@#@#@#@#@#@#@#@#@#@#@#@#@#@#@#@#@#@#@#@#@#@#@#@#@#@#@#@#@#@#@#@#@

// - ret uniformly distributed in [0 to 1]
float hash11(in float position)
{
    position = fract(position * 8.103142);
    position *= position + 33.13123;
    position *= position + position;

    return fract(position);
}

// Useful for small ranges.
// - ret uniformly distributed in [0 to 1]
float hash21_small(in vec2 position)
{
    float p = position.x + (position.y * 7.36762545);

    p = fract(p * 8.103142); // The formula can probably be improved.
    p *= p + 33.13123;
    p *= p + p;

    return fract(position);
}

// repeats in gradients from bottom-left to top-right.
// Useful for large ranges.
// - ret uniformly distributed in [0 to 1]
float hash21(in vec2 position)
{
    vec3 p3  = fract(vec3(position.xyx) * .1031);
    p3 += dot(p3, p3.yzx + 33.33);

    return fract((p3.x + p3.y) * p3.z);
}

// repeats in gradients from bottom-left to top-right
// - ret in [0 to 1] with average at 0.5
float hash31(in vec3 position)
{
    position  = fract(position * .1031);
    position += dot(position, position.zyx + 31.32);

    return fract((position.x + position.y) * position.z);
}

// repeats in gradients from bottom-left to top-right
// - ret in [0 to 1] with average at 0.5
float hash41(in vec4 position)
{
    position = fract(position  * vec4(.1031, .1030, .0973, .1099));
    position += dot(position, position.wzxy + 33.33);

    return fract((position.x + position.y) * (position.z + position.w));
}

// repeats in gradients
// - ret.xy in [0 to 1]
vec2 hash12(in float position)
{
    vec3 p3 = fract(vec3(position) * vec3(.1031, .1030, .0973));
    p3 += dot(p3, p3.yzx + 33.33);

    return fract((p3.xx + p3.yz) * p3.zy);
}

// repeats in gradients from bottom-left to top-right
// - ret.xy in [0 to 1] with average at 0.5
vec2 hash22(in vec2 position)
{
    vec3 p3 = fract(vec3(position.xyx) * vec3(.1031, .1030, .0973));
    p3 += dot(p3, p3.yzx + 33.33);

    return fract((p3.xx + p3.yz) * p3.zy);
}

// repeats in gradients from bottom-left to top-right
// - ret.xy in [0 to 1] with average at 0.5
vec2 hash32(in vec3 position)
{
    position = fract(position * vec3(.1031, .1030, .0973));
    position += dot(position, position.yzx + 33.33);

    return fract((position.xx + position.yz) * position.zy);
}

// repeats in gradients from bottom-left to top-right
// - ret.xyz in [0 to 1]
vec3 hash13(in float position)
{
    vec3 p3 = fract(vec3(position) * vec3(.1031, .1030, .0973));
    p3 += dot(p3, p3.yzx + 33.33);

    return fract((p3.xxy + p3.yzz) * p3.zyx); 
}

// repeats in gradients from bottom-left to top-right
// - ret.xyz in [0 to 1] with average at 0.5
vec3 hash23(vec2 position)
{
    vec3 p3 = fract(vec3(position.xyx) * vec3(.1031, .1030, .0973));
    p3 += dot(p3, p3.yxz + 33.33);

    return fract((p3.xxy + p3.yzz) * p3.zyx);
}

// repeats in gradients from bottom-left to top-right
// - ret.xyz in [0 to 1] with average at 0.5
vec3 hash33(in vec3 position)
{
    position = fract(position * vec3(.1031, .1030, .0973));
    position += dot(position, position.yxz + 33.33);

    return fract((position.xxy + position.yxx) * position.zyx);
}

// repeats in gradients from bottom-left to top-right
// - ret.xyzw in [0 to 1]
vec4 hash14(in float position)
{
    vec4 p4 = fract(vec4(position) * vec4(.1031, .1030, .0973, .1099));
    p4 += dot(p4, p4.wzxy + 33.33);

    return fract((p4.xxyz + p4.yzzw) * p4.zywx);
}

// repeats in gradients from bottom-left to top-right
// - ret.xyzw in [0 to 1] with average at 0.5
vec4 hash24(in vec2 position)
{
    vec4 p4 = fract(vec4(position.xyxy) * vec4(.1031, .1030, .0973, .1099));
    p4 += dot(p4, p4.wzxy + 33.33);

    return fract((p4.xxyz + p4.yzzw) * p4.zywx);
}

// repeats in gradients from bottom-left to top-right
// - ret.xyzw in [0 to 1] with average at 0.5
vec4 hash34(in vec3 position)
{
    vec4 p4 = fract(vec4(position.xyzx)  * vec4(.1031, .1030, .0973, .1099));
    p4 += dot(p4, p4.wzxy + 33.33);

    return fract((p4.xxyz + p4.yzzw) * p4.zywx);
}

// repeats in gradients from bottom-left to top-right
// - ret.xyzw in [0 to 1] with average at 0.5
vec4 hash44(in vec4 position)
{
    position = fract(position  * vec4(.1031, .1030, .0973, .1099));
    position += dot(position, position.wzxy + 33.33);

    return fract((position.xxyz + position.yzzw) * position.zywx);
}

// @#@#@#@#@#@#@#@#@#@#@#@#@#@#@#@#@#@#@#@#@#@#@#@#@#@#@#@#@#@#@#@#@#@#@#@#@#@#@#@#@#@#@#@

// - ret.xy uniformly distributed in [-0.5 to 0.5]
vec2 random2(in vec2 position)
{
	float j = 12.9898 * sin(dot(position, vec2(17.0, 59.4)));
	vec2 r;
    
	r.y = fract(2707.2707 * j);
	r.x = fract(3023.3023 * j);

	return r - 0.5; // bring the value from [0 to 1] to [-0.5 to 0.5]
}

// - ret.xyz uniformly distributed in [-0.5 to 0.5]
vec3 random3(in vec3 position)
{
	float j = 12.9898 * sin(dot(position, vec3(17.0, 59.4, 15.0)));
	vec3 r;
    
	r.y = fract(2707.2707 * j);
	r.x = fract(3023.3023 * j);
	r.z = fract(3361.3361 * j);

	return r - 0.5; // bring the value from [0 to 1] to [-0.5 to 0.5]
}

// - ret.xyzw in [-0.5 to 0.5]
vec4 random4(in vec4 position)
{
	float j = 12.9898 * sin(dot(position, vec4(17.0, 59.4, 15.0, 33.7)));
	vec4 r;
    
	r.y = fract(2707.2707 * j);
	r.x = fract(3023.3023 * j);
	r.z = fract(3361.3361 * j);
	r.w = fract(3677.3677 * j);

	return r - 0.5; // bring the value from [0 to 1] to [-0.5 to 0.5]
}

// Tileable hash.
// Can be used with basically any noise.
// To use with FBM, the scale needs to be set proportionally to frequency.

float Hash(in vec2 p, in float scale)
{
	// This is tiling part, adjusts with the scale...
	p = mod(p, scale);

	return fract(sin(dot(p, vec2(27.16898, 38.90563))) * 5151.5473453);
}
