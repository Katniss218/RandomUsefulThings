using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace RandomUsefulThings.Misc._System
{
    public static class TypeEx
    {
        public static bool HasConversionTo( this Type sourceType, Type targetType )
        {
#warning TODO - check the target type for backwards conversion as well.

            MethodInfo[] methods = targetType.GetMethods( BindingFlags.Static | BindingFlags.Public );
            foreach( MethodInfo method in methods )
            {
                if( method.Name == "op_Implicit" || method.Name == "op_Explicit" )
                {
                    if( method.ReturnType == targetType && method.GetParameters()[0].ParameterType == sourceType )
                    {
#warning TODO - add an overload that returns the operators as methodinfo as well.
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
