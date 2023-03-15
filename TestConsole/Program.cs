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

            x = Trigonometry.AsinTaylor( 0.5f );
            x = (float)Math.Tan( 4.7125f );

            double xd;

            xd = MathMethods.BellCurve( 0, 0.4, 0 );
            xd = MathMethods.BellCurve( 1, 0.4, 0 );
            xd = MathMethods.BellCurve( 2, 0.4, 0 );

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
            b.Add( "Custom fac(double)", ( x ) =>
            {
                return MathMethods.ExpInt( (int)x, 3 );
            } );

            b.Run();
        }
    }
}
