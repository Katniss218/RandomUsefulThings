﻿using System;
using System.Collections.Generic;
using System.Text;

namespace RandomUsefulThings.Misc.Sorting
{
    public static class Array_Ex_BubbleSort
    {
        public static void BubbleSort( this int[] array )
        {
            int length = array.Length;

            for( int i = 0; i < length - 1; i++ )
            {
                for( int j = 0; j < length - i - 1; j++ )
                {
                    if( array[j] > array[j + 1] )
                    {
                        // swap temp and arr[i]
                        int temp = array[j];
                        array[j] = array[j + 1];
                        array[j + 1] = temp;
                    }
                }
            }
        }
    }
}
