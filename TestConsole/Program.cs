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
                { 1, 3, 1, 9 },
                { 1, 1, -1, 1 },
                { 3, 11, 5, 35 },
            } );

            Matrix U = m1.Eliminate();

        }
    }
}
