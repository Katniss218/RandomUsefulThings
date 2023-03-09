using System;
using System.Collections.Generic;
using System.Text;

namespace Physics
{
    public class Stresses
    {
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
