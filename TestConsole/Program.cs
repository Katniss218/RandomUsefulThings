using Geometry;
using MathMethods;
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
            float x;

            x = Trigonometry.Cos( 3.1416f );

            SweepBenchmarkMath<float, double> b = new SweepBenchmarkMath<float, double>( 100000, 1000 )
            {
                ParameterFunc = ( t ) => t * 4000000 + 0.001f,
                //Reference = ( x ) => Math.Sin( x ),
                Reference = ( x ) => Math.Sqrt( x ),
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

            b.Add( "Math.Exp(double)", ( x ) =>
            {
                return Math.Sqrt( x );
            } );
            b.Add( "Custom Exp(double)", ( x ) =>
            {
                return MathMethods.MathMethods.Sqrt( x );
            } );

            b.Run();
        }
    }
}
