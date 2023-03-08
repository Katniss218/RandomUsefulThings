using System;
using System.Collections.Generic;
using System.Text;

namespace Miscellaneous
{
    public static class DeltaV
    {
        // required constant acceleration to achieve a change in position with given start and end velocities
        // ((v * f^2) - (v * i^2))/(2 * deltaPosition)

        // propellant mass needed for given deltav
        // propmass = (d*m * e^(deltaV / ve) - dm         // ???

        // Desired constant acceleration to achieve a current position give initial positions, velocities and times:
        // a = 2*(xf/t^2 - x0/t^2 - v0/t)



        // ΔV = Ve * ln(Mi/Mf)

        // ΔV = desired change in velocity (DeltaV)
        // Ve = exhaust velocity (specific impulse) of the rocket engine
        // Mi = initial total mass of the rocket (including propellant)
        // Mf = final total mass of the rocket (after all propellant has been consumed) includes the dry mass of the current stage, and the stages above it.

        // untested
        // Mi = Mf * e^(ΔV/Ve)
        // prop mass below. (Mp = Mi - Mf)
        // Mp = Mf * (e^(ΔV/Ve) - 1)

        // untested
        // Ve = ΔV / ln(1 + Mp/Mf)

        /// <summary>
        /// Calculates the propellant mass required to achieve the desired Delta-V.
        /// </summary>
        /// <param name="endMass">The final mass, after the burn has completed.</param>
        /// <param name="deltaV">The Delta-V accumulated during the burn.</param>
        /// <param name="exhaustVelocity">The effective exhaust velocity of the engine.</param>
        /// <returns>The propellant mass that was spent.</returns>
        public static double CalculatePropellantMass( double endMass, double deltaV, double exhaustVelocity )
        {
            // correct.
            double propellantMass = endMass * (Math.Exp( deltaV / exhaustVelocity ) - 1);
            return propellantMass;
        }

        [Obsolete( "untested" )]
        public static double CalculateExhaustVelocity( double initialMass, double endMass, double deltaV )
        {
            double massRatio = initialMass / endMass;
            double exhaustVelocity = deltaV / Math.Log( massRatio );
            return exhaustVelocity;
        }

        [Obsolete( "untested" )]
        public static double CalculateDeltaV( double initialMass, double endMass, double exhaustVelocity )
        {
            double deltaV = exhaustVelocity * Math.Log( initialMass / endMass );
            return deltaV;
        }
    }
}
