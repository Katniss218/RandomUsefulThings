using System;
using System.Collections.Generic;
using System.Text;

namespace RandomUsefulThings.Physics
{
    public static class Stress
    {
        // Stress is a quantity that describes the distribution of internal forces within a body.
        // Stress = internalForce/area (unit [N/m^2] i.e. [Pa] (metric) or [lb/in^2] (US))

        // A body like a bar will fail if its maximum allowable stress is smaller than the actual stress.
        // uniaxial-loaded bar => `sigmaYield = const`, and `sigmaNormal = Force / CrossSectionalArea`

        // convention: tensile stresses are positive, compressive stresses are negative.




        // sigma1, sigma2, sigma3 are the principal stress axes.
        // by convention, sigma1 >= sigma2 >= sigma3
        // Convention: shear stress is `tau`, normal stress is `sigma`

        // Principal stresses:
        // - Occur when the stress element is rotated in such a way that the shear stresses are 0.
        // - Are the min and max normal stresses.
        // - sigma1 is the maximum principal stress, sigma2 is the minimum principal stress (2D), for 3D, sigma3 is the minimum, and sigma2 is between them.

        // "3 principal stresses" are the three stresses that occur on the 3 principal axes.
        // they are related to the eigenvalues of the stress tensor.

        // yielding is only caused by stresses that cause shape distortion.

        // triaxialStress = hydrostaticStress + deviatoricStress

        // - "deviatoric" because it causes the shape to deviate from the original?


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


        public static double GetMaximumShearStress( double normalStressX, double normalStressY, double shearStressXY )
        {
            // From radius of mohr's circle.
            // τmax = sqrt((σx - σy/2)^2 + τxy^2)
            double c1 = (normalStressX - normalStressY) / 2.0;

            return System.Math.Sqrt( (c1 * c1) + (shearStressXY * shearStressXY) );
        }

        // principlal stresses can be calculated by getting the center of the mohr's circle, and adding or subtracting the radius.


        public static double GetYieldStrength( double referenceStress, double kY, double avgGrainDiameter )
        {
            // Hall-Petch Equation
            // sigma0 + ky * d^-0.5
            return referenceStress + kY * (1.0 / System.Math.Sqrt( avgGrainDiameter ));
        }


        // stress tensor 2D
        // [ sigmaX, TauXY  ]
        // [ TauXY,  SigmaY ]


        // stress tensor 3D
        // [ sigmaX, TauXY ,  TauXZ  ]
        // [ TauYX,  SigmaY, TauYZ   ]
        // [ TauZX,  TauZY ,  SigmaZ ]

        public static (double normalStressX, double normalStressY, double shearStress) Rotate2D( double normalStressX, double normalStressY, double shearStressXY, double angle )
        {
            // angle (when positive) is a counterclockwise rotation from original x to the new x axis.

            double normalXOut = ((normalStressX + normalStressY) / 2.0) + (((normalStressX - normalStressY) / 2.0) * System.Math.Cos( 2 * angle )) + (shearStressXY * System.Math.Sin( 2 * angle ));
            double normalYOut = ((normalStressX + normalStressY) / 2.0) - (((normalStressX - normalStressY) / 2.0) * System.Math.Cos( 2 * angle )) - (shearStressXY * System.Math.Sin( 2 * angle ));
            double shearXYOut = -(((normalStressX - normalStressY) / 2.0) * System.Math.Sin( 2 * angle )) + (shearStressXY * System.Math.Cos( 2 * angle ));

            return (normalXOut, normalYOut, shearXYOut);
        }

        // Rotations for which there are no shear stresses are called principal planes.
        // And the normal stresses corresponding to those angles are called principal stresses.



        [Obsolete( "Unconfirmed" )]
        public static (double normalStressX, double normalStressY, double normalStressZ, double shearStressXY, double shearStressYZ, double shearStressZX) Rotate3D( double normalStressX, double normalStressY, double normalStressZ, double shearStressXY, double shearStressYZ, double shearStressZX, double angleX, double angleY, double angleZ )
        {
            // angleX, angleY, and angleZ are the Euler angles defining the rotation sequence (XYZ).

            double cosX = System.Math.Cos( angleX );
            double sinX = System.Math.Sin( angleX );
            double cosY = System.Math.Cos( angleY );
            double sinY = System.Math.Sin( angleY );
            double cosZ = System.Math.Cos( angleZ );
            double sinZ = System.Math.Sin( angleZ );

            // Calculate the transformation matrix based on the Euler angles
            double[,] transformMatrix = new double[,]
            {
                { cosY * cosZ,                      cosY * sinZ,                      -sinY,         0,            0,           0    },
                { sinX * sinY * cosZ - cosX * sinZ, sinX * sinY * sinZ + cosX * cosZ,  sinX * cosY,  0,            0,           0    },
                { cosX * sinY * cosZ + sinX * sinZ, cosX * sinY * sinZ - sinX * cosZ,  cosX * cosY,  0,            0,           0    },
                { 0,                                0,                                 0,            cosY,         sinY,        0    },
                { 0,                                0,                                 0,           -sinX * sinY,  sinX * cosY, cosX },
                { 0,                                0,                                 0,            cosX * sinY, -cosX * cosY, sinX }
            };

            // Create a stress vector from the input stress components
            double[] stressVector = new double[] { normalStressX, normalStressY, normalStressZ, shearStressXY, shearStressYZ, shearStressZX };

            // Transform the stress vector using the transformation matrix
            double[] transformedStressVector = new double[6];
            for( int i = 0; i < 6; i++ )
            {
                transformedStressVector[i] = 0;
                for( int j = 0; j < 6; j++ )
                {
                    transformedStressVector[i] += transformMatrix[i, j] * stressVector[j];
                }
            }

            // Extract the transformed stress components from the transformed stress vector
            double normalStressXOut = transformedStressVector[0];
            double normalStressYOut = transformedStressVector[1];
            double normalStressZOut = transformedStressVector[2];
            double shearStressXYOut = transformedStressVector[3];
            double shearStressYZOut = transformedStressVector[4];
            double shearStressZXOut = transformedStressVector[5];

            return (normalStressXOut, normalStressYOut, normalStressZOut, shearStressXYOut, shearStressYZOut, shearStressZXOut);
        }


        // other useful things to have include:

        // - way to calculate the failure stress on a thin skin of material that's held rigidly between infinitely strong members (aircraft skin, etc).

        /// <param name="columnEffectiveLength">Effective length of the column in [m]</param>
        /// <param name="youngsModulus">The youngs modulus of the material in [Pa]</param>
        /// <param name="areaMomentOfInertia">Calculate the area moment of inertia using units of [m] for dimensions.</param>
        /// <returns>The force in [N] needed to buckle the column.</returns>
        public static double EulersBucklingFormula( double columnEffectiveLength, double youngsModulus, double areaMomentOfInertia )
        {
            // height or length, interchangeable really.
            // effectiveLength = length for a column that is pinned (hinged) at both ends (ends can rotate, but not move).

            // if the column is shaped in such a way that the area moment of inertia is distributed unevenly radially, the axis parallel to the smallest value must be used.
            // - (i.e. for a wooden 2x4, the axis with the 2 should be used)
            return ((System.Math.PI * System.Math.PI) * youngsModulus * areaMomentOfInertia) / (columnEffectiveLength * columnEffectiveLength);

            // for a column that is free at one end and completely fixed (in rotation and translation) at the other, the effective length is 2*length.
            // completely fixed at both ends -> 0.5*length, fixed at one, pinned at the other -> 0.7 * length
            // This formula only really works for fairly long and slender columns.
        }


        public static double CalculateCylinderMass( double height, double radius, double thickness, double density )
        {
            // dimensions in meters
            // density in kg/m^3
            // result in kg
            // Calculate the outer and inner radii of the cylinder
            double outerRadius = radius;
            double innerRadius = radius - thickness;

            double volume = System.Math.PI * height * (System.Math.Pow( outerRadius, 2 ) - System.Math.Pow( innerRadius, 2 ));

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

            double momentOfInertia = AreaMomentOfInertia.GetMomentOfInertiaHollowCylinder( radius, thickness );

            // bending stiffness K
            // It is a function of the Young's modulus E, the second moment of area I of the beam cross-section about the axis of interest, length of the beam and beam boundary condition.
            double bendingStiffness = (System.Math.PI * System.Math.PI) * modulusOfElasticity * momentOfInertia / ((height * height) * (1.0 - (poissonsRatio * poissonsRatio)));

            // critical buckling load F.
            double criticalBucklingLoad = bendingStiffness * System.Math.Pow( System.Math.PI / (2 * height), 2 );
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
            double secondMomentOfArea = System.Math.PI / 4 * (System.Math.Pow( outerRadius, 4 ) - System.Math.Pow( innerRadius, 4 ));
            double effectiveLength = length / boundaryConditionFactor;
            double maxAxialLoad = System.Math.Pow( System.Math.PI, 2 ) * elasticModulus * secondMomentOfArea / System.Math.Pow( effectiveLength, 2 );
            if( maxAxialLoad > yieldStrength )
            {
                // The cylinder will yield and deform permanently
                throw new InvalidOperationException( "The maximum axial load exceeds the yield strength of the material." );
            }
            return maxAxialLoad;
        }

    }
}
