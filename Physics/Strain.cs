using System;
using System.Collections.Generic;
using System.Text;

namespace RandomUsefulThings.Physics
{
    class Strain
    {
        // For a uniaxially loaded rod of some OriginalLength = ???
        // NormalStrain = ChangeInLength / OriginalLength
        // - change in length due to forge stretching the rod.

        /// <summary>
        /// Calculates the strain in an object deformed after a load has been applied.
        /// </summary>
        /// <param name="originalLength">The original length of the object, before the application of the load.</param>
        /// <param name="newLength">The new length of the object, after the application of the load.</param>
        /// <returns>The strain. Positive for tensile loads, negative for compressive.</returns>
        public static double GetStrainFromDisplacement( double originalLength, double newLength )
        {
            double deltaLength = originalLength - newLength;

            return deltaLength / originalLength;
        }

        public static double GetStrain( double axialStress, double youngsModulus )
        {
            // Only works for objects under uniaxial stress (single loaded axis).
            // Uniaxial / classic Hooke's Law.
            return axialStress / youngsModulus;
        }

        public static double GetStrainX( double stressX, double stressY, double stressZ, double poissonsRatio, double youngsModulus )
        {
            // Transform the input axes (swap them around) to get strain for Y and Z.
            //return (stressX / youngsModulus) - (poissonsRatio * (stressY / youngsModulus)) - (poissonsRatio * (stressZ / youngsModulus));

            // Generalized Hooke's Law.
            return (1.0 / youngsModulus) * (stressX - poissonsRatio * (stressY + stressZ)); // returns strainX
            //return (1.0 / youngsModulus) * (stressY - poissonsRatio * (stressX + stressZ)); // returns strainY
            //return (1.0 / youngsModulus) * (stressZ - poissonsRatio * (stressX + stressY)); // returns strainZ
        }

        // Volumetric strain is also a thing.
        // - it is a measure of how the volume changes in response to stress.
        public static double GetVolumetricStrain( double strainX, double strainY, double strainZ )
        {
            return strainX + strainY + strainZ;
        }

        public static double GetVolumetricStrain( double stressX, double stressY, double stressZ, double poissonsRatio, double youngsModulus )
        {
            // For materials with poisson's ratio of 0.5, this should return 0. These are called "incompressible" materials.

            // Uses rearanged Generalized Hooke's Law.
            return ((1 - 2 * poissonsRatio) / youngsModulus) * (stressX + stressY + stressZ);
        }
    }
}
