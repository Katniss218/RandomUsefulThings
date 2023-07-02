using System;
using System.Collections.Generic;
using System.Text;

namespace RandomUsefulThings.Physics.AtmosphericScattering
{
    public static class MieScattering
    {
        // Mie Scattering `M` is the scattering from aerosols (particles) larger that 10% of the light’s wavelength.

        /// <param name="n">The refractive index (of the medium through which the light is passing).</param>
        /// <param name="N">The molecular density at the bottom of the atmosphere.</param>
        /// <returns>The scattering coefficient βs for Mie scattering M.</returns>
        public static double ScatteringCoefficientFunction( double n, double N )
        {
            // Source: https://www.gamedevs.org/uploads/deferred-rendering-of-planetary-terrains-with-accurate-atmospheres.pdf Equation 2-3
            // (8pi^3 * (n^2 - 1)^2) / (3N)

            const double EightPiCubed = 248.050213442; // 8 * pi^3

            double nSquaredMinus1 = (n * n) - 1;

            return (EightPiCubed * (nSquaredMinus1 * nSquaredMinus1)) / (3 * N);
        }

        /// <param name="θ">The angle [rad] between the direction of incident light and the viewing direction.</param>
        /// <returns>The relative light intensity for a given scattering angle θ in radians.</returns>
        public static double PhaseFunction( double θ, double g )
        {
            // Source: https://www.gamedevs.org/uploads/deferred-rendering-of-planetary-terrains-with-accurate-atmospheres.pdf Equation 2-4

            const double ThreeEighthPi = 0.119366207319; // 3 / (8*pi)

            double cosAngle = System.Math.Cos( θ );
            double gSquared = g * g;

            return ThreeEighthPi * (((1 - gSquared) * (cosAngle * cosAngle)) / ((2 + gSquared) * (1 + gSquared - (2 * g * cosAngle))));
        }
    }
}
