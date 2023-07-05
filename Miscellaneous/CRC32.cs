using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace RandomUsefulThings.Misc
{
    [Obsolete( "unconfirmed" )]
    public class CRC32 : HashAlgorithm
    {
        private const uint Polynomial = 0xEDB88320;
        private uint[] lookupTable;

        public CRC32()
        {
            lookupTable = GenerateLookupTable();
            HashSizeValue = 32;
            Initialize();
        }

        public override void Initialize()
        {
            // Initialize the CRC32 calculation
            HashValue = new byte[4];
        }

        protected override void HashCore( byte[] array, int ibStart, int cbSize )
        {
            // Update the CRC32 calculation with the new bytes
            uint crc = uint.MaxValue;
            for( int i = ibStart; i < cbSize; i++ )
            {
                byte index = (byte)((crc ^ array[i]) & 0xFF);
                crc = (crc >> 8) ^ lookupTable[index];
            }
            HashValue = BitConverter.GetBytes( ~crc );
        }

        protected override byte[] HashFinal()
        {
            // Finalize the CRC32 calculation
            byte[] hashValue = HashValue;
            Array.Reverse( hashValue );
            return hashValue;
        }

        private static uint[] GenerateLookupTable()
        {
            uint[] lookupTable = new uint[256];
            for( uint i = 0; i < 256; i++ )
            {
                uint crc = i;
                for( int j = 0; j < 8; j++ )
                {
                    crc = (crc >> 1) ^ ((crc & 1) * Polynomial);
                }
                lookupTable[i] = crc;
            }
            return lookupTable;
        }
    }
}
