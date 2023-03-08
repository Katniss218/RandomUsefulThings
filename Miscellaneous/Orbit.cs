using System;
using System.Collections.Generic;
using System.Text;

namespace Miscellaneous
{
    class Orbit
    {
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
        }
    }
}
