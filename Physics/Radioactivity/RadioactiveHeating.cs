using System;
using System.Collections.Generic;
using System.Text;

namespace Physics.Radioactivity
{
    public static class RadioactiveHeating
    {
        // radioactive decay equation
        // dQ/dt = λ * N * ΔE
        // where Q is the heat produced, λ is the decay constant, N is the number of radioactive atoms, and ΔE is the energy released per decay.
        // λ = ln(2) / t1/2 - ln2 over halflife

        [Obsolete( "it's wrong" )]
        public static double CalculatePu238Temperature( double mass )
        {
            // C# method that uses the radioactive decay equation and the Stefan-Boltzmann law to calculate the equilibrium temperature of a sample of plutonium-238:
            double halfLife = 87.7 * 365.25 * 24 * 60 * 60; // in seconds
            double decayConstant = Math.Log( 2 ) / halfLife;
            double energyPerDecay = 5.59 * Math.Pow( 10, 6 ) * 1.602 * Math.Pow( 10, -19 ); // in Joules
            double numAtoms = mass / (238.05 * Math.Pow( 10, -3 )) * 6.022 * Math.Pow( 10, 23 );
            double heatRate = decayConstant * numAtoms * energyPerDecay;
            // heatRate is dQ/dt

            double radius = Math.Pow( mass / (19.8 * Math.Pow( 10, 3 )), 1.0 / 3.0 ); // wrong?
            double surfaceArea = 4 * Math.PI * Math.Pow( radius, 2 );
            double sbConstant = 5.67 * Math.Pow( 10, -8 );
            double temperature = heatRate / (surfaceArea * sbConstant);

            return temperature;
        }
    }
}
