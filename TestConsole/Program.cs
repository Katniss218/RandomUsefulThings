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
            float asin1 = Trigonometry.Asin( 0.1f );
            float asin2 = Trigonometry.Asin( 0.2f );
            float asin3 = Trigonometry.Asin( 0.3f );
            float asin4 = Trigonometry.Asin( 0.4f );
            float asin5 = Trigonometry.Asin( 0.5f );
            float asin6 = Trigonometry.Asin( -0.6f );
            float asin7 = Trigonometry.Asin( 0.7f );
            float asin8 = Trigonometry.Asin( 0.8f );
            float asin9 = Trigonometry.Asin( -0.9f );
            float asin10 = Trigonometry.Asin( 0.95f );
            float asin11 = Trigonometry.Asin( 0.99f );
            float asin12 = Trigonometry.Asin( 0.999f );

            float rasin1 = (float)System.Math.Asin( 0.1f );
            float rasin2 = (float)System.Math.Asin( 0.2f );
            float rasin3 = (float)System.Math.Asin( 0.3f );
            float rasin4 = (float)System.Math.Asin( 0.4f );
            float rasin5 = (float)System.Math.Asin( 0.5f );
            float rasin6 = (float)System.Math.Asin( -0.6f );
            float rasin7 = (float)System.Math.Asin( 0.7f );
            float rasin8 = (float)System.Math.Asin( 0.8f );
            float rasin9 = (float)System.Math.Asin( -0.9f );
            float rasin10 = (float)System.Math.Asin( 0.95f );
            float rasin11 = (float)System.Math.Asin( 0.99f );
            float rasin12 = (float)System.Math.Asin( 0.999f );


            RandomUsefulThings.Misc.UnscaledTimeBenchmark b = new RandomUsefulThings.Misc.UnscaledTimeBenchmark();

            int val;
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

            b.Run( RandomUsefulThings.Misc.UnscaledTimeBenchmark.Mode.UnscaledTimeUnit );
        }
    }
}
