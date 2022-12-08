using System;
using System.Collections.Generic;
using System.Text;

namespace RandomUsefulThings
{
    public struct Matrix4x4
    {
        public float M00 { get; }
        public float M01 { get; }
        public float M02 { get; }
        public float M03 { get; }
        public float M10 { get; }
        public float M11 { get; }
        public float M12 { get; }
        public float M13 { get; }
        public float M20 { get; }
        public float M21 { get; }
        public float M22 { get; }
        public float M23 { get; }
        public float M30 { get; }
        public float M31 { get; }
        public float M32 { get; }
        public float M33 { get; }

        /// <summary>
        /// An identity matrix is a square matrix with 1s on the diagonal and 0s everywhere else.
        /// </summary>
        public static Matrix4x4 Identity
        {
            get => new Matrix4x4(
                1, 0, 0, 0,
                0, 1, 0, 0,
                0, 0, 1, 0,
                0, 0, 0, 1 
            );
        }

        public Matrix4x4( float m00, float m01, float m02, float m03,
                         float m10, float m11, float m12, float m13,
                         float m20, float m21, float m22, float m23,
                         float m30, float m31, float m32, float m33 )
        {
            this.M00 = m00;
            this.M01 = m01;
            this.M02 = m02;
            this.M03 = m03;
            this.M10 = m10;
            this.M11 = m11;
            this.M12 = m12;
            this.M13 = m13;
            this.M20 = m20;
            this.M21 = m21;
            this.M22 = m22;
            this.M23 = m23;
            this.M30 = m30;
            this.M31 = m31;
            this.M32 = m32;
            this.M33 = m33;
        }

        public Matrix4x4 Transposed()
        {
            // To transpose a matrix, we simply swap the elements at positions (i, j) and (j, i) for all possible values of i and j.
            // For example, the element at position (0, 1) in the original matrix becomes the element at position (1, 0) in the transposed matrix, and so on.

            return new Matrix4x4(
                M00, M10, M20, M30,
                M01, M11, M21, M31,
                M02, M12, M22, M32,
                M03, M13, M23, M33
            );
        }

        [Obsolete( "Unconfirmed" )]
        public float Determinant()
        {
            // Compute the cofactors of the first row of the matrix.
            float c00 = M11 * (M22 * M33 - M23 * M32)
                      - M12 * (M21 * M33 - M23 * M31)
                      + M13 * (M21 * M32 - M22 * M31);
            float c01 = M12 * (M20 * M33 - M23 * M30)
                      - M13 * (M20 * M32 - M22 * M30)
                      + M10 * (M22 * M31 - M21 * M32);
            float c02 = M13 * (M20 * M31 - M21 * M30)
                      - M10 * (M22 * M31 - M21 * M32)
                      + M11 * (M20 * M32 - M22 * M30);
            float c03 = M10 * (M21 * M33 - M23 * M31)
                      - M11 * (M20 * M33 - M23 * M30)
                      + M12 * (M20 * M31 - M21 * M30);

            // Return the determinant as the dot product of the first row
            // and the vector of cofactors.
            return M00 * c00 + M01 * c01 + M02 * c02 + M03 * c03;
        }

        /*public Matrix4x4 Inverse()
        {
            float determinant = Determinant();
            if( determinant == 0 )
            {
                throw new InvalidOperationException( "Matrix is not invertible." );
            }

            float invDet = 1 / determinant;
            return new Matrix4x4(
                invDet * (M11 * M22 * M33 + M12 * M23 * M31 + M13 * M21 * M32 - M11 * M23 * M32 - M12 * M21 * M33 - M13 * M22 * M31),
                invDet * (M01 * M23 * M32 + M02 * M21 * M33 + M03 * M22 * M31 - M01 * M22 * M33 - M02 * M23 * M31 - M03 * M21 * M32),
                invDet * (M01 * M12 * M33 + M02 * M13 * M31 + M03 * M11 * M32 - M01 * M13 * M32 - M02 * M11 * M33 - M03 * M12 * M31),
                invDet * (M01 * M13 * M22 + M02 * M11 * M23 + M03 * M12 * M21 - M01 * M12 * M23 - M02 * M13 * M21 - M03 * M11 * M22),
                invDet * (M10 * M23 * M32 + M12 * M20 * M33 + M13 * M22 * M30 - M10 * M22 * M33 - M12 * M23 * M30 - M13 * M20 * M32),
                invDet * (M00 * M22 * M33 + M02 * M23 * M30 + M03 * M20 * M32 - M00 * M23 * M32 - M02 * M
            
            // AI failed here.

                );
        }*/

        public static Matrix4x4 Translate( Vector3 translation )
        {
            return new Matrix4x4(
                1, 0, 0, translation.X,
                0, 1, 0, translation.Y,
                0, 0, 1, translation.Z,
                0, 0, 0, 1
            );
        }

        public static Matrix4x4 Scale( Vector3 scale )
        {
            return new Matrix4x4(
                scale.X, 0, 0, 0,
                0, scale.Y, 0, 0,
                0, 0, scale.Z, 0,
                0, 0, 0, 1
            );
        }

