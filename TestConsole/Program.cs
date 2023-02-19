using Geometry;
using MathMethods;
using System;
using static MathMethods.Thermodynamics;

namespace TestConsole
{
    class Program
    {
        static void Main( string[] args )
        {
            Console.WriteLine( MathMethods.MathMethods.Factorial( 1 ) );
            Console.WriteLine( MathMethods.MathMethods.Factorial( 2 ) );
            Console.WriteLine( MathMethods.MathMethods.Factorial( 3 ) );
            Console.WriteLine( MathMethods.MathMethods.Factorial( 4 ) );
            Console.WriteLine( MathMethods.MathMethods.Factorial( 5 ) );
            Console.WriteLine( MathMethods.MathMethods.Factorial( 6 ) );

            Quaternion q = Quaternion.FromEulerAngles( -1.4f, 0.5f, 1.6f );

            Quaternion qi = q.Inverse();
            Quaternion qb = qi.Inverse();

            Vector3 euler = q.ToEulerAngles();

            FluidData inlet = new FluidData()
            {
                Velocity = 550,
                Pressure = 1013250, // 10 atm
                Temperature = 365,
                //Density = 1.293
            };

            Console.WriteLine( CompressibleSupersonic( 1, 100, inlet ) );

            Thermodynamics3.NozzleSegment ns = new Thermodynamics3.NozzleSegment()
            {
                InletArea = 1.0,
                ExitArea = 20.0,
                Velocity = 2000.0,
                Pressure = 1000000, // 1 MPa
                Temperature = 2500.0
            };

            Console.WriteLine( Thermodynamics3.CalculateExitProperties( ns ) );
        }
    }
}
