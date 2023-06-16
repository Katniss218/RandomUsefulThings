using BenchmarkDotNet.Running;
using Benchmarking;
using Geometry;
using Newtonsoft.Json.Linq;
using RandomUsefulThings.Math;
using RandomUsefulThings.Math.LinearAlgebra;
using RandomUsefulThings.Math.LinearAlgebra.NumericalMethods;
using RandomUsefulThings.Misc;
using RandomUsefulThings.Physics.FluidSim;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityPlus.Serialization;
using UnityPlus.Serialization.Json;

namespace TestConsole
{
    public static class Program
    {
        public static double CalculateDistance( double initialVelocity, double acceleration )
        {
            // returns the distance at which the object with a given velocity and acceleration will reach 0 velocity.
            double distance = Math.Pow( initialVelocity, 2 ) / (2 * acceleration);
            return distance;
        }

        public static void Main( string[] args )
        {
            string json = "{ \"test\": \"Hello \\\" world!\", \"val2\": -4.1543E+5 }";
            string json2 = "[ 5.2342, 342, -4372.11 ]";

            int _pos = 18;
            string _s = json;

            json2 = File.ReadAllText( "c:/test/testjson.json" );

            SerializedObject serobject = new UnityPlus.Serialization.Json.JsonReader( System.IO.File.ReadAllText( "c:/test/testjson.json" ) ).Parse();

            EasyBenchmarkTool.RootObject deserializedObject = System.Text.Json.JsonSerializer.Deserialize<EasyBenchmarkTool.RootObject>( json2, new System.Text.Json.JsonSerializerOptions() );

            bool b = _s[_pos] != '"' && _s[_pos - 1] != '\\';

            var xxxx = JObject.Parse( json2 );
            JsonReader reader2 = new JsonReader( json2 );

            //var obj = reader.EatObject();
            var j2 = reader2.Parse();

            //var x = new Vector3( (float)j[0], (float)j[1], (float)j[2] );

             BenchmarkRunner.Run<EasyBenchmarkTool>();
           // BenchmarkRunner.Run( new Type[] { typeof( FloatBenchmark ), typeof( DoubleBenchmark ) } );
        }
    }
}
