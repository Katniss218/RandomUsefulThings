using System;
using System.Collections.Generic;
using System.Text;

namespace Noise
{
    public static class Hash
    {
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

        const float M1 = 1597334677; // 1719413 * 929
        const float M2 = 3812015801; // 140473 * 2467 * 11

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
    }
}
