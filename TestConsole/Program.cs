using Geometry;
using MathMethods;
using RandomUsefulThings.Math.LinearAlgebra;
using System;
using static MathMethods.Thermodynamics;

namespace TestConsole
{
    class Program
    {
        static void Main( string[] args )
        {

            Matrix m1 = new Matrix( new double[,]
            {
                { 1, 1, 0 }, // {}x + {}y = {}
                { -1, 1, 1 }, // {}x + {}y = {}
            } );

            Matrix U = m1.Eliminate();
            double[] values = Matrix.BackSubstitution( U );

        }
    }
}
