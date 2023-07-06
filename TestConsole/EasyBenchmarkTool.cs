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

namespace TestConsole
{
    [MemoryDiagnoser]
    public class EasyBenchmarkTool
    {
        public float x, a, b, c, d, e, f;

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
        }

        [Benchmark]
        public string Serialize_Custom_string()
        {
            return "";
        }

    }
}