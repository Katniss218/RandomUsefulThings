using Geometry;
using RandomUsefulThings.Math;
using RandomUsefulThings.Math.LinearAlgebra;
using RandomUsefulThings.Math.LinearAlgebra.NumericalMethods;
using RandomUsefulThings.Misc;
using RandomUsefulThings.Physics.FluidSim;
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

            double[,] M = new double[,]
            {
                { 2, 1 },
                { 1, 1 },
            };
            double[] v = new double[]
                { 1, 2 };

          //  double[] s = GaussSeidel.SolveFast( M, v, new double[] { 0.1, 0.1 } );

            Euler2DReference fsim = new Euler2DReference( 1000.0f, 50, 50, 0.1f );
            //fsim.fluidAccelerationRelativeToContainer = new Vector2( 0, -9.8f );
           // fsim.OverrelaxationFactor = 1.9f;

            for( int i = 0; i < 10; i++ )
            {
           //     fsim.Simulate( 0.02f, -9.81f, 50 );
            }

           // fsim.Print();


            double st = CalculateDistance( 170, 1.03 );

            double d333 = System.Math.Sqrt( 9990 );
            int s1 = MathMethods.SqrtInt( 9990 );
            int s2 = MathMethods.SqrtIntFast( 5000 );

            UnscaledTimeBenchmark bu = new UnscaledTimeBenchmark( 10000, 10000 );

            double[,] M1 = new double[,]
            {
                { 2, 1 },
                { 1, 1 },
            };
            double[] v1 = new double[]
                { 1, 2 };

            double[] res;

            float fr;
            float f1 = 50f;
            float f2 = 5063452352534563543f;
            double dr;
            double d1 = 50f;
            double d2 = 5063452352534563543f;

            bu.Add( "assign float 4x", () =>
            {
                fr = f1;
                fr = f1;
                fr = f1;
                fr = f1;
            } );

            bu.Add( "assign add float 4x", () =>
            {
                fr = f1 + f2;
                fr = f1 + f2;
                fr = f1 + f2;
                fr = f1 + f2;
            } );
            
            bu.Add( "assign add double 4x", () =>
            {
                dr = d1 + d2;
                dr = d1 + d2;
                dr = d1 + d2;
                dr = d1 + d2;
            } );
            
            bu.Add( "assign `(float)System.Math.Sqrt( (implicit double)float )` 4x", () =>
            {
                fr = (float)System.Math.Sqrt( f1 );
                fr = (float)System.Math.Sqrt( f1 );
                fr = (float)System.Math.Sqrt( f1 );
                fr = (float)System.Math.Sqrt( f1 );
            } );
            
            bu.Add( "assign `(float)System.Math.Sqrt( double )` 4x", () =>
            {
                fr = (float)System.Math.Sqrt( d1 );
                fr = (float)System.Math.Sqrt( d1 );
                fr = (float)System.Math.Sqrt( d1 );
                fr = (float)System.Math.Sqrt( d1 );
            } );
            
            bu.Add( "assign `System.Math.Sqrt( (implicit double)float )` 4x", () =>
            {
                dr = System.Math.Sqrt( f1 );
                dr = System.Math.Sqrt( f1 );
                dr = System.Math.Sqrt( f1 );
                dr = System.Math.Sqrt( f1 );
            } );

            bu.Add( "assign `System.Math.Sqrt( double )` 4x", () =>
            {
                dr = System.Math.Sqrt( d1 );
                dr = System.Math.Sqrt( d1 );
                dr = System.Math.Sqrt( d1 );
                dr = System.Math.Sqrt( d1 );
            } );

            bu.Run( UnscaledTimeBenchmark.Mode.Nanosecond );

            /*
            SweepBenchmarkMath<float, float> bn = new SweepBenchmarkMath<float, float>( 1000, 1000 )
            {
                ParameterFunc = ( t ) => (t * 10000),
                //Reference = ( x ) => Math.Sin( x ),
                Reference = ( x ) => (float)System.Math.Sqrt(x),
                GetError = ( a, b ) => a - b
            };

            bn.Add( "Solve", ( x ) =>
            {
                return (float)System.Math.Sqrt( x );
            } );
            bn.Add( "Solve2", ( x ) =>
            {
                return MathMethods.Sqrt( x );
            } );

            bn.Run();*/
        }
    }
}
