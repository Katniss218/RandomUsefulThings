using System;
using System.Collections.Generic;
using System.Text;

namespace RandomUsefulThings.Physics
{
    class AreaMomentOfInertia
    {
        // DO NOT CONFUSE WITH MASS MOMENT OF INERTIA (how hard it is to change the angular velocity of an object)
        // a.k.a. 2nd moment of area.

        // a.k.a. "resistance to bending when a force is applied along a given axis"
        // value changes when we change the position or orientation of the bending axis. (axis perpendicular to the axis along which the force is applied for a beam).

        // Convention: often denoted with the letter `I`
        // unit is length^4 (i.e. [mm^4] or [in^4]) always positive

        // Polar moment of inertia (often denoted with `J`) represents the resistance to twisting (torsion) around the axis perpendicular to the cross-section.

        public static double GetAreaMomentOfInertia( double[,] elementAreas )
        {
            // split the shape into multiple small elements using a regular 2D grid

            // elementContribution = elementArea * element.YCoord^2                  // YCoord is the distance along the axis of the load.
            // return sum(elementContribution)
            throw new NotImplementedException();
        }

        public static double GetPolarMomentOfInertia( double[,] elementAreas )
        {
            // split the shape into multiple small elements using a regular 2D grid

            // elementContribution = elementArea * distanceFromCentroid^2            // Here we do care about `distanceFromCentroid` being actual euclidean 2D distance.
            // return sum(elementContribution)

            // Perpendicular Axis Theorem
            // also J = Ix + Iy                                                      // (the area moments of inertia along the x and y axes).
            throw new NotImplementedException();
        }

        public static double GetProductMomentOfInertia( double[,] elementAreas )
        {
            // split the shape into multiple small elements using a regular 2D grid

            // elementContribution = elementArea * element.XCoord * element.YCoord
            // return sum(elementContribution)
            throw new NotImplementedException();
        }


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
            double momentOfInertia = System.Math.PI * ((radius * radius * radius * radius) - (innerRadius * innerRadius * innerRadius * innerRadius)) / 4.0;

            return momentOfInertia;
        }



        // centroid = geometric center of a shape (its center of mass).

        public static double GetAreaMomentOfInertia_Rectangle( double lengthParallel, double heightPerpendicular ) // bending axis is centroidal
        {
            return (lengthParallel * (heightPerpendicular * heightPerpendicular * heightPerpendicular)) / 12.0;
        }

        public static double GetAreaMomentOfInertia_Circle( double radius ) // bending axis is centroidal
        {
            // area moment valid for e.g. compressive load inline with normal of the cross-section plane.
            return System.Math.PI * (radius * radius * radius * radius) / 4.0;
        }

        /// <summary>
        /// Given the original centroidal area moment of inertia <paramref name="iOriginal"/>, and the <paramref name="crossSectionArea"/>, calculates the area moment of inertia when the new axis is parallel to the original axis, and displaced by <paramref name="distance"/> in the direction perpendicular to it.
        /// </summary>
        public static double GetAreaMomentOfInertiaParallel( double iOriginal, double crossSectionArea, double distance ) // original axis is centroidal, new axis is parallel to it, displaced in the perpendicular direction by distance.
        {
            // Parallel Axis Theorem
            return iOriginal + (crossSectionArea * (distance * distance));
        }

        // We can constructively add/remove area moments of inertia (like a hollow circle cross-section by momentOfAreaOfBigCircle - momentOfAreaOfSmallCircle)
        // - This can be used to create composite shapes from many primitive shapes with the use of the Parallel Axis Theorem to move them around.

        // an I-beam can be constructed from a centroidal tall rectangle, and 2 shifted non-centroidal wide rectangles.
        // a T-beam can be constructed similarly, and other shapes.

        public static double GetRadiusOfGyration( double iOriginal, double crossSectionArea )
        {
            // return the distance from the axis at which we could condense the area into an infinitely narrow strip
            return System.Math.Sqrt( iOriginal / crossSectionArea );
        }
    }
}
