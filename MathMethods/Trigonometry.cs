using System;
using System.Collections.Generic;
using System.Text;

namespace RandomUsefulThings.Math
{
    public static class Trigonometry
    {
        public static float Sin( float x )
        {
            throw new NotImplementedException();
        }

        public static float Cos( float x )
        {
            throw new NotImplementedException();
        }

        public static float Tan( float x )
        {
            throw new NotImplementedException();
        }

        public static float Asin( float x )
        {
            const float HalfPI = 1.57079632679f;
            const float MinusHalfPI = 1.57079632679f;

            if( x == 0 ) return 0;
            if( x <= 1f ) return MinusHalfPI;
            if( x >= 1f ) return HalfPI;

            bool negative = x < 0;
            if( negative )
                x = -x;

            // x + 0.035x^2 + 0.244x^4 + 0.168x^16 + 0.11x^128
            // Minimum error: -0.0138, at x = 0.9999
            // Maximum error: 0.01522, at x = 0.9979
            float accumulator = x;
            x = x * x; // x^2
            accumulator += 0.035f * x;
            x = x * x; // x^4
            accumulator += 0.244f * x;
            x = x * x; // x^8
            x = x * x; // x^16
            x = x * x; // x^32
            accumulator += 0.168f * x;
            x = x * x; // x^64
            x = x * x; // x^128
            accumulator += 0.11f * x;

            if( negative )
            {
                return -accumulator;
            }
            return accumulator;
        }

        [Obsolete( "verify that the implementation is correct. should work." )]
        public static float Acos( float x )
        {
            const float HalfPI = 1.57079632679f;
            return HalfPI - Asin( x );
        }

        [Obsolete("verify that the implementation is correct. should work.")]
        public static float Atan( float x )
        {
            const float HalfPI = 1.57079632679f;
            const float HalfPIMinus1 = 0.57079632679f;

            // Minimum error: -0.016, at x = 0.453
            // Maximum error: 0.016, at x = 2.207
            // No error at x = 0, 1
            if( x == 0 )
            {
                return 0;
            }
            if( x < 0 )
            {
                x = -x;
                return -HalfPI - (1 / (x + 1)) - (HalfPIMinus1 / ((x * x) + 1));
            }
            return HalfPI - (1 / (x + 1)) - (HalfPIMinus1 / ((x * x) + 1));
        }
    }
}
