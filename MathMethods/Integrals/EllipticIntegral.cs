using System;
using System.Collections.Generic;
using System.Text;

namespace RandomUsefulThings.Math.Integrals
{
    public static class EllipticIntegral
    {
        private static double FirstKindIntegrand( double theta, double k )
        {
            // Returns the value of the integrand of the elliptic integral of the first kind at any point theta.
            double sinTheta = System.Math.Sin( theta );

            return 1.0 / System.Math.Sqrt( 1 - (k * k) * (sinTheta * sinTheta) );
        }

        private static double SecondKindIntegrand( double theta, double k )
        {
            // Returns the value of the integrand of the elliptic integral of the second kind at any point theta.
            double sinTheta = System.Math.Sin( theta );

            return System.Math.Sqrt( 1 - (k * k) * (sinTheta * sinTheta) );
        }

        private static double ThirdKindIntegrand( double theta, double k, double n )
        {
            // Returns the value of the integrand of the elliptic integral of the third kind at any point theta.
            double sinTheta = System.Math.Sin( theta );

            return 1.0 / (1 - n - sinTheta * sinTheta) * System.Math.Sqrt( 1 - (k * k) * (sinTheta * sinTheta) );
        }

        [Obsolete( "Incomplete, see comment." )]
        public static double FirstKind( double phi, double k )
        {
            // integral of the function: F(φ,k) = ∫[0,φ] 1 / sqrt(1 - k^2 * sin^2θ) * dθ       --for k^2 between 0 and 1
            // k - modulus
            // phi - variable of integration. Apparently equal to the amplitude of the function sin(theta)
            // integral from 0 to phi.
            double theta = 0.0; // variable of integration (this is swept from 0 to phi in slices of width delta-theta when approximating).
            double deltaTheta = 0.0f;

            // Two ways of writing this. either `(1.0 / denom) * deltaTheta` or `deltaTheta / denom`.
            return FirstKindIntegrand( theta, phi ) * deltaTheta; // integral of this from 0 to phi is equal to the value of F(phi,k)
        }

        // 2nd kind
        // E(φ,k) = ∫[0,φ] sqrt(1 - k^2 * sin^2θ) * dθ       --for k^2 between 0 and 1

        // 3rd kind
        // Π(n,φ,k) = ∫[0,φ] 1 / (1 - n * sin^2θ) * sqrt(1 - k^2 * sin^2θ) * dθ       --for k^2 between 0 and 1 

        //

        // elliptic lambda function

        // elliptic logarithm

        // more...
    }
}
