using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace RandomUsefulThings.Misc
{
    public static class CompiledExpressionBuilder
    {
        public static Action BuildParameterlessLambdaForMethod( object target, MethodInfo methodInfo, object[] values )
        {
            /*  Dynamically creates the following lambda:

            () => Method( value0, value1, ... );

            */

            ParameterInfo[] methodParams = methodInfo.GetParameters();
            if( values.Length != methodParams.Length )
            {
                throw new ArgumentException( $"The number of parameter values needs to match the number of parameters of the method." );
            }

            ConstantExpression[] lambdaValues = new ConstantExpression[methodParams.Length];
            for( int i = 0; i < lambdaValues.Length; i++ )
            {
                object parameter = values[i];

                if( !methodParams[i].ParameterType.IsInstanceOfType( parameter ) ) // type safety
                {
                    throw new ArgumentException( $"The provided parameter type `{parameter.GetType().FullName}` of parameter `{i}` doesn't match the type `{methodParams[i].ParameterType}` expected by the method `{methodInfo.Name}`.", nameof( values ) );
                }

                lambdaValues[i] = Expression.Constant( parameter, methodParams[i].ParameterType );
            }

            var body = Expression.Call( Expression.Constant( target ), methodInfo, lambdaValues );
            var lambda = Expression.Lambda<Action>( body );
            return lambda.Compile();
        }

        public static Action<object[]> BuildLambdaForMethod( object target, MethodInfo methodInfo )
        {
            /*  Dynamically creates the following lambda:

            (args) => Method( (Type0)args[0], (Type1)args[1], ... );

            */

            ParameterInfo[] methodParams = methodInfo.GetParameters();
            ParameterExpression[] calleeParameters = methodParams.Select( p => Expression.Parameter( p.ParameterType, p.Name ) ).ToArray();
            ParameterExpression lambdaParameter = Expression.Parameter( typeof( object[] ), "args" );

            IEnumerable<UnaryExpression> castArguments = calleeParameters.Select( ( p, index ) => Expression.Convert( Expression.ArrayIndex( lambdaParameter, Expression.Constant( index ) ), p.Type ) );

            var body = Expression.Call( Expression.Constant( target ), methodInfo, castArguments );
            var lambda = Expression.Lambda<Action<object[]>>( body, lambdaParameter );
            return lambda.Compile();
        }
    }
}