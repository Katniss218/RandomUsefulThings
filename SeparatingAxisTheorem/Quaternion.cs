using System;
using System.Collections.Generic;
using System.Text;

namespace Geometry
{
    public struct Quaternion
    {
        public float X { get; }
        public float Y { get; }
        public float Z { get; }
        public float W { get; }

        public float LengthSquared { get => (X * X) + (Y * Y) + (Z * Z) + (W * W); }

        public float Length { get => (float)Math.Sqrt( LengthSquared ); }

        public Quaternion( float x, float y, float z, float w )
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
            this.W = w;
        }

        public Quaternion Normalized()
        {
            float length = this.Length;

            return new Quaternion(
                X / length,
                Y / length,
                Z / length,
                W / length );
        }

        public Quaternion Inverse()
        {
            float lengthSquared = LengthSquared;

            // not sure if the `-` matters here.
            return new Quaternion( -X / lengthSquared, -Y / lengthSquared, -Z / lengthSquared, W / lengthSquared );
        }

        public static float Dot( Quaternion q1, Quaternion q2 )
        {
            return (q1.X * q2.X) + (q1.Y * q2.Y) + (q1.Z * q2.Z) + (q1.W * q2.W);
        }

        public Vector3 ToEulerAngles()
        {
            // returns euler angles in Radians

            // Calculate the Euler angles from the quaternion
            float roll = (float)Math.Atan2( 2 * ((W * X) + (Y * Z)), 1 - 2 * ((X * X) + (Y * Y)) );
            float pitch = (float)Math.Asin( 2 * ((W * Y) - (Z * X)) );
            float yaw = (float)Math.Atan2( 2 * ((W * Z) + (X * Y)), 1 - 2 * ((Y * Y) + (Z * Z)) );

            // Return the Euler angles as a Vector3
            return new Vector3( roll, pitch, yaw );
        }

        public static Quaternion FromEulerAngles( Vector3 euler )
        {
            return FromEulerAngles( euler.X, euler.Y, euler.Z );
        }

        public static Quaternion FromEulerAngles( float x, float y, float z )
        {
            // euler input in radians.

            // Calculate the quaternion from the Euler angles
            float c3 = (float)Math.Cos( x / 2 );
            float c1 = (float)Math.Cos( y / 2 );
            float c2 = (float)Math.Cos( z / 2 );

            float s3 = (float)Math.Sin( x / 2 );
            float s1 = (float)Math.Sin( y / 2 );
            float s2 = (float)Math.Sin( z / 2 );

            float qw = (c1 * c2 * c3) - (s1 * s2 * s3);
            float qx = (s1 * s2 * c3) + (c1 * c2 * s3);
            float qy = (s1 * c2 * c3) + (c1 * s2 * s3);
            float qz = (c1 * s2 * c3) - (s1 * c2 * s3);

            // Return the calculated quaternion
            return new Quaternion( qx, qy, qz, qw );
        }

        public static float Angle( Quaternion q1, Quaternion q2 )
        {
            float dotProduct = Quaternion.Dot( q1, q2 );
            float absoluteDotProduct = Math.Abs( dotProduct );

            // Clamp to [-1, 1]
            float dotClamped = Math.Clamp( absoluteDotProduct, -1.0f, 1.0f );

            const float eps = 0.000001f;

            // Is the dot product of two quaternions within tolerance for them to be considered equal?
            // Returns false in the presence of NaN values.
            if( dotClamped > 1.0f - eps )
                return 0.0f;
            else
                return (float)Math.Acos( dotClamped ) * 2.0f;
        }

        [Obsolete( "Unconfirmed" )]
        public static Quaternion Slerp( Quaternion start, Quaternion end, float amount )
        {
            // Calculate the dot product of the start and end quaternions.
            float dot = Dot( start, end );

            // Clamp the dot product to the range [-1, 1] to prevent any invalid calculations.
            dot = Math.Clamp( dot, -1.0f, 1.0f );

            // Calculate the angle between the two quaternions.
            float angle = (float)Math.Acos( dot ) * amount;

            // Calculate the interpolated quaternion using a formula based on the angle and the start and end quaternions.
            Quaternion direction = end - start * dot;
            direction = direction.Normalized();
            return ((start * (float)Math.Cos( angle )) + (direction * (float)Math.Sin( angle ))).Normalized();
        }

        public static Quaternion Add( Quaternion q1, Quaternion q2 )
        {
            return new Quaternion(
                q1.X + q2.X,
                q1.Y + q2.Y,
                q1.Z + q2.Z,
                q1.W + q2.W
            );
        }
        
        public static Quaternion Subtract( Quaternion q1, Quaternion q2 )
        {
            return new Quaternion(
                q1.X - q2.X,
                q1.Y - q2.Y,
                q1.Z - q2.Z,
                q1.W - q2.W
            );
        }

        public static Quaternion Multiply( Quaternion q, float f )
        {
            return new Quaternion(
                q.X * f,
                q.Y * f,
                q.Z * f,
                q.W * f
            );
        }

        public static Quaternion operator +( Quaternion q1, Quaternion q2 )
        {
            return Add( q1, q2 );
        }
        
        public static Quaternion operator -( Quaternion q1, Quaternion q2 )
        {
            return Subtract( q1, q2 );
        }
        

        public static Quaternion operator *( Quaternion q, float f )
        {
            return Multiply( q, f );
        }

        public static Quaternion operator *( float f, Quaternion q )
        {
            return Multiply( q, f );
        }

        // pseudo-cross product
        public static Quaternion operator *( Quaternion q1, Quaternion q2 )
        {
            return new Quaternion(
                (q1.W * q2.X) + (q1.X * q2.W) + (q1.Y * q2.Z) - (q1.Z * q2.Y),
                (q1.W * q2.Y) + (q1.Y * q2.W) + (q1.Z * q2.X) - (q1.X * q2.Z),
                (q1.W * q2.Z) + (q1.Z * q2.W) + (q1.X * q2.Y) - (q1.Y * q2.X),
                (q1.W * q2.W) - (q1.X * q2.X) - (q1.Y * q2.Y) - (q1.Z * q2.Z) );
        }
    }
}