using Geometry;
using System;

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
        }
    }
}
