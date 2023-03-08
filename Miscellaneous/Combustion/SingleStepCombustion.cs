using System;
using System.Collections.Generic;
using System.Text;

namespace Miscellaneous.Combustion
{
    public class SingleStepCombustion
    {
        [Obsolete( "untested" )]
        public static double CalculateEnthalpy( double molarMass, double moleFraction, double temperature )
        {
            const double gasConstant = 8314; // J/(kmol*K)
            double specificHeatRatio = 1.4; // Assume constant value for simplicity
            double cp = gasConstant / (molarMass / 1000) * specificHeatRatio / (specificHeatRatio - 1);
            return cp * (temperature - 298) * moleFraction;
        }

        [Obsolete("untested, probably wrong")]
        public static void SimulateCombustion( double initialTemperature, double initialPressure, double massFlowRate, double fuelMassFraction, double oxidizerMassFraction, double specificHeatRatio, double exhaustGasPressure, double exhaustGasTemperature, double combustionHeatRelease, double timeStep, int numSteps )
        {
            // One commonly used combustion model is the one-step reaction model,
            //   which assumes that the fuel and oxidizer mix instantaneously and react in a single step to form combustion products.
            // The model typically involves solving the continuity, momentum, energy, and species conservation equations,
            //   along with the thermodynamic equations of state.

            // Define constants
            const double gasConstant = 8314; // J/(kmol*K)
            const double molarMassFuel = 16.04; // kg/kmol
            const double molarMassOxidizer = 28.96; // kg/kmol

            // Define initial conditions
            double fuelMassFlowRate = massFlowRate * fuelMassFraction;
            double oxidizerMassFlowRate = massFlowRate * oxidizerMassFraction;
            double fuelMolarFlowRate = fuelMassFlowRate / molarMassFuel;
            double oxidizerMolarFlowRate = oxidizerMassFlowRate / molarMassOxidizer;
            double totalMolarFlowRate = fuelMolarFlowRate + oxidizerMolarFlowRate;
            double fuelMassFractionMolar = fuelMolarFlowRate / totalMolarFlowRate;
            double oxidizerMassFractionMolar = oxidizerMolarFlowRate / totalMolarFlowRate;
            double fuelEnthalpy = CalculateEnthalpy( molarMassFuel, fuelMassFractionMolar, initialTemperature );
            double oxidizerEnthalpy = CalculateEnthalpy( molarMassOxidizer, oxidizerMassFractionMolar, initialTemperature );
            double initialEnthalpy = (fuelEnthalpy * fuelMassFlowRate + oxidizerEnthalpy * oxidizerMassFlowRate) / massFlowRate;
            double initialDensity = initialPressure * molarMassFuel * fuelMassFractionMolar / (gasConstant * initialTemperature) + initialPressure * molarMassOxidizer * oxidizerMassFractionMolar / (gasConstant * initialTemperature);
            double initialVelocity = massFlowRate / (initialDensity * Math.PI * Math.Pow( 0.5, 2 ));
            double initialMass = 1; // kg

            double deltaMass, deltaMomentum, deltaEnergy, deltaFuelMassFlowRate, deltaOxidizerMassFlowRate, deltaFuelEnthalpy, deltaOxidizerEnthalpy;
            double currentTemperature = initialTemperature, currentPressure = initialPressure, currentMass = initialMass, currentEnthalpy = initialEnthalpy, currentDensity = initialDensity, currentVelocity = initialVelocity;

            for( int i = 0; i < numSteps; i++ )
            {
                // Calculate change in mass flow rates
                deltaFuelMassFlowRate = -massFlowRate * fuelMassFractionMolar;
                deltaOxidizerMassFlowRate = -massFlowRate * oxidizerMassFractionMolar;

                // Calculate change in fuel and oxidizer enthalpies
                deltaFuelEnthalpy = CalculateEnthalpy( molarMassFuel, fuelMassFractionMolar, currentTemperature + 0.5 * timeStep ) - fuelEnthalpy;
                deltaOxidizerEnthalpy = CalculateEnthalpy( molarMassOxidizer, oxidizerMassFractionMolar, currentTemperature + 0.5 * timeStep ) - oxidizerEnthalpy;

                // Calculate change in mass, momentum, and energy
                deltaMass = -massFlowRate * timeStep;
                deltaMomentum = massFlowRate * currentVelocity * timeStep;
                deltaEnergy = (combustionHeatRelease * massFlowRate * timeStep) + (deltaFuelMassFlowRate * deltaFuelEnthalpy) + (oxidizerMassFlowRate * deltaOxidizerEnthalpy);
                // Calculate new values for fuel and oxidizer mass flow rates and enthalpies
                fuelMassFlowRate += deltaFuelMassFlowRate;
                oxidizerMassFlowRate += deltaOxidizerMassFlowRate;
                fuelEnthalpy += deltaFuelEnthalpy;
                oxidizerEnthalpy += deltaOxidizerEnthalpy;

                // Calculate new values for mass, momentum, energy, and enthalpy
                currentMass += deltaMass;
                currentVelocity += deltaMomentum / currentMass;
                currentEnthalpy += deltaEnergy / currentMass;
                currentDensity = currentPressure * molarMassFuel * fuelMassFractionMolar / (gasConstant * currentTemperature) + currentPressure * molarMassOxidizer * oxidizerMassFractionMolar / (gasConstant * currentTemperature);
                currentPressure = (currentDensity * gasConstant * currentTemperature) / (molarMassFuel * fuelMassFractionMolar + molarMassOxidizer * oxidizerMassFractionMolar);
                currentTemperature = currentEnthalpy / (specificHeatRatio * gasConstant);

                // Check if chamber pressure has dropped to the exhaust gas pressure
                if( currentPressure <= exhaustGasPressure )
                {
                    // Chamber pressure has dropped to the exhaust gas pressure, combustion has finished
                    break;
                }
                else if( i == numSteps - 1 )
                {
                    // Maximum number of steps reached, combustion has not finished
                    Console.WriteLine( "Combustion did not finish within the maximum number of steps." );
                }

                Console.WriteLine( i );
                Console.WriteLine( "c temperature: " + currentTemperature + " K" );
                Console.WriteLine( "c pressure: " + currentPressure + " Pa" );
                Console.WriteLine( "c mass: " + currentMass + " kg" );
                Console.WriteLine( "c velocity: " + currentVelocity + " m/s" );
            }

            // Print final results
            Console.WriteLine( "Final temperature: " + currentTemperature + " K" );
            Console.WriteLine( "Final pressure: " + currentPressure + " Pa" );
            Console.WriteLine( "Final mass: " + currentMass + " kg" );
            Console.WriteLine( "Final velocity: " + currentVelocity + " m/s" );
        }
    }
}