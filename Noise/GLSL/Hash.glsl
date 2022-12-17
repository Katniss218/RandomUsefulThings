 float hash_FractSin( vec2 p )
{
    return fract( sin( dot( p, vec2( 12.9898, 78.233 ) ) ) * 43758.5453 );
}

//https://www.shadertoy.com/view/MsV3z3
float hash_2DWeyl( ivec2 c )
{
    int x = 0x3504f333 * c.x * c.x + c.y;
    int y = 0xf1bbcdcb * c.y * c.y + c.x;

    return float( x * y ) * (2.0 / 8589934592.0) + 0.5;
}

//https://www.shadertoy.com/view/4tXyWN
float hash_IQ3( uvec2 x )
{
    uvec2 q = 1103515245U * ((x >> 1U) ^ (x.yx));
    uint n = 1103515245U * ((q.x) ^ (q.y >> 3U));

    return float( n ) * (1.0 / float( 0xffffffffU ));
}

//https://www.shadertoy.com/view/4djSRW
float hash_WithoutSine( vec2 p )
{
    vec3 p3 = fract( vec3( p.xyx ) * .1031 );
    p3 += dot( p3, p3.yzx + 19.19 );

    return fract( (p3.x + p3.y) * p3.z );
}

// @#@#@#@#@#@#@#@#@#@#@#@#@#@#@#@#@#@#@#@#@#@#@#@#@#@#@#@#@#@#@#@#@#@#@#@#@#@#@#@#@#@#@#@

// - ret in [0 to 1] with average at 0.5
float hash11(float p)
{
    p = fract(p * .1031);
    p *= p + 33.33;
    p *= p + p;

    return fract(p);
}

// - ret in [0 to 1] with average at 0.5
float hash21(vec2 p)
{
    vec3 p3  = fract(vec3(p.xyx) * .1031);
    p3 += dot(p3, p3.yzx + 33.33);

    return fract((p3.x + p3.y) * p3.z);
}

// - ret in [0 to 1] with average at 0.5
float hash31(vec3 p3)
{
    p3  = fract(p3 * .1031);
    p3 += dot(p3, p3.zyx + 31.32);

    return fract((p3.x + p3.y) * p3.z);
}

// - ret in [0 to 1] with average at 0.5
float hash41(vec4 p4)
{
    p4 = fract(p4  * vec4(.1031, .1030, .0973, .1099));
    p4 += dot(p4, p4.wzxy + 33.33);

    return fract((p4.x + p4.y) * (p4.z + p4.w));
}

// - ret.xy in [0 to 1] with average at 0.5
vec2 hash12(float p)
{
    vec3 p3 = fract(vec3(p) * vec3(.1031, .1030, .0973));
    p3 += dot(p3, p3.yzx + 33.33);

    return fract((p3.xx + p3.yz) * p3.zy);
}

// - ret.xy in [0 to 1] with average at 0.5
vec2 hash22(vec2 p)
{
    vec3 p3 = fract(vec3(p.xyx) * vec3(.1031, .1030, .0973));
    p3 += dot(p3, p3.yzx + 33.33);

    return fract((p3.xx + p3.yz) * p3.zy);
}

// - ret.xy in [0 to 1] with average at 0.5
vec2 hash32(vec3 p3)
{
    p3 = fract(p3 * vec3(.1031, .1030, .0973));
    p3 += dot(p3, p3.yzx + 33.33);

    return fract((p3.xx + p3.yz) * p3.zy);
}

// - ret.xyz in [0 to 1] with average at 0.5
vec3 hash13(float p)
{
    vec3 p3 = fract(vec3(p) * vec3(.1031, .1030, .0973));
    p3 += dot(p3, p3.yzx + 33.33);

    return fract((p3.xxy + p3.yzz) * p3.zyx); 
}

// - ret.xyz in [0 to 1] with average at 0.5
vec3 hash23(vec2 p)
{
    vec3 p3 = fract(vec3(p.xyx) * vec3(.1031, .1030, .0973));
    p3 += dot(p3, p3.yxz + 33.33);

    return fract((p3.xxy + p3.yzz) * p3.zyx);
}

// - ret.xyz in [0 to 1] with average at 0.5
vec3 hash33(vec3 p3)
{
    p3 = fract(p3 * vec3(.1031, .1030, .0973));
    p3 += dot(p3, p3.yxz + 33.33);

    return fract((p3.xxy + p3.yxx) * p3.zyx);
}

// - ret.xyzw in [0 to 1] with average at 0.5
vec4 hash14(float p)
{
    vec4 p4 = fract(vec4(p) * vec4(.1031, .1030, .0973, .1099));
    p4 += dot(p4, p4.wzxy + 33.33);

    return fract((p4.xxyz + p4.yzzw) * p4.zywx);
}

// - ret.xyzw in [0 to 1] with average at 0.5
vec4 hash24(vec2 p)
{
    vec4 p4 = fract(vec4(p.xyxy) * vec4(.1031, .1030, .0973, .1099));
    p4 += dot(p4, p4.wzxy + 33.33);

    return fract((p4.xxyz + p4.yzzw) * p4.zywx);
}

// - ret.xyzw in [0 to 1] with average at 0.5
vec4 hash34(vec3 p)
{
    vec4 p4 = fract(vec4(p.xyzx)  * vec4(.1031, .1030, .0973, .1099));
    p4 += dot(p4, p4.wzxy + 33.33);

    return fract((p4.xxyz + p4.yzzw) * p4.zywx);
}

// - ret.xyzw in [0 to 1] with average at 0.5
vec4 hash44(vec4 p4)
{
    p4 = fract(p4  * vec4(.1031, .1030, .0973, .1099));
    p4 += dot(p4, p4.wzxy + 33.33);

    return fract((p4.xxyz + p4.yzzw) * p4.zywx);
}

// @#@#@#@#@#@#@#@#@#@#@#@#@#@#@#@#@#@#@#@#@#@#@#@#@#@#@#@#@#@#@#@#@#@#@#@#@#@#@#@#@#@#@#@

// - ret.xy uniformly distributed in [-0.5 to 0.5]
vec2 random2(in vec2 position)
{
	float j = 4096.0 * sin(dot(position, vec2(17.0, 59.4)));
	vec2 r;

	r.y = fract(512.0 * j);
	j *= .125;
	r.x = fract(512.0 * j);

	return r - 0.5; // bring the value from [0 to 1] to [-0.5 to 0.5]
}

// - ret.xyz uniformly distributed in [-0.5 to 0.5]
vec3 random3(in vec3 position)
{
	float j = 4096.0 * sin(dot(position, vec3(17.0, 59.4, 15.0)));
	vec3 r;

	r.z = fract(512.0 * j);
	j *= .125;
	r.x = fract(512.0 * j);
	j *= .125;
	r.y = fract(512.0 * j);

	return r - 0.5; // bring the value from [0 to 1] to [-0.5 to 0.5]
}

// - ret.xyzw in [-0.5 to 0.5]
vec4 random4(in vec4 position)
{
	float j = 4096.0 * sin(dot(position, vec4(17.0, 59.4, 15.0, 33.7)));
	vec4 r;

	r.z = fract(512.0 * j);
	j *= .125;
	r.x = fract(512.0 * j);
	j *= .125;
	r.y = fract(512.0 * j);
	j *= .125;
	r.w = fract(512.0 * j);

	return r - 0.5; // bring the value from [0 to 1] to [-0.5 to 0.5]
}