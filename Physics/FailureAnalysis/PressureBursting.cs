using System;
using System.Collections.Generic;
using System.Text;

namespace RandomUsefulThings.Physics.FailureAnalysis
{
    public class PressureBursting
    {
        /// <param name="wallThickness">[inches/mm]</param>
        /// <param name="outsideDiameter">[inches/mm]</param>
        /// <param name="ultimateTensileStrength">[psi/MPa]</param>
        /// <returns>fluid pressure [psi/MPa]</returns>
        public static double GetBurstPressure( double wallThickness, double outsideDiameter, double ultimateTensileStrength )
        {
            // Barlow's Formula.

            // instead of UTS, yield strength can be used to calculate pressure at which permanent deformation starts.
            // for safety factor, divide the UTS by the factor, e.g. ultimateTensileStrength / 1.5

            // [] * [lb/(in*in)] * [in] / [in]
            // for conversion to metric.
            return (2 * ultimateTensileStrength * wallThickness) / outsideDiameter;
        }
    }
}
