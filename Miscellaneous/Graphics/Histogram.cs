using System;
using System.Collections.Generic;
using System.Text;

namespace RandomUsefulThings.Misc.Graphics
{
    public static class Histogram
    {
        // A histogram is an array that counts the number of occurences of each value in an input set.

        public static int[] Compute( params byte[] array )
        {
            int[] histogram = new int[256]; // Count all possible distinct values of `byte`.

            for( int i = 0; i < array.Length; i++ )
            {
                histogram[array[i]]++; // Use the value as index to the array to get rid of a lookup.
            }

            return histogram;
        }

        // an arbitrary histogram can be computed using a dictionary to store the count for each key.

        public static Dictionary<T, int> Compute<T>( T[] values )
        {
            Dictionary<T, int> histogram = new Dictionary<T, int>();

            foreach( T value in values )
            {
                if( histogram.ContainsKey( value ) )
                {
                    histogram[value]++;
                }
                else
                {
                    histogram[value] = 1;
                }
            }

            return histogram;
        }

        [Obsolete("Should work, *should*")]
        public static int[] ComputeHistogram( float[] values, float rangeMin, float rangeMax, int numBins )
        {
            // Here's some fancier stuff that rounds each value into a bin that's closest to it, and adds 1 to that bin's count.
            // numBins = number of possible values, spaced equally in range [rangeMin..rangeMax].
            int[] histogram = new int[numBins];
            float binWidth = (rangeMax - rangeMin) / numBins;
            for( int i = 0; i < values.Length; i++ )
            {
                int binIndex = (int)((values[i] - rangeMin) / binWidth);
                if( binIndex >= 0 && binIndex < numBins ) // don't include values out of range.
                {
                    histogram[binIndex]++;
                }
            }
            return histogram; // multiply bin index by the bin width and add rangeMin to get the value of the bin.
        }
    }
}
