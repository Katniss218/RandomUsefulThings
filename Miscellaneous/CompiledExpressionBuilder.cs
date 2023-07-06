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
        public static Action BuildCachedMethodCall( object target, MethodInfo methodInfo, object[] values )
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

        public static Action<object[]> BuildMethodCall( object target, MethodInfo methodInfo )
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

        public static Func<object, T> BuildPropertyGetter<T>( Type type, string propertyName )
        {
            PropertyInfo propertyInfo = type.GetProperty( propertyName );

            // can be modified to instead use a cast (implicit/explicit) if it is defined.
            if( !typeof( T ).IsAssignableFrom( propertyInfo.PropertyType ) )
            {
                throw new ArgumentException( $"Can't create a lambda to return a `{typeof( T ).FullName}` from a property of type `{propertyInfo.PropertyType}`." );
            }

            ParameterExpression instanceParameter = Expression.Parameter( typeof( object ), "targetObject" );
            UnaryExpression castedInstance = Expression.Convert( instanceParameter, propertyInfo.DeclaringType );

            var result = Expression.Convert(
                Expression.Property( castedInstance, propertyInfo ),
                typeof( T ) );

            var lambda = Expression.Lambda<Func<object, T>>( result, instanceParameter );
            return lambda.Compile();
        }

        public static Action<object, T> BuildPropertySetter<T>( Type type, string propertyName )
        {
            PropertyInfo propertyInfo = type.GetProperty( propertyName );

            // can be modified to instead use a cast (implicit/explicit) if it is defined.
            if( !propertyInfo.PropertyType.IsAssignableFrom( typeof( T ) ) )
            {
                throw new ArgumentException( $"Can't create a lambda to return a `{typeof( T ).FullName}` from a property of type `{propertyInfo.PropertyType}`." );
            }

            ParameterExpression instanceParameter = Expression.Parameter( typeof( object ), "instance" );
            ParameterExpression valueParameter = Expression.Parameter( typeof( T ), "value" );

            UnaryExpression castedInstance = Expression.Convert( instanceParameter, propertyInfo.DeclaringType );
            UnaryExpression castedValue = Expression.Convert( valueParameter, propertyInfo.PropertyType );

            var assign = Expression.Assign(
                Expression.Property( castedInstance, propertyInfo ),
                castedValue );

            return Expression.Lambda<Action<object, T>>( assign, instanceParameter, valueParameter ).Compile();
        }

        public static Action<T[], Func<T, T>> ForeachUnrolled<T>( int startIndex, int endIndex )
        {
            ParameterExpression instanceParameter = Expression.Parameter( typeof( T[] ), "array" );
            ParameterExpression actionFuncParameter = Expression.Parameter( typeof( Func<T, T> ), "action" );
            MethodInfo actionMethod = typeof( Func<T, T> ).GetMethod( "Invoke" );

            Expression[] bodyExprs = new Expression[endIndex - startIndex];
            for( int i = startIndex; i < endIndex; i++ )
            {
                var arrayAccessExpr = Expression.ArrayAccess( instanceParameter, Expression.Constant( i ) );
                var assignmentExpr = Expression.Assign( arrayAccessExpr, Expression.Call( actionFuncParameter, actionMethod, arrayAccessExpr ) );
                bodyExprs[i - startIndex] = assignmentExpr;
            }

            BlockExpression block = Expression.Block( bodyExprs );

            var lambda = Expression.Lambda<Action<T[], Func<T, T>>>( block, instanceParameter, actionFuncParameter );
            return lambda.Compile();
        }

        public static Action<T[], Func<T[], int, T>> ForeachUnrolled2<T>( int startIndex, int endIndex )
        {
            ParameterExpression instanceParameter = Expression.Parameter( typeof( T[] ), "array" );
            ParameterExpression actionFuncParameter = Expression.Parameter( typeof( Func<T[], int, T> ), "action" );
            MethodInfo actionMethod = typeof( Func<T[], int, T> ).GetMethod( "Invoke" );

            Expression[] bodyExprs = new Expression[endIndex - startIndex];
            for( int i = startIndex; i <= endIndex; i++ )
            {
                var assignmentExpr = Expression.Assign(
                    Expression.ArrayAccess(
                        instanceParameter,
                        Expression.Constant( i ) ),
                    Expression.Call(
                        actionFuncParameter,
                        actionMethod,
                        instanceParameter,
                        Expression.Constant( i ) )
                    );
                bodyExprs[i - startIndex] = assignmentExpr;
            }

            BlockExpression block = Expression.Block( bodyExprs );

            var lambda = Expression.Lambda<Action<T[], Func<T[], int, T>>>( block, instanceParameter, actionFuncParameter );
            return lambda.Compile();
        }
    }
}