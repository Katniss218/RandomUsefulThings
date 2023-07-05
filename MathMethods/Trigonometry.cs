using System;
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
        const float PISquared = 9.86960440109f;

        // https://github.com/lattera/glibc/blob/master/sysdeps/ieee754/dbl-64/s_sin.c

        /// <summary>
        /// Calculates the approximation of the sine of the specified angle in radians.
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

            // For small values of x, sin(x) ~= x.
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

        public static float CosBhaskara( float x )
        {
            if( x < -HalfPI || x > HalfPI )
                throw new ArgumentOutOfRangeException( $"For now, we don't do range reduction or flipping." );

            // Bhāskara's approximation
            // (pi^2 - 4x^2) / (pi^2 + x^2)
            // error range [-0.0013..00016]
            float xSquared = x * x;
            return (PISquared - 4 * xSquared) / (PISquared + xSquared);
        }

        /// <summary>
        /// Calculates the approximation of the cosine of the specified angle in radians.
        /// </summary>
        /// <returns>The value of the cosine. Maximum absolute value of error: 10^-4, median: 10^-7.</returns>
        public static float Cos( float x )
        {
            // Taylor Series approximation: Cos(x) = 1 - x^2/2! + x^4/4! - x^6/6! + x^8/8! - ...

            // also cos(x) = sin(x + pi/2)

            x = ((x + PI) % TwoPI) - PI;

            float x2 = x * x;

            float x4 = x2 * x2;
            float x6 = x4 * x2;
            float x8 = x6 * x2;
            float x10 = x8 * x2;
            float x12 = x10 * x2;

            return
                1
                - (x2 / 2)
                + (x4 / 24)
                - (x6 / 720)
                + (x8 / 40320)
                - (x10 / 3628800)
                + (x12 / 479001600);
        }

        /// <summary>
        /// Calculates the approximation of the tangent of the specified angle in radians.
        /// </summary>
        /// <returns>The value of the cosine. Maximum absolute value of error: ?, median: ?.</returns>
        public static float Tan( float x )
        {
            // Taylor Series approximation: Tan(x) = x + x^3/3 + 2x^5/15 + 17x^7/315 + 62x^9/2835 + ...
            // Not used.

            // Tan(x) = Sin(x) / Cos(x)

            float sin = Sin( x );
            float cos = Cos( x );
            return sin / cos; // Generally close, but could be better when close to the asymptotes.
        }

        /// <summary>
        /// Calculates the angle in radians whose sine is the specified value.
        /// </summary>
        /// <returns>The value of the angle.</returns>
        public static float AsinTaylor( float x )
        {
            // Taylor Series approximation: Arcsin(x) =~ x + ((1/2)x^3)/3 + ((1/2)(3/4)x^5)/5 + ((1/2)(3/4)(5/6)x^7)/7 + ...
            // Preserves the derivatives.

            float xPow2 = x * x;
            float xPower = x;
            float coefficient = 1.0f;

            int ITERATIONS = (int)(2 + (xPow2 * xPow2 * xPow2) * 50); // Dynamic iterations based on where they're needed (roughly).
            // 2+5x^{2}\ +50x^{32} seems to be a better curve.

            float accumulator = x;

            for( int i = 0; i < ITERATIONS; i++ )
            {
                // new coeffs: i * 2 + 1, i * 2 + 2;
                // 1,2 ; 3,4 ; 5,6 ; ...

                float iTimes2 = i * 2.0f;
                coefficient *= ((iTimes2 + 1) / (iTimes2 + 2));
                xPower *= xPow2;

                accumulator += (coefficient * xPower) / (iTimes2 + 3);
            }

            return accumulator;
        }

        /// <summary>
        /// Calculates the approximation of the angle in radians whose sine is the specified value.
        /// </summary>
        /// <returns>The value of the angle. Maximum absolute value of error: ?, median [0..1]: ?</returns>
        public static float Asin( float x )
        {
            // Custom approximation.
            // Generally does not preserve the derivatives. Faster than taylor.

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

            // alternative for x in [0.77..1] => c_{2}=\pi/2-0.9992\sqrt{(1-x^{2})}-0.491\sqrt{(1-x)^{3}}

            // close when x = 1, arcsin(x) =~ π/2 - sqrt((1 - x^2)) - (1/6)*(1 - x^2)^(3/2)
            // good in about [0.95..1]

            return negative ? -accumulator : accumulator; // Arcsin(x) =~ x + 0.034x^2 + 0.2407x^4 + 0.009x^8 + 0.1865x^16 - 0.124x^32 + 0.309x^64 - 0.306x^128 + 0.217x^256
        }

        /// <summary>
        /// Calculates the approximation of the angle in radians whose cosine is the specified value.
        /// </summary>
        /// <returns>The value of the angle. Maximum absolute value of error: ?, median [0..1]: ?</returns>
        public static float Acos( float x )
        {
            // Generally does not preserve the derivatives. Faster than taylor.
            // Arccos(x) = π/2 - arcsin(x)

            const float HalfPI = 1.57079632679f;

            return HalfPI - Asin( x );
        }

        /// <summary>
        /// Calculates the angle in radians whose cosine is the specified value.
        /// </summary>
        /// <returns>The value of the angle.</returns>
        public static float AcosTaylor( float x )
        {
            // Arccos(x) = π/2 - arcsin(x)

            const float HalfPI = 1.57079632679f;

            return HalfPI - AsinTaylor( x );
        }

        /// <summary>
        /// Calculates the approximation of the angle in radians whose tangent is the specified value.
        /// </summary>
        /// <returns>The value of the angle. Maximum absolute value of error: 10^-3, median [0..100]: 10^-5.</returns>
        public static float Atan( float x )
        {
            // Generally does not preserve the derivatives. Faster than taylor.

            // Taylor Series approximation: Arctan(x) = x - x^3/3 + x^5/5 - x^7/7 + x^9/9 - ...
            // Not used here. Not very useful for values far from the origin.

            const float HalfPI = 1.57079632679f;

            // All the terms in the series' numerators have to add up to 0.5*PI. 
            // Arctan(x) =~ 0.5*PI - 1/(x + 1) - (0.5*PI - 0.83)/(x^2 + 1) + 0.17/(x^3 + 1)
            if( x == 0 )
            {
                return 0;
            }
            if( x < 0 )
            {
                x = -x;
                return -(HalfPI // anti-mirror
                - (1f / (x + 1))
                - ((HalfPI - 0.83f) / ((x * x) + 1))
                + (0.17f / ((x * x * x) + 1)));
            }
            return HalfPI
                - (1f / (x + 1)) // 1.0 / blah is important here, gives the slope=1 at x=0.
                - ((HalfPI - 0.83f) / ((x * x) + 1))
                + (0.17f / ((x * x * x) + 1));
        }

        // secant
        // Sec(x) = 1/Cos(x)

        // cosecant
        // Csc(x) = 1/Sin(x)

        // cotangent
        // Cot(x) = 1/Tan(x)

        // chord
        // Crd(x) = 2*Sin(x/2)

    }
}