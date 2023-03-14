using System;
using System.Collections.Generic;
using System.Text;

namespace RandomUsefulThings.Math
{
    // pressure / Math.Pow(density, gamma) = constant

    public class Thermodynamics
    {
        public struct FluidData
        {
            public double Velocity { get; set; }
            public double Pressure { get; set; }
            public double Temperature { get; set; }

            public override string ToString()
            {
                return $"V: {Velocity}, P: {Pressure}, T: {Temperature}";
            }
        }

        // Constants for air
        private const double GAMMA = 1.4;
        private const double R_AIR = 287;
        private const double RHO = 1.293;

        // returns the conditions at the exit for subsonic flow
        public static FluidData CompressibleSubsonic( double areaInlet, double areaOutlet, FluidData inlet )
        {
            double machInlet = inlet.Velocity / CalculateSpeedOfSound( inlet.Temperature );

            double machExit = CalculateMachNumber( areaInlet, areaOutlet, machInlet );
            double pressureExit = inlet.Pressure * System.Math.Pow( (1 + ((GAMMA - 1) / 2) * System.Math.Pow( machInlet, 2 )), (GAMMA / (GAMMA - 1)) );
            double temperatureExit = inlet.Temperature * (1 + ((GAMMA - 1) / 2) * System.Math.Pow( machInlet, 2 ));

            double velocityExit = machExit * CalculateSpeedOfSound( temperatureExit );

            return new FluidData
            {
                Velocity = velocityExit,
                Pressure = pressureExit,
                Temperature = temperatureExit
            };
        }

        // returns the conditions at the exit for supersonic flow
        public static FluidData CompressibleSupersonic( double areaInlet, double areaOutlet, FluidData inlet )
        {
            double machInlet = inlet.Velocity / System.Math.Sqrt( GAMMA * inlet.Pressure / RHO ); // seems correct too.

            double machExit = System.Math.Sqrt( (1 + (GAMMA - 1) / 2 * System.Math.Pow( machInlet, 2 )) / (GAMMA * System.Math.Pow( machInlet, 2 ) - (GAMMA - 1) / 2) ) * System.Math.Pow( areaInlet / areaOutlet, 1 / GAMMA );

            double pressureExit = inlet.Pressure * System.Math.Pow( 1 + (GAMMA - 1) / 2 * System.Math.Pow( machInlet, 2 ) / System.Math.Pow( 1 + (GAMMA - 1) / 2 * System.Math.Pow( machInlet, 2 ) * System.Math.Pow( areaInlet / areaOutlet, 2 / GAMMA ), GAMMA / (GAMMA - 1) ), -1 / GAMMA );
            double temperatureExit = inlet.Temperature * System.Math.Pow( pressureExit / inlet.Pressure, (GAMMA - 1) / GAMMA );
            double velocityExit = machExit * System.Math.Sqrt( GAMMA * pressureExit / RHO );

            return new FluidData
            {
                Pressure = pressureExit,
                Temperature = temperatureExit,
                Velocity = velocityExit
            };
        }

        // Helper method to calculate the speed of sound for air
        private static double CalculateSpeedOfSound( double temperature )
        {
            // sqrt(kRT) correct for isentropic flow
            return System.Math.Sqrt( GAMMA * R_AIR * temperature );
        }

        // Helper method to calculate the exit Mach number
        private static double CalculateMachNumber( double areaInlet, double areaOutlet, double machInlet )
        {
            return System.Math.Sqrt( (1 / ((areaOutlet / areaInlet) * (1 / System.Math.Pow( machInlet, 2 ))) + (GAMMA - 1) / (2 * GAMMA)) ) / System.Math.Sqrt( (GAMMA - 1) / (2 * GAMMA) );
        }
    }

    public class Thermodynamics2
    {
        public struct FluidData
        {
            public double Velocity { get; set; }
            public double Pressure { get; set; }
            public double Temperature { get; set; }
            public double Density { get; set; }

            public override string ToString()
            {
                return $"V: {Velocity}, P: {Pressure}, T: {Temperature}, Rho: {Density}";
            }
        }

        [Obsolete( "Unconfirmed" )]
        // returns the conditions at the exit for a compressible subsonic flow
        public static FluidData CompressibleSubsonic( double areaInlet, double areaOutlet, FluidData inlet )
        {
            double gamma = 1.4; // specific heat ratio for air
            double M = inlet.Velocity / System.Math.Sqrt( gamma * inlet.Pressure / inlet.Density ); // Mach number at inlet

            if( M < 1 ) // subsonic flow
            {
                double P2 = inlet.Pressure * System.Math.Pow( areaInlet / areaOutlet, gamma ); // pressure at exit
                double T2 = inlet.Temperature * System.Math.Pow( P2 / inlet.Pressure, (gamma - 1) / gamma ); // temperature at exit
                double rho2 = inlet.Density * System.Math.Pow( P2 / inlet.Pressure, 1 / gamma ); // density at exit
                double V2 = inlet.Velocity * (areaInlet / areaOutlet); // velocity at exit

                return new FluidData()
                {
                    Velocity = V2,
                    Pressure = P2,
                    Temperature = T2,
                    Density = rho2
                };
            }
            else // supersonic flow
            {
                throw new ArgumentException( "Cannot use CompressibleSubsonic() for supersonic flow" );
            }
        }

        [Obsolete( "Unconfirmed" )]
        // returns the conditions at the exit for a compressible supersonic flow
        public static FluidData CompressibleSupersonic( double areaInlet, double areaOutlet, FluidData inlet )
        {
            double gamma = 1.4; // specific heat ratio for air
            double M = inlet.Velocity / System.Math.Sqrt( gamma * inlet.Pressure / inlet.Density ); // Mach number at inlet

            if( M > 1 ) // supersonic flow
            {
                double P2 = inlet.Pressure * System.Math.Pow( (2 * gamma / (gamma + 1)) + ((gamma - 1) / (gamma + 1)) * (inlet.Velocity * inlet.Velocity / (gamma * inlet.Pressure)), gamma / (gamma - 1) ); // pressure at exit
                double T2 = inlet.Temperature * System.Math.Pow( P2 / inlet.Pressure, (gamma - 1) / gamma ); // temperature at exit
                double rho2 = inlet.Density * System.Math.Pow( P2 / inlet.Pressure, 1 / gamma ); // density at exit
                double V2 = System.Math.Sqrt( (2 * gamma / (gamma - 1)) * inlet.Pressure * ((System.Math.Pow( P2 / inlet.Pressure, (gamma - 1) / gamma )) - 1) ); // velocity at exit

                return new FluidData()
                {
                    Velocity = V2,
                    Pressure = P2,
                    Temperature = T2,
                    Density = rho2
                };
            }
            else // subsonic flow
            {
                throw new ArgumentException( "Cannot use CompressibleSupersonic() for subsonic flow" );
            }
        }
    }

    public class Thermodynamics3
    {
        //private const double R_AIR = 287; // gas constant of gas in nozzle.

        private const double GAMMA = 1.4;
        private const double R_AIR = 287;

        public struct NozzleSegment
        {
            public double InletArea;
            public double ExitArea;
            public double Velocity;
            public double Pressure;
            public double Temperature;

            public override string ToString()
            {
                return $"V: {Velocity}, P: {Pressure}, T: {Temperature}";
            }
        }

        public static NozzleSegment CalculateExitProperties( NozzleSegment inletSegment )
        {
            // Calculate the exit area based on the given diameters
            //double exitArea = inletSegment.ExitArea;

            // Calculate the mass flow rate using the known inlet properties and area
            //double massFlowRate = inletSegment.InletArea * Math.Sqrt( inletSegment.Pressure * inletSegment.Temperature / (R_AIR * inletSegment.InletArea) );

            // Calculate the sonic velocity at the inlet
            double sonicVelocity = System.Math.Sqrt( inletSegment.Pressure / inletSegment.Temperature * GAMMA * R_AIR );

            // Calculate the velocity ratio for the isentropic flow
            double velocityRatio = System.Math.Pow( (1 + 0.5 * (GAMMA - 1) * System.Math.Pow( inletSegment.Velocity / sonicVelocity, 2 )), -GAMMA / (GAMMA - 1) );

            // Calculate the exit velocity using the velocity ratio and the sonic velocity
            double exitVelocity = velocityRatio * sonicVelocity;

            // Calculate the exit temperature using the isentropic expansion relation
            double exitTemperature = inletSegment.Temperature * System.Math.Pow( velocityRatio, 2 * (GAMMA - 1) / GAMMA );

            // Solve for the exit pressure using the mass flow rate and exit velocity
            double exitMachNumber = exitVelocity / System.Math.Sqrt( GAMMA * R_AIR * exitTemperature );
            double exitVelocityRatio = System.Math.Pow( (1 + 0.5 * (GAMMA - 1) * System.Math.Pow( exitMachNumber, 2 )), -GAMMA / (GAMMA - 1) );
            double exitPressure = inletSegment.Pressure * System.Math.Pow( exitVelocityRatio, GAMMA / (GAMMA - 1) );

            // Create and return a new NozzleSegment struct with the exit properties and area
            NozzleSegment exitSegment = new NozzleSegment();
            exitSegment.InletArea = inletSegment.ExitArea;
            //exitSegment.ExitArea = exitArea;
            exitSegment.Velocity = exitVelocity;
            exitSegment.Pressure = exitPressure;
            exitSegment.Temperature = exitTemperature;

            return exitSegment;
        }
    }
}