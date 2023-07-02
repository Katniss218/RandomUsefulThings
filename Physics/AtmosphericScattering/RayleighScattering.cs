using System;
using System.Collections.Generic;
using System.Text;

namespace RandomUsefulThings.Physics.AtmosphericScattering
{
    public static class RayleighScattering
    {
        // Rayleigh Scattering `R` is the scattering from molecules and particles smaller than 10% of the light's wavelength.


        /// <param name="λ">The wavelength.</param>
        /// <param name="n">The refractive index (of the medium through which the light is passing).</param>
        /// <param name="N">The molecular density at the bottom of the atmosphere.</param>
        /// <returns>The scattering coefficient βs for Rayleigh scattering R.</returns>
        public static double ScatteringCoefficientFunction( double λ, double n, double N )
        {
            // Source: https://www.gamedevs.org/uploads/deferred-rendering-of-planetary-terrains-with-accurate-atmospheres.pdf Equation 2-1
            // (8pi^3 * (n^2 - 1)^2) / (3N * λ^4)

            const double EightPiCubed = 248.050213442; // 8 * pi^3

            double nSquaredMinus1 = (n * n) - 1;
            double λTo4thPower = λ * λ;
            λTo4thPower *= λTo4thPower;

            return (EightPiCubed * (nSquaredMinus1 * nSquaredMinus1)) / ((3 * N) * λTo4thPower);
        }

        /// <param name="θ">The angle [rad] between the direction of incident light and the viewing direction.</param>
        /// <returns>The relative light intensity for a given scattering angle θ in radians.</returns>
        public static double PhaseFunction( double θ )
        {
            // Source: https://www.gamedevs.org/uploads/deferred-rendering-of-planetary-terrains-with-accurate-atmospheres.pdf Equation 2-2
            // (3 / (16*pi)) * cos(θ)^2

            const double ThreeSixteenthPi = 0.0596831036595; // 3 / (16*pi)

            double cosAngle = System.Math.Cos( θ );
            return ThreeSixteenthPi * (1 + (cosAngle * cosAngle));
        }
    }
}
