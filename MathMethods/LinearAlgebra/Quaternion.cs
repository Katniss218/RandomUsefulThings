using System;
using System.Collections.Generic;
using System.Text;

namespace Geometry
{
    public struct Quaternion
    {
        /// <summary>
        /// First imaginary coefficient ('b' from 'a + bi + cj + dk').
        /// </summary>
        public float X { get; }

        /// <summary>
        /// Second imaginary coefficient ('c' from 'a + bi + cj + dk').
        /// </summary>
        public float Y { get; }

        /// <summary>
        /// Third imaginary coefficient ('d' from 'a + bi + cj + dk').
        /// </summary>
        public float Z { get; }

        /// <summary>
        /// The real coefficient of the quaternion ('a' from 'a + bi + cj + dk').
        /// </summary>
        public float W { get; }

        public float LengthSquared { get => (X * X) + (Y * Y) + (Z * Z) + (W * W); }

        /// <summary>
        /// Returns the length (norm) of the quaternion.
        /// </summary>
        public float Length { get => (float)Math.Sqrt( LengthSquared ); }

        public Quaternion( float x, float y, float z, float w )
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
            this.W = w;
        }

        public static Quaternion Identity { get => new Quaternion( 0.0f, 0.0f, 0.0f, 1.0f ); }

        /// <summary>
        /// Returns a quaternion with its length (norm) set to 1.
        /// </summary>
        public Quaternion Normalized()
        {
            float length = this.Length;

            return new Quaternion(
                X / length,
                Y / length,
                Z / length,
                W / length );
        }

        /// <summary>
        /// Returns the quaternion with its imaginary coefficients flipped.
        /// </summary>
        public static Quaternion Conjugate( Quaternion q )
        {
            return new Quaternion( -q.X, -q.Y, -q.Z, q.W );
        }

        /// <summary>
        /// Returns the square root of the sum of the coefficients squared (i.e. length).
        /// </summary>
        public float Norm()
        {
            return this.Length;
        }

        /// <summary>
        /// Calculates the inverse of a quaternion (a quatarnion that represents a rotation that is the reverse of the original).
        /// </summary>
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
        public static float Dot( in Quaternion q1, in Quaternion q2 )
        {
            return (q1.X * q2.X) + (q1.Y * q2.Y) + (q1.Z * q2.Z) + (q1.W * q2.W);
        }

        /// <summary>
        /// Converts the quaternion back into Euler Angles (same rotation order as in Unity 2022.1).
        /// </summary>
        /// <remarks>
        /// This method is the inverse of <see cref="Quaternion.FromEulerAngles(Vector3)"/>.
        /// </remarks>
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

            float x = (float)Math.Asin( 2.0f * (this.W * this.X - this.Y * this.Z) );                                                               // Yaw
            float y = (float)Math.Atan2( 2.0f * this.W * this.Y + 2.0f * this.Z * this.X, 1.0f - 2.0f * (this.X * this.X + this.Y * this.Y) );      // Pitch
            float z = (float)Math.Atan2( 2.0f * this.W * this.Z + 2.0f * this.X * this.Y, 1.0f - 2.0f * (this.Z * this.Z + this.X * this.X) );      // Roll

            return new Vector3( x, y, z );

        }

        /// <summary>
        /// Converts Euler Angles into a quaternion (same rotation order as in Unity 2022.1).
        /// </summary>
        /// <remarks>
        /// This method is the inverse of <see cref="Quaternion.ToEulerAngles"/>.
        /// </remarks>
        public static Quaternion FromEulerAngles( in Vector3 euler )
        {
            return FromEulerAngles( euler.X, euler.Y, euler.Z );
        }

        /// <summary>
        /// Converts Euler Angles into a quaternion (same rotation order as in Unity 2022.1).
        /// </summary>
        /// <remarks>
        /// This method is the inverse of <see cref="Quaternion.ToEulerAngles"/>.
        /// </remarks>
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
            float cosX = (float)Math.Cos( x * 0.5f );
            float cosY = (float)Math.Cos( y * 0.5f );
            float cosZ = (float)Math.Cos( z * 0.5f );

            float sinX = (float)Math.Sin( x * 0.5f );
            float sinY = (float)Math.Sin( y * 0.5f );
            float sinZ = (float)Math.Sin( z * 0.5f );

            float qx = (cosX * sinY * sinZ) + (sinX * cosY * cosZ); // Unity.
            float qy = (cosX * sinY * cosZ) - (sinX * cosY * sinZ); // Unity.
            float qz = (cosX * cosY * sinZ) - (sinX * sinY * cosZ); // Unity.
            float qw = (cosX * cosY * cosZ) + (sinX * sinY * sinZ); // Unity.

            // Return the calculated quaternion
            return new Quaternion( qx, qy, qz, qw ); // possibly needs normalizing??
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

        public static Quaternion AngleAxis( Vector3 axis, float radians )
        {
            if( axis.LengthSquared == 0.0f )
            {
                return Quaternion.Identity;
            }

            radians *= 0.5f; // Quaternions use half angles.
            Vector3 axisSin = axis.Normalized() * (float)Math.Sin( radians );

            return new Quaternion( axisSin.X, axisSin.Y, axisSin.Z, (float)Math.Cos( radians ) ).Normalized();
        }

        public void ToAxisAngleRad( out Vector3 axis, out float angle ) // works.
        {
            //if( Math.Abs( this.W ) > 1.0f )
            //    q.Normalize();

            const float epsilon = 0.0001f;

            angle = 2.0f * (float)Math.Acos( this.W ); // angle
            float den = (float)Math.Sqrt( 1.0 - (this.W * this.W) );

            if( den < epsilon )
            {
                // This occurs when the angle is zero. Not a problem, just set an arbitrary normalized axis.
                axis = Vector3.Forward;
                return;
            }

            axis = new Vector3( this.X / den, this.Y / den, this.Z / den );
        }

        public static Quaternion Slerp( Quaternion start, Quaternion end, float amount ) // seems to work.
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
        public static Quaternion Multiply( Quaternion q1, Quaternion q2 ) // works.
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