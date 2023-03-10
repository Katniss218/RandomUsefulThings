using System;
using System.Collections.Generic;
using System.Text;

namespace Geometry
{
    public class Sphere
    {
        /// <summary>
        /// Z+ is up (V+), seam is in the direction of X-.
        /// U increases from 0 on the Y- side of the seam, decreases from 1 on the Y+ side.
        /// </summary>
        public static void CartesianToUV( double x, double y, double z, out double u, out double v )
        {
            // If we know for sure that the coordinates are for a unit sphere, we can remove the radius calculation entirely.
            // Also remove the division by radius, since division by 1 (unit-sphere) doesn't change the divided number.
            double radius = Math.Sqrt( x * x + y * y + z * z );
            double theta = Math.Atan2( y, x );
            double phi = Math.Acos( z / radius );


            // The thing returned here seems to also be the lat/lon but normalized to the range [0..1]
            // The outputs are normalized by the respective denominators.
            u = (theta + Math.PI) / (2 * Math.PI); // dividing by 2 * pi ensures that the output is in [0..1]
            v = phi / Math.PI;
        }


        /// <summary>
        /// Returns the coordinates on a unit sphere.
        /// </summary>
        public static void UVToCartesian( double u, double v, out double x, out double y, out double z )
        {
            double theta = u * (2 * Math.PI) - Math.PI; // Multiplying by 2 because the input is in range [0..1]
            double phi = v * Math.PI;


            x = Math.Sin( phi ) * Math.Cos( theta );
            y = Math.Sin( phi ) * Math.Sin( theta );
            z = Math.Cos( phi );
        }

    }
}
