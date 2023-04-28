using System;
using System.Collections.Generic;
using System.Text;

namespace RandomUsefulThings.Physics
{
    public static class YoungsModulus
    {
        // Young's modulus is the slope of the elastic deformation region of the stress-strain curve.
        // It represents the tendency of an object to deform along a given axis when opposing forces are applied along that same axis.
        //      or, how much stress (force) for an amount of strain (deformation) - per unit area/volume/etc ofc.
        //      stress / strain

        /// <summary>
        /// Calculates the youngs modulus from a known stress and strain.
        /// </summary>
        public static double GetYoungsModulus( double stress, double strain )
        {
            // Stress and strain should be along the same axis. The axis that is loaded by a force.
            return stress / strain;
        }

        [Obsolete( "Unconfirmed" )]
        public static double GetBulkModulus( double stressX, double stressY, double stressZ, double strainX, double strainY, double strainZ )
        {
            // Compute the bulk modulus, which is a measure of a material's resistance to compression.
            double deltaVolume = (strainX + strainY + strainZ);
            double avgStress = (stressX + stressY + stressZ) / 3.0;
            return -avgStress / deltaVolume;
        }

        // Young's moduli are typically so large that they are expressed in gigapascals (GPa).

        /*
         * first number range is in GPa, 2nd is in imperial (Mpsi)
        Aluminium (13Al)	68	9.86
        Amino-acid molecular crystals	21 – 44	3.05 – 6.38
        Aramid (for example, Kevlar)	70.5 – 112.4	10.2 – 16.3
        Aromatic peptide-nanospheres	230 – 275	33.4 – 39.9
        Aromatic peptide-nanotubes	19 – 27	2.76 – 3.92
        Bacteriophage capsids	1 – 3	0.145 – 0.435
        Beryllium (4Be)	287	41.6	
        Bone, human cortical	14	2.03	
        Brass	106	15.4	
        Bronze	112	16.2	
        Carbon nitride (CN2)	822	119	
        Carbon-fiber-reinforced plastic (CFRP), 50/50 fibre/matrix, biaxial fabric	30 – 50	4.35 – 7.25
        Carbon-fiber-reinforced plastic (CFRP), 70/30 fibre/matrix, unidirectional, along fibre	181	26.3	
        Cobalt-chrome (CoCr)	230	33.4	
        Copper (Cu), annealed	110	16	
        Diamond (C), synthetic	1050 – 1210	152 – 175
        Diatom frustules, largely silicic acid	0.35 – 2.77	0.051 – 0.058
        Flax fiber	58	8.41	
        Float glass	47.7 – 83.6	6.92 – 12.1	
        Glass-reinforced polyester (GRP)	17.2	2.49
        Gold	77.2	11.2	
        Graphene	1050	152	
        Hemp fiber	35	5.08	
        High-density polyethylene (HDPE)	0.97 – 1.38	0.141 – 0.2
        High-strength concrete	30	4.35	
        Lead (82Pb), chemical	13	1.89	
        Low-density polyethylene (LDPE), molded	0.228	0.0331	
        Magnesium alloy	45.2	6.56
        Medium-density fiberboard (MDF)	4	0.58
        Molybdenum (Mo), annealed	330	47.9
        Monel	180	26.1	[11]
        Mother-of-pearl (largely calcium carbonate)	70	10.2
        Nickel (28Ni), commercial	200	29
        Nylon 66	2.93	0.425
        Osmium (76Os)	525 – 562	76.1 – 81.5
        Osmium nitride (OsN2)	194.99 – 396.44	28.3 – 57.5
        Polycarbonate (PC)	2.2	0.319
        Polyethylene terephthalate (PET), unreinforced	3.14	0.455
        Polypropylene (PP), molded	1.68	0.244
        Polystyrene, crystal	2.5 – 3.5	0.363 – 0.508
        Polystyrene, foam	0.0025 – 0.007	0.000363 – 0.00102
        Polytetrafluoroethylene (PTFE), molded	0.564	0.0818
        Rubber, small strain	0.01 – 0.1	0.00145 – 0.0145
        Silicon, single crystal, different directions	130 – 185	18.9 – 26.8
        Silicon carbide (SiC)	90 – 137	13.1 – 19.9
        Single-walled carbon nanotube   >1000   >140
        Steel, A36	200	29
        Stinging nettle fiber	87	12.6
        Titanium (22Ti)	116	16.8
        Titanium alloy, Grade 5	114	16.5
        Tooth enamel, largely calcium phosphate	83	12
        Tungsten carbide (WC)	600 – 686	87 – 99.5
        Wood, American beech	9.5 – 11.9	1.38 – 1.73
        Wood, black cherry	9 – 10.3	1.31 – 1.49	
        Wood, red maple	9.6 – 11.3	1.39 – 1.64
        Wrought iron	193	28
        Yttrium iron garnet (YIG), polycrystalline	193	28
        Yttrium iron garnet (YIG), single-crystal	200	29
        Zinc (30Zn)	108	15.7
        Zirconium (40Zr), commercial	95	13.8
        */
    }
}