using System;
using System.Collections.Generic;
using System.Text;

namespace RandomUsefulThings.Physics
{
    public static class PoissonsRatio
    {
        // Poisson's Ratio is the ratio transverseStrain / axialStrain
        //      or in other words, the ratio of how much the material bulges out, to how much it's squished.
        //      When an object is stressed in a single direction (axially).

        // Most materials have Poisson's ratios between 0.0 and 0.5.
        // Softer materials tend to have higher values, brittle materials tend to have lower.
        /*
        rubber	            0.4999
        gold	            0.42 – 0.44
        saturated clay soil 0.40 – 0.49
        magnesium	        0.252 – 0.289
        titanium	        0.265 – 0.34
        copper	            0.33
        aluminium-alloy     0.32
        clay	            0.30 – 0.45
        stainless steel	    0.30 – 0.31
        steel	            0.27 – 0.30
        cast iron	        0.21 – 0.26
        sand	            0.20 – 0.455
        concrete	        0.1 – 0.2
        glass	            0.18 – 0.3
        metallic glass	    0.276 – 0.409
        foam	            0.10 – 0.50
        cork	            0.0
        */

        public static float Steel_Mild() => 0.3f;
        public static float StainlessSteel_304L() => 0.3f;
        public static (float min, float max) Rubber() => (0.4999f, 0.4999f);
        public static float Fe() => 0.3f; // Iron
        public static float Mg() => 0.35f; // Magnesium
        public static float Al() => 0.33f; // Aluminum
        public static float Cu() => 0.36f; // Copper
        public static float Ag() => 0.37f; // Silver
        public static float Au() => 0.42f; // Gold
        public static float Pt() => 0.39f; // Platinum
        public static float Ni() => 0.31f; // Nickel
        public static float Mb() => 0.32f; // Molybdenum
        public static float Zn() => 0.25f; // Zinc
        public static float Sn() => 0.33f; // Tin
        public static float Ta() => 0.35f; // tantalum
        public static float Th() => 0.27f; // Thorium
        public static float W() => 0.28f; // Tungsten

        public static float U() => 0.21f; // Uranium
        public static (float min, float max) Pu() => (0.15f, 0.21f); // Plutonium
        public static float Pb() => 0.44f; // Lead
    }
}