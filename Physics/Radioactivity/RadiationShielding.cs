using System;
using System.Collections.Generic;
using System.Text;

namespace Physics.Radioactivity
{
    public class RadiationShielding
    {
        const double NaturalLog2 = 3.960841032;

        public static double GetLinAttenuationCoeff( double hvl )
        {
            // hvl in [cm]
            // return in [cm^-1]
            return NaturalLog2 * hvl;
        }

        public static double GetMassAttenuationCoeff( double hvl, double density )
        {

            // hvl in [cm]
            // density in [g/cm^3]
            // return uF in [cm^2/g]
            return GetLinAttenuationCoeff( hvl ) / density;
        }

        /*
        
        HVL table for gamma rays at certain energy level [cm]			
        Absorber	100 keV	200 keV	500 keV
        Air (sea level)	3555	4359	6189
        Water	4.15	5.1	7.15
        Carbon	2.07	2.53	3.54
        Aluminium	1.59	2.14	3.05
        Iron	0.26	0.64	1.06
        Copper	0.18	0.53	0.95
        Lead	0.012	0.068	0.42

        */

        public static float GetGammaThrough( double hvl, double density, double thickness )
        {
            // HVL [cm]
            // Density [g/cm^3]
            // Thickness [cm]
            const float MAX = 1.0f;
            // returns MAX scaled by the percentage of radiation that passed through.

            return (float)(MAX * Math.Exp( -1.0f * GetMassAttenuationCoeff( hvl, density ) * density * thickness ));
        }

        public static float GetNeutronsThrough( double thickness, double atomicMass, double density, double neutronCrossSection )
        {
            // neutron x-section table: https://www.nndc.bnl.gov/sigma/index.jsp

            double nuclearDensity = GetNuclearDensity( atomicMass, density );
            double characteristicLength = GetCharacteristicLength( nuclearDensity, neutronCrossSection );

            const float MAX = 1.0f;
            return (float)(MAX * (1.0 - Math.Exp( -1.0 * thickness / characteristicLength )));
        }

        public static double GetCharacteristicLength( double nuclearDensity, double neutronCrossSection )
        {
            // neutron x-section table: https://www.nndc.bnl.gov/sigma/index.jsp
            const double BARN_TO_CM2 = 1.00E-24;
            // Nuclear Density [count/cm^3]	
            // neutronCrossSection [barn]

            return 1.0 / (nuclearDensity * neutronCrossSection * BARN_TO_CM2);
        }

        public static double GetNuclearDensity( double atomicMass, double density )
        {
            const double avogadro = 6.02252E+23;

            // atomicMass in dalton (u)
            // Density [g/cm^3]
            // return Nuclear Density [count/cm^3]	
            return (atomicMass / density) * avogadro;
        }
    }
}
