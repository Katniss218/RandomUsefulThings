using System;
using System.Collections.Generic;
using System.Text;

namespace RandomUsefulThings.Misc
{
    public class Base64Decoder
    {
        [Obsolete("works *almost*, the last char is corrupted.")]
        public static string DecodeBase64String( string base64 )
        {
            const string Base64Alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/";

            base64 = base64.Replace( '-', '+' ).Replace( '_', '/' );
            int countOfPadding = base64.EndsWith( "==" ) ? 2 : (base64.EndsWith( "=" ) ? 1 : 0);
            byte[] outputBytes = new byte[(base64.Length / 4) * 3 - countOfPadding];

            for( int i = 0, j = 0; i < base64.Length; i += 4, j += 3 )
            {
                int value = (Base64Alphabet.IndexOf( base64[i] ) << 18) |
                            (Base64Alphabet.IndexOf( base64[i + 1] ) << 12) |
                            (Base64Alphabet.IndexOf( base64[i + 2] ) << 6) |
                            Base64Alphabet.IndexOf( base64[i + 3] );

                if( j < outputBytes.Length )
                    outputBytes[j] = (byte)((value >> 16) & 0xFF);
                if( j + 1 < outputBytes.Length )
                    outputBytes[j + 1] = (byte)((value >> 8) & 0xFF);
                if( j + 2 < outputBytes.Length )
                    outputBytes[j + 2] = (byte)(value & 0xFF);
            }

            return System.Text.Encoding.UTF8.GetString( outputBytes );
        }
    }
}