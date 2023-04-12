using System;
using System.Collections.Generic;
using System.Text;

namespace RandomUsefulThings.Math.LinearAlgebra
{
    public struct Matrix2x2
    {
        public static double Determinant2x2( double[,] matrix )
        {
            double det = (matrix[0, 0] * matrix[1, 1]) - (matrix[0, 1] * matrix[1, 0]);
            return det;
        }
    }
}
