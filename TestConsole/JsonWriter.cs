using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityPlus.Serialization;

namespace UnityPlus.Serialization.Json
{
    public static class JsonWriter
    {
        static Encoding enc = Encoding.UTF8;

        public static void WriteJson( this SerializedObject obj, Stream stream )
        {
            stream.Write( enc.GetBytes( "{" ), 0, 1 );

            bool seen = false;
            foreach( var child in obj )
            {
                if( seen )
                {
                    stream.Write( enc.GetBytes( "," ), 0, 1 );
                }
                else
                {
                    seen = true;
                }

                var str = $"\"{child.Key}\":";

                stream.Write( enc.GetBytes( str ), 0, str.Length );

                child.Value.WriteJson( stream );
            }

            stream.Write( enc.GetBytes( "}" ), 0, 1 );
        }

        public static void WriteJson( this SerializedArray obj, Stream stream )
        {
            stream.Write( enc.GetBytes( "[" ), 0, 1 );

            bool seen = false;
            foreach( var child in obj )
            {
                if( seen )
                {
                    stream.Write( enc.GetBytes( "," ), 0, 1 );
                }
                else
                {
                    seen = true;
                }
                child.WriteJson( stream );
            }

            stream.Write( enc.GetBytes( "]" ), 0, 1 );
        }

        public static void WriteJson( this SerializedValue value, Stream stream )
        {
            if( value == null )
            {
                stream.Write( enc.GetBytes( "null" ), 0, "null".Length );
                return;
            }

            string s = null;
            switch( value._valueType )
            {
                case SerializedValue.DataType.Boolean:
                    s = value._value.boolean ? "true" : "false"; break;
                case SerializedValue.DataType.Int:
                    s = value._value.@int.ToString( CultureInfo.InvariantCulture ); break;
                case SerializedValue.DataType.UInt:
                    s = value._value.@uint.ToString( CultureInfo.InvariantCulture ); break;
                case SerializedValue.DataType.Float:
                    s = value._value.@float.ToString( CultureInfo.InvariantCulture ); break;
                case SerializedValue.DataType.Decimal:
                    s = value._value.@decimal.ToString( CultureInfo.InvariantCulture ); break;
                case SerializedValue.DataType.String:
                    s = $"\"{(string)value._value.obj}\""; break;
                case SerializedValue.DataType.Object:
                    ((SerializedObject)value._value.obj).WriteJson( stream ); return;
                case SerializedValue.DataType.Array:
                    ((SerializedArray)value._value.obj).WriteJson( stream ); return;
            }

            stream.Write( enc.GetBytes( s ), 0, s.Length );
        }
    }
}

namespace UnityPlus.Serialization2.Json
{
    public static class JsonWriter
    {
        static Encoding enc = Encoding.UTF8;

        static byte[] openObject = enc.GetBytes( "{" );
        static byte[] closeObject = enc.GetBytes( "}" );
        static byte[] openArray = enc.GetBytes( "[" );
        static byte[] closeArray = enc.GetBytes( "]" );
        static byte[] comma = enc.GetBytes( "," );

        public static void WriteJson( this SerializedObject obj, Stream stream )
        {
            stream.Write( openObject, 0, 1 );

            bool seen = false;
            foreach( var child in obj )
            {
                if( seen )
                {
                    stream.Write( comma, 0, 1 );
                }
                else
                {
                    seen = true;
                }

                var str = $"\"{child.Key}\":";

                stream.Write( enc.GetBytes( str ), 0, str.Length );

                child.Value.WriteJson( stream );
            }

            stream.Write( closeObject, 0, 1 );
        }

        public static void WriteJson( this SerializedArray obj, Stream stream )
        {
            stream.Write( openArray, 0, 1 );

            bool seen = false;
            foreach( var child in obj )
            {
                if( seen )
                {
                    stream.Write(comma, 0, 1 );
                }
                else
                {
                    seen = true;
                }
                child.WriteJson( stream );
            }

            stream.Write( closeArray, 0, 1 );
        }

