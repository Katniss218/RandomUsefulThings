using System;
using System.Collections.Generic;
using System.Text;

namespace RandomUsefulThings.Math.LinearAlgebra
{
    public struct Matrix3x3
    {
        [Obsolete("Untested but looks okay")]
        public static double Determinant3x3Sarrus( double[,] matrix )
        {
            double det = (matrix[0, 0] * matrix[1, 1] * matrix[2, 2]) + (matrix[1, 0] * matrix[2, 1] * matrix[0, 2]) + (matrix[2, 0] * matrix[0, 1] * matrix[1, 2]) - (matrix[0, 2] * matrix[1, 1] * matrix[2, 0]) - (matrix[1, 2] * matrix[2, 1] * matrix[0, 0]) - (matrix[2, 2] * matrix[0, 1] * matrix[1, 0]);
            return det;
        }
    }
}
