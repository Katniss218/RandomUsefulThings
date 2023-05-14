using System;
using System.Collections.Generic;
using System.Text;

namespace RandomUsefulThings.Physics.HeatTransfer
{
    public class RadiativeHeatTransfer
    {
        /// <summary>
        /// Stephan boltzmann constant `sigma`
        /// </summary>
        public const double StefanBoltzmannConstant = 0.00000005670374419;

        public static double GetEmissivePower( double temperature, double emissivity )
        {
            // Emissivity in [0..1], 1 being a blackbody.
            // Emissivity to be determined experimentally.

            // Metals, especially polished, have low emissivities (and high reflectivities).
            temperature *= temperature;
            temperature *= temperature; // temperature ^ 4

            return StefanBoltzmannConstant * temperature * emissivity;
        }

        // absorbtivity + reflectivity + transmissivity = 1

        public static double GetLuminosity( double temperature, double surfaceArea ) // total energy emitted by a blackbody, Watts
        {
            return StefanBoltzmannConstant * (temperature * temperature * temperature * temperature) * surfaceArea;
        }
    }
}
