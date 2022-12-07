using System;
using System.Collections.Generic;
using System.Text;

namespace RandomUsefulThings
{
    public struct Vector3
    {
        public float x, y, z;

        public Vector3( float x, float y, float z )
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public float LengthSquared { get => (x * x) + (y * y) + (z * z); }

        public float Length { get => (float)Math.Sqrt( LengthSquared ); }

        public Vector3 Normalized()
        {
            float length = this.Length;
            return new Vector3( x / length, y / length, z / length );
        }

        public Vector3 Reflect( Vector3 planeNormal )
        {
            // Project the vector onto the plane defined by the normal
            Vector3 projection = this - Vector3.Dot( this, planeNormal ) * planeNormal;

            // Reflect the vector off of the plane
            return projection * 2 - this;
        }

        public Vector3 ProjectOntoPlane( Vector3 planeNormal )
        {
            // The projection of vector onto a plane can be calculated by subtracting the component of the vector that is orthogonal to the plane from the original vector.
            Vector3 orthogonalComponent = Vector3.Dot( this, planeNormal ) * planeNormal;

            return this - orthogonalComponent;
        }

        public static float Dot( Vector3 v1, Vector3 v2 )
        {
            return v1.x * v2.x + v1.y * v2.y + v1.z * v2.z;
        }

        public static Vector3 operator -( Vector3 v1, Vector3 v2 )
        {
            return new Vector3( v1.x - v2.x, v1.y - v2.y, v1.z - v2.z );
        }

        public static Vector3 operator *( Vector3 v, float f )
        {
            return new Vector3( v.x * f, v.y * f, v.z * f );
        }

        public static Vector3 operator *( float f, Vector3 v )
        {
            return new Vector3( v.x * f, v.y * f, v.z * f );
        }

        // rotate a vector by a quaternion (assuming origin is (0,0,0) ??)
        public static Vector3 operator *( Quaternion q, Vector3 v )
        {
            Quaternion vq = new Quaternion( v.x, v.y, v.z, 0 );
            Quaternion r = q * vq * q.Inverse();
            return new Vector3( r.x, r.y, r.z );
        }

        // cross-product
        public static Quaternion operator *( Vector3 v1, Vector3 v2 )
        {
            return new Quaternion(
                v1.y * v2.z - v1.z * v2.y,
                v1.z * v2.x - v1.x * v2.z,
                v1.x * v2.y - v1.y * v2.x,
                0 );
        }

        // One of the main uses for this is getting the rotation between two rotations.
        // For example if you wanted to know the Quaternion that would get you from rotationA to rotationB you would do something like this:
        /// FromAtoB = Quaternion.Inverse(rotationA) * rotationB
    }
}
