using System;
using System.Collections.Generic;
using System.Text;

namespace RandomUsefulThings
{
    public struct Vector3
    {
        public float X { get; }
        public float Y { get; }
        public float Z { get; }

        public float LengthSquared { get => (X * X) + (Y * Y) + (Z * Z); }

        public float Length { get => (float)Math.Sqrt( LengthSquared ); }

        public Vector3( float x, float y, float z )
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        public Vector3 Normalized()
        {
            float length = this.Length;
            return new Vector3( X / length, Y / length, Z / length );
        }

        public Vector3 Reflect( Vector3 planeNormal )
        {
            // Project the vector onto the plane defined by the normal
            Vector3 projection = this - Vector3.Dot( this, planeNormal ) * planeNormal;

            // Reflect the vector off of the plane
            return projection * 2 - this;
        }

        [Obsolete( "Unconfirmed" )]
        public Vector3 ProjectOnto( Vector3 target )
        {
            throw new NotImplementedException( "I don't think this works right" );
            float dotProduct = Vector3.Dot( this, target );

            // Calculate the projection of the vector onto the other vector
            float projection = dotProduct / target.Length;

            // Return the projection as a Vector3
            return new Vector3( projection * target.X, projection * target.Y, projection * target.Z );
        }

        public Vector3 ProjectOntoPlane( Vector3 targetNormal )
        {
            // The projection of vector onto a plane can be calculated by subtracting the component of the vector that is orthogonal to the plane from the original vector.
            Vector3 orthogonalComponent = Vector3.Dot( this, targetNormal ) * targetNormal;

            return this - orthogonalComponent;
        }

        public static Vector3 Multiply( Vector3 vector, Matrix4x4 matrix )
        {
            return new Vector3(
                vector.X * matrix.M00 + vector.Y * matrix.M01 + vector.Z * matrix.M02 + matrix.M03,
                vector.X * matrix.M10 + vector.Y * matrix.M11 + vector.Z * matrix.M12 + matrix.M13,
                vector.X * matrix.M20 + vector.Y * matrix.M21 + vector.Z * matrix.M22 + matrix.M23
            );
        }

        public static float Dot( Vector3 v1, Vector3 v2 )
        {
            return v1.X * v2.X + v1.Y * v2.Y + v1.Z * v2.Z;
        }

        public static Vector3 operator -( Vector3 v1, Vector3 v2 )
        {
            return new Vector3( v1.X - v2.X, v1.Y - v2.Y, v1.Z - v2.Z );
        }

        public static Vector3 operator *( Vector3 v, float f )
        {
            return new Vector3( v.X * f, v.Y * f, v.Z * f );
        }

        public static Vector3 operator *( float f, Vector3 v )
        {
            return new Vector3( v.X * f, v.Y * f, v.Z * f );
        }

        // rotate a vector by a quaternion (assuming origin is (0,0,0) ??)
        public static Vector3 operator *( Quaternion q, Vector3 v )
        {
            Quaternion vq = new Quaternion( v.X, v.Y, v.Z, 0 );
            Quaternion r = q * vq * q.Inverse();
            return new Vector3( r.X, r.Y, r.Z );
        }

        // cross-product
        public static Quaternion operator *( Vector3 v1, Vector3 v2 )
        {
            return new Quaternion(
                v1.Y * v2.Z - v1.Z * v2.Y,
                v1.Z * v2.X - v1.X * v2.Z,
                v1.X * v2.Y - v1.Y * v2.X,
                0 );
        }

        // One of the main uses for this is getting the rotation between two rotations.
        // For example if you wanted to know the Quaternion that would get you from rotationA to rotationB you would do something like this:
        /// FromAtoB = Quaternion.Inverse(rotationA) * rotationB
        /*public Quaternion RotationFromTo( Quaternion from, Quaternion to )
        {
            // Calculate the dot product of the two Quaternions
            float dot = from.x * to.x + from.y * to.y + from.z * to.z + from.w * to.w;

            // If the dot product is negative, negate one of the Quaternions to ensure
            // that the result is a valid rotation
            if( dot < 0 )
            {
                from.x = -from.x;
                from.y = -from.y;
                from.z = -from.z;
                from.w = -from.w;
            }

            // Calculate the Quaternion representing the rotation from the first Quaternion to the second one
            Quaternion result = new Quaternion(
                to.x - from.x,
                to.y - from.y,
                to.z - from.z,
                to.w - from.w
            );

            // Normalize the Quaternion to ensure it is a valid rotation
            result.Normalize();

            return result;
        }*/
    }
}
