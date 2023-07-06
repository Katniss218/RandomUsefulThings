using System;
using System.Collections.Generic;
using System.Text;

namespace Miscellaneous
{
    public static class Array_Int_Ex
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
    }
}