        [Obsolete( "Unconfirmed" )]
        public static Matrix4x4 Rotate( Quaternion rotation )
        {
            float xSq = rotation.X * rotation.X;
            float ySq = rotation.Y * rotation.Y;
            float zSq = rotation.Z * rotation.Z;

            float xy = rotation.X * rotation.Y;
            float xz = rotation.X * rotation.Z;
            float yz = rotation.Y * rotation.Z;
            float wx = rotation.W * rotation.X;
            float wy = rotation.W * rotation.Y;
            float wz = rotation.W * rotation.Z;

            return new Matrix4x4(
                1 - 2 * (ySq + zSq), 2 * (xy - wz), 2 * (xz + wy), 0,
                2 * (xy + wz), 1 - 2 * (xSq + zSq), 2 * (yz - wx), 0,
                2 * (xz - wy), 2 * (yz + wx), 1 - 2 * (xSq + ySq), 0,
                0, 0, 0, 1
            );
        }

        [Obsolete( "Unconfirmed" )]
        public static Matrix4x4 Transform( Vector3 translation, Quaternion rotation, Vector3 scale )
        {
            var translationMatrix = Translate( translation );
            var rotationMatrix = Rotate( rotation );
            var scaleMatrix = Scale( scale );

            // Multiply the matrices in the correct order to obtain the final transformation matrix
            return translationMatrix * rotationMatrix * scaleMatrix;
        }

        public static Matrix4x4 Multiply( Matrix4x4 m, float f )
        {
            return new Matrix4x4(
                m.M00 * f, m.M01 * f, m.M02 * f, m.M03 * f,
                m.M10 * f, m.M11 * f, m.M12 * f, m.M13 * f,
                m.M20 * f, m.M21 * f, m.M22 * f, m.M23 * f,
                m.M30 * f, m.M31 * f, m.M32 * f, m.M33 * f
            );
        }

        [Obsolete( "Unconfirmed" )]
        public static Matrix4x4 Multiply( Matrix4x4 m1, Matrix4x4 m2 )
        {
            return new Matrix4x4(
                (m1.M00 * m2.M00) + (m1.M01 * m2.M10) + (m1.M02 * m2.M20) + (m1.M03 * m2.M30),
                (m1.M00 * m2.M01) + (m1.M01 * m2.M11) + (m1.M02 * m2.M21) + (m1.M03 * m2.M31),
                (m1.M00 * m2.M02) + (m1.M01 * m2.M12) + (m1.M02 * m2.M22) + (m1.M03 * m2.M32),
                (m1.M00 * m2.M03) + (m1.M01 * m2.M13) + (m1.M02 * m2.M23) + (m1.M03 * m2.M33),

                (m1.M10 * m2.M00) + (m1.M11 * m2.M10) + (m1.M12 * m2.M20) + (m1.M13 * m2.M30),
                (m1.M10 * m2.M01) + (m1.M11 * m2.M11) + (m1.M12 * m2.M21) + (m1.M13 * m2.M31),
                (m1.M10 * m2.M02) + (m1.M11 * m2.M12) + (m1.M12 * m2.M22) + (m1.M13 * m2.M32),
                (m1.M10 * m2.M03) + (m1.M11 * m2.M13) + (m1.M12 * m2.M23) + (m1.M13 * m2.M33),

                (m1.M20 * m2.M00) + (m1.M21 * m2.M10) + (m1.M22 * m2.M20) + (m1.M23 * m2.M30),
                (m1.M20 * m2.M01) + (m1.M21 * m2.M11) + (m1.M22 * m2.M21) + (m1.M23 * m2.M31),
                (m1.M20 * m2.M02) + (m1.M21 * m2.M12) + (m1.M22 * m2.M22) + (m1.M23 * m2.M32),
                (m1.M20 * m2.M03) + (m1.M21 * m2.M13) + (m1.M22 * m2.M23) + (m1.M23 * m2.M33),

                (m1.M30 * m2.M00) + (m1.M31 * m2.M10) + (m1.M32 * m2.M20) + (m1.M33 * m2.M30),
                (m1.M30 * m2.M01) + (m1.M31 * m2.M11) + (m1.M32 * m2.M21) + (m1.M33 * m2.M31),
                (m1.M30 * m2.M02) + (m1.M32 * m2.M12) + (m1.M32 * m2.M22) + (m1.M33 * m2.M32),
                (m1.M30 * m2.M03) + (m1.M33 * m2.M13) + (m1.M32 * m2.M23) + (m1.M33 * m2.M33)
            );
        }

        public static Matrix4x4 operator *( Matrix4x4 m1, float f )
        {
            return Multiply( m1, f );
        }

        public static Matrix4x4 operator *( float f, Matrix4x4 m1 )
        {
            return Multiply( m1, f );
        }

        [Obsolete( "Unconfirmed" )]
        public static Matrix4x4 operator *( Matrix4x4 m1, Matrix4x4 m2 )
        {
            return Multiply( m1, m2 );
        }
    }
}