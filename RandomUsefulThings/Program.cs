using System;

namespace RandomUsefulThings
{
    class Program
    {
        public static double ToRadians( double degrees )
        {
            return (Math.PI / 180) * degrees;
        }

        static void Main( string[] args )
        {
            Quaternion q1 = Quaternion.FromEulerAngles( new Vector3( (float)ToRadians( 90 ), (float)ToRadians( 0 ), (float)ToRadians( 0 ) ) );

            Vector3 euler = q1.ToEulerAngles();

            Quaternion q12 = Quaternion.FromEulerAngles( euler );

            Vector3 reflected = new Vector3( 1, 1, 1 ).Reflect( new Vector3( 0, 1, 0 ) );

            Console.WriteLine( "Hello World!" );
        }
    }
}
