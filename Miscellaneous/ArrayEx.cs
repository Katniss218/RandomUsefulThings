using System;
using System.Collections.Generic;
using System.Text;

namespace Miscellaneous
{
    public static class ArrayEx
    {
        [Obsolete( "untested" )]
        public static (int first, int second) TwoSum( this int[] nums, int target )
        {
            // return indices of 2 elements that sum up to the desired value.
            Dictionary<int, int> seen = new Dictionary<int, int>();
            for( int i = 0; i < nums.Length; i++ )
            {
                //if we've seen the matching number to our number
                if( seen.ContainsKey( target - nums[i] ) )
                {
                    //then return the matching numbers index and our own
                    return (seen[target - nums[i]], i);
                }
                //otherwise add our value to the dictionary and continue
                //if we've already seen this value we can ignore it since both indexes would be valid.
                if( !seen.ContainsKey( nums[i] ) )
                {
                    seen.Add( nums[i], i );
                }

            }

            //Since they state there is always a solution to the problem we should never actually hit this.
            return (-1, -1);
        }

        [Obsolete( "untested" )]
        public static string LongestCommonPrefix( this string[] array )
        {
            // returns the longest prefix shared by all of the strings in the array.
            if( array == null || array.Length == 0 )
            {
                return "";
            }
            if( array.Length == 1 )
            {
                return array[0];
            }

            var prefixString = "";

            // Go through all the letters of the first word, since the common prefix can't be longer than any of the elements of the array.
            for( int i = 0; i < array[0].Length; i++ )
            {
                for( int j = 0; j < array[i].Length; j++ )
                {
                    // If i is higher then the length of the word
                    // there is no longer a prefix to match
                    if( i > array[j].Length - 1 )
                    {
                        return prefixString;
                    }
                    // If the i-th letter of the string doesn't match the i-th 
                    // letter of the first word we've reached the end of the
                    // common prefix
                    if( array[0][i] != array[j][i] )
                    {
                        return prefixString;
                    }
                }
                // If we make it through the inner foreach all of the 
                prefixString += array[0][i];
            }

            return prefixString;
        }
    }
}
