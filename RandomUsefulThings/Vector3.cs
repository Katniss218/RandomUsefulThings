using System;
using System.Collections.Generic;
using System.Text;

namespace RandomUsefulThings
{
    public class Vector3
    {
        public float x, y, z;

        public Vector3( float x, float y, float z )
        {
            this.x = x;
            this.y = y;
            this.z = z;
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
