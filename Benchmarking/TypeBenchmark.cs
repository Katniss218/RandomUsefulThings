using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benchmarking
{
    [ShortRunJob]
    public class TypeBenchmark
    {
        public Type type1 = typeof( string );
        public Type type2 = typeof( List<int> );
        public Type type3 = typeof( Tuple<Dictionary<string, string>, List<float>, StringBuilder> );

        public string type1fn = typeof( string ).FullName;
        public string type2fn = typeof( List<int> ).FullName;
        public string type3fn = typeof( Tuple<Dictionary<string, string>, List<float>, StringBuilder> ).FullName;

        public string type1aqn = typeof( string ).AssemblyQualifiedName;
        public string type2aqn = typeof( List<int> ).AssemblyQualifiedName;
        public string type3aqn = typeof( Tuple<Dictionary<string, string>, List<float>, StringBuilder> ).AssemblyQualifiedName;

        [GlobalSetup]
        public void Setup()
        {
        }

        [Benchmark]
        public string _1x_string_fullname() => type1.FullName;
        [Benchmark]
        public string _1x_list_fullname() => type2.FullName;
        [Benchmark]
        public string _1x_tuple_fullname() => type3.FullName;

        [Benchmark]
        public string _1x_string_assemblyqualifiedname() => type1.AssemblyQualifiedName;
        [Benchmark]
        public string _1x_list_assemblyqualifiedname() => type2.AssemblyQualifiedName;
        [Benchmark]
        public string _1x_tuple_assemblyqualifiedname() => type3.AssemblyQualifiedName;

        [Benchmark]
        public Type _1x_FROM_string_fullname() => Type.GetType(type1fn);
        [Benchmark]
        public Type _1x_FROM_list_fullname() => Type.GetType(type2fn);
        [Benchmark]
        public Type _1x_FROM_tuple_fullname() => Type.GetType(type3fn);

        [Benchmark]
        public Type _1x_FROM_string_assemblyqualifiedname() => Type.GetType(type1aqn);
        [Benchmark]
        public Type _1x_FROM_list_assemblyqualifiedname() => Type.GetType( type2aqn );
        [Benchmark]
        public Type _1x_FROM_tuple_assemblyqualifiedname() => Type.GetType( type3aqn );
    }
}
