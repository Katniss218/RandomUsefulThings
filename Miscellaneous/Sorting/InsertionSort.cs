using System;
using System.Collections.Generic;
using System.Text;

namespace RandomUsefulThings.Misc.Sorting
{
    public static class Array_Ex_InsertionSort
    {
        [Obsolete( "untested" )]
        // Function to sort array
        // using insertion sort
        public static void InsertionSort( this int[] arr )
        {
            int n = arr.Length;
            for( int i = 1; i < n; ++i )
            {
                int key = arr[i];
                int j = i - 1;

                // Move elements of arr[0..i-1],
                // that are greater than key,
                // to one position ahead of
                // their current position
                while( j >= 0 && arr[j] > key )
                {
                    arr[j + 1] = arr[j];
                    j = j - 1;
                }
                arr[j + 1] = key;
            }
        }
    }
}
