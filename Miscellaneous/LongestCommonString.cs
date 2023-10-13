using System;
using System.Collections.Generic;
using System.Text;

namespace RandomUsefulThings.Misc
{
    public static class LongestCommonString
    {
        public static int GetLengthOfLCS( string text1, string text2 )
        {
            int[,] dp = new int[text1.Length + 1, text2.Length + 1];

            int lengthAns = 0;
            for( int i = 1; i <= text1.Length; i++ )
            {
                for( int j = 1; j <= text2.Length; j++ )
                {
                    if( text1[i - 1] == text2[j - 1] )
                    {
                        dp[i, j] = 1 + dp[i - 1, j - 1];
                        lengthAns = System.Math.Max( lengthAns, dp[i, j] );
                    }
                }
            }
            return lengthAns;
        }

        public static string LongestCommonSubsequence( string text1, string text2 )
        {
            throw new NotImplementedException();
        }
    }
}
