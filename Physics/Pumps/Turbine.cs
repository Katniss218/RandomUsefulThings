using System;
using System.Collections.Generic;
using System.Text;

namespace Physics.Pumps
{
    public class Turbine
    {
        [Obsolete("unconfirmed")]
        public static double CalculateTurbinePower( double flowRate, double inletPressure, double outletPressure, double efficiency, double gasDensity )
        {
            // Power equation for turbines
            const double GravityAcceleration = 9.81; // m/s^2
            double pressureRatio = inletPressure / outletPressure; // probably wrong since it doesn't even use it.
            double power = (flowRate * (inletPressure - outletPressure)) / (efficiency * gasDensity * GravityAcceleration);
            return power / 1000; // Convert to kW
        }
    }
}
