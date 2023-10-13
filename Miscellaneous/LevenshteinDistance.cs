using System;
using System.Collections.Generic;
using System.Text;

namespace RandomUsefulThings.Misc
{
    public static class LevenshteinDistance
    {
        // This exists and is faster https://github.com/Turnerj/Quickenshtein

        /// <summary>
        /// Calculate the difference between 2 strings using the Levenshtein distance algorithm
        /// </summary>
        [Obsolete( "untested" )]
        public static int Calculate( string source1, string source2 ) //O(n*m)
        {
            // Naive implementation.
            var matrix = new int[source1.Length + 1, source2.Length + 1];

            // First calculation, if one entry is empty return full length
            if( source1.Length == 0 )
                return source2.Length;

            if( source2.Length == 0 )
                return source1.Length;

            // Initialize the matrix to the value of the index.
            for( int i = 0; i <= source1.Length; matrix[i, 0] = i++ ) { }
            for( int j = 0; j <= source2.Length; matrix[0, j] = j++ ) { }

            // Calculate row and column distances
            for( int i = 1; i <= source1.Length; i++ )
            {
                for( int j = 1; j <= source2.Length; j++ )
                {
                    int cost = (source2[j - 1] == source1[i - 1]) ? 0 : 1;

                    matrix[i, j] = System.Math.Min( System.Math.Min( matrix[i - 1, j] + 1, matrix[i, j - 1] + 1 ), matrix[i - 1, j - 1] + cost );
                }
            }

            return matrix[source1.Length, source2.Length];
        }
    }
}
