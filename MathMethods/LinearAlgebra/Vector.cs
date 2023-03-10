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
            // |v1| = sqrt( v1.x^2 + v1.y^2 + v1.z^2 + ... )

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

        public static Vector LinearCombination( (double s, Vector v)[] elements )
        {
            // Linear combination of scalars and matrices:
            // - Needs the same number of scalars as matrices.
            // - Multiply the matrices by their corresponding scalars, then add the matrices together.
            if( elements == null || elements.Length < 1 )
            {
                throw new InvalidOperationException( "Can't do linear combination of less than 2 elements." );
            }

            int rows = elements[0].v.Rows;
            foreach( var (s, v) in elements )
            {
                if( v.Rows != rows )
                {
                    throw new InvalidOperationException( "Can't do linear combination of vectors that have different dimensions." );
                }
            }

            Vector result = new Vector( rows );
            foreach( var (s, v) in elements )
            {
                for( int i = 0; i < rows; i++ )
                {
                    result[i] += s * v[i]; // multiply element by scalar, and add to the accumulated value. We can do that because addition and multiplication are element-wise.
                }
            }
            return result;
        }

        // Linear combination of scalars and vectors:
        // - Needs the same number of scalars as vectors.
        // - Multiply the vectors by their corresponding scalars, then add the vectors together.

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
