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

        /// <summary>
        /// The real coefficient of the quaternion.
        /// </summary>
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

        public static Quaternion Conjugate( Quaternion q )
        {
            return new Quaternion( -q.X, -q.Y, -q.Z, q.W );
        }

        public float Norm()
        {
            // Norm is defined as the square root of the sum of the coefficients squared (i.e. length).
            return this.Length;
        }

        public Quaternion Inverse()
        {
            // q^(-1) = conjugate(q) / norm(q)^2
            float lengthSquared = LengthSquared;

            if( lengthSquared == 0.0f )
            {
                return this;
            }

            float lengthSquaredInverse = 1.0f / lengthSquared;
            return new Quaternion( X * -lengthSquaredInverse, Y * -lengthSquaredInverse, Z * -lengthSquaredInverse, W * lengthSquaredInverse );
        }

        /// <summary>
        /// Performs a "dot product", treating the quaternion as if it was a 4-dimensional vector.
        /// </summary>
        public static float Dot( Quaternion q1, Quaternion q2 )
        {
            return (q1.X * q2.X) + (q1.Y * q2.Y) + (q1.Z * q2.Z) + (q1.W * q2.W);
        }

        public Vector3 ToEulerAngles()
        {
            // returns euler angles in Radians

            /*float sqw = this.W * this.W;
            float sqx = this.X * this.X;
            float sqy = this.y * this.y;
            float sqz = this.z * this.z;
            float unit = sqx + sqy + sqz + sqw; // if normalised is one, otherwise is correction factor
            float test = this.x * this.w - this.y * this.z;
            Vector3 v;

            if( test > 0.4995f * unit )
            { // singularity at north pole
                v.y = 2f * Math.Atan2( this.y, this.x );
                v.x = Math.PI / 2;
                v.z = 0;
                return NormalizeAngles( v * Mathf.Rad2Deg );
            }
            if( test < -0.4995f * unit )
            { // singularity at south pole
                v.y = -2f * Mathf.Atan2( this.y, this.x );
                v.x = -Mathf.PI / 2;
                v.z = 0;
                return NormalizeAngles( v * Mathf.Rad2Deg );
            }
            */

            float x = (float)Math.Asin( 2f * (this.W * this.X - this.Y * this.Z) );                                                        // Yaw
            float y = (float)Math.Atan2( 2f * this.W * this.Y + 2f * this.Z * this.X, 1 - 2f * (this.X * this.X + this.Y * this.Y) );      // Pitch
            float z = (float)Math.Atan2( 2f * this.W * this.Z + 2f * this.X * this.Y, 1 - 2f * (this.Z * this.Z + this.X * this.X) );      // Roll

            return new Vector3( x, y, z );
            
        }

        public static Quaternion FromEulerAngles( Vector3 euler )
        {
            return FromEulerAngles( euler.X, euler.Y, euler.Z );
        }

        public static Quaternion FromEulerAngles( float x, float y, float z )
        {
            // euler input in radians.
            // x = yaw, y = pitch, z = roll

            // Intrinsic rotations apply to axis in rotated coordinate system
            // - Coordinate system of next rotation relative to previous rotation

            // Extrinsic rotations apply to axis in world coordinate system
            // - Coordinate system of next rotation relative to(fixed) world coordinate system

            // Calculate the quaternion from the Euler angles
            // Division by 2 because Quaternions use half-angles to represent rotations.
            float cosX = (float)Math.Cos( x / 2 );
            float cosY = (float)Math.Cos( y / 2 );
            float cosZ = (float)Math.Cos( z / 2 );

            float sinX = (float)Math.Sin( x / 2 );
            float sinY = (float)Math.Sin( y / 2 );
            float sinZ = (float)Math.Sin( z / 2 );

            float qx = (cosX * sinY * sinZ) + (sinX * cosY * cosZ); // Unity.
            float qy = (cosX * sinY * cosZ) - (sinX * cosY * sinZ); // Unity.
            float qz = (cosX * cosY * sinZ) - (sinX * sinY * cosZ); // Unity.
            float qw = (cosX * cosY * cosZ) + (sinX * sinY * sinZ); // Unity.

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
            {
                return 0.0f;
            }
            else
            {
                return (float)Math.Acos( dotClamped ) * 2.0f;
            }
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

        [Obsolete( "Does this work?" )]
        public static Quaternion Add( Quaternion q1, Quaternion q2 )
        {
            // possibly, this might need to be normalized.
            return new Quaternion(
                q1.X + q2.X,
                q1.Y + q2.Y,
                q1.Z + q2.Z,
                q1.W + q2.W
            );
        }

        [Obsolete( "Does this work?" )]
        public static Quaternion Subtract( Quaternion q1, Quaternion q2 )
        {
            // possibly, this might need to be normalized.
            return new Quaternion(
                q1.X - q2.X,
                q1.Y - q2.Y,
                q1.Z - q2.Z,
                q1.W - q2.W
            );
        }

        [Obsolete( "Does this work?" )]
        public static Quaternion Multiply( Quaternion q, float f )
        {
            // possibly, this might need to be normalized.
            return new Quaternion(
                q.X * f,
                q.Y * f,
                q.Z * f,
                q.W * f
            );
        }

        /// <summary>
        /// Combines the rotations in order: q1, then q2.
        /// </summary>
        public static Quaternion Multiply( Quaternion q1, Quaternion q2 )
        {
            return new Quaternion(
                (q1.W * q2.X) + (q1.X * q2.W) + (q1.Y * q2.Z) - (q1.Z * q2.Y),
                (q1.W * q2.Y) + (q1.Y * q2.W) + (q1.Z * q2.X) - (q1.X * q2.Z),
                (q1.W * q2.Z) + (q1.Z * q2.W) + (q1.X * q2.Y) - (q1.Y * q2.X),
                (q1.W * q2.W) - (q1.X * q2.X) - (q1.Y * q2.Y) - (q1.Z * q2.Z) );
        }

        /// <summary>
        /// Removes ('subtracts') the q2 rotation from q1.
        /// </summary>
        public static Quaternion Divide( Quaternion q1, Quaternion q2 )
        {
            // subtracting out a rotation from another rotation is equivalent to adding in its inverse, since the inverse will reverse the rotation done by its input.
            return Multiply( q1, q2.Inverse() );
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
        [Obsolete( "Does this work?" )]
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