// https://www.shadertoy.com/view/7lBBR3

//++++++++++++++++++++++++++++++++++++++++++++++++++++++
// Normal Mapping Shadow (NMS)
// Based on paper http://enbdev.com/doc_normalmappingshadows.htm
//++++++++++++++++++++++++++++++++++++++++++++++++++++++
#define iSampleCount			32
#define SampleCount				(float(iSampleCount))
#define HeightScale				1.5
#define ShadowHardness			2.0
#define ShadowLength			0.05

void mainImage( out vec4 fragColor, in vec2 fragCoord )
{
	vec3 lightposition = vec3( 0.0, 0.0, 0.1 );
	vec3 planeposition = vec3( fragCoord.xy / iResolution.y, 0.0 );

	vec2 cursorposition = iMouse.xy / iResolution.y;
	lightposition.xy = cursorposition;
	if( iMouse.z <= 0.0 )
	{
		lightposition.x = (sin( iTime * 1.0 ) + 1.0);
		lightposition.y = (cos( iTime * 2.5 ) + 1.0);
	}

	float samplecount = SampleCount;
	float invsamplecount = 1.0 / samplecount;

	float hardness = HeightScale * ShadowHardness;

	vec3 lightdir = lightposition - planeposition;

	vec2 dir = lightdir.xy * HeightScale;

	lightdir = normalize( lightdir.xyz );

	vec2 uv = fract( fragCoord.xy / iResolution.y );
	vec3 normal = texture( iChannel0, uv ).xyz;
	normal = normal * 2.0 - 1.0;

	//lighting with flat normals (from vertex or depth generated)
	float lighting = clamp( dot( lightdir, normal ), 0.0, 1.0 );

	float step = invsamplecount * ShadowLength;

	//randomization
	vec2 noise = fract( fragCoord.xy * 0.5 );
	noise.x = (noise.x * 0.5 + noise.y) * (1.0 / 1.5 - 0.25);

	float pos = step * noise.x;

	//do not compute on back faces/pixels //disabled cause while() not supported in Shadertoy
	//pos = (-lighting >= 0.0) ? 1.001 : pos;

	float slope = -lighting;
	float maxslope = 0.0;
	float shadow = 0.0;
	for( int i = 0; i < iSampleCount; i++ )
	{
		vec3 tmpNormal = texture( iChannel0, uv + dir * pos ).xyz;
		tmpNormal = tmpNormal * 2.0 - 1.0;

		float tmpLighting = dot( lightdir, tmpNormal );

		float shadowed = -tmpLighting;

		//for g-buffer normals of deferred render insert here depth comparison to occlude objects, abstract code example:
		//vec2	cropminmax = clamp(1.0 - (depth - tmpDepth) * vec2(4000.0, -600.0), 0.0, 1.0);
		//cropminmax.x = cropminmax.x * cropminmax.y;
		//shadowed *= cropminmax.x;

		slope += shadowed;

		//if (slope > 0.0) //cheap, but not correct, suitable for hard shadow with early exit
		if( slope > maxslope ) //more suitable for calculating soft shadows by distance or/and angle
		{
			shadow += hardness * (1.0 - pos);
		}
		maxslope = max( maxslope, slope );

		pos += step;
	}

	shadow = clamp( 1.0 - shadow * invsamplecount, 0.0, 1.0 );

	//disable shadow on click
	//if (iMouse.w > 0.0) shadow = 1.0;

	//coloring
	vec3 ambientcolor = vec3( 0.15, 0.4, 0.6 ) * 0.7;
	vec3 lightcolor = vec3( 1.0, 0.7, 0.3 ) * 1.2;
	float ao = clamp( normal.z, 0.0, 1.0 );
	fragColor.xyz = shadow * lighting * lightcolor;
	//v1
	//fragColor.xyz+= ambientcolor * (clamp(normal.z, 0.0, 1.0) * 0.5 + 0.5);
	//v2
	fragColor.xyz += ambientcolor;
	fragColor.xyz *= (clamp( normal.z, 0.0, 1.0 ) * 0.5 + 0.5); //kinda diffuse
	fragColor.w = 1.0;
}
