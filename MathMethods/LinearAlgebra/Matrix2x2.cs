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

        public static Matrix2x2 Multiply_Strassen( Matrix2x2 m1, Matrix2x2 m2 )
        {
            float p1 = (m1.M00 + m1.M11) * (m2.M00 + m2.M11);
            float p2 = (m1.M10 + m1.M11) * (m2.M00);
            float p3 = (m1.M00) * (m2.M01 - m2.M11);
            float p4 = (m1.M11) * (-m2.M00 + m2.M10);
            float p5 = (m1.M00 + m1.M01) * (m2.M11);
            float p6 = (-m1.M00 + m1.M10) * (m2.M00 + m2.M01);
            float p7 = (m1.M01 - m1.M11) * (m2.M10 + m2.M11);

            float m00 = p1 + p4 - p5 + p7;
            float m01 = p3 + p5;
            float m10 = p2 + p4;
            float m11 = p1 + p2 - p3 + p6;

            return new Matrix2x2(
                m00, m01,
                m10, m11 );
        }
    }
}