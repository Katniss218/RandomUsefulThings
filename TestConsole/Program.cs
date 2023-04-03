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

            double f1 = RandomUsefulThings.Math.Integrals.NumericalMethods.TrapezoidalRule.Integrate( x => System.Math.Sin( x ), 0.0, 5.0, 50 );
            double f2 = RandomUsefulThings.Math.Integrals.NumericalMethods.TrapezoidalRule.Integrate2( x => System.Math.Sin( x ), 0.0, 5.0, 50 );
            double f3 = RandomUsefulThings.Math.Integrals.NumericalMethods.SimpsonRule.Integrate( x => System.Math.Sin( x ), 0.0, 5.0, 50 );

            double e12 = f1 - f2;
            double e13 = f1 - f3;

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

            b.Run();
        }
    }
}
