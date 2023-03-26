using Geometry;
using System;
using System.Collections.Generic;
using System.Text;

namespace Miscellaneous
{
    class Orbit3D
    {
        // Formula for a 2D (!!!) orbit shape with periapsis on the left, at y=0, E is eccentricity, P is the periapsis.
        // x and y are coordinates.
        // (1 - E^2)*x^2 - 2Px + y^2 = 0

        const float G = 6.6743e-11f;
        /*
        [Obsolete( "unconfirmed" )]
        public static Vector3d CalculateWorldPosition( double semiMajorAxis, double eccentricity, double inclination, double argumentOfPeriapsis, double longitudeOfAscendingNode, double trueAnomaly, double parentMass )
        {
            double gravitationalConstant = 6.6743e-11;
            double parentRadius = 6371000; // radius of Earth in meters

            // Calculate the standard gravitational parameter of the parent body
            double standardGravParameter = gravitationalConstant * parentMass;

            // Convert orbital elements from degrees to radians
            inclination *= Math.PI / 180.0;
            argumentOfPeriapsis *= Math.PI / 180.0;
            longitudeOfAscendingNode *= Math.PI / 180.0;
            trueAnomaly *= Math.PI / 180.0;

            // Calculate the semi-latus rectum and distance to parent body at periapsis
            double semiLatusRectum = semiMajorAxis * (1 - eccentricity * eccentricity);
            double periapsisDistance = semiMajorAxis * (1 - eccentricity);

            // Calculate the position of the satellite in the orbital plane
            double xOrbitalPlane = semiLatusRectum * Math.Cos( trueAnomaly ) / (1 + eccentricity * Math.Cos( trueAnomaly ));
            double yOrbitalPlane = semiLatusRectum * Math.Sin( trueAnomaly ) / (1 + eccentricity * Math.Cos( trueAnomaly ));

            // Apply the inclination, argument of periapsis, and longitude of ascending node to get the position in the world frame
            double x = (Math.Cos( argumentOfPeriapsis ) * Math.Cos( longitudeOfAscendingNode ) - Math.Sin( argumentOfPeriapsis ) * Math.Sin( longitudeOfAscendingNode ) * Math.Cos( inclination )) * xOrbitalPlane
                     - (Math.Sin( argumentOfPeriapsis ) * Math.Cos( longitudeOfAscendingNode ) + Math.Cos( argumentOfPeriapsis ) * Math.Sin( longitudeOfAscendingNode ) * Math.Cos( inclination )) * yOrbitalPlane;
            double y = (Math.Cos( argumentOfPeriapsis ) * Math.Sin( longitudeOfAscendingNode ) + Math.Sin( argumentOfPeriapsis ) * Math.Cos( longitudeOfAscendingNode ) * Math.Cos( inclination )) * xOrbitalPlane
                     + (-Math.Sin( argumentOfPeriapsis ) * Math.Sin( longitudeOfAscendingNode ) + Math.Cos( argumentOfPeriapsis ) * Math.Cos( longitudeOfAscendingNode ) * Math.Cos( inclination )) * yOrbitalPlane;
            double z = Math.Sin( argumentOfPeriapsis ) * Math.Sin( inclination ) * xOrbitalPlane
                     + Math.Cos( argumentOfPeriapsis ) * Math.Sin( inclination ) * yOrbitalPlane;

            // Scale the position to meters and add the parent body's radius to get the world position
            return new Vector3d( x, y, z ) * 1000 + new Vector3d( parentRadius, parentRadius, parentRadius );
        }

        [Obsolete( "unconfirmed" )]
        public static (Vector3d position, Vector3d velocity) CalculateWorldPositionAndVelocity( double semiMajorAxis, double eccentricity, double inclination, double argumentOfPeriapsis, double longitudeOfAscendingNode, double trueAnomaly, Vector3d parentPosition, double parentMass )
        {
            double gravitationalConstant = 6.6743e-11;
            double parentRadius = 6371000; // radius of Earth in meters

            // Calculate the standard gravitational parameter of the parent body
            double standardGravParameter = gravitationalConstant * parentMass;

            // Calculate the specific angular momentum
            double angularMomentumMagnitude = Math.Sqrt( standardGravParameter * semiMajorAxis * (1 - eccentricity * eccentricity) );
            Vector3d angularMomentum = new Vector3d( angularMomentumMagnitude * Math.Cos( inclination * Math.PI / 180.0 ) * Math.Sin( argumentOfPeriapsis * Math.PI / 180.0 ),
                                                     angularMomentumMagnitude * Math.Sin( inclination * Math.PI / 180.0 ) * Math.Sin( argumentOfPeriapsis * Math.PI / 180.0 ),
                                                     angularMomentumMagnitude * Math.Cos( argumentOfPeriapsis * Math.PI / 180.0 ) );

            // Calculate the position and velocity in the orbital plane
            double r = semiMajorAxis * (1 - eccentricity * eccentricity) / (1 + eccentricity * Math.Cos( trueAnomaly * Math.PI / 180.0 ));
            Vector3d positionOrbitalPlane = new Vector3d( r * Math.Cos( trueAnomaly * Math.PI / 180.0 ), r * Math.Sin( trueAnomaly * Math.PI / 180.0 ), 0 );
            Vector3d velocityOrbitalPlane = new Vector3d( -Math.Sqrt( standardGravParameter / semiMajorAxis ) * Math.Sin( trueAnomaly * Math.PI / 180.0 ), Math.Sqrt( standardGravParameter / semiMajorAxis ) * (eccentricity + Math.Cos( trueAnomaly * Math.PI / 180.0 )), 0 );

            // Rotate the position and velocity to the correct orientation in space
            Quaterniond rotation1 = Quaterniond.FromAxisAngle( Vector3d.Up, (longitudeOfAscendingNode - 90) * Math.PI / 180.0 );
            Quaterniond rotation2 = Quaterniond.FromAxisAngle( rotation1 * Vector3d.Right, inclination * Math.PI / 180.0 );
            Quaterniond rotation3 = Quaterniond.FromAxisAngle( rotation2 * rotation1 * Vector3d.Up, argumentOfPeriapsis * Math.PI / 180.0 );
            Vector3d position = rotation3 * rotation2 * rotation1 * positionOrbitalPlane + parentPosition;
            Vector3d velocity = rotation3 * rotation2 * rotation1 * velocityOrbitalPlane;

            return (position, velocity);
        }

        [Obsolete( "unconfirmed" )]
        public static (double semiMajorAxis, double eccentricity, double inclination, double argumentOfPeriapsis, double longitudeOfAscendingNode, double trueAnomaly) CalculateOrbitalElements( Vector3d position, Vector3d velocity, Vector3d parentPosition, double parentMass )
        {
            double gravitationalConstant = 6.6743e-11;
            double parentRadius = 6371000; // radius of Earth in meters

            // Calculate the standard gravitational parameter of the parent body
            double standardGravParameter = gravitationalConstant * parentMass;

            // Calculate the position and velocity relative to the parent body
            Vector3d relativePosition = position - parentPosition;
            Vector3d relativeVelocity = velocity - Vector3d.Cross( Vector3d.Up, relativePosition ).normalized * Math.Sqrt( standardGravParameter / relativePosition.magnitude );

            // Calculate the specific angular momentum and its magnitude
            Vector3d angularMomentum = Vector3d.Cross( relativePosition, relativeVelocity );
            double h = angularMomentum.magnitude;

            // Calculate the inclination
            double inclination = Math.Acos( angularMomentum.z / h ) * 180.0 / Math.PI;

            // Calculate the longitude of ascending node
            double longitudeOfAscendingNode;
            if( angularMomentum.x == 0 && angularMomentum.y == 0 )
            {
                longitudeOfAscendingNode = 0;
            }
            else
            {
                longitudeOfAscendingNode = Math.Atan2( angularMomentum.x, -angularMomentum.y ) * 180.0 / Math.PI;
                if( longitudeOfAscendingNode < 0 )
                {
                    longitudeOfAscendingNode += 360;
                }
            }

            // Calculate the eccentricity vector and its magnitude
            Vector3d eccentricityVector = Vector3d.Cross( relativeVelocity, angularMomentum ) / standardGravParameter - relativePosition.normalized;
            double e = eccentricityVector.magnitude;

            // Calculate the argument of periapsis and true anomaly
            double argumentOfPeriapsis, trueAnomaly;
            if( e < 1e-10 )
            {
                // Circular orbit
                argumentOfPeriapsis = 0;
                trueAnomaly = Math.Acos( Vector3d.Dot( relativePosition.normalized, Vector3d.Cross( angularMomentum.normalized, relativePosition.normalized ) ) ) * 180.0 / Math.PI;
            }
            else
            {
                // Elliptical or hyperbolic orbit
                argumentOfPeriapsis = Math.Acos( Vector3d.Dot( eccentricityVector.normalized, Vector3d.Cross( Vector3d.Up, angularMomentum.normalized ) ) ) * 180.0 / Math.PI;
                if( eccentricityVector.z < 0 )
                {
                    argumentOfPeriapsis = 360 - argumentOfPeriapsis;
                }

                trueAnomaly = Math.Acos( Vector3d.Dot( eccentricityVector.normalized, relativePosition.normalized ) ) * 180.0 / Math.PI;
                if( Vector3d.Dot( relativePosition, relativeVelocity ) < 0 )
                {
                    trueAnomaly = 360 - trueAnomaly;
                }

                if( e > 1 && trueAnomaly < 180 )
                {
                    trueAnomaly += 360;
                }
            }

            // Calculate the semi-major axis
            double semiMajorAxis = 1 / (2 / relativePosition.magnitude - relativeVelocity.sqrMagnitude / standardGravParameter);
            // Return the calculated orbital elements
            return (semiMajorAxis, e, inclination, argumentOfPeriapsis, longitudeOfAscendingNode, trueAnomaly);
        }*/

