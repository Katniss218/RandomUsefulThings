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
            /*
            [ 2, -1,  0]     [ 0]      [ 1]
        A = [-1,  2, -1] b = [-1]      [-6]
            [ 0, -3,  4]     [ 4]      [19]

            
            [ 0,  1,  0]
        A = [ 0, -2, -4]
            [ 0,  3, 16]

            */

            Matrix m = new Matrix( new double[,]
            {
                { 2, -1, 0 },
                { -1, 2, -1 },
                { 0, -3, 4 }
            } );
            Vector v = new Vector( new double[]
                { 0, -1, 4 } );

            Vector vr2 = Matrix.Multiply( m, v );
        }
    }
}
