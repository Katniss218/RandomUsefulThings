using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using Geometry;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using UnityPlus.Serialization;
using UnityPlus.Serialization.Json;

namespace TestConsole
{
    [MemoryDiagnoser]
    public class EasyBenchmarkTool
    {
        public float x, a, b, c, d, e, f;

        //public Vector3 _vec;
        //public JArray _vecJson;
        //public SerializedArray _vecJson2;

        public string json = System.IO.File.ReadAllText( "c:/test/testjson.json" );
        //public string _vectorJsonString = "[ 5.2342, 342, -4372.11 ]";

        public JObject jobject = JObject.Parse( System.IO.File.ReadAllText( "c:/test/testjson.json" ) );
        public SerializedObject serobject = new UnityPlus.Serialization.Json.JsonReader( System.IO.File.ReadAllText( "c:/test/testjson.json" ) ).Parse();

        public UnityPlus2.Serialization.SerializedObject serobject2 = (UnityPlus2.Serialization.SerializedObject)new UnityPlus2.Serialization.Json.JsonStringReader( File.ReadAllText( "c:/test/testjson.json" ) ).Read();

        [GlobalSetup]
        public void Setup()
        {
            x = 1;
            a = 2;
            b = 3;
            c = 4;
            d = 5;
            e = 6;
            f = 7;

            //_vec = new Vector3( 5.2342f, 342, 4372.11f );
            //_vecJson = new JArray() { _vec.X, _vec.Y, _vec.Z };
            //_vecJson2 = new UnityEngine.Serialization1.SerializedArray() { _vec.X, _vec.Y, _vec.Z };
            // json = "{ \"test\": \"Hello \\\" world!\", \"val2\": -4.1543E+5 }";
        }

        public JObject jsonObj = new JObject()
        {
            { "Vector", new JArray()
            {
                4.34251f,
                12432f,
                -2421f
            } }
        };

        public SerializedValue serVal = new SerializedObject()
        {
            { "Vector", (SerializedValue)new SerializedArray()
            {
                4.34251f,
                12432f,
                -2421f
            } }
        };

        public class Thumbnail
        {
            public string Url { get; set; }
            public int Height { get; set; }
            public int Width { get; set; }
        }

        public class Image
        {
            public int Width { get; set; }
            public int Height { get; set; }
            public string Title { get; set; }
            public Thumbnail Thumbnail { get; set; }
            public bool Animated { get; set; }
            public List<int> IDs { get; set; }
        }

        public class RootObject
        {
            public Image Image { get; set; }
        }


        [Benchmark]
        public float Float_Add6() => x + a + b + c + d + e + f;


        [Benchmark]
        public JObject Deserialize_NewtonsoftLinq()
        {
            return JObject.Parse( json );
        }

        [Benchmark]
        public UnityPlus2.Serialization.SerializedData Deserialize_Custom_string()
        {
            StringBuilder sb = new StringBuilder();

            var x = new UnityPlus2.Serialization.Json.JsonStringReader( json ).Read();
            return x;
        }

        [Benchmark( Baseline = true )]
        public string Serialize_NewtonsoftLinq()
        {
            return JsonConvert.SerializeObject( jobject );
        }

        [Benchmark]
        public string Serialize_Custom()
        {
            using( MemoryStream s = new MemoryStream() )
            {
                new UnityPlus2.Serialization.Json.JsonStreamWriter( serobject2, s ).Write();
                return Encoding.UTF8.GetString( s.ToArray() );
            }
        }

        [Benchmark]
        public string Serialize_Custom_string()
        {
            StringBuilder sb = new StringBuilder();

            new UnityPlus2.Serialization.Json.JsonStringWriter( serobject2, sb ).Write();
            return sb.ToString();
        }

        /* [Benchmark( Baseline = true )]
         public Vector3 Lookup_NewtonsoftLinq()
         {
             var jsonA = (JArray)jsonObj["Vector"];
             return new Vector3( (float)jsonA[0], (float)jsonA[1], (float)jsonA[2] );
         }

         [Benchmark]
         public Vector3 Lookup_Custom()
         {
             var jsonA = (UnityPlus.Serialization.SerializedArray)((UnityPlus.Serialization.SerializedObject)serVal)["Vector"];
             return new Vector3( (float)jsonA[0], (float)jsonA[1], (float)jsonA[2] );
         }
        */
        /*[Benchmark]
        public Vector3 DeserializeVec3Full()
        {
            JToken j = JArray.Parse( _vectorJsonString );
            return new Vector3( (float)j[0], (float)j[1], (float)j[2] );
        }

        [Benchmark]
        public Vector3 DeserializeVec3Full_Custom()
        {
            UnityEngine.Serialization.Json.JsonReader r = new UnityEngine.Serialization.Json.JsonReader( _vectorJsonString );

            SerializedArray j = r.EatArray();
            return new Vector3( (float)(j[0]), (float)(j[1]), (float)(j[2]) );
        }*/

        /*[Benchmark]
        public string SerializeVec3Full() => JsonConvert.SerializeObject( new JArray() { _vec.X, _vec.Y, _vec.Z } );

        [Benchmark]
        public string SerializeVec3Full_Custom()
        {
            using( Stream s = new MemoryStream( 50 ) )
            {
                _vecJson2.WriteJson( s );
                return s.ToString();
            }
        }*/
    }
}