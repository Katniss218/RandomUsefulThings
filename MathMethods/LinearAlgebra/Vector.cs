using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace RandomUsefulThings.Math.LinearAlgebra
{
    public struct Vector : IEquatable<Vector>
    {
        readonly double[] _values;

        public int Rows { get; }

        public double this[int row]
        {
            get
            {
                return this._values[row];
            }
            set // I won't make this immutable because it adds overhead.
            {
                this._values[row] = value;
            }
        }

        public Vector( int rows )
        {
            if( rows < 1 )
            {
                throw new ArgumentException( $"Tried to create a {nameof( Vector )} with {rows} rows. A {nameof( Vector )} must have at least 1 row." );
            }

            _values = new double[rows];
            Rows = rows;
        }

        public Vector( double[] values ) : this( values.Length )
        {
            for( int i = 0; i < this.Rows; i++ )
            {
                this[i] = values[i];
            }
        }

        /// <summary>
        /// Creates a new matrix that's a copy of the original one.
        /// </summary>
        public Vector( Vector original ) : this( original.Rows )
        {
            for( int i = 0; i < this.Rows; i++ )
            {
                this[i] = original[i];
            }
        }

        /// <summary>
        /// Calculates the squared magnitude (length) of the vector.
        /// </summary>
        /// <remarks>
        /// It's faster than calculating the magnitude, because it doesn't require square-rooting.
        /// </remarks>
        /// <returns>The squared magnitude.</returns>
        public double GetSquaredMagnitude()
        {
            // |v1|^2 = v1.x^2 + v1.y^2 + v1.z^2 + ...

            double acc = 0;
            for( int i = 0; i < this.Rows; i++ )
            {
                var val = this[i];
                acc += val * val;
            }
            return acc;
        }

        /// <summary>
        /// Calculates the magnitude (length) of the vector.
        /// </summary>
        /// <returns>The magnitude.</returns>
        public double GetMagnitude()
        {
            // |v1| = sqrt( v1.x^2 + v1.y^2 + v1.z^2 + ... )

            return System.Math.Sqrt( GetSquaredMagnitude() );
        }

        /// <summary>
        /// Performs a linear combination on a list of (scalar, vector) terms.
        /// </summary>
        /// <returns>The vector containing the result of the operation `s1*v1 + s2*v2 + s3*v3 + ...`.</returns>
        public static Vector LinearCombination( (double s, Vector v)[] terms )
        {
            // Linear combination of scalars and vectors:
            // - Needs the same number of scalars as vectors.
            // - Multiply the vectors by their corresponding scalars, then add the vectors together.
            if( terms == null || terms.Length < 1 )
            {
                throw new InvalidOperationException( "Can't do linear combination of less than 2 elements." );
            }

            // Check if the number of elements in each vector term is the same.
            int rows = terms[0].v.Rows;
            foreach( var (s, v) in terms )
            {
                if( v.Rows != rows )
                {
                    throw new InvalidOperationException( "Can't do linear combination of vectors that have different dimensions." );
                }
            }

            // We will multiply and add at the same time.
            // We can do that because vector*scalar is element-wise, and vector+vector is also element-wise.
            // I.e. the result for each element of the vector is independent from the other elements.
            Vector result = new Vector( rows );
            foreach( var (s, v) in terms )
            {
                for( int i = 0; i < rows; i++ )
                {
                    result[i] += s * v[i];
                }
            }
            return result;
        }

        public static double Dot( Vector v1, Vector v2 )
        {
            // v1 dot v2 = v1.x*v2.x + v1.y*v2.y + v1.z*v2.z + ...

            if( v1.Rows != v2.Rows )
            {
                throw new InvalidOperationException( $"Can't take a dot product of a {nameof( Vector )}{v1.Rows} with a {nameof( Vector )}{v2.Rows}." );
            }

            // Multiply corresponding elements and add them together.
            double acc = 0;
            for( int i = 0; i < v1.Rows; i++ )
            {
                acc += v1[i] * v2[i];
            }
            return acc;
        }

        public bool Equals( Vector other )
        {
            if( other.Rows != this.Rows )
            {
                return false;
            }
            for( int i = 0; i < this.Rows; i++ )
            {
                if( this[i] != other[i] )
                {
                    return false;
                }
            }
            return true;
        }
    }
}
