using Geometry;
using RandomUsefulThings.Math;
using RandomUsefulThings.Math.LinearAlgebra;
using RandomUsefulThings.Misc;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace TestConsole
{
    public static class Program
    {
        public static void Main( string[] args )
        {
            double d1 = System.Math.Sqrt( 9990 );
            int s1 = MathMethods.SqrtInt( 9990 );
            int s2 = MathMethods.SqrtIntFast( 5000 );


            SweepBenchmarkMath<float, float> b = new SweepBenchmarkMath<float, float>( 1000, 1000 )
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

            b.Add( "(float)System.Math.Sqrt(x)", ( x ) =>
            {
                return (float)System.Math.Sqrt( x );
            } );
            b.Add( "MathMethods.Sqrt", ( x ) =>
            {
                return MathMethods.Sqrt( x );
            } );

            b.Run();
        }
    }
}
