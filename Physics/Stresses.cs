using System;
using System.Collections.Generic;
using System.Text;

namespace Physics
{
    class Stresses
    {
        // shear stress = force / area

        // area = the total area that the force can shear.


        [Obsolete("unconfirmed")]
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
