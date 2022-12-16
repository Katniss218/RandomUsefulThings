﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Noise
{
    public static class Hash
    {
        const float M1 = 1597334677; //1719413*929
        const float M2 = 3812015801; //140473*2467*11

        public static float Hash1( float n )
        {
            return n - (float)Math.Floor( Math.Sin( n ) * 43758.5453 );
        }

        public static (float x, float y) Hash2( float x, float y )
        {
            float dot1 = MathMethods.MathMethods.Dot( x, y, 127.1f, 311.7f );
            float dot2 = MathMethods.MathMethods.Dot( x, y, 269.5f, 183.3f );

            float sinD1 = (float)(Math.Sin( dot1 ) * 43758.5453);
            float sinD2 = (float)(Math.Sin( dot2 ) * 43758.5453);

            return (sinD1 - (float)Math.Floor( sinD1 ), sinD2 - (float)Math.Floor( sinD2 ));
        }

        public static float Get( float x, float y )
        {
            x *= M1;
            y *= M2;

            float n = (float)((uint)x ^ (uint)y) * M1;

            return n * (1.0f / 0xffffffff);
        }

        //https://www.shadertoy.com/view/4dVBzz
        //much improved version, still comparable in speed to fract(sin()),
        //but with much better bit quality for making vec3 and vec4's
        public static float GetTong( float x, float y )
        {
            x *= M1;
            y *= M2;

            uint n = (uint)x ^ (uint)y;

            n = n * (n ^ (n >> 15));

            return (float)(n * (1.0f / 0xffffffff));
        }

        /*public static float hash_FractSin( vec2 p )
        {
            return fract( sin( dot( p, vec2( 12.9898, 78.233 ) ) ) * 43758.5453 );
        }*/

        //https://www.shadertoy.com/view/MsV3z3
        /*public static float hash_2DWeyl( ivec2 c )
        {
            int x = 0x3504f333 * c.x * c.x + c.y;
            int y = 0xf1bbcdcb * c.y * c.y + c.x;

            return float( x * y ) * (2.0 / 8589934592.0) + 0.5;
        }*/

        //https://www.shadertoy.com/view/4tXyWN
        /*public static float hash_IQ3( uvec2 x )
        {
            uvec2 q = 1103515245U * ((x >> 1U) ^ (x.yx));
            uint n = 1103515245U * ((q.x) ^ (q.y >> 3U));
            return float( n ) * (1.0 / float( 0xffffffffU ));
        }*/

        //https://www.shadertoy.com/view/4djSRW
        /*public static float hash_WithoutSine( vec2 p )
        {
            vec3 p3 = fract( vec3( p.xyx ) * .1031 );
            p3 += dot( p3, p3.yzx + 19.19 );
            return fract( (p3.x + p3.y) * p3.z );
        }*/

        /*
        //----------------------------------------------------------------------------------------
        //  1 out, 1 in...
        float hash11(float p)
        {
            p = fract(p * .1031);
            p *= p + 33.33;
            p *= p + p;
            return fract(p);
        }

        //----------------------------------------------------------------------------------------
        //  1 out, 2 in...
        float hash12(vec2 p)
        {
            vec3 p3  = fract(vec3(p.xyx) * .1031);
            p3 += dot(p3, p3.yzx + 33.33);
            return fract((p3.x + p3.y) * p3.z);
        }

        //----------------------------------------------------------------------------------------
        //  1 out, 3 in...
        float hash13(vec3 p3)
        {
            p3  = fract(p3 * .1031);
            p3 += dot(p3, p3.zyx + 31.32);
            return fract((p3.x + p3.y) * p3.z);
        }
        //----------------------------------------------------------------------------------------
        // 1 out 4 in...
        float hash14(vec4 p4)
        {
            p4 = fract(p4  * vec4(.1031, .1030, .0973, .1099));
            p4 += dot(p4, p4.wzxy+33.33);
            return fract((p4.x + p4.y) * (p4.z + p4.w));
        }

        //----------------------------------------------------------------------------------------
        //  2 out, 1 in...
        vec2 hash21(float p)
        {
            vec3 p3 = fract(vec3(p) * vec3(.1031, .1030, .0973));
            p3 += dot(p3, p3.yzx + 33.33);
            return fract((p3.xx+p3.yz)*p3.zy);

        }

        //----------------------------------------------------------------------------------------
        ///  2 out, 2 in...
        vec2 hash22(vec2 p)
        {
            vec3 p3 = fract(vec3(p.xyx) * vec3(.1031, .1030, .0973));
            p3 += dot(p3, p3.yzx+33.33);
            return fract((p3.xx+p3.yz)*p3.zy);

        }

        //----------------------------------------------------------------------------------------
        ///  2 out, 3 in...
        vec2 hash23(vec3 p3)
        {
            p3 = fract(p3 * vec3(.1031, .1030, .0973));
            p3 += dot(p3, p3.yzx+33.33);
            return fract((p3.xx+p3.yz)*p3.zy);
        }

        //----------------------------------------------------------------------------------------
        //  3 out, 1 in...
        vec3 hash31(float p)
        {
            vec3 p3 = fract(vec3(p) * vec3(.1031, .1030, .0973));
            p3 += dot(p3, p3.yzx+33.33);
            return fract((p3.xxy+p3.yzz)*p3.zyx); 
        }


        //----------------------------------------------------------------------------------------
        ///  3 out, 2 in...
        vec3 hash32(vec2 p)
        {
            vec3 p3 = fract(vec3(p.xyx) * vec3(.1031, .1030, .0973));
            p3 += dot(p3, p3.yxz+33.33);
            return fract((p3.xxy+p3.yzz)*p3.zyx);
        }

        //----------------------------------------------------------------------------------------
        ///  3 out, 3 in...
        vec3 hash33(vec3 p3)
        {
            p3 = fract(p3 * vec3(.1031, .1030, .0973));
            p3 += dot(p3, p3.yxz+33.33);
            return fract((p3.xxy + p3.yxx)*p3.zyx);
        }

        //----------------------------------------------------------------------------------------
        // 4 out, 1 in...
        vec4 hash41(float p)
        {
            vec4 p4 = fract(vec4(p) * vec4(.1031, .1030, .0973, .1099));
            p4 += dot(p4, p4.wzxy+33.33);
            return fract((p4.xxyz+p4.yzzw)*p4.zywx);
        }

        //----------------------------------------------------------------------------------------
        // 4 out, 2 in...
        vec4 hash42(vec2 p)
        {
            vec4 p4 = fract(vec4(p.xyxy) * vec4(.1031, .1030, .0973, .1099));
            p4 += dot(p4, p4.wzxy+33.33);
            return fract((p4.xxyz+p4.yzzw)*p4.zywx);
        }

        //----------------------------------------------------------------------------------------
        // 4 out, 3 in...
        vec4 hash43(vec3 p)
        {
            vec4 p4 = fract(vec4(p.xyzx)  * vec4(.1031, .1030, .0973, .1099));
            p4 += dot(p4, p4.wzxy+33.33);
            return fract((p4.xxyz+p4.yzzw)*p4.zywx);
        }

        //----------------------------------------------------------------------------------------
        // 4 out, 4 in...
        vec4 hash44(vec4 p4)
        {
            p4 = fract(p4  * vec4(.1031, .1030, .0973, .1099));
            p4 += dot(p4, p4.wzxy+33.33);
            return fract((p4.xxyz+p4.yzzw)*p4.zywx);
        }
        */


        /*  Good for simplex.
         *  
        // discontinuous pseudorandom uniformly distributed in [-0.5, +0.5]^3
        vec2 random2(vec2 c)
        {
	        float j = 4096.0*sin(dot(c,vec2(17.0, 59.4)));
	        vec2 r;
	        r.y = fract(512.0*j);
	        j *= .125;
	        r.x = fract(512.0*j);
	        return r-0.5;
        }

        // discontinuous pseudorandom uniformly distributed in [-0.5, +0.5]^3
        vec3 random3(vec3 c)
        {
	        float j = 4096.0*sin(dot(c,vec3(17.0, 59.4, 15.0)));
	        vec3 r;
	        r.z = fract(512.0*j);
	        j *= .125;
	        r.x = fract(512.0*j);
	        j *= .125;
	        r.y = fract(512.0*j);
	        return r-0.5;
        }
        */
    }
}