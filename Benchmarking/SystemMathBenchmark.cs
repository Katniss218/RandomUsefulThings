using BenchmarkDotNet;
using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benchmarking
{
    [ShortRunJob]
    public class SystemMathBenchmark
    {
        [Params( 1, 500 )]
        public double d;

        public double d1 = 50, d2 = 5000000;

        //[Params( 10, 100000, 1000000000 )]
        //double l;

        [GlobalSetup]
        public void Setup()
        {
        }

        [Benchmark]
        public double _1x_Sin_d() => System.Math.Sin( d );
        [Benchmark]
        public double _1x_Asin_d() => System.Math.Asin( d );

        [Benchmark]
        public double _1x_Cos_d() => System.Math.Cos( d );
        [Benchmark]
        public double _1x_Acos_d() => System.Math.Acos( d );

        [Benchmark]
        public double _1x_Tan_d() => System.Math.Tan( d );
        [Benchmark]
        public double _1x_Atan_d() => System.Math.Atan( d );

        [Benchmark]
        public double _1x_Sqrt_d() => System.Math.Sqrt( d );
        [Benchmark]
        public double _1x_Log_d() => System.Math.Log( d );

        [Benchmark]
        public double _1x_Pow_d_d() => System.Math.Pow( d, d );
        [Benchmark]
        public double _1x_Round_d() => System.Math.Round( d );

        [Benchmark]
        public double _1x_Min_d1_d2() => System.Math.Min( d1, d2 );
        [Benchmark]
        public double _1x_Max_d1_d2() => System.Math.Max( d1, d2 );

    }
}
