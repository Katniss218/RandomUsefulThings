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
            string s = MathMethods.NumberToString( -12345, new[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' } );
            string s2 = MathMethods.NumberToString( -12345, new[] { '0', '1' } );

            int i = MathMethods.NumberFromString( s, new[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' } );
            int i2 = MathMethods.NumberFromString( s2, new[] { '0', '1' } );

            SweepBenchmarkMath<float, double> b = new SweepBenchmarkMath<float, double>( 100, 1000 )
            {
                ParameterFunc = ( t ) => t * 100 - 50,
                //Reference = ( x ) => Math.Sin( x ),
                Reference = ( x ) => Math.Exp( x ),
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

            b.Add( "Custom fac(double)", ( x ) =>
            {
                return Math.Exp( x );
            } );
            b.Add( "Custom fac(double)", ( x ) =>
            {
                return MathMethods.Exp( x );
            } );
            b.Add( "Custom fac(double)", ( x ) =>
            {
                return MathMethods.ExpFunky( x );
            } );

            b.Run();
        }
    }
}
