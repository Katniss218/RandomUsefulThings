using System;
using System.Collections.Generic;
using System.Text;

namespace RandomUsefulThings.Physics.AtmosphericScattering
{
    public static class OpticalDepth
    {
        // The optical depth and transmittance are measures of how much light is scattered or absorbed by a path through a medium.

        // The optical depth of a path S can be calculated by integrating the extinction coefficients `βe` and the density ratio `ρ` over this particular path.
        // The density ratio `ρ` is the ratio of the atmospheric density at a given point `x` to the density at the bottom of the atmosphere (sea level / surface).


        /// <param name="h">The (vertical) distance of the given point from the bottom of the atmosphere.</param>
        /// <param name="H">The scale height.</param>
        /// <returns>The density at the given point relative to the density at the bottom of the atmosphere (at <paramref name="h"/> = 0).</returns>
        public static double AtmosphereDensityRatio( double h, double H )
        {
            // https://www.gamedevs.org/uploads/deferred-rendering-of-planetary-terrains-with-accurate-atmospheres.pdf Equation 2-5, `Rg` is bottom of the atmosphere.
            // Apparently Rayleigh and Mie scattering can have completely separate scale heights.

            // e^-(h/H)
            return System.Math.Exp( -(h / H) );
        }

        /// <param name="t">The optical depth of the path (integrated from start to end).</param>
        /// <returns>The value of transmittance for a given optical depth <paramref name="t"/>.</returns>
        public static double Transmittance( double t )
        {
            // https://www.gamedevs.org/uploads/deferred-rendering-of-planetary-terrains-with-accurate-atmospheres.pdf Equation 2-7

            // Beer's Law
            // Mathf.Exp( -distance * absorption )
            // e^-t
            return System.Math.Exp( -t );
        }
    }
}
