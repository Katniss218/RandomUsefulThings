using System;
using System.Collections.Generic;
using System.Text;

namespace RandomUsefulThings.Math.LinearAlgebra
{
    public struct Matrix2x2
    {
        public float M00 { get; }
        public float M01 { get; }
        public float M10 { get; }
        public float M11 { get; }

        public Matrix2x2( float m00, float m01,
                          float m10, float m11 )
        {
            this.M00 = m00;
            this.M01 = m01;
            this.M10 = m10;
            this.M11 = m11;
        }

        public float Determinant()
        {
            float det = (M00 * M11) - (M01 * M10);
            return det;
        }

        public static Matrix2x2 Rotation( float angle )
        {
            // counterclockwise rotation (if x points right, and y points up).

            return new Matrix2x2(
                (float)System.Math.Cos( angle ), (float)System.Math.Sin( angle ),
                (float)-System.Math.Sin( angle ), (float)System.Math.Cos( angle )
            );
        }
    }
}
