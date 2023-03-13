using Geometry;
using MathMethods;
using RandomUsefulThings.Math;
using RandomUsefulThings.Math.LinearAlgebra;
using System;
using static MathMethods.Thermodynamics;

namespace TestConsole
{
    class Program
    {

        static void Main( string[] args )
        {
            float x1 = MathMethods.MathMethods.Modulo( 5.44f, 2.5f );
            float x2 = MathMethods.MathMethods.Modulo( 10.4f, 2f );
            float x3 = MathMethods.MathMethods.Modulo( -65.44f, -2.5f );
            float x4 = MathMethods.MathMethods.Modulo( 6.44f, -5f );


            RandomUsefulThings.Misc.UnscaledTimeBenchmark b = new RandomUsefulThings.Misc.UnscaledTimeBenchmark();

            /*int val;
            int a = 5;
            int bb = 10;
            int c = 0;
            bool boolV;
            int i = 0;
            double vald = 50.4353;
            float valf = 50.4353f;
            b.Add( "i++", () =>
            {
                i++;
            } );
            b.Add( "c += a + b;", () =>
             {
                 c += a + bb;
             } );
            b.Add( "c += a + b; (times 10)", () =>
            {
                c += a + bb;
                c += a + bb;
                c += a + bb;
                c += a + bb;
                c += a + bb;
                c += a + bb;
                c += a + bb;
                c += a + bb;
                c += a + bb;
                c += a + bb;
            } );
            b.Add( "boolV = c < (b - a);", () =>
            {
                boolV = (c < (bb - a));
            } );
            b.Add( "MySqrt2( 100 )", () =>
             {
                 val = MySqrt2( 100 );
             } );
            b.Add( "MySqrt2( 1000 )", () =>
             {
                 val = MySqrt2( 1000 );
             } );
            b.Add( "MySqrt2( 10000 )", () =>
             {
                 val = MySqrt2( 10000 );
             } );
            b.Add( "MySqrt2( 100000 )", () =>
             {
                 val = MySqrt2( 100000 );
             } );
            b.Add( "MySqrt2( 1000000 )", () =>
             {
                 val = MySqrt2( 1000000 );
             } );
            b.Add( "atan", () =>
            {
                vald = Math.Atan( vald );
            } );
            b.Add( "custom atan", () =>
            {
                valf = Trigonometry.Atan( valf );
            } );

            b.Run( RandomUsefulThings.Misc.UnscaledTimeBenchmark.Mode.UnscaledTimeUnit );*/
        }
    }
}
