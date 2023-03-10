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
                { 1, 2, 1 },
                { 3, 6, 1 },
                { 0, 4, 1 },
            } ); 
            Matrix m2 = new Matrix( new double[,]
            {
                { 1, 0, 0 },
                { 0, 1, 0 },
                { 0, 0, 1 },
            } );

            Matrix U = Matrix.Multiply( m1, m2 );

            Test( null );
        }
    }
}
