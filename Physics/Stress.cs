using System;
using System.Collections.Generic;
using System.Text;

namespace Physics
{
    public static class Stress
    {
        // Stress is a quantity that describes the distribution of internal forces within a body.
        // Stress = internalForce/area (unit [N/m^2] i.e. [Pa] (metric) or [lb/in^2] (US))

        // A body like a bar will fail if its maximum allowable stress is smaller than the actual stress.
        // uniaxial-loaded bar => `sigmaYield = const`, and `sigmaNormal = Force / CrossSectionalArea`

        // convention: tensile stresses are positive, compressive stresses are negative.





        // "3 principal stresses" are the three stresses that occur on the 3 principal axes.
        // they are related to the eigenvalues of the stress tensor.

        // yielding is only caused by stresses that cause shape distortion.

        // triaxialStress = hydrostaticStress + deviatoricStress

        // - "deviatoric" because it causes the shape to deviate from the original?


        // sigma1, sigma2, sigma3 are the principled stress axes.
        // by convention, sigma1 >= sigma2 >= sigma3
        // Convention: shear stress is `tau`, normal stress is `sigma`

        /// <summary>
        /// Decent for brittle. Don't use for ductile.
        /// </summary>
        public static class Rankine // "Maximum principal stress theory"
        {
            // sigma1 = sigmaYield, sigmaUltimate
            // sigma3 = -sigmaYield, -sigmaUltimate

            // sigma1 is maxStress
            // sigma3 is minStress
        }

        /// <summary>
        /// Good for ductiles, easier to apply than <see cref="VonMises"/>.
        /// </summary>
        public static class Tresca // "Maximum shear stress theory"
        {
            // tauMax = tauYield
            // tauMax = (sigmaMax - sigmaMin) / 2

            // sigmaMax - sigmaMin = sigmaYield


            // special case of CoulombMohr?
        }

        /// <summary>
        /// Very good for ductiles.
        /// </summary>
        public static class VonMises // "Maximum distortion energy theory"
        {
           /// "Yielding occurs when the `maximum distortion energy` is equal to the distortion energy at yielding in a uniaxial tensile test.

            // sigmaYield = sqrt(0.5 *((sigma1 - sigma2)^2 + (sigma2 - sigma3)^2 + (sigma3 - sigma1)^2))

            // VonMises equivalent stress can be returned as an output from static analysis, and used to identify areas at risk of yielding
        }

        // brittles typically have 2 different ultimate strengths - for tension and compression.
        // - strain at fracture < 5% is often considered brittle.
        // elastic materials typically have the same (or very similar) values for tensile and compressive strengths.
        
        /// <summary>
        /// Decent for brittle.
        /// </summary>
        public static class CoulombMohr
        {

        }

        /// <summary>
        /// Good for brittle.
        /// </summary>
        public static class ModifiedMohr
        {

        }

        public static double GetYieldStrength( double referenceStress, double kY, double avgGrainDiameter )
        {
            // Hall-Petch Equation
            // sigma0 + ky * d^-0.5
            return referenceStress + kY * (1.0 / System.Math.Sqrt( avgGrainDiameter ));
        }


        // stress tensor @D
        // [ sigmaX, TauXY  ]
        // [ TauYX,  SigmaY ]


        // stress tensor 3D
        // [ sigmaX, TauXY ,  TauXZ  ]
        // [ TauYX,  SigmaY, TauYZ   ]
        // [ TauZX,  TauZY ,  SigmaZ ]


        // other useful things to have include:

        // - way to calculate the failure stress on a thin skin of material that's held rigidly between infinitely strong members (aircraft skin, etc).


        public static double GetMomentOfInertiaHollowCylinder( double radius, double thickness )
        {
            // x-sections around axis of load:
            // filled circle
            //      pi/2 * r^4                          (torsional)
            //      pi/4 * r^4                          (perpendicular)
            // hollow circle
            //      pi/2 * (rOuter^4 - rInner^4)        (torsional)
            //      pi/4 * (rOuter^4 - rInner^4)        (perpendicular)
            // thin-walled circle
            //      pi * r^4 * thickness                (perpendicular)

            // moment of inertia describes the resistance to bending/torsional twisting of a shape.

            double innerRadius = radius - thickness;

            // 2nd moment of area (moment of inertia) I
            double momentOfInertia = Math.PI * ((radius * radius * radius * radius) - (innerRadius * innerRadius * innerRadius * innerRadius)) / 4.0;

            return momentOfInertia;
        }

        public static double CalculateCylinderMass( double height, double radius, double thickness, double density )
        {
            // dimensions in meters
            // density in kg/m^3
            // result in kg
            // Calculate the outer and inner radii of the cylinder
            double outerRadius = radius;
            double innerRadius = radius - thickness;

            double volume = Math.PI * height * (Math.Pow( outerRadius, 2 ) - Math.Pow( innerRadius, 2 ));

            double mass = volume * density;
            return mass;
        }

        // shear stress = force / area

        // area = the total area that the force can shear.

        [Obsolete( "seems to give roughlyyyy right result" )]
        public static double ComputeMaxLoad( double height, double radius, double thickness, double modulusOfElasticity, double poissonsRatio )
        {
            /*
            double height = 10; // height of the fuselage in meters
            double radius = 1; // radius of the fuselage in meters
            double thickness = 0.01; // thickness of the fuselage in meters
            double E = 70E9; // modulus of elasticity of the material in Pa
            double nu = 0.3; // Poisson's ratio of the material
            output in newtons [N]
            */

            double momentOfInertia = GetMomentOfInertiaHollowCylinder( radius, thickness );

            // bending stiffness K
            // It is a function of the Young's modulus E, the second moment of area I of the beam cross-section about the axis of interest, length of the beam and beam boundary condition.
            double bendingStiffness = (Math.PI * Math.PI) * modulusOfElasticity * momentOfInertia / ((height * height) * (1.0 - (poissonsRatio * poissonsRatio)));

            // critical buckling load F.
            double criticalBucklingLoad = bendingStiffness * Math.Pow( Math.PI / (2 * height), 2 );
            return criticalBucklingLoad;
        }

        [Obsolete( "unconfirmed" )]
        public static double CalculateMaxAxialLoad( double outerRadius, double innerRadius, double length, double elasticModulus, double yieldStrength, double boundaryConditionFactor )
        {
            /*
            outerRadius: The outer radius of the hollow cylinder.
            innerRadius: The inner radius of the hollow cylinder.
            length: The length of the hollow cylinder.
            elasticModulus: The Young's modulus of the material of the hollow cylinder.
            yieldStrength: The yield strength of the material of the hollow cylinder.
            boundaryConditionFactor: The effective length factor, which depends on the boundary conditions. This value should be 1 for a fixed-fixed boundary condition and 0.5 for a fixed-free condition.
            */
            double secondMomentOfArea = Math.PI / 4 * (Math.Pow( outerRadius, 4 ) - Math.Pow( innerRadius, 4 ));
            double effectiveLength = length / boundaryConditionFactor;
            double maxAxialLoad = Math.Pow( Math.PI, 2 ) * elasticModulus * secondMomentOfArea / Math.Pow( effectiveLength, 2 );
            if( maxAxialLoad > yieldStrength )
            {
                // The cylinder will yield and deform permanently
                throw new InvalidOperationException( "The maximum axial load exceeds the yield strength of the material." );
            }
            return maxAxialLoad;
        }

    }
}
