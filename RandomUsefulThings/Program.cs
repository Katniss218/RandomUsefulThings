using MathMethods;
using System;

namespace RandomUsefulThings
{
    class Program
    {
        public static double ToRadians( double degrees )
        {
            return (Math.PI / 180) * degrees;
        }

        public static double ToDegrees( double radians )
        {
            return (180 / Math.PI) * radians;
        }

        static void Main( string[] args )
        {
            //Quaternion q1 = Quaternion.FromEulerAngles( new Vector3( (float)ToRadians( 90 ), (float)ToRadians( 0 ), (float)ToRadians( 0 ) ) );
            Quaternion q2 = Quaternion.FromEulerAngles( new Vector3( (float)ToRadians( 0 ), (float)ToRadians( 90 ), (float)ToRadians( 0 ) ) );
            Vector3 vback = q2.ToEulerAngles();


            // (0,0,5) * y90 => (5,0,0)

            //float angle2 = (float)ToDegrees( Quaternion.Angle( q1, q2 ) );

            //Vector3 v1 = new Vector3( 2, 1, 0 );
            //Vector3 vp = v1.ProjectOnto( new Vector3( 0, 1, 0 ) );

            Matrix4x4 m = Matrix4x4.Rotation( q2 );
            Matrix4x4 mInverted = m.Invert();

            Vector3 pos = new Vector3( 0, 0, 5 );

            pos = m.MultiplyPoint3x4( pos );

            pos = mInverted.MultiplyPoint3x4( pos );

            Console.WriteLine( "Hello World!" );
        }
    }
}
