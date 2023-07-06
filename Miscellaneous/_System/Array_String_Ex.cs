using System;
using System.Collections.Generic;
using System.Text;

namespace Miscellaneous
{
    public static class Array_String_Ex
    {
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
