﻿using System;
using System.Collections.Generic;
using System.Text;

namespace RandomUsefulThings.Math
{
    public static class Trigonometry
    {
        //public const double QuarterPI = 0.78539816339744830961566084581987572104929234984377645524373614807695410157155224965700870633552926699553702162832057666177346115238764555793133985203212027936257102567548463027638991115573723873259549;
        //public const double HalfPI = 1.570796326794896619231321691639751442098584699687552910487472296153908203143104499314017412671058533991074043256641153323546922304775291115862679704064240558725142051350969260552779822311474477465191;
        //public const double PI = 3.141592653589793238462643383279502884197169399375105820974944592307816406286208998628034825342117067982148086513282306647093844609550582231725359408128481117450284102701938521105559644622948954930382;
        //public const double TwoPI = 6.2831853071795864769252867665590057683943387987502116419498891846156328125724179972560696506842341359642961730265646132941876892191011644634507188162569622349005682054038770422111192892458979098607639;


        // 
        // 

        // 
        // 
        // 

        // can be approximated with just +-*/% by precomputing factorials and using multiplication for powers.

        // CORDIC algorithm can be used to calculate approximate values of trig functions.


        const float HalfPI = 1.570796326794f;
        const float PI = 3.141592653589f;
        const float TwoPI = 6.283185307179f;

        // https://github.com/lattera/glibc/blob/master/sysdeps/ieee754/dbl-64/s_sin.c

        /// <summary>
        /// Calculates the sine of the specified angle in radians.
        /// </summary>
        /// <returns>The value of the sine. Maximum absolute value of error: 10^-6, median: 10^-8.</returns>
        public static float Sin( float x )
        {
            // Taylor Series approximation: Sin(x) =~ x - (x^3)/3! + (x^5)/5! - (x^7)/7! + (x^9)/9! - ...

            // Range reduction.
            float newX = x % TwoPI;
            if( newX > PI )
            {
                newX -= TwoPI;
            }
            else if( newX < -PI )
            {
                newX += TwoPI;
            }

            if( newX > HalfPI ) // flip around +- PI/2 to further reduce range
            {
                newX += 2 * (HalfPI - newX);
            }
            else if( newX < -HalfPI )
            {
                newX += 2 * (-HalfPI - newX);
            }

            // Small values of pi.
            if( newX < 0.0175f && newX > -0.0175f )
            {
                return newX;
            }

            float x2 = newX * newX;

            float x3 = x2 * newX;
            float x5 = x3 * x2;
            float x7 = x5 * x2;
            float x9 = x7 * x2;

            return // Need up to x^15 terms to get above float32 precision in [-pi..pi]. Up to x^9 with range reduction.
                newX
                - (x3 / 6)
                + (x5 / 120)
                - (x7 / 5040)
                + (x9 / 362880);
        }

        /// <summary>
        /// Calculates the cosine of the specified angle in radians.
        /// </summary>
        /// <returns>The value of the cosine. Maximum absolute value of error: 10^-4, median: 10^-7.</returns>
        public static float Cos( float x )
        {
            // Taylor Series approximation: Cos(x) = 1 - x^2/2! + x^4/4! - x^6/6! + x^8/8! - ...

            x = ((x + PI) % TwoPI) - PI;

            float x2 = x * x;

            float x4 = x2 * x2;
            float x6 = x4 * x2;
            float x8 = x6 * x2;
            float x10 = x8 * x2;
            float x12 = x10 * x2;

            return
                1
                -(x2 / 2) 
                + (x4 / 24) 
                - (x6 / 720) 
                + (x8 / 40320)
                - (x10 / 3628800)
                + (x12 / 479001600);
        }

        public static float Tan( float x )
        {
            // Taylor Series approximation: Tan(x) = x + x^3/3 + 2x^5/15 + 17x^7/315 + 62x^9/2835 + ...
            // Not used.

            // Tan(x) = Sin(x) / Cos(x)

            float sin = Sin( x );
            float cos = Cos( x );
            return sin / cos; // Generally close, but could be better when close to the asymptotes.
        }

        public static float Asin( float x )
        {
            // Taylor Series approximation: Arcsin(x) =~ x + (1/2)x^3/3 + (1/2)(3/4)x^5/5 + (1/2)(3/4)(5/6)x^7/7 + ...
            // Not used here.

            const float HalfPI = 1.57079632679f;

            if( x > -0.0175f && x < 0.0175f ) 
                return x;
            if( x <= -1f ) 
                return -HalfPI;
            if( x >= 1f ) 
                return HalfPI;

            bool negative = x < 0;
            if( negative )
                x = -x;

            // Minimum error: -0.0138, at x = 0.9999
            // Maximum error: 0.01522, at x = 0.9979
            float accumulator = x;
            x = x * x; // x^2
            accumulator += 0.034f * x;
            x = x * x; // x^4

            if( x > -0.6f && x < 0.6f )
            {
                accumulator += 0.24121f * x;
                return negative ? -accumulator : accumulator; // Arcsin(x) =~ x + 0.034x^2 + 0.24121x^4
            }
            accumulator += 0.2407f * x;
            x = x * x; // x^8

            if( x > -0.8f && x < 0.8f )
            {
                x = x * x; // x^16
                accumulator += 0.22f * x;
                return negative ? -accumulator : accumulator; // Arcsin(x) =~ x + 0.034x^2 + 0.2407x^4 + 0.22x^16
            }
            accumulator += 0.009f * x;
            x = x * x; // x^16
            accumulator += 0.1875f * x;
            x = x * x; // x^32
            accumulator -= 0.124f * x;
            x = x * x; // x^64
            accumulator += 0.309f * x;
            x = x * x; // x^128
            accumulator -= 0.306f * x;
            x = x * x; // x^256
            accumulator += 0.217f * x;

            return negative ? -accumulator : accumulator; // Arcsin(x) =~ x + 0.034x^2 + 0.2407x^4 + 0.009x^8 + 0.1865x^16 - 0.124x^32 + 0.309x^64 - 0.306x^128 + 0.217x^256
        }

        public static float Acos( float x )
        {
            // Arccos(x) = π/2 - arcsin(x)

            const float HalfPI = 1.57079632679f;

            return HalfPI - Asin( x );
        }

        public static float Atan( float x )
        {
            // Taylor Series approximation: Arctan(x) = x - x^3/3 + x^5/5 - x^7/7 + x^9/9 - ...
            // Not used.

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