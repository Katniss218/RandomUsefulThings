using System;
using System.Collections.Generic;
using System.Text;

namespace RandomUsefulThings.Math
{
	public static class FourierTransform
	{
		// adpatation of
		// https://github.com/Samson-Mano/Fast_Fourier_Transform/blob/main/Fast_fourier_transform/Fast_fourier_transform/process_data.cs#L84

		public static Complex[] FFT1D( Complex[] input, bool inverse )
		{
			// only for input lengths of power of 2.

			if( input.Length == 1 )
				return input;

			int N = input.Length;

			// Even
			Complex[] evenList = new Complex[N / 2];
			Complex[] oddList = new Complex[N / 2];
			for( int i = 0; i < (N / 2); i++ )
			{
				evenList[i] = input[2 * i];
				oddList[i] = input[(2 * i) + 1];
			}
			evenList = FFT1D( evenList, inverse );
			oddList = FFT1D( oddList, inverse );

			// Result
			Complex[] output = new Complex[N];

			for( int i = 0; i < (N / 2); i++ )
			{
				double w = (inverse ? 2 : -2) * System.Math.PI * i / N;
				Complex twiddle = new Complex( System.Math.Cos( w ), System.Math.Sin( w ) ); // wk
				Complex even = evenList[i];
				Complex odd = oddList[i];

				// Combine even and odd components using butterfly operation.
				output[i] = even + (twiddle * odd);
				output[i + (N / 2)] = even - (twiddle * odd);
			}

			// Instead of dividing by N only for inverse, we could divide by Sqrt(N) for both forward and inverse.
			// Some people don't divide here at all. Not sure why.
			if( inverse )
			{
				for( int i = 0; i < N; i++ )
				{
					output[i] /= N;
				}
			}

			return output;
		}

		// chat gpt says this is a 2D fft.

		public static Complex[,] FFT2D( Complex[,] input, bool inverse )
		{
			int rows = input.GetLength( 0 );
			int cols = input.GetLength( 1 );

			Complex[,] output = new Complex[rows, cols];

			// Rows - Process along the rows
			for( int r = 0; r < rows; r++ )
			{
				Complex[] row = new Complex[cols];
				for( int c = 0; c < cols; c++ )
				{
					row[c] = input[r, c];
				}

				Complex[] transformedRow = FFT1D( row, inverse );

				for( int c = 0; c < cols; c++ )
				{
					output[r, c] = transformedRow[c];
				}
			}

			// Columns - Process along the columns
			for( int c = 0; c < cols; c++ )
			{
				Complex[] col = new Complex[rows];
				for( int r = 0; r < rows; r++ )
				{
					col[r] = output[r, c]; // Take signal already processed by `rows` section.
				}

				Complex[] transformedCol = FFT1D( col, inverse );

				for( int r = 0; r < rows; r++ )
				{
					output[r, c] = transformedCol[r];
				}
			}

			return output;
		}


		// chatgpt says this is a 3D FFT

		public static Complex[,,] FFT3D( Complex[,,] input, bool inverse )
		{
			int dim1 = input.GetLength( 0 );
			int dim2 = input.GetLength( 1 );
			int dim3 = input.GetLength( 2 );

			Complex[,,] output = new Complex[dim1, dim2, dim3];

			// Process along the first dimension
			for( int i = 0; i < dim1; i++ )
			{
				Complex[,] plane = new Complex[dim2, dim3];
				for( int j = 0; j < dim2; j++ )
				{
					for( int k = 0; k < dim3; k++ )
					{
						plane[j, k] = input[i, j, k];
					}
				}

				Complex[,] transformedPlane = FFT2D( plane, inverse );

				for( int j = 0; j < dim2; j++ )
				{
					for( int k = 0; k < dim3; k++ )
					{
						output[i, j, k] = transformedPlane[j, k];
					}
				}
			}

			// Process along the second dimension
			for( int j = 0; j < dim2; j++ )
			{
				Complex[,] plane = new Complex[dim1, dim3];
				for( int i = 0; i < dim1; i++ )
				{
					for( int k = 0; k < dim3; k++ )
					{
						plane[i, k] = output[i, j, k];
					}
				}

				Complex[,] transformedPlane = FFT2D( plane, inverse );

				for( int i = 0; i < dim1; i++ )
				{
					for( int k = 0; k < dim3; k++ )
					{
						output[i, j, k] = transformedPlane[i, k];
					}
				}
			}

			// Process along the third dimension
			for( int k = 0; k < dim3; k++ )
			{
				Complex[,] plane = new Complex[dim1, dim2];
				for( int i = 0; i < dim1; i++ )
				{
					for( int j = 0; j < dim2; j++ )
					{
						plane[i, j] = output[i, j, k];
					}
				}

				Complex[,] transformedPlane = FFT2D( plane, inverse );

				for( int i = 0; i < dim1; i++ )
				{
					for( int j = 0; j < dim2; j++ )
					{
						output[i, j, k] = transformedPlane[i, j];
					}
				}
			}

			return output;
		}
	}
}