        /* public void CalculateNewOrbitalElements(
         Vector3 position, Vector3 velocity, Vector3 deltaV,
         out float semiMajorAxis, out float eccentricity, out float inclination,
         out float longitudeOfAscendingNode, out float argumentOfPeriapsis )
         {
             // Step 2: Update the velocity vector of the spacecraft
             velocity += deltaV;

             // Step 3: Calculate the new angular momentum vector
             Vector3 angularMomentum = Vector3.Cross( position, velocity );

             // Step 4: Determine the new eccentricity vector
             Vector3 eccentricityVector = ((velocity.LengthSquared - G * position.Length) * position - Vector3.Dot( position, velocity ) * velocity) / G;

             // Step 5: Calculate the new eccentricity
             eccentricity = eccentricityVector.Length;

             // Step 6: Calculate the new semi-major axis
             semiMajorAxis = -G * position.Length / (velocity.LengthSquared - 2 * G * position.Length);

             // Step 7: Calculate the new inclination
             inclination = Math.Acos( angularMomentum.Z / angularMomentum.Length ) * Math.Rad2Deg;

             // Step 8: Calculate the new longitude of ascending node
             Vector3 ascendingNodeVector = Vector3.Cross( Vector3.Forward, angularMomentum );
             if( ascendingNodeVector.Length > 0 )
             {
                 ascendingNodeVector.Normalized();
                 longitudeOfAscendingNode = Math.Atan2( ascendingNodeVector.Y, ascendingNodeVector.X ) * Math.Rad2Deg;
             }
             else
             {
                 longitudeOfAscendingNode = 0;
             }

             // Step 9: Calculate the new argument of periapsis
             Vector3 nodeVector = Vector3.Cross( Vector3.Forward, eccentricityVector );
             if( nodeVector.Length > 0 )
             {
                 nodeVector.Normalized();
                 argumentOfPeriapsis = Math.Acos( Vector3.Dot( nodeVector, eccentricityVector ) / (nodeVector.Length * eccentricityVector.Length) ) * Math.Rad2Deg;
             }
             else
             {
                 argumentOfPeriapsis = 0;
             }
        
             // To calculate the true anomaly, you can use the following equation:
             // I have no idea if it's correct
             // cos(ν) = (a * (1 - e ^ 2) / r - 1) / e
             // v = acos(cos(v))
         }

         public float CalculateTrueAnomaly( Vector3 position, Vector3 velocity )
         {
             // Calculate the magnitude of the position vector
             float r = position.Length;

             // Calculate the magnitude of the velocity vector
             float v = velocity.Length;

             // Calculate the specific angular momentum vector
             Vector3 h = Vector3.Cross( position, velocity );

             // Calculate the magnitude of the specific angular momentum vector
             float hMag = h.Length;

             // Calculate the eccentricity vector
             Vector3 eVec = Vector3.Cross( velocity, h ) / G - position / r;

             // Calculate the magnitude of the eccentricity vector
             float e = eVec.Length;

             // Calculate the semi-major axis
             float a = 1 / (2 / r - v * v / (G * (1 + e)));

             // Calculate the argument of periapsis
             float argPeriapsis = Math.Acos( Vector3.Dot( eVec, h ) / (e * hMag) ) * Math.Rad2Deg;

             // Calculate the true anomaly
             float cosTrueAnomaly = (a * (1 - e * e) / r - 1) / e;
             float trueAnomaly = (float)Math.Acos( cosTrueAnomaly );
             float sinTrueAnomaly = Vector3.Dot( position, velocity ) / (r * v * (float)Math.Sin( trueAnomaly ));
             trueAnomaly *= Math.Sign( sinTrueAnomaly );

             return trueAnomaly * Math.Rad2Deg;
         }*/

