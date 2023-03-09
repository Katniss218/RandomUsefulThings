using System;
using System.Collections.Generic;
using System.Text;

namespace Physics
{
    public class ThinWalledPressureCylinder
    {
        // A thin-walled pressure vessel is one with its radius over 10x greater than its wall thickness.
        static bool IsThinWalled( float radius, float thickness ) => radius / 10.0f > thickness;

        /// <summary>
        /// Calculates the stress in the tangent direction (along the wall of the cylinder).
        /// </summary>
        public static double CalculateHoopStress( double pressure, double internalRadius, double thickness )
        {
            // In thin-walled vessels, the hoop stress gradient (variation) in the outward direction is negligible. Thus no lerp-factor input.
            return (pressure * internalRadius) / thickness;
        }

        /// <summary>
        /// Calculates the stress in the longitudinal direction (along the axis of the cylinder).
        /// </summary>
        /// <param name="pressure"></param>
        /// <param name="internalRadius"></param>
        /// <param name="thickness"></param>
        /// <returns></returns>
        public static double CalculateLongitudinalStress( double pressure, double internalRadius, double thickness )
        {
            return (pressure * internalRadius) / (2.0 * thickness);
        }

        public static double CalculateRadialStress( double pressure, float insideToOutsideFactor )
        {
            // the negative weirds me out, but apparently that's correct.
            return MathMethods.Interpolation.Lerp( -pressure, 0.0, insideToOutsideFactor );
        }
    }
}
