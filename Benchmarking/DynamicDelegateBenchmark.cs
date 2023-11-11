using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Benchmarking
{
    [ShortRunJob]
    public class DynamicDelegateBenchmark
    {
        private static Action<object> GenerateLambdaCallingParameterless( MethodInfo targetMethod )
        {
            // Generates a lambda that looks like this: `(object o) => targetMethod();`
            // 150x faster than InvokeDynamic, and doesn't require ugly modifications to the event system itself, not serializable though but we don't need that.

            ParameterExpression parameter = Expression.Parameter( typeof( object ), "o" );
            MethodCallExpression methodCallExpression = Expression.Call( null, targetMethod );

            return (Action<object>)Expression.Lambda( methodCallExpression, parameter ).Compile();
        }

        private static Action<object> GenerateLambdaCallingDowncasted( MethodInfo targetMethod, Type targetType )
        {
            // Generates a lambda that looks like this: `(object o) => targetMethod( (targetType)o );`
            // 150x faster than InvokeDynamic, and doesn't require ugly modifications to the event system itself, not serializable though but we don't need that.

            ParameterExpression parameter = Expression.Parameter( typeof( object ), "o" );
            UnaryExpression convertExpression = Expression.Convert( parameter, targetType );
            MethodCallExpression methodCallExpression = Expression.Call( null, targetMethod, convertExpression );

            return (Action<object>)Expression.Lambda( methodCallExpression, parameter ).Compile();
        }

        static int x;
        public static int ssss = 3;

        public class Test
        {
            public static void M( Tuple<int, string> e )
            {
                x = e.Item1;
            }
            public static void M2()
            {
                x = ssss;
            }
        }

        Delegate del = Delegate.CreateDelegate( typeof( Action<Tuple<int, string>> ), typeof( Test ).GetMethod( "M" ) );

        Action<Tuple<int, string>> delTyped = (Action<Tuple<int, string>>)Delegate.CreateDelegate( typeof( Action<Tuple<int, string>> ), typeof( Test ).GetMethod( "M" ) );

        Action<object> delTypedLambda = GenerateLambdaCallingDowncasted( typeof( Test ).GetMethod( "M" ), typeof( Tuple<int, string> ) );

        Action<object> delTypedLambdaParameterless = GenerateLambdaCallingParameterless( typeof( Test ).GetMethod( "M2" ) );

        Tuple<int, string> tuple = new Tuple<int, string>( 5, "aa" );

        [GlobalSetup]
        public void Setup()
        {
        }

        [Benchmark]
        public void Dynamic() => del.DynamicInvoke( tuple );

        [Benchmark]
        public void Typed() => delTyped.Invoke( tuple );

        [Benchmark]
        public void TypedLambda() => delTypedLambda.Invoke( tuple );

        [Benchmark]
        public void TypedLambdaParamless() => delTypedLambdaParameterless.Invoke( tuple );
    }
}