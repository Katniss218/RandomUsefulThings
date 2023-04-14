using System;
using System.Collections.Generic;
using System.Text;

namespace RandomUsefulThings.Misc
{
    public static class NumberUtils
    {
        /// <summary>
        /// Converts an integer into a string representation in a given base.
        /// </summary>
        /// <param name="baseChars">The characters of the base. Base = baseChars.Length</param>
        public static string NumberToString( int n, char[] baseChars )
        {
            // Example in base10: NumberToString( -12345, new[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' } );

            StringBuilder sb = new StringBuilder();
            int index = 0;
            if( n < 0 )
            {
                n = -n;
                sb.Append( '-' );
                index = 1; // insert after the negative sign.
            }
            int @base = baseChars.Length;

            while( n > 0 )
            {
                int digit = n % @base; // get the current last digit.
                sb.Insert( index, baseChars[digit] );
                n /= @base; // divide the number, so the next digit will be retrieved next.
            }

            return sb.ToString();
        }

        /// <summary>
        /// Converts a string representation of an integer in a given base into its numeric value.
        /// </summary>
        /// <param name="baseChars">The characters of the base. Base = baseChars.Length</param>
        public static int NumberFromString( string s, char[] baseChars )
        {
            // Example in base10: NumberFromString( s, new[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' } );

            // Notes: Might not handle non-number cases correctly. Assumes that the string contains a valid integer in the given base and nothing else.
            int result = 0;
            int @base = baseChars.Length;
            int digitPositionalMultiplier = 1;
            for( int i = s.Length - 1; i >= 0; i-- )
            {
                int digit = 0;
                for( int j = 0; j < @base; j++ )
                {
                    if( baseChars[j] == s[i] )
                    {
                        digit = j;
                        break; // will skip unknown symbols.
                    }
                }
                result += digit * digitPositionalMultiplier;
                digitPositionalMultiplier *= @base;
            }
            if( s[0] == '-' )
            {
                result = -result;
            }
            return result;
        }
    }
}