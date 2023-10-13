using System;
using System.Collections.Generic;
using System.Text;

namespace RandomUsefulThings.Misc.Search
{
    public static class BinarySearch
    {
        [Obsolete( "untested" )]
        public static int FindIndex<T>( T[] sortedData, T item ) where T : IComparable<T>
        {
            //data has to be sorted in ascending order.
            // bounds of the target area.
            int left = 0;
            int right = sortedData.Length - 1;

            while( left <= right )
            {
                int midpoint = left + (right - left) / 2;

                if( item.CompareTo( sortedData[midpoint] ) > 0 )
                {
                    left = midpoint + 1;
                    continue;
                }

                if( item.CompareTo( sortedData[midpoint] ) < 0 )
                {
                    right = midpoint - 1;
                    continue;
                }

                return midpoint;
            }

            return -1;
        }
    }
}