using Geometry;
using MathMethods;
using RandomUsefulThings.Math.LinearAlgebra;
using System;
using static MathMethods.Thermodynamics;

namespace TestConsole
{
    class Program
    {
        public static int MySqrt2( int x )
        {
            double Prev = 0;
            double Cur = x;
            while( (int)Prev != (int)Cur )
            {
                Prev = Cur;
                Cur = Cur - ((Cur * Cur) - x) / (2 * Cur);
            }
            return (int)(Cur - (Cur % 1));
        }

        static void Main( string[] args )
        {
            var x = MySqrt2( 9 );
            var y = MySqrt2( 2 );
            var z = MySqrt2( 4 );

            Matrix m1 = new Matrix( new double[,]
            {
                { 1, 1, 0 }, // {}x + {}y = {}
                { -1, 1, 1 }, // {}x + {}y = {}
            } );

            Matrix U = m1.Eliminate();
            double[] values = Matrix.BackSubstitution( U );

            RandomUsefulThings.Misc.UnscaledTimeBenchmark b = new RandomUsefulThings.Misc.UnscaledTimeBenchmark();

            int val;
            int a = 5;
            int bb = 10;
            int c = 0;
            bool boolV;
            int i = 0;
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

            b.Run( RandomUsefulThings.Misc.UnscaledTimeBenchmark.Mode.UnscaledTimeUnit );
        }
    }
}
