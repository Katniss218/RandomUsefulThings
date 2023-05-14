using Geometry;
using RandomUsefulThings.Math;
using RandomUsefulThings.Math.LinearAlgebra;
using RandomUsefulThings.Math.LinearAlgebra.NumericalMethods;
using RandomUsefulThings.Misc;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace TestConsole
{
    public static class Program
    {
        public static double CalculateDistance( double initialVelocity, double acceleration )
        {
            // returns the distance at which the object with a given velocity and acceleration will reach 0 velocity.
            double distance = Math.Pow( initialVelocity, 2 ) / (2 * acceleration);
            return distance;
        }

        public static void Main( string[] args )
        {
            
            double st = CalculateDistance( 170, 1.03 );

            double d1 = System.Math.Sqrt( 9990 );
            int s1 = MathMethods.SqrtInt( 9990 );
            int s2 = MathMethods.SqrtIntFast( 5000 );


            SweepBenchmarkMath<float, float> bn = new SweepBenchmarkMath<float, float>( 1000, 1000 )
            {
                ParameterFunc = ( t ) => (t * 10000),
                //Reference = ( x ) => Math.Sin( x ),
                Reference = ( x ) => (float)System.Math.Sqrt(x),
                GetError = ( a, b ) => a - b
            };

            /*b.Add( "Math.Sin(double)", ( x ) =>
            {
                return Math.Sin( x );
            } );
            b.Add( "Custom Sin(float)", ( x ) =>
            {
                return Trigonometry.Sin( x );
            } );*/

            bn.Add( "(float)System.Math.Sqrt(x)", ( x ) =>
            {
                return (float)System.Math.Sqrt( x );
            } );
            bn.Add( "MathMethods.Sqrt", ( x ) =>
            {
                return MathMethods.Sqrt( x );
            } );

            bn.Run();
        }
    }
}
