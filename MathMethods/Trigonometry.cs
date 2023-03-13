using System;
using System.Collections.Generic;
using System.Text;

namespace RandomUsefulThings.Math
{
    public static class Trigonometry
    {
        public static float Sin( float x )
        {
            const float PI = 3.14159265359f;

#warning TODO - this is actually wrong, should mod to twopi.
            x = MathMethods.MathMethods.Modulo(x, PI);

            const float Fac11 = 39916800; // 11!
            const float Fac9 = 362880; // 9!
            const float Fac7 = 5040; // 7!
            const float Fac5 = 120;
            const float Fac3 = 6;

            float x2 = x * x;

            float x3 = x2 * x;
            float x5 = x3 * x2;
            float x7 = x5 * x2;
            float x9 = x7 * x2;
            float x11 = x9 * x2;

            return x - (x3 / Fac3) + (x5 / Fac5) - (x7 / Fac7) + (x9 / Fac9) - (x11 / Fac11);
        }

        public static float Cos( float x )
        {
#warning TODO - this is actually wrong, should mod to twopi.
            const float Fac10 = 3628800; // 10!
            const float Fac8 = 40320; // 8!
            const float Fac6 = 720; // 6!
            const float Fac4 = 24;
            const float Fac2 = 2;

            float x2 = x * x;

            float x4 = x2 * x2;
            float x6 = x4 * x2;
            float x8 = x6 * x2;
            float x10 = x8 * x2;

            return 1 - (x2 / Fac2) + (x4 / Fac4) - (x6 / Fac6) + (x8 / Fac8) - (x10 / Fac10);
        }

        public static float Tan( float x )
        {
            // rough approximation
            // x + 0.5x^4 + 0.01x^16 + 0.000005x^32
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
