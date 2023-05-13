using System;
using System.Collections.Generic;
using System.Text;

namespace RandomUsefulThings.Math.LinearAlgebra
{
    public struct Matrix3x3
    {
        public float M00 { get; }
        public float M01 { get; }
        public float M02 { get; }

        public float M10 { get; }
        public float M11 { get; }
        public float M12 { get; }

        public float M20 { get; }
        public float M21 { get; }
        public float M22 { get; }

        [Obsolete("Untested but looks okay")]
        public double Determinant()
        {
            // Sarrus' rule.
            double det = (M00 * M11 * M22) + (M10 * M21 * M02) + (M20 * M01 * M12)
                       - (M02 * M11 * M20) - (M12 * M21 * M00) - (M22 * M01 * M10);
            return det;
        }
    }
}