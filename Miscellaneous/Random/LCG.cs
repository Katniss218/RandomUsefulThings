using System;
using System.Collections.Generic;
using System.Text;

namespace Miscellaneous.Random
{
    /// <summary>
    /// A class implementing a simple Linear Congruential Generator (LCG).
    /// </summary>
    public class LCG
    {
        const uint MULTIPLIER = 1664525u;
        const uint INCREMENT = 1013904223u;
        const uint MODULUS = 4294967295u;

        private uint _seed;

        public LCG( uint seed )
        {
            _seed = seed;
        }

        private void InternalNext()
        {
            // Steps the LCG.
            // LCG formula:
            _seed = (MULTIPLIER * _seed + INCREMENT) % MODULUS;
        }

        /// <summary>
        /// Returns the next unsigned integer.
        /// </summary>
        public uint Next()
        {
            InternalNext();
            return _seed;
        }

        /// <summary>
        /// Returns the next integer.
        /// </summary>
        public int NextInt()
        {
            InternalNext();
            return (int)(_seed % int.MaxValue);
        }

        /// <summary>
        /// Returns the next integer.
        /// </summary>
        public int NextInt(int min, int maxExclusive)
        {
            if( min >= maxExclusive )
            {
                throw new ArgumentException( "Min must be less than max" );
            }

            InternalNext();
            return (int)(_seed % (maxExclusive - min) + min);
        }
    }
}
