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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="thrust">[N]</param>
        /// <param name="massFlow">[kg/s]</param>
        /// <returns>Effective exhaust velocity in [m/s]</returns>
        public static double GetExhaustVelocity( double thrust, double massFlow )
        {
            return thrust / massFlow;
        }

        public static double GetThrust( double massFlow, double exhaustVelocity )
        {
            return massFlow * exhaustVelocity;
        }

        public static double GetMassFlow( double thrust, double exhaustVelocity )
        {
            return thrust / exhaustVelocity;
        }

        public static double MassToVolumeRatio( double OFMassRatio, double ODensity, double FDensity )
        {
            // OFMassRatio is e.g. ~5.5 for hydrolox, ~2.5 for kerolox ORSC.
            // densities in [kg/l]

            // totalMassFraction = 1 + OFMassRatio
            // oxMassFraction = OFMassRatio / totalMassFraction
            // fuelMassFraction = 1 / totalMassFraction

            double oContrib = OFMassRatio / ODensity;
            double fContrib = 1 - FDensity;
            double totalContrib = oContrib + fContrib;

            double OVolumeFraction = oContrib / totalContrib;
            // FVolumeFraction = 1 - OFVolumeRatio
            return OVolumeFraction;
        }

        public static double VolumeToMassRatio( double OVolumeFraction, double ODensity, double FDensity )
        {
            // densities in [kg/l]
            double oContrib = OVolumeFraction / ODensity;
            double fContrib = (1 - OVolumeFraction) / FDensity;

            double OFMassRatio = oContrib / fContrib;
            return OFMassRatio;
        }

        public static double GetTWR( double surfaceGravity, double thrust, double mass )
        {
            // for all three:
            // thrust in [N]
            // mass in [kg]
            // surfaceGravity in [m/s]

            return thrust / (surfaceGravity * mass);
        }

        public static double GetMass( double surfaceGravity, double thrust, double twr )
        {
            return thrust / twr / surfaceGravity;
        }

        public static double GetThrust( double surfaceGravity, double mass, double twr )
        {
            return mass * surfaceGravity * twr;
        }

        public static double GetMassFlowFromFlux( double massFlux, double area )
        {
            return massFlux * area;
        }
        public static double GetMassFlowFromFluxCircle( double massFlux, double radius )
        {
            return GetMassFlowFromFlux( massFlux, Math.PI * (radius * radius) );
        }
    }
}
