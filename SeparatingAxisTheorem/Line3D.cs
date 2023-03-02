using System;
using System.Collections.Generic;
using System.Text;

namespace Geometry
{
    public struct Line3D
    {
        public Vector3 Point { get; }
        public Vector3 Direction { get; }

        public static Vector3? LinePlaneIntersection( Vector3 linePoint, Vector3 lineDir, Vector3 planePoint, Vector3 planeNormal )
        {
            // Distance from `l.point` to the point of line-plane intersection (if denominator != 0)
            //     dist = dot((p.point - l.point), p.normal) / dot( l.dir, p.normal )
            float denom = Vector3.Dot( lineDir, planeNormal );

            if( Math.Abs( denom ) < 0.000001f )
            {
                // line is parallel to the plane.
                return null;
            }

            // This is NOT supposed to be normalized. We take the dot product of a NON-UNIT vector.
            // Otherwise, it doesn't work.
            Vector3 lineToPlane = planePoint - linePoint;
            // Returns the distance from the `planePoint` to the intersection with the plane.
            float distance = Vector3.Dot( lineToPlane, planeNormal ) / denom;

            // This can also calculate the distance along the normal vector instead, just don't divide by `denom`.
            //     It works because the dot product returns the SIGNED magnitude of `lineToPlane` projected onto `planeNormal`.
            // `planeNormal` has to be a unit vector for it to work.
            //     If it's not normalized, then the distance is in units of 1/magnitude => (1 / magn) * returnedDist = realDist
            //     where `realDist` is the distance returned if `planeNormal` was a unit vector.

            // we can check here if the distance is negative and reject the intersection (this case would be ray-plane).

            // We can also use this to check for linesegment-plane intersections.
            //     If `linePoint` is vertex1 and `lineDir` is (vertex2 - vertex1) and is NOT NORMALIZED.
            //     Just check if `t` is in [0..1]. If it was normalized, then it would lie in [0..length_of_edge].
            // And it is not dependent on which vertex we pick. If we pick the other vertex, then the sign of `t` also changes
            //   and the distance is now measured to the other side, which is equivalent.

            return linePoint + (distance * lineDir);
        }

        public static Vector3? ProjectedLineIntersection(
            Vector3 line1Point, Vector3 line1Dir,
            Vector3 line2Point, Vector3 line2Dir,
            Vector3 projectionDir )
        {
            // projects line1 along the projectiondir and calculate where it intersects with line2 (if anywhere).

            // We will represent the projected line as a plane.
            Vector3 planeNormal = Vector3.Cross( line1Dir, projectionDir );

            float dot = Vector3.Dot( line2Dir, planeNormal );
            if( Math.Abs( dot ) < 0.000001f )
            {
                // the plane representing the projection of the 1st line is parallel to the 2nd line.
                return null;
            }

            // Plane is the projection of line1, calculate where line2 hits that projection.
            Vector3? planeLine2Intersection = LinePlaneIntersection( line2Point, line2Dir, line1Point, planeNormal );
            if( planeLine2Intersection == null )
            {
                return null;
            }
            // This can do plane to edge intersection as well, if we don't normalize the direction.
            // So we could do line projected onto an edge.

            Vector3 perpendicularProjDir = projectionDir.ProjectOntoPlane( line1Dir );
            // This works because the vector pointing from any point on the line to the intersection will have a positive dot product with the projection direction.
            // The projection direction needs to be aligned so that it's perpendicular to the line direction (this doesn't actually even change the result if it was done earlier).
            if( Vector3.Dot( (planeLine2Intersection.Value - line1Point), perpendicularProjDir ) <= 0 ) // reject the back half of the plane.
            {
                // The point lies "behind" the projection, i.e. would intersect if we inverted the projection direction.
                return null;
            }
            return planeLine2Intersection;
        }
    }
}
