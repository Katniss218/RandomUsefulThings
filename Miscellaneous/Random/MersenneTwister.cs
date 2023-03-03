using System;
using System.Collections.Generic;
using System.Text;

namespace Miscellaneous.Random
{
    [Obsolete( "I'm not sure if this is actually the mersenne twister, it needs to be checked and confirmed." )]
    public class MersenneTwister
    {
        private const int N = 624;
        private const int M = 397;
        private const uint MatrixA = 0x9908b0dfU;
        private const uint UpperMask = 0x80000000U;
        private const uint LowerMask = 0x7fffffffU;

        private uint[] _mt = new uint[N];
        private int _mti = N + 1;

        public MersenneTwister( uint seed )
        {
            init_genrand( seed );
        }

        public MersenneTwister() : this( (uint)DateTime.Now.Ticks )
        {
        }

        public void init_genrand( uint s )
        {
            _mt[0] = s;
            for( _mti = 1; _mti < N; _mti++ )
            {
                _mt[_mti] = 1812433253U * (_mt[_mti - 1] ^ (_mt[_mti - 1] >> 30)) + (uint)_mti;
            }
        }

        public uint genrand_int32()
        {
            uint y;
            if( _mti >= N )
            {
                int kk;
                if( _mti == N + 1 )
                {
                    init_genrand( 5489U );
                }
                for( kk = 0; kk < N - M; kk++ )
                {
                    y = (_mt[kk] & UpperMask) | (_mt[kk + 1] & LowerMask);
                    _mt[kk] = _mt[kk + M] ^ (y >> 1) ^ (y & 1) * MatrixA;
                }
                for( ; kk < N - 1; kk++ )
                {
                    y = (_mt[kk] & UpperMask) | (_mt[kk + 1] & LowerMask);
                    _mt[kk] = _mt[kk + (M - N)] ^ (y >> 1) ^ (y & 1) * MatrixA;
                }
                y = (_mt[N - 1] & UpperMask) | (_mt[0] & LowerMask);
                _mt[N - 1] = _mt[M - 1] ^ (y >> 1) ^ (y & 1) * MatrixA;
                _mti = 0;
            }
            y = _mt[_mti++];
            y ^= y >> 11;
            y ^= (y << 7) & 0x9d2c5680U;
            y ^= (y << 15) & 0xefc60000U;
            y ^= y >> 18;
            return y;
        }

        public double genrand_real1()
        {
            return genrand_int32() * (1.0 / 4294967295.0);
        }
    }
}
