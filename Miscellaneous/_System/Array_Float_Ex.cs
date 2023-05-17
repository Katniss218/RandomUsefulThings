using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Text;

namespace RandomUsefulThings.Misc._System
{
    public static class Array_Float_Ex
    {
        /// <summary>
        /// Adds two array vectors together.
        /// </summary>
        public static void Add( this float[] v1, float[] v2 )
        {
            if( v1.Length != v2.Length )
            {
                throw new ArgumentException( $"Both array vectors have to be the same length." );
            }

            int length = v1.Length;

            for( int i  = 0; i < length; i++ )
            {
                v1[i] += v2[i];
            }
        }

        /// <summary>
        /// Multiplies two array vectors together (element-wise).
        /// </summary>
        public static void Multiply( this float[] v1, float[] v2 )
        {
            if( v1.Length != v2.Length )
            {
                throw new ArgumentException( $"Both array vectors have to be the same length." );
            }

            int length = v1.Length;

            for( int i  = 0; i < length; i++ )
            {
                v1[i] *= v2[i];
            }
        }
    }
}
