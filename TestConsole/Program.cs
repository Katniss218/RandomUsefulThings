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

            Matrix m = new Matrix( new double[,]
            {
                { 1, 2, 1 },
                { 3, 6, 1 },
                { 0, 4, 1 },
            } );

            Matrix U = m.Eliminate();
        }
    }
}
