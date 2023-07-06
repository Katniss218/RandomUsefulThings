using System;
using System.Collections.Generic;
using System.Text;

namespace RandomUsefulThings.Misc._System
{
    public static class StringEx
    {
        public static int Count( this string s, char c, int start = 0, int step = 1 )
        {
            if( start < 0 || start >= s.Length )
            {
                throw new ArgumentOutOfRangeException( nameof( start ), $"The start parameter must be a valid index of the string." );
            }
            if( step == 0 )
            {
                throw new ArgumentOutOfRangeException( nameof( step ), $"The step can't be 0" );
            }

            if( s.Length == 0 )
                return 0;

            int sum = 0;
            for( int i = start; ; i += step )
            {
                if( step < 0 )
                {
                    if( i < 0 )
                        break;
                }
                else
                {
                    if( i >= s.Length )
                        break;
                }

                if( s[i] == c )
                    sum++;
            }
            return sum;
        }

    }
}
