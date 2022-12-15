using System;

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

        public float Determinant()
        {
            float _00 = this.M00, _01 = this.M01, _02 = this.M02, _03 = this.M03;
            float _10 = this.M10, _11 = this.M11, _12 = this.M12, _13 = this.M13;
            float _20 = this.M20, _21 = this.M21, _22 = this.M22, _23 = this.M23;
            float _30 = this.M30, _31 = this.M31, _32 = this.M32, _33 = this.M33;

            float kp_lo = (_22 * _33) - (_23 * _32);
            float jp_ln = (_21 * _33) - (_23 * _31);
            float jo_kn = (_21 * _32) - (_22 * _31);
            float ip_lm = (_20 * _33) - (_23 * _30);
            float io_km = (_20 * _32) - (_22 * _30);
            float in_jm = (_20 * _31) - (_21 * _30);

            float a11 = +(_11 * kp_lo - _12 * jp_ln + _13 * jo_kn);
            float a12 = -(_10 * kp_lo - _12 * ip_lm + _13 * io_km);
            float a13 = +(_10 * jp_ln - _11 * ip_lm + _13 * in_jm);
            float a14 = -(_10 * jo_kn - _11 * io_km + _12 * in_jm);

            float det = (_00 * a11) + (_01 * a12) + (_02 * a13) + (_03 * a14);
            return det;
        }
        /*public float Example4() // validate if this returns the same value for a wide array of different matrices.
        {
            float _00 = this.M00, _01 = this.M01, _02 = this.M02, _03 = this.M03;
            float _10 = this.M10, _11 = this.M11, _12 = this.M12, _13 = this.M13;
            float _20 = this.M20, _21 = this.M21, _22 = this.M22, _23 = this.M23;
            float _30 = this.M30, _31 = this.M31, _32 = this.M32, _33 = this.M33;

            float a11 = _00 * _11 - _01 * _10;
            float a12 = _00 * _12 - _02 * _10;
            float a13 = _00 * _13 - _03 * _10;
            float a14 = _01 * _12 - _02 * _11;
            float a15 = _01 * _13 - _03 * _11;
            float a16 = _02 * _13 - _03 * _12;

            float a21 = _20 * _31 - _21 * _30;
            float a22 = _20 * _32 - _22 * _30;
            float a23 = _20 * _33 - _23 * _30;
            float a24 = _21 * _32 - _22 * _31;
            float a25 = _21 * _33 - _23 * _31;
            float a26 = _22 * _33 - _23 * _32;

            float x = a11 * a26 - a12 * a25 + a13 * a24 + a14 * a23 - a15 * a22 + a16 * a21;
            return x;
        }*/

        public Matrix4x4 Invert()
        {
            float _00 = this.M00, _01 = this.M01, _02 = this.M02, _03 = this.M03;
            float _10 = this.M10, _11 = this.M11, _12 = this.M12, _13 = this.M13;
            float _20 = this.M20, _21 = this.M21, _22 = this.M22, _23 = this.M23;
            float _30 = this.M30, _31 = this.M31, _32 = this.M32, _33 = this.M33;

            float kp_lo = (_22 * _33) - (_23 * _32);
            float jp_ln = (_21 * _33) - (_23 * _31);
            float jo_kn = (_21 * _32) - (_22 * _31);
            float ip_lm = (_20 * _33) - (_23 * _30);
            float io_km = (_20 * _32) - (_22 * _30);
            float in_jm = (_20 * _31) - (_21 * _30);

            float a11 = +(_11 * kp_lo - _12 * jp_ln + _13 * jo_kn);
            float a12 = -(_10 * kp_lo - _12 * ip_lm + _13 * io_km);
            float a13 = +(_10 * jp_ln - _11 * ip_lm + _13 * in_jm);
            float a14 = -(_10 * jo_kn - _11 * io_km + _12 * in_jm);

            float det = (_00 * a11) + (_01 * a12) + (_02 * a13) + (_03 * a14);

            if( Math.Abs( det ) < float.Epsilon )
            {
                throw new Exception( "Matrix is not invertible" );
            }

            float invDet = 1.0f / det;

            float gp_ho = _12 * _33 - _13 * _32;
            float fp_hn = _11 * _33 - _13 * _31;
            float fo_gn = _11 * _32 - _12 * _31;
            float ep_hm = _10 * _33 - _13 * _30;
            float eo_gm = _10 * _32 - _12 * _30;
            float en_fm = _10 * _31 - _11 * _30;

            float gl_hk = _12 * _23 - _13 * _22;
            float fl_hj = _11 * _23 - _13 * _21;
            float fk_gj = _11 * _22 - _12 * _21;
            float el_hi = _10 * _23 - _13 * _20;
            float ek_gi = _10 * _22 - _12 * _20;
            float ej_fi = _10 * _21 - _11 * _20;

            return new Matrix4x4(
                a11 * invDet, -(_01 * kp_lo - _02 * jp_ln + _03 * jo_kn) * invDet, +(_01 * gp_ho - _02 * fp_hn + _03 * fo_gn) * invDet, -(_01 * gl_hk - _02 * fl_hj + _03 * fk_gj) * invDet,
                a12 * invDet, +(_00 * kp_lo - _02 * ip_lm + _03 * io_km) * invDet, -(_00 * gp_ho - _02 * ep_hm + _03 * eo_gm) * invDet, +(_00 * gl_hk - _02 * el_hi + _03 * ek_gi) * invDet,
                a13 * invDet, -(_00 * jp_ln - _01 * ip_lm + _03 * in_jm) * invDet, +(_00 * fp_hn - _01 * ep_hm + _03 * en_fm) * invDet, -(_00 * fl_hj - _01 * el_hi + _03 * ej_fi) * invDet,
                a14 * invDet, +(_00 * jo_kn - _01 * io_km + _02 * in_jm) * invDet, -(_00 * fo_gn - _01 * eo_gm + _02 * en_fm) * invDet, +(_00 * fk_gj - _01 * ek_gi + _02 * ej_fi) * invDet
            );
        }

        public static Matrix4x4 Translation( Vector3 translation )
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

        public static Matrix4x4 Rotation( Quaternion rotation )
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
            var translationMatrix = Translation( translation );
            var rotationMatrix = Rotation( rotation );
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

        // Transforms a position by this matrix, without a perspective divide. (fast)
        public Vector3 MultiplyPoint3x4( Vector3 v )
        {
            return new Vector3(
                this.M00 * v.X + this.M01 * v.Y + this.M02 * v.Z + this.M03,
                this.M10 * v.X + this.M11 * v.Y + this.M12 * v.Z + this.M13,
                this.M20 * v.X + this.M21 * v.Y + this.M22 * v.Z + this.M23
            );
        }

        // Transforms a direction by this matrix.
        public Vector3 MultiplyVector( Vector3 v )
        {
            return new Vector3(
                this.M00 * v.X + this.M01 * v.Y + this.M02 * v.Z,
                this.M10 * v.X + this.M11 * v.Y + this.M12 * v.Z,
                this.M20 * v.X + this.M21 * v.Y + this.M22 * v.Z
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