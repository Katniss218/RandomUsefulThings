using System;
using System.Collections.Generic;
using System.Text;

namespace RandomUsefulThings.Math.LinearAlgebra
{
    public class Matrix
    {
        readonly double[,] _values;
        public int Rows { get; }
        public int Cols { get; }

        public double this[int row, int col]
        {
            get
            {
                return this._values[row, col];
            }
            set
            {
                this._values[row, col] = value;
            }
        }

        public Matrix( int rows, int cols )
        {
            if( rows < 1 )
            {
                throw new ArgumentException( $"Tried to create a {nameof( Matrix )} with {rows} rows. A {nameof( Matrix )} must have at least 1 row." );
            }
            if( cols < 1 )
            {
                throw new ArgumentException( $"Tried to create a {nameof( Matrix )} with {cols} columns. A {nameof( Matrix )} must have at least 1 column." );
            }

            _values = new double[rows, cols];
            Rows = rows;
            Cols = cols;
        }

        public Matrix( double[,] values ) : this( values.GetLength( 0 ), values.GetLength( 1 ))
        {
            for( int i = 0; i < this.Rows; i++ )
            {
                for( int j = 0; j < this.Cols; j++ )
                {
                    this[i, j] = values[i, j];
                }
            }
        }

        /// <summary>
        /// Creates a new matrix that's a copy of the original one.
        /// </summary>
        public Matrix( Matrix original ) : this( original.Rows, original.Cols )
        {
            for( int i = 0; i < this.Rows; i++ )
            {
                for( int j = 0; j < this.Cols; j++ )
                {
                    this[i, j] = original[i, j];
                }
            }
        }

        /*
         * There is a thing called Linear Combination, and it's useful.
        
        2x -  y      =  0
        -x + 2y -  z = -1
            -3y + 4z =  4

        row picture
        [ 2, -1,  0]     [ 0]
    A = [-1,  2, -1] b = [-1]
        [ 0, -3,  4]     [ 4]

        // matrix form: Ax = b
        // does just multiplying the two give the result?

        // matrix times a vector gives you a vector.


        if one of the columns, is in the same plane as the others, it doesn't help to narrow down the solutions.
        apparently that means the matrix is not invertable. random matrices don't lie in the same plane
        */

        public static Vector Multiply( Matrix m, Vector v )
        {
            // Multiplication of matrix with vector will always result in a vector with the number of rows equal to the number of rows source matrix.
            if( v.Rows != m.Cols )
            {
                throw new InvalidOperationException( $"Can't multiply a {nameof( Vector )}{v.Rows} with a {nameof( Matrix )}{m.Rows}x{m.Cols}." );
            }

            Vector vRet = new Vector( m.Rows );

            // Element {i} of the result vector is equal to the dot product of the row {i} of the source matrix, and the source vector.
            // I.e. multiply corresponding elements and sum them together.
            for( int i = 0; i < m.Rows; i++ )
            {
                double acc = 0;
                for( int j = 0; j < m.Cols; j++ )
                {
                    acc += m[i, j] * v[j];
                }
                vRet[i] = acc;
            }

            return vRet;
        }
    }
}
