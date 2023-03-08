using System;
using System.Collections.Generic;
using System.Text;

namespace Physics.Pumps
{
    public class CentrifugalPump
    {
        [Obsolete( "untested" )]
        public static double CalculateOutputPressure( double pumpSpeed, double pumpHead, double fluidDensity )
        {
            const double pi = Math.PI;
            const double g = 9.81; // acceleration due to gravity in m/s^2
            const double impellerDiameter = 0.3; // in meters

            // Calculate the flow rate in m^3/s using the pump speed and impeller diameter
            double flowRate = (pi * impellerDiameter * pumpSpeed) / 60.0;

            // Calculate the velocity head of the fluid in m
            double velocityHead = Math.Pow( (flowRate / (pi * Math.Pow( (impellerDiameter / 2), 2 ))), 2 ) / (2 * g);

            // Calculate the total head of the pump in m
            double totalHead = pumpHead + velocityHead;

            // Calculate the pressure head of the fluid in Pa
            double pressureHead = totalHead * fluidDensity * g;

            // Convert pressure head to output pressure in bar
            double outputPressure = pressureHead / 100000.0;

            return outputPressure;
        }

        [Obsolete( "untested" )]
        public static double CalculateOutputPressure2( double pumpHead, double fluidDensity, double gravity )
        {
            // head [m]
            // density [kg/m^3]
            // gravity [m/s^2]
            double outputPressure = (pumpHead * fluidDensity * gravity) / 1000; // Pressure in kPa
            return outputPressure;
        }

        [Obsolete( "unconfirmed, but I think it's right" )]
        public static double GetNewPressure( double newRpm, double oldRpm, double newRadius, double oldRadius, double oldPressure )
        {
            // returns the new pressure as a multiple of the old pressure.
            // can be rearranged to solve for the other variables.
            return oldPressure * (((newRpm / oldRpm) * (newRpm / oldRpm)) * ((newRadius / oldRadius) * (newRadius / oldRadius)));
        }

        [Obsolete( "unconfirmed, but I think it's right" )]
        public static double GetNewVolumeCapacity( double newRpm, double oldRpm, double newRadius, double oldRadius, double oldVolumeCapacity )
        {
            // returns the new volume capacity as a multiple of the old volume capacity.
            // can be rearranged to solve for the other variables.
            return oldVolumeCapacity * ((newRpm / oldRpm) * (newRadius / oldRadius));
        }

        [Obsolete( "unconfirmed, but I think it's right" )]
        public static double GetNewPowerUse( double newRpm, double oldRpm, double newRadius, double oldRadius, double oldPowerUse )
        {
            // returns the new power use as a multiple of the old power use.
            // can be rearranged to solve for the other variables.
            return oldPowerUse * (((newRpm / oldRpm) * (newRpm / oldRpm) * (newRpm / oldRpm)) * ((newRadius / oldRadius) * (newRadius / oldRadius) * (newRadius / oldRadius)));
        }

        [Obsolete( "unconfirmed" )]
        public static double CalculatePower( double flowRate, double head, double fluidDensity, double efficiency )
        {
            // P = Q x H x ρ x g / η
            // where P is the power in watts,
            // Q is the flow rate in cubic meters per second,
            // H is the head in meters,
            // ρ is the density of the fluid in kilograms per cubic meter,
            // g is the acceleration due to gravity in meters per second squared,
            // η is the pump efficiency (a dimensionless value between 0 and 1).
            // inputs in SI.
            const double GravityAcceleration = 9.81; // m/s^2
            return (flowRate * head * fluidDensity * GravityAcceleration) / efficiency;
        }

        [Obsolete("unconfirmed")]
        public static double CalculateEfficiency( double flowRate, double head, double power, double fluidDensity )
        {
            // inputs in SI.
            const double GravityAcceleration = 9.81; // m/s^2
            return (flowRate * head) / (power * fluidDensity * GravityAcceleration);
        }

        // Affinity laws of pumps.
        // https://en.wikipedia.org/wiki/Affinity_laws
        // 1 - With impeller diameter held constant
        // 1.a Flow is proportional to shaft speed
        // 1.b Pressure or Head is proportional to the square of shaft speed
        // 1.c Power is proportional to the cube of shaft speed
        // 2 - With shaft speed held constant
        // 2.a Flow is proportional to the impeller diameter
        // 2.b Pressure or Head is proportional to the square of impeller diameter
        // 3.c Power is proportional to the cube of impeller diameter

        // These laws assume that the pump/fan efficiency remains constant which is rarely exactly true

        public static double CalculateFlowRate( double flowRate1, double rotationalSpeed1, double rotationalSpeed2 )
        {
            // inputs in SI.
            //affinity laws - return flowRate2
            // Flow rate law: This law states that the flow rate of a centrifugal pump is proportional to its rotational speed:
            return (flowRate1 * rotationalSpeed2) / rotationalSpeed1;
        }

        public static double CalculateHead( double head1, double rotationalSpeed1, double rotationalSpeed2 )
        {
            // inputs in SI.
            //affinity laws - return head2
            // Head law: This law states that the head of a centrifugal pump is proportional to the square of its rotational speed:
            return head1 * Math.Pow( rotationalSpeed2 / rotationalSpeed1, 2 );
        }

        public static double CalculatePower( double power1, double rotationalSpeed1, double rotationalSpeed2 )
        {
            // inputs in SI.
            //affinity laws - return power2
            // Power law: This law states that the power required to drive a centrifugal pump is proportional to the cube of its rotational speed:
            return power1 * Math.Pow( rotationalSpeed2 / rotationalSpeed1, 3 );
        }

        [Obsolete("untested")]
        public static double CalculateNetPositiveSuctionHead( double inletPressure, double vaporPressure, double fluidDensity, double inletVelocity, double inletDiameter )
        {
            // Net Positive Suction Head

            const double GravityAcceleration = 9.81; // m/s^2
            double velocityHead = Math.Pow( inletVelocity, 2 ) / (2 * GravityAcceleration);
            double pressureHead = inletPressure / (fluidDensity * GravityAcceleration);
            double vaporPressureHead = vaporPressure / (fluidDensity * GravityAcceleration);
            double inletDiameterRatio = Math.Pow( inletDiameter, 2 ) / Math.Pow( inletDiameter, 2 );
            return pressureHead - vaporPressureHead - velocityHead - inletDiameterRatio;
        }

        public static double CalculatePumpSpecificSpeed( double flowRate, double head, double rotationalSpeed )
        {
            // The specific speed is a dimensionless parameter that characterizes the geometry and performance of the pump.
            // https://en.wikipedia.org/wiki/Specific_speed
            // - Centrifugal pump impellers have specific speed values ranging from 500 to 10,000 (English units),
            // - with radial flow pumps at 500-4000,
            // - mixed flow at 2000-8000,
            // - and axial flow pumps at 7000-20,000.
            // - Values of specific speed less than 500 are associated with positive displacement pumps.
            // Pump specific speed can be calculated using British gallons or using Metric units (m3/s or L/s and metres head), changing the values listed above.

            return (rotationalSpeed * Math.Sqrt( flowRate )) / Math.Pow( head, 0.75 /* 3/4 */ );
            // multiply by g 9.81 m/s^2 for dimensionless.
        }

        public static double CalculateSuctionSpecificSpeed( double flowRate, double netPositiveSuctionHead, double rotationalSpeed )
        {
            return (rotationalSpeed * Math.Sqrt( flowRate )) / Math.Pow( netPositiveSuctionHead, 0.75 /* 3/4 */ );
            // multiply by g 9.81 m/s^2 for dimensionless.
        }

        public static double CalculatePumpAffinity( double impellerDiameter1, double impellerDiameter2, double rpm1, double rpm2 )
        {
            // The pump affinity is a dimensionless parameter that indicates how similar the pumps are in terms of their flow and pressure characteristics.
            return (impellerDiameter2 / impellerDiameter1) * (rpm2 / rpm1);
        }

        [Obsolete("unconfirmed")]
        public static double CalculateBrakeHorsepower( double flowRate, double head, double efficiency, double fluidDensity )
        {
            const double GravityAcceleration = 9.81; // m/s^2
            double power = (flowRate * head * fluidDensity * GravityAcceleration) / efficiency;
            return power / 745.7; // Convert to horsepower
        }
    }
}