        [Obsolete("Unconfirmed")]
        public static double[] CalculateOrbitalElements( double[] stateVector, double mu )
        {
            // Extract position and velocity vectors from state vector
            double[] positionVector = new double[] { stateVector[0], stateVector[1], stateVector[2] };
            double[] velocityVector = new double[] { stateVector[3], stateVector[4], stateVector[5] };

            // Calculate specific angular momentum vector and its magnitude
            double[] angularMomentumVector = Vector3D.CrossProduct( positionVector, velocityVector );
            double angularMomentumMagnitude = angularMomentumVector.Length;

            // Calculate eccentricity vector and its magnitude
            double[] eccentricityVector = Vector3D.CrossProduct( velocityVector, angularMomentumVector ) / mu - positionVector / Vector3D.Distance( positionVector, Vector3D.Zero );
            double eccentricityMagnitude = eccentricityVector.Length;

            // Calculate inclination
            double inclination = Math.Acos( angularMomentumVector.Z / angularMomentumMagnitude );

            // Calculate longitude of ascending node
            double longitudeAscendingNode = Math.Atan2( angularMomentumVector.X, -angularMomentumVector.Y );
            if( longitudeAscendingNode < 0 )
            {
                longitudeAscendingNode += 2 * Math.PI;
            }

            // Calculate argument of periapsis
            double argumentOfPeriapsis = Math.Atan2( Vector3D.DotProduct( angularMomentumVector, eccentricityVector ) / (angularMomentumMagnitude * eccentricityMagnitude), Math.Cos( inclination ) );
            if( argumentOfPeriapsis < 0 )
            {
                argumentOfPeriapsis += 2 * Math.PI;
            }

            // Calculate true anomaly
            double trueAnomaly = Math.Atan2( Vector3D.DotProduct( positionVector, Vector3D.CrossProduct( angularMomentumVector, velocityVector ) ) / (angularMomentumMagnitude * Vector3D.Distance( positionVector, Vector3D.Zero )), Vector3D.DotProduct( positionVector, velocityVector ) / (Vector3D.Distance( positionVector, Vector3D.Zero ) * Vector3D.Distance( velocityVector, Vector3D.Zero )) );
            if( trueAnomaly < 0 )
            {
                trueAnomaly += 2 * Math.PI;
            }

            // Calculate semimajor axis
            double semimajorAxis = 1 / (2 / Vector3D.Distance( positionVector, Vector3D.Zero ) - Vector3D.DotProduct( velocityVector, velocityVector ) / mu);

            // Return array of orbital elements
            return new double[] { semimajorAxis, eccentricityMagnitude, inclination, longitudeAscendingNode, argumentOfPeriapsis, trueAnomaly };
        }

