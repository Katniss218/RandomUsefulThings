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

        public static double GetVolumeSphere( double mass, double density )
        {
            return mass / density;
        }

        public static double GetRadiusSphere( double volume )
        {
            return Math.Pow( (3.0 * volume) / (4.0 * Math.PI), 1.0 / 3.0 );
        }

        public static double GetEqulibriumTemp( double netEmittance, double surfaceArea )
        {
            double sbConstant = 5.67e-8; // Stefan-Boltzmann constant SIGMA in W/(m^2*K^4)
            return Math.Pow( netEmittance / (sbConstant * surfaceArea), 0.25 );
        }

        public static double CalculateNumAtoms( double mass, double atomicMass )
        {
            // atomic mass in kg/mol

            double avogadro = 6.02214076e23; // Avogadro's number
            double numMoles = mass / atomicMass; // number of moles of Pu-238
            double numAtoms = numMoles * avogadro; // number of atoms of Pu-238

            return numAtoms;
        }

        /// <param name="energyPerDecay">Energy per decay of Pu-238 in [Joules]</param>
        /// <param name="halfLife">Half-life of Pu-238 in [seconds]</param>
        /// <param name="mass">Mass in [kg].</param>
        /// <returns>[Joules/second] ???</returns>
        public static double CalculateDecayHeating( double energyPerDecay, double halfLife, double mass )
        {
            // Decay rate calculation
            double decayConstant = Math.Log( 2 ) / halfLife;
            double numAtoms = CalculateNumAtoms( mass, 238.05 * 1e-3 );
            double decayRate = decayConstant * numAtoms;

            return decayRate * energyPerDecay;
        }

        [Obsolete( "Seems to work" )]
        public static double CalculatePu238Temperature( double mass )
        {
            // Constants
            double density = 19800.0; // Density of Pu-238 in kg/m^3
            double energyPerDecay = 8.95e-13; // Energy per decay of Pu-238 in Joules
            double halfLife = 87.74 * 365.25 * 24 * 60 * 60; // Half-life of Pu-238 in seconds

            // for a sphere that contains other additives than just Pu238 (that might be inert), we need to find the radius separately, from the average density of the sphere.
            double volume = GetVolumeSphere( mass, density );
            double radius = GetRadiusSphere( volume );

            // calculate temperature with Stefan-Boltzmann law from total net emittance.
            // `Total emittance = heat produced` because ALL heat produced HAS TO BE emitted to achieve equilibrium.
            double surfaceArea = 4 * Math.PI * Math.Pow( radius, 2 );
            double temperature = GetEqulibriumTemp( CalculateDecayHeating( energyPerDecay, halfLife, mass ), surfaceArea );

            return temperature;
        }
    }
}
