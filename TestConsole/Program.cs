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

            double[] s = GaussSeidel.SolveFast( M, v, new double[] { 0.1, 0.1 } );

            Euler2DReference fsim = new Euler2DReference( 1000.0f, 50, 50, 0.1f );
            //fsim.fluidAccelerationRelativeToContainer = new Vector2( 0, -9.8f );
           // fsim.OverrelaxationFactor = 1.9f;

            for( int i = 0; i < 10; i++ )
            {
           //     fsim.Simulate( 0.02f, -9.81f, 50 );
            }

           // fsim.Print();


            double st = CalculateDistance( 170, 1.03 );

            double d1 = System.Math.Sqrt( 9990 );
            int s1 = MathMethods.SqrtInt( 9990 );
            int s2 = MathMethods.SqrtIntFast( 5000 );

            UnscaledTimeBenchmark bu = new UnscaledTimeBenchmark();

            double[,] M1 = new double[,]
            {
                { 2, 1 },
                { 1, 1 },
            };
            double[] v1 = new double[]
                { 1, 2 };

            double[] res;

            bu.Add( "Jacobi", () =>
            {
                res = Jacobi.Solve2( M1, v1, 1e-6f, 1000 );
            } );
            bu.Add( "GaussSeidel Fast", () =>
            {
                res = GaussSeidel.SolveFast( M1, v1, null, 1e-6f, 1000 );
            } );
            bu.Add( "GaussSeidel", () =>
            {
                res = GaussSeidel.Solve2( M1, v1, 1e-6f, 1000 );
            } );
            bu.Add( "SOR 1.1", () =>
            {
                res = SuccessiveOverRelaxation.Solve2( M1, v1, 1.1, 1e-6f, 1000 );
            } );
            bu.Add( "SOR 1.1 Fast-ish", () =>
            {
                res = SuccessiveOverRelaxation.SolveFastIsh( M1, v1, 1.1, 1e-6f, 1000 );
            } );
            bu.Add( "SOR 1.5", () =>
            {
                res = SuccessiveOverRelaxation.Solve2( M1, v1, 1.5, 1e-6f, 1000 );
            } );
            bu.Add( "SOR 1.9", () =>
            {
                res = SuccessiveOverRelaxation.Solve2( M1, v1, 1.9, 1e-6f, 1000 );
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