        [Obsolete( "Unconfirmed" )]
        public static double[] CalculateStateVector( double[] orbitalElements, double mu )
        {
            // Extract orbital elements
            double semimajorAxis = orbitalElements[0];
            double eccentricityMagnitude = orbitalElements[1];
            double inclination = orbitalElements[2];
            double longitudeAscendingNode = orbitalElements[3];
            double argumentOfPeriapsis = orbitalElements[4];
            double trueAnomaly = orbitalElements[5];

            // Calculate semilatus rectum
            double semilatusRectum = semimajorAxis * (1 - eccentricityMagnitude * eccentricityMagnitude);

            // Calculate position and velocity vectors in perifocal coordinates
            double[] positionVectorPerifocal = new double[] { semilatusRectum / (1 + eccentricityMagnitude * Math.Cos( trueAnomaly )), 0, 0 };
            double[] velocityVectorPerifocal = new double[] { Math.Sqrt( mu / semilatusRectum ) * eccentricityMagnitude * Math.Sin( trueAnomaly ), Math.Sqrt( mu / semilatusRectum ) * (eccentricityMagnitude + Math.Cos( trueAnomaly )), 0 };

            // Create rotation matrices to transform from perifocal to geocentric equatorial coordinates
            // 3x3 matrices
            double[,] rotation1 = new double[,] { { Math.Cos( argumentOfPeriapsis ), -Math.Sin( argumentOfPeriapsis ), 0 }, { Math.Sin( argumentOfPeriapsis ), Math.Cos( argumentOfPeriapsis ), 0 }, { 0, 0, 1 } };
            double[,] rotation2 = new double[,] { { 1, 0, 0 }, { 0, Math.Cos( inclination ), -Math.Sin( inclination ) }, { 0, Math.Sin( inclination ), Math.Cos( inclination ) } };
            double[,] rotation3 = new double[,] { { Math.Cos( longitudeAscendingNode ), -Math.Sin( longitudeAscendingNode ), 0 }, { Math.Sin( longitudeAscendingNode ), Math.Cos( longitudeAscendingNode ), 0 }, { 0, 0, 1 } };

            // Combine rotation matrices
            double[,] combinedRotation = Matrix3D.Multiply( Matrix3D.Multiply( rotation1, rotation2 ), rotation3 );

            // Transform position and velocity vectors from perifocal to geocentric equatorial coordinates
            double[] positionVector = Matrix3D.Multiply( combinedRotation, positionVectorPerifocal );
            double[] velocityVector = Matrix3D.Multiply( combinedRotation, velocityVectorPerifocal );

            // Return state vector
            return new double[] { positionVector[0], positionVector[1], positionVector[2], velocityVector[0], velocityVector[1], velocityVector[2] };
        }
    }
}
