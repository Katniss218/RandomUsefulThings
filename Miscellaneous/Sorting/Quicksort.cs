using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace RandomUsefulThings.Misc.Sorting
{
    public static class Array_Ex_Quicksort
    {
        // A utility function to swap two elements
        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        private static void Swap( int[] arr, int i, int j )
        {
            int temp = arr[i];
            arr[i] = arr[j];
            arr[j] = temp;
        }

        /* This function takes last element as pivot, places
             the pivot element at its correct position in sorted
             array, and places all smaller (smaller than pivot)
             to left of pivot and all greater elements to right
             of pivot */
        private static int Partition( int[] arr, int low, int high )
        {

            // pivot
            int pivot = arr[high];

            // Index of smaller element and
            // indicates the right position
            // of pivot found so far
            int i = (low - 1);

            for( int j = low; j <= high - 1; j++ )
            {

                // If current element is smaller
                // than the pivot
                if( arr[j] < pivot )
                {

                    // Increment index of
                    // smaller element
                    i++;
                    Swap( arr, i, j );
                }
            }
            Swap( arr, i + 1, high );
            return (i + 1);
        }

        /* The main function that implements QuickSort
                    arr[] --> Array to be sorted,
                    low --> Starting index,
                    high --> Ending index
           */
        public static void Quicksort( this int[] arr, int low, int high )
        {
            if( low < high )
            {

                // pi is partitioning index, arr[p]
                // is now at right place
                int pi = Partition( arr, low, high );

                // Separately sort elements before
                // partition and after partition
                Quicksort( arr, low, pi - 1 );
                Quicksort( arr, pi + 1, high );
            }
        }
    }
}
