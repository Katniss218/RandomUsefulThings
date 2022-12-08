using System;
using System.Collections.Generic;
using System.Text;

namespace RandomUsefulThings
{
    public struct Matrix4x4
    {
        private readonly float
            m00, m01, m02, m03,
            m10, m11, m12, m13,
            m20, m21, m22, m23,
            m30, m31, m32, m33;

        public Matrix4x4( float m00, float m01, float m02, float m03,
                         float m10, float m11, float m12, float m13,
                         float m20, float m21, float m22, float m23,
                         float m30, float m31, float m32, float m33 )
        {
            this.m00 = m00;
            this.m01 = m01;
            this.m02 = m02;
            this.m03 = m03;
            this.m10 = m10;
            this.m11 = m11;
            this.m12 = m12;
            this.m13 = m13;
            this.m20 = m20;
            this.m21 = m21;
            this.m22 = m22;
            this.m23 = m23;
            this.m30 = m30;
            this.m31 = m31;
            this.m32 = m32;
            this.m33 = m33;
        }

        [Obsolete("Unconfirmed")]
        public Matrix4x4 Translate( Vector3 translation )
        {
            return new Matrix4x4(
                1, 0, 0, translation.X,
                0, 1, 0, translation.Y,
                0, 0, 1, translation.Z,
                0, 0, 0, 1
            );
        }

        [Obsolete( "Unconfirmed" )]
        public Matrix4x4 Scale( Vector3 scale )
        {
            return new Matrix4x4(
                scale.X, 0, 0, 0,
                0, scale.Y, 0, 0,
                0, 0, scale.Z, 0,
                0, 0, 0, 1
            );
        }

        [Obsolete( "Unconfirmed" )]
        public Matrix4x4 Rotate( Quaternion rotation )
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
        public Matrix4x4 Transform( Vector3 translation, Vector3 scale, Quaternion rotation )
        {
            var translationMatrix = Translate( translation );
            var scaleMatrix = Scale( scale );
            var rotationMatrix = Rotate( rotation );

            // Multiply the matrices in the correct order to obtain the final transformation matrix
            return translationMatrix * rotationMatrix * scaleMatrix;
        }

        [Obsolete( "Unconfirmed" )]
        public static Matrix4x4 Multiply( Matrix4x4 a, Matrix4x4 b )
        {
            return new Matrix4x4(
                (a.m00 * b.m00) + a.m01 * b.m10 + a.m02 * b.m20 + a.m03 * b.m30,
                (a.m00 * b.m01) + a.m01 * b.m11 + a.m02 * b.m21 + a.m03 * b.m31,
                (a.m00 * b.m02) + a.m01 * b.m12 + a.m02 * b.m22 + a.m03 * b.m32,
                (a.m00 * b.m03) + a.m01 * b.m13 + a.m02 * b.m23 + a.m03 * b.m33,

                (a.m10 * b.m00) + (a.m11 * b.m10) + (a.m12 * b.m20) + (a.m13 * b.m30),
                (a.m10 * b.m01) + (a.m11 * b.m11) + (a.m12 * b.m21) + (a.m13 * b.m31),
                (a.m10 * b.m02) + (a.m11 * b.m12) + (a.m12 * b.m22) + (a.m13 * b.m32),
                (a.m10 * b.m03) + (a.m11 * b.m13) + (a.m12 * b.m23) + (a.m13 * b.m33),

                (a.m20 * b.m00) + (a.m21 * b.m10) + (a.m22 * b.m20) + (a.m23 * b.m30),
                (a.m20 * b.m01) + (a.m21 * b.m11) + (a.m22 * b.m21) + (a.m23 * b.m31),
                (a.m20 * b.m02) + (a.m21 * b.m12) + (a.m22 * b.m22) + (a.m23 * b.m32),
                (a.m20 * b.m03) + (a.m21 * b.m13) + (a.m22 * b.m23) + (a.m23 * b.m33),

                (a.m30 * b.m00) + (a.m31 * b.m10) + (a.m32 * b.m20) + (a.m33 * b.m30),
                (a.m30 * b.m01) + (a.m31 * b.m11) + (a.m32 * b.m21) + (a.m33 * b.m31),
                (a.m30 * b.m02) + (a.m32 * b.m12) + (a.m32 * b.m22) + (a.m33 * b.m32),
                (a.m30 * b.m03) + (a.m33 * b.m13) + (a.m32 * b.m23) + (a.m33 * b.m33)
            );
        }

        [Obsolete( "Unconfirmed" )]
        public static Matrix4x4 operator *( Matrix4x4 a, Matrix4x4 b )
        {
            return Multiply( a, b );
        }
    }
}