﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace RandomUsefulThings.Math.LinearAlgebra
{
	public struct Matrix : IEquatable<Matrix>
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
			set // I won't make this immutable because it adds overhead. Size is immutable though.
			{
				this._values[row, col] = value;
			}
		}

		/// <summary>
		/// Creates a matrix of given size containing only zeros.
		/// </summary>
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

		public Matrix( double[,] values ) : this( values.GetLength( 0 ), values.GetLength( 1 ) )
		{
			for( int i = 0; i < this.Rows; i++ )
			{
				for( int j = 0; j < this.Cols; j++ )
				{
					this._values[i, j] = values[i, j];
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
					this._values[i, j] = original[i, j];
				}
			}
		}

		/// <summary>
		/// Constructs the identity for matrix-matrix multiplication.
		/// </summary>
		public static Matrix Identity( int size )
		{
			// Identity matrix is one that has all 0, except for all 1 on the main diagonal (top-left to bottom-right).
			Matrix m = new Matrix( size, size ); // initialized to 0.
			for( int i = 0; i < size; i++ )
			{
				m[i, i] = 1;
			}
			return m;
		}

		/*
        
        2x -  y      =  0
        -x + 2y -  z = -1
            -3y + 4z =  4

        row picture
        [ 2, -1,  0]     [ 0]
    A = [-1,  2, -1] b = [-1]
        [ 0, -3,  4]     [ 4]

        if one of the columns, is in the same plane as the others, it doesn't help to narrow down the solutions.
        apparently that means the matrix is not invertable. randomly generated matrices typically don't lie in the same plane.
        */

		/// <summary>
		/// Returns the vector that contains the specified row, left-most element first.
		/// </summary>
		public Vector GetRow( int row )
		{
			Vector v = new Vector( this.Cols );
			for( int i = 0; i < this.Cols; i++ )
			{
				v[i] = this[row, i];
			}
			return v;
		}

		/// <summary>
		/// Returns the vector that contains the specified column, top-most element first.
		/// </summary>
		public Vector GetColumn( int col )
		{
			Vector v = new Vector( this.Rows );
			for( int i = 0; i < this.Rows; i++ )
			{
				v[i] = this[i, col];
			}
			return v;
		}

		public void SwapRows( int srcRow, int dstRow )
		{
			for( int i = 0; i < this.Cols; i++ )
			{
				double temp = this[dstRow, i];
				this[dstRow, i] = this[srcRow, i];
				this[srcRow, i] = temp;
			}
		}

		public void SwapColumns( int srcCol, int dstCol )
		{
			for( int i = 0; i < this.Rows; i++ )
			{
				double temp = this[i, dstCol];
				this[i, dstCol] = this[i, srcCol];
				this[i, srcCol] = temp;
			}
		}

		public double Trace() // Trace is the sum of the diagonal elements.
		{
			if( this.Cols != this.Rows )
			{
				throw new InvalidOperationException( "Trace can only be performed on a square matrix." );
				// for non-square, the main diagonal *could* me defined as the diagonal from (0,0) to (m,m) where m is min(rows,cols). It is the diagonal used for Gaussian Elimination.
			}

			double acc = 0.0;
			for( int i = 0; i < this.Cols; i++ )
			{
				acc += this[i, i];
			}
			return acc;
		}

		/// <summary>
		/// Performs a linear combination on a list of (scalar, matrix) terms.
		/// </summary>
		/// <returns>The vector containing the result of the operation `s1*m1 + s2*m2 + s3*m3 + ...`.</returns>
		public static Matrix LinearCombination( (double s, Matrix m)[] terms )
		{
			// Linear combination of scalars and matrices:
			// - Needs the same number of scalars as matrices.
			// - Multiply the matrices by their corresponding scalars, then add the matrices together.
			if( terms == null || terms.Length < 1 )
			{
				throw new InvalidOperationException( "Can't do linear combination of less than 2 elements." );
			}

			// Check if the number of rows and columns is constant for every matrix term.
			int rows = terms[0].m.Rows, cols = terms[0].m.Cols;
			foreach( var (s, m) in terms )
			{
				if( m.Rows != rows || m.Cols != cols )
				{
					throw new InvalidOperationException( "Can't do linear combination of matrices that have different dimensions." );
				}
			}

			// We will multiply and add at the same time.
			// We can do that because matrix*scalar is element-wise, and matrix+matrix is also element-wise.
			// I.e. the result for each element of the matrix is independent from the other elements.
			Matrix result = new Matrix( rows, cols );
			foreach( var (s, m) in terms )
			{
				for( int i = 0; i < rows; i++ )
				{
					for( int j = 0; j < cols; j++ )
					{
						result[i, j] += s * m[i, j];
					}
				}
			}
			return result;
		}

		// There's also a possibility of a linear combination of rows/columns of a matrix.
		// There is just one matrix in that case, no list of elements (well, an implicit list of row/column vectors).

		public static Matrix Add( Matrix m1, Matrix m2 )
		{
			// Matrix addition is element-wise addition.
			// It is Commutative    => m1 + m2 = m2 + m1
			// It is Associative    => (m1 + m2) + m3 = m1 + (m2 + m3)
			if( m1.Rows != m2.Rows || m1.Cols != m2.Cols )
			{
				throw new InvalidOperationException( $"Can't add a {nameof( Matrix )}{m1.Rows}x{m1.Cols} and a {nameof( Matrix )}{m2.Rows}x{m2.Cols}." );
			}

			Matrix mr = new Matrix( m1.Rows, m2.Cols );
			for( int i = 0; i < m1.Rows; i++ )
			{
				for( int j = 0; j < m1.Cols; j++ )
				{
					mr[i, j] = m1[i, j] + m2[i, j];
				}
			}
			return mr;
		}

		public static Matrix Multiply( Matrix m, double s )
		{
			// Matrix multiplication by scalar is element-wise multiplication.
			// It is Commutative    => m*s = s*m
			// It is Distributive   => s*(m1 + m2) = s*m1 + s*m2
			//                      => (s1 + s2)*m = s1*m + s2*m
			Matrix mRet = new Matrix( m.Rows, m.Cols );
			for( int i = 0; i < m.Rows; i++ )
			{
				for( int j = 0; j < m.Cols; j++ )
				{
					mRet[i, j] = m[i, j] * s;
				}
			}
			return mRet;
		}

		public static Vector Multiply( Matrix m, Vector v )
		{
			if( v.Rows != m.Cols )
			{
				throw new InvalidOperationException( $"Can't multiply a {nameof( Vector )}{v.Rows} with a {nameof( Matrix )}{m.Rows}x{m.Cols}." );
			}

			// Multiplication of matrix with vector will always result in a vector with the number of rows equal to the number of rows source matrix.
			Vector vRet = new Vector( m.Rows );

			// Element {i} of the result vector is equal to the dot product of the row {i} of the source matrix, and the source vector.
			// I.e. multiply corresponding elements and sum them together into an accumulator.
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

		public static Matrix Multiply( Matrix m1, Matrix m2 )
		{
			if( m1.Rows != m2.Cols )
			{
				throw new InvalidOperationException( $"Can't multiply a {nameof( Matrix )}{m1.Rows}x{m1.Cols} with a {nameof( Matrix )}{m2.Rows}x{m2.Cols}." );
			}
			// Not commutative. Yes associative.
			// E * m => E performs row operations on m.
			// m * E => E performs column operations on m.

			// Multiplication of the following matrix with some other matrix (walkthrough):
			// [ 1, 0, 0 ] row0 => takes  1*row0 + 0*row1 + 0*row2, and puts that into row 0 (adds together vectors multiplied by scalars)
			// [-3, 1, 0 ] row1 => takes -3*row0 + 1*row1 + 0*row2, and puts that into row 1 (adds together vectors multiplied by scalars)
			// [ 0, 0, 1 ] row2 => takes  0*row0 + 0*row1 + 1*row2, and puts that into row 2 (adds together vectors multiplied by scalars)

			// (m1 * m2)[i,j] = m1.GetRow(i) dot m2.GetColumn(j)
			// (m1 * m2)[i,j] = (m1[i,0] * m2[0,j]) + (m1[i,1] * m2[1,j]) + (m1[i,2] * m2[2,j]) + ...

			Matrix mr = new Matrix( m1.Rows, m2.Cols );
			for( int i = 0; i < m1.Rows; i++ )
			{
				for( int j = 0; j < m2.Cols; j++ )
				{
					double acc = 0;
					for( int k = 0; k < m1.Cols; k++ )
					{
						acc += m1[i, k] * m2[k, j];
					}
					mr[i, j] = acc;
				}
			}
			return mr;
		}

		/// <summary>
		/// Brings the matrix to a Row Echelon Form (NOT Reduced Row Echelon Form!).
		/// </summary>
		/// <returns>A new matrix, in the Row Echelon Form.</returns>
		public Matrix Eliminate()
		{
			// A matrix is in echelon form if it has the shape resulting from a Gaussian elimination.

			if( this.Rows < 2 )
			{
				throw new Exception( "Matrix must have at least 2 rows to perform elimination." );
			}

			Matrix U = new Matrix( this );

			for( int row = 0; row < U.Rows - 1; row++ )
			{
				int pivotRow = row;
				int pivotCol = row;
				// Pivot will always be on the diagonal starting at (0,0).
				// Pivot a.k.a. Leading Coefficient.

				double pivotValue = U[pivotRow, pivotCol];

				// Pivot can't be 0, so if it is, try to find the first nonzero pivot below the current row and swap the rows.
				if( pivotValue == 0 )
				{
					bool foundPivot = false;
					for( int row2 = pivotRow + 1; row2 < U.Rows; row2++ ) // Find a row that has a non-zero value in the pivot column.
					{
						if( U[row2, pivotCol] != 0 )
						{
							U.SwapRows( pivotRow, row2 );
							pivotValue = U[pivotRow, pivotCol];
							foundPivot = true;
							break;
						}
					}

					if( !foundPivot )
					{
						// Invertible a.k.a. Nonsingular
						throw new Exception( "Matrix is not invertible." );
					}
				}

				// Eliminate all the rows below the pivot.
				for( int row3 = pivotRow + 1; row3 < U.Rows; row3++ )
				{
					double factor = U[row3, pivotCol] / pivotValue;

					// Eliminate everything below (row > pivotRow) and to the right (col >= pivotCol) of the pivot.
					for( int col3 = pivotCol; col3 < U.Cols; col3++ )
					{
						U[row3, col3] -= factor * U[pivotRow, col3];
					}
				}
			}

			// after elimination, we can backsubstitute to solve the system of equations.
			// we solve in reverse order (starting with the row that has 1 element).


			// m1 * Inverse(m1) = Identity = Inverse(m1) * m1 (for square matrices)
			// Matrix M is not invertible when there exists a nonzero vector x for which M * x = 0 (the result vector is zeroed).
			// - This is because if the result vector is zeroed, we can't multiply it back into the original, because anything times zero is zero.

			return U;
		}


		[Obsolete( "I think it should work, I vaguely remember testing it." )]
		/// <param name="A">The augumented matrix form of a system of linear equations in its Row Echelon Form (after Gaussian elimination).</param>
		/// <returns>An array of double values that are a solution to the system of equations.</returns>
		public double[] BackSubstitution()
		{
			// This doesn't modify the matrix, so technically it's not suitable for inverting.

			double[] solutions = new double[this.Cols - 1]; // I guess could also be rows, without `- 1`, because the un-augumented matrix is square.

			// Iterate over rows in reverse order.
			for( int row = this.Rows - 1; row >= 0; row-- )
			{
				double sum = 0;
				// Iterate over columns to the right of the pivot (col > pivotCol).
				for( int col = row + 1; col < this.Cols - 1; col++ )
				{
					sum += this[row, col] * solutions[col];
				}

				// Calculate the solution for the variable using previous the solutions and the coefficient of the diagonal element and the constant term.
				solutions[row] = (this[row, this.Cols - 1] - sum) / this[row, row];
			}

			return solutions;
		}

		[Obsolete( "Unconfirmed" )]
		public static Matrix Convolve( Matrix input, Matrix kernel )
		{
			// Convolution is an operation that loops through each element in the input matrix, and for each element:
			// - The kernel is centered over the current input element.
			// - The surrounding elements in the input matrix are multiplied with the corresponding elements in the kernel.
			// - The multiplied elements are added together to produce a new value for the current input element.

			// I'm not sure what if the kernel rows/cols are an even number. Hard to center over anything then.

			int kernelSize = kernel.Rows;
			int padding = kernelSize / 2;
			Matrix output = new Matrix( input.Rows, input.Cols );

			for( int row = padding; row < input.Rows - padding; row++ )
			{
				for( int col = padding; col < input.Cols - padding; col++ )
				{
					double sum = 0;
					for( int i = 0; i < kernelSize; i++ )
					{
						for( int j = 0; j < kernelSize; j++ )
						{
							sum += input[row + i - padding, col + j - padding] * kernel[i, j];
						}
					}
					output[row, col] = sum;
				}
			}
			return output;
		}

		/// <summary>
		/// Check for the |A[i, i]| >= sum(j!=i)|Aij|
		/// </summary>
		/// <returns>True, if A[i, i] is greater or equal than other elements.</returns>
		[Obsolete( "untested" )]
		public bool IsDiagonallyDominant()
		{
			// A matrix is diagonally dominant if every element on the main diagonal is greater or equal than any non-diagonal element.

			// https://github.com/3approx4/Numerical-Methods/blob/master/Numerical-Methods.Libs/Matrix.cs
			bool dominant = true;
			for( int i = 0; i < this.Rows; i++ )
			{
				dominant &= CheckRowDominance( i );
			}

			return dominant;
		}

		/// <summary>
		/// Check row for the dominance of the rowIndex located value
		/// </summary>
		/// <param name="rowIndex">Row number</param>
		/// <returns>True, if A[i, i] is  greater or equal than other elements in the row.</returns>
		[Obsolete( "untested" )]
		private bool CheckRowDominance( int rowIndex )
		{
			// https://github.com/3approx4/Numerical-Methods/blob/master/Numerical-Methods.Libs/Matrix.cs
			double diagValue = System.Math.Abs( this[rowIndex, rowIndex] );
			double sum = 0;
			for( int j = 0; j < this.Cols; j++ )
			{
				if( j != rowIndex )
					sum += System.Math.Abs( this[rowIndex, j] );
			}

			return diagValue >= sum;
		}

		// To find eigenvalues of a square NxN matrix, we can solve an N-dimensional polynomial.
		// there will be at most N eigenvalues/vectors, but can be as little as 0 in some cases.


		// Invariant of a matrix M is a value that doesn't change after the change of basis.
		// change of basis for a matrix/tensor M => M' = Q * M * transpose(Q)            invariants of M and M' are the same. Q is a coordinate transformation matrix

		// Principal invariants of a tensor (this case matrix):
		// 1. = Trace(M) (sum of main diagonals)
		// 2. = (Trace(M)^2 - Trace(M^2)) / 2
		// 3. = Determinant(M).

		// Tensor Q is orthogonal when `Q * Transpose(Q) = Identity`
		// - determinant is 1 or -1     (if 1 - rotation, if -1 - reflection)
		// - Transpose(Q) = Inverse(Q)

		// Tensor is symmetric if:
		// - Transpose(Q) = Q           (Q_ij = Q_ji)
		// property: dot(Q*v1, v2) = dot(v1, Q*v2)
		// property: every NxN symmetric tensor always has N real eigenvalues/eigenvectors.

		[Obsolete( "unconfirmed" )]
		public void ApplyCompletePivoting( int rowIndex, int colIndex )
		{
			int numRows = this.Rows;
			int numCols = this.Cols;
			int maxRow = rowIndex;
			int maxCol = colIndex;

			// Find the maximum value in the submatrix
			for( int i = rowIndex; i < numRows; i++ )
			{
				for( int j = colIndex; j < numCols; j++ )
				{
					if( System.Math.Abs( this[i, j] ) > System.Math.Abs( this[maxRow, maxCol] ) )
					{
						maxRow = i;
						maxCol = j;
					}
				}
			}

			// Swap the rows if needed
			if( maxRow != rowIndex )
			{
				this.SwapRows( maxRow, rowIndex );
			}

			// Swap the columns if needed
			if( maxCol != colIndex )
			{
				this.SwapColumns( maxCol, colIndex );
			}
		}

		[Obsolete( "unconfirmed" )]
		public void ApplyPartialPivoting( int rowIndex )
		{
			int numRows = this.Rows;
			int numCols = this.Cols;
			int maxRow = rowIndex;

			// Find the row with the maximum value in the current column
			for( int i = rowIndex + 1; i < numRows; i++ )
			{
				if( System.Math.Abs( this[i, rowIndex] ) > System.Math.Abs( this[maxRow, rowIndex] ) )
				{
					maxRow = i;
				}
			}

			// Swap the rows if needed
			if( maxRow != rowIndex )
			{
				throw new NotImplementedException();
				//this.SwapRows( maxRow, rowIndex, numCols ); // ?
			}
		}

		[Obsolete( "untested" )]
		static void MatDecomposeQR( Matrix mat, out Matrix q, out Matrix r )
		{
			// https://visualstudiomagazine.com/Articles/2024/01/03/matrix-inverse.aspx
			// QR decomposition, Householder algorithm.
			int m = mat.Rows;  // assumes mat is nxn
			int n = mat.Cols;  // check m == n

			Matrix Q = Matrix.Identity( m );
			Matrix R = new Matrix( mat );
			int end = n - 1;
			// if (m == n) end = n - 1; else end = n;

			for( int i = 0; i < end; i++ )
			{
				Matrix H = Matrix.Identity( m );
				Vector a = new Vector( n - i );
				int k = 0;
				for( int ii = i; ii < n; ii++ )
				{
					a[k++] = R[ii, i];
				}

				double normA = a.GetMagnitude();
				if( a[0] < 0.0 )
				{
					normA = -normA;
				}

				Vector v = new Vector( a.Rows );
				for( int j = 0; j < v.Rows; j++ )
				{
					v[j] = a[j] / (a[0] + normA);
				}
				v[0] = 1.0;

				Matrix h = Matrix.Identity( a.Rows );
				double vMag = v.GetSquaredMagnitude();
				Matrix alpha = VecToMat( v, v.Rows, 1 );
				Matrix beta = VecToMat( v, 1, v.Rows );
				Matrix aMultB = Matrix.Multiply( alpha, beta );

				for( int ii = 0; ii < h.Rows; ii++ )
				{
					for( int jj = 0; jj < h.Cols; jj++ )
					{
						h[ii, jj] -= (2.0 / vMag) * aMultB[ii, jj];
					}
				}

				// copy h into lower right of H
				int d = n - h.Rows;
				for( int ii = 0; ii < h.Rows; ii++ )
				{
					for( int jj = 0; jj < h.Cols; jj++ )
					{
						H[ii + d, jj + d] = h[ii, jj];
					}
				}

				Q = Matrix.Multiply( Q, H );
				R = Matrix.Multiply( H, R );
			}

			q = Q;
			r = R;
		}

		static Matrix VecToMat( Vector vec, int nRows, int nCols )
		{
			// ugly
			Matrix result = new Matrix( nRows, nCols );
			int k = 0;
			for( int i = 0; i < nRows; ++i )
				for( int j = 0; j < nCols; ++j )
					result[i, j] = vec[k++];
			return result;
		}

		public bool Equals( Matrix other )
		{
			if( other.Rows != this.Rows || other.Cols != this.Cols )
			{
				return false;
			}
			for( int i = 0; i < this.Rows; i++ )
			{
				for( int j = 0; j < this.Cols; j++ )
				{
					if( this[i, j] != other[i, j] )
					{
						return false;
					}
				}
			}
			return true;
		}
	}
}