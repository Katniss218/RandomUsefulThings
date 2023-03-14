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
            float x;

            x = Trigonometry.Atan( 4984.8f );
            x = (float)Math.Tan( 4.7125f );

            double xd;

            xd = MathMethods.BellCurve( 0, 0.4, 0 );
            xd = MathMethods.BellCurve( 1, 0.4, 0 );
            xd = MathMethods.BellCurve( 2, 0.4, 0 );

            SweepBenchmarkMath<float, double> b = new SweepBenchmarkMath<float, double>( 100000, 1000 )
            {
                ParameterFunc = ( t ) => t * 100,
                //Reference = ( x ) => Math.Sin( x ),
                Reference = ( x ) => Math.Atan( x ),
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

            b.Add( "Math.Acos(double)", ( x ) =>
            {
                return Math.Atan( x );
            } );
            b.Add( "Custom Acos(double)", ( x ) =>
            {
                return Trigonometry.Atan( x );
            } );

            b.Run();
        }
    }
}
