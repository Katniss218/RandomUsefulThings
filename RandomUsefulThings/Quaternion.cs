using System;
using System.Collections.Generic;
using System.Text;

namespace RandomUsefulThings
{
    public struct Quaternion
    {
        public float x, y, z, w;

        public Quaternion( float x, float y, float z, float w )
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }

        public float LengthSquared { get => (x * x) + (y * y) + (z * z) + (w * w); }

        public Quaternion Inverse()
        {
            float lengthSquared = LengthSquared;

            // not sure if the `-` matters here.
            return new Quaternion( -x / lengthSquared, -y / lengthSquared, -z / lengthSquared, w / lengthSquared );
        }

        public static Quaternion operator *( Quaternion q1, Quaternion q2 )
        {
            return new Quaternion(
                q1.w * q2.x + q1.x * q2.w + q1.y * q2.z - q1.z * q2.y,
                q1.w * q2.y + q1.y * q2.w + q1.z * q2.x - q1.x * q2.z,
                q1.w * q2.z + q1.z * q2.w + q1.x * q2.y - q1.y * q2.x,
                q1.w * q2.w - q1.x * q2.x - q1.y * q2.y - q1.z * q2.z );
        }

        public Vector3 ToEulerAngles()
        {
            // returns euler angles in Radians

            // Calculate the Euler angles from the quaternion
            float roll = (float)Math.Atan2( 2 * (w * x + y * z), 1 - 2 * (x * x + y * y) );
            float pitch = (float)Math.Asin( 2 * (w * y - z * x) );
            float yaw = (float)Math.Atan2( 2 * (w * z + x * y), 1 - 2 * (y * y + z * z) );

            // Return the Euler angles as a Vector3
            return new Vector3( roll, pitch, yaw );
        }

        public static Quaternion FromEulerAngles( Vector3 euler )
        {
            // euler input in radians.

            float roll = euler.x;
            float pitch = euler.y;
            float yaw = euler.z;

            // Calculate the quaternion from the Euler angles
            float c1 = (float)Math.Cos( yaw / 2 );
            float c2 = (float)Math.Cos( pitch / 2 );
            float c3 = (float)Math.Cos( roll / 2 );
            float s1 = (float)Math.Sin( yaw / 2 );
            float s2 = (float)Math.Sin( pitch / 2 );
            float s3 = (float)Math.Sin( roll / 2 );
            float w = c1 * c2 * c3 - s1 * s2 * s3;
            float x = s1 * s2 * c3 + c1 * c2 * s3;
            float y = s1 * c2 * c3 + c1 * s2 * s3;
            float z = c1 * s2 * c3 - s1 * c2 * s3;

            // Return the calculated quaternion
            return new Quaternion( x, y, z, w );
        }
    }
}