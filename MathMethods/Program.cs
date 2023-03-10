using System;

namespace Geometry
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
            Quaternion q = Quaternion.FromEulerAngles( new Vector3( (float)ToRadians( 0 ), (float)ToRadians( 90 ), (float)ToRadians( 0 ) ) );
            Matrix4x4 m = Matrix4x4.Rotation( q );
            Console.WriteLine( m.Determinant() );
            //Console.WriteLine( m.Example4() );
            Quaternion q2 = Quaternion.FromEulerAngles( new Vector3( (float)ToRadians( 45 ), (float)ToRadians( 0 ), (float)ToRadians( 54 ) ) );
            Matrix4x4 m2 = Matrix4x4.Rotation( q2 );
            Console.WriteLine( m2.Determinant() );
            //Console.WriteLine( m2.Example4() );

            // (0,0,5) * y90 => (5,0,0)

            Matrix4x4 mInverted = m.Invert();

            Vector3 pos = new Vector3( 0, 0, 5 );

            pos = m.MultiplyPoint3x4( pos );

            pos = mInverted.MultiplyPoint3x4( pos );

            Console.WriteLine( "Hello World!" );
        }
    }
}
