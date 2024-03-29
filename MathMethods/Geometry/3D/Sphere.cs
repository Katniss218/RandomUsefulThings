﻿using System;
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

        [Obsolete( "Unconfirmed" )]
        public static double[] CartesianToSpherical( double[] cartesian )
        {
            double x = cartesian[0];
            double y = cartesian[1];
            double z = cartesian[2];
            double r = Math.Sqrt( x * x + y * y + z * z );
            double theta = Math.Atan2( y, x );
            double phi = Math.Acos( z / r );
            return new double[] { r, theta, phi };
        }

        [Obsolete( "Unconfirmed" )]
        public static double[] SphericalToCartesian( double[] spherical )
        {
            double r = spherical[0];
            double theta = spherical[1];
            double phi = spherical[2];
            double x = r * Math.Sin( phi ) * Math.Cos( theta );
            double y = r * Math.Sin( phi ) * Math.Sin( theta );
            double z = r * Math.Cos( phi );
            return new double[] { x, y, z };
        }

        [Obsolete( "Unconfirmed" )]
        public static double GetSphericalDistance( Vector3 point1, Vector3 point2 )
        {
            double cosDistance = Vector3.Dot( point1, point2 );
            double distance = Math.Acos( cosDistance );
            return distance;
        }


        /// <summary>
        /// Calculates the height of a truncated unit sphere with the given volume as a [0..1] percentage of the unit sphere's volume.
        /// </summary>
        public static float SolveHeightOfTruncatedSphere( float volumePercent )
        {
            // https://math.stackexchange.com/questions/2364343/height-of-a-spherical-cap-from-volume-and-radius

            const float UnitSphereVolume = 4.18879020479f; // 4/3 * pi     -- radius=1
            const float TwoPi = 6.28318530718f;            // 2 * pi       -- radius=1
            const float Sqrt3 = 1.73205080757f;

            float Volume = UnitSphereVolume * volumePercent;

            float A = 1.0f - ((3.0f * Volume) / TwoPi); // A is a coefficient, [-1..1] for volumePercent in [0..1]
            float OneThirdArccosA = 0.333333333f * (float)Math.Acos( A );
            float height = Sqrt3 * (float)Math.Sin( OneThirdArccosA ) - (float)Math.Cos( OneThirdArccosA ) + 1.0f;
            return height;
        }
    }
}