        public static void WriteJson( this SerializedValue value, Stream stream )
        {
            if( value == null )
            {
                stream.Write( enc.GetBytes( "null" ), 0, "null".Length );
                return;
            }

            string s = null;
            switch( value._valueType )
            {
                case SerializedValue.DataType.Boolean:
                    s = value._value.boolean ? "true" : "false"; break;
                case SerializedValue.DataType.Int:
                    s = value._value.@int.ToString( CultureInfo.InvariantCulture ); break;
                case SerializedValue.DataType.UInt:
                    s = value._value.@uint.ToString( CultureInfo.InvariantCulture ); break;
                case SerializedValue.DataType.Float:
                    s = value._value.@float.ToString( CultureInfo.InvariantCulture ); break;
                case SerializedValue.DataType.Decimal:
                    s = value._value.@decimal.ToString( CultureInfo.InvariantCulture ); break;
                case SerializedValue.DataType.String:
                    s = $"\"{(string)value._value.obj}\""; break;
                case SerializedValue.DataType.Object:
                    ((SerializedObject)value._value.obj).WriteJson( stream ); return;
                case SerializedValue.DataType.Array:
                    ((SerializedArray)value._value.obj).WriteJson( stream ); return;
            }

            stream.Write( enc.GetBytes( s ), 0, s.Length );
        }
    }
}

namespace UnityPlus.Serialization3.Json
{
    public static class JsonWriter
    {
        public static void WriteJson( this SerializedObject obj, StringBuilder sb)
        {
            sb.Append('{');

            bool seen = false;
            foreach( var child in obj )
            {
                if( seen )
                {
                    sb.Append( ':' );
                }
                else
                {
                    seen = true;
                }

                var str = $"\"{child.Key}\":";

                sb.Append( str );

                child.Value.WriteJson( sb );
            }

            sb.Append( '}' );
        }

        public static void WriteJson( this SerializedArray obj, StringBuilder sb )
        {
            sb.Append( '[' );

            bool seen = false;
            foreach( var child in obj )
            {
                if( seen )
                {
                    sb.Append( ',' );
                }
                else
                {
                    seen = true;
                }
                child.WriteJson( sb );
            }

            sb.Append( ']' );
        }

        public static void WriteJson( this SerializedValue value, StringBuilder sb )
        {
            if( value == null )
            {
                sb.Append( "null" );
                return;
            }

            string s = null;
            switch( value._valueType )
            {
                case SerializedValue.DataType.Boolean:
                    s = value._value.boolean ? "true" : "false"; break;
                case SerializedValue.DataType.Int:
                    s = value._value.@int.ToString( CultureInfo.InvariantCulture ); break;
                case SerializedValue.DataType.UInt:
                    s = value._value.@uint.ToString( CultureInfo.InvariantCulture ); break;
                case SerializedValue.DataType.Float:
                    s = value._value.@float.ToString( CultureInfo.InvariantCulture ); break;
                case SerializedValue.DataType.Decimal:
                    s = value._value.@decimal.ToString( CultureInfo.InvariantCulture ); break;
                case SerializedValue.DataType.String:
                    s = $"\"{(string)value._value.obj}\""; break;
                case SerializedValue.DataType.Object:
                    ((SerializedObject)value._value.obj).WriteJson( sb ); return;
                case SerializedValue.DataType.Array:
                    ((SerializedArray)value._value.obj).WriteJson( sb ); return;
            }

            sb.Append( s );
        }
    }
}
namespace UnityPlus.Serialization4.Json
{
    public class JsonStreamWriter
    {
        static readonly Encoding enc = Encoding.UTF8;

        static readonly byte[] openObject = enc.GetBytes( "{" );
        static readonly byte[] closeObject = enc.GetBytes( "}" );
        static readonly byte[] openArray = enc.GetBytes( "[" );
        static readonly byte[] closeArray = enc.GetBytes( "]" );
        static readonly byte[] comma = enc.GetBytes( "," );

        Stream _stream;
        SerializedData _data;

        public JsonStreamWriter( SerializedData data, Stream stream )
        {
            this._data = data;
            this._stream = stream;
        }

        public void Write()
        {
            if( _data is SerializedObject o )
                WriteJson( o );
            else if( _data is SerializedArray a )
                WriteJson( a );
            else if( _data is SerializedValue v )
                WriteJson( v );
        }

        void WriteJson( SerializedObject obj )
        {
            _stream.Write( openObject, 0, 1 );

            bool seen = false;
            foreach( var child in obj )
            {
                if( seen )
                {
                    _stream.Write( comma, 0, 1 );
                }
                else
                {
                    seen = true;
                }

                var str = $"\"{child.Key}\":";

                _stream.Write( enc.GetBytes( str ), 0, str.Length );

                WriteJson( child.Value );
            }

            _stream.Write( closeObject, 0, 1 );
        }

        void WriteJson( SerializedArray obj )
        {
            _stream.Write( openArray, 0, 1 );

            bool seen = false;
            foreach( var child in obj )
            {
                if( seen )
                {
                    _stream.Write( comma, 0, 1 );
                }
                else
                {
                    seen = true;
                }
                WriteJson( child );
            }

            _stream.Write( closeArray, 0, 1 );
        }

        void WriteJson( SerializedValue value )
        {
            if( value == null )
            {
                _stream.Write( enc.GetBytes( "null" ), 0, "null".Length );
                return;
            }

            string s = null;
            switch( value._valueType )
            {
                case SerializedValue.DataType.Boolean:
                    s = value._value.boolean ? "true" : "false"; break;
                case SerializedValue.DataType.Int:
                    s = value._value.@int.ToString( CultureInfo.InvariantCulture ); break;
                case SerializedValue.DataType.UInt:
                    s = value._value.@uint.ToString( CultureInfo.InvariantCulture ); break;
                case SerializedValue.DataType.Float:
                    s = value._value.@float.ToString( CultureInfo.InvariantCulture ); break;
                case SerializedValue.DataType.Decimal:
                    s = value._value.@decimal.ToString( CultureInfo.InvariantCulture ); break;
                case SerializedValue.DataType.String:
                    WriteString( (string)value._value.obj ); return;
                case SerializedValue.DataType.Object:
                    WriteJson( (SerializedObject)value._value.obj ); return;
                case SerializedValue.DataType.Array:
                    WriteJson( (SerializedArray)value._value.obj ); return;
            }

            _stream.Write( enc.GetBytes( s ), 0, s.Length );
        }

        static readonly byte[] quote = enc.GetBytes( "\"" );
        static readonly byte[] escapedBackslash = enc.GetBytes( "\\\\" );
        static readonly byte[] escapedQuote = enc.GetBytes( "\\\"" );
        static readonly byte[] escapedNewLine = enc.GetBytes( "\\n" );
        static readonly byte[] escapedR = enc.GetBytes( "\\r" );
        static readonly byte[] escapedTab = enc.GetBytes( "\\t" );
        static readonly byte[] escapedB = enc.GetBytes( "\\b" );
        static readonly byte[] escapedF = enc.GetBytes( "\\f" );

        void WriteString( string sIn )
        {
            _stream.Write( quote, 0, quote.Length );

            int i = 0;
            int start = 0;
            foreach( var c in sIn )
            {
                if( c is '\\' )
                {
                    byte[] b = enc.GetBytes( sIn[start..i] );
                    _stream.Write( b, 0, b.Length );
                    _stream.Write( escapedBackslash, 0, escapedBackslash.Length );
                }
                else if( c is '\"' )
                {
                    byte[] b = enc.GetBytes( sIn[start..i] );
                    _stream.Write( b, 0, b.Length );
                    _stream.Write( escapedQuote, 0, escapedQuote.Length );
                }
                else if( c is '\n' )
                {
                    byte[] b = enc.GetBytes( sIn[start..i] );
                    _stream.Write( b, 0, b.Length );
                    _stream.Write( escapedBackslash, 0, escapedBackslash.Length );
                }
                else if( c is '\r' )
                {
                    byte[] b = enc.GetBytes( sIn[start..i] );
                    _stream.Write( b, 0, b.Length );
                    _stream.Write( escapedR, 0, escapedR.Length );
                }
                else if( c is '\t' )
                {
                    byte[] b = enc.GetBytes( sIn[start..i] );
                    _stream.Write( b, 0, b.Length );
                    _stream.Write( escapedTab, 0, escapedTab.Length );
                }
                else if( c is '\b' )
                {
                    byte[] b = enc.GetBytes( sIn[start..i] );
                    _stream.Write( b, 0, b.Length );
                    _stream.Write( escapedB, 0, escapedB.Length );
                }
                else if( c is '\f' )
                {
                    byte[] b = enc.GetBytes( sIn[start..i] );
                    _stream.Write( b, 0, b.Length );
                    _stream.Write( escapedF, 0, escapedF.Length );
                }
                else
                {
                    i++;
                    continue;
                }

                i++;
                start = i;
            }

            if( i - start > 0 ) // write last (or the only if no escaping) part 
            {
                byte[] b = enc.GetBytes( sIn[start..i] );
                _stream.Write( b, 0, b.Length );
            }

            _stream.Write( quote, 0, quote.Length );
        }
    }
}