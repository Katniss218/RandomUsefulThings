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
    public class DoubleBenchmark
    {
        public double f0, f1, f2, f3, f4, f5, f6, f7, f8, f9, fa;

        [GlobalSetup]
        public void Setup()
        {
            f0 = 1000000000.0f; f1 = 124.14f; f2 = 5433; f3 = 42.631f; f4 = -36.75746561f; f5 = 54353.61f; f6 = -1543674232f; f7 = 3.653645771f; f8 = 125234164.5f; f9 = -0.5436342351f; fa = -5.3254222f;
        }

        [Benchmark]
        public double _10x_double_ADD() => f0 + f1 + f2 + f3 + f4 + f5 + f6 + f7 + f8 + f9 + fa;

        [Benchmark]
        public double _10x_double_SUB() => f0 - f1 - f2 - f3 - f4 - f5 - f6 - f7 - f8 - f9 - fa;

        [Benchmark]
        public double _10x_double_MUL() => f0 * f1 * f2 * f3 * f4 * f5 * f6 * f7 * f8 * f9 * fa;

        [Benchmark]
        public double _10x_double_DIV() => f0 / f1 / f2 / f3 / f4 / f5 / f6 / f7 / f8 / f9 / fa;

        public bool x1 = false, x2 = false, x3 = false, x4 = false;

        [Benchmark]
        public bool _4x_bool_OR() => x1 || x2 || x3 || x4;

        public double f10 = 5f, f11 = 4f, f12 = 3f, f13 = 2f, f14 = 1f, f15 = 0f;

        [Benchmark]
        public bool _5x_double_LESSTHAN_with_4x_bool_OR() => f10 < f11 || f11 < f12 || f12 < f13 || f13 < f14 || f14 < f15;

        public double f20 = 0f, f21 = 1f, f22 = 2f, f23 = 3f, f24 = 4f, f25 = 5f;

        [Benchmark]
        public bool _5x_double_GREATERTHAN_with_4x_bool_OR() => f20 > f21 || f21 > f22 || f22 > f23 || f23 > f24 || f24 > f15;

        [Benchmark]
        public bool _5x_double_EQUALS_with_4x_bool_OR() => f20 == f21 || f21 == f22 || f22 == f23 || f23 == f24 || f24 == f15;
    }
}
