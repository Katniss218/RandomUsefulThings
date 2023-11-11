using BenchmarkDotNet.Running;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Benchmarking
{
    public static class Program
    {
        static void Main( string[] args )
        {
            BenchmarkRunner.Run<DynamicDelegateBenchmark>();
            Console.ReadKey();
        }
    }
}