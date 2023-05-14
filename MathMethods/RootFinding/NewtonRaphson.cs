using System;
using System.Collections.Generic;
using System.Text;

namespace RandomUsefulThings.Math.RootFinding
{
    public static class NewtonRaphson
    {
        public static float Solve( Func<float, float> f, float x0, float tolerance = 0.0001f, int maxIterations = 100 )
        {
            float x = x0;
            int i = 0;
            while( i < maxIterations )
            {
                float fx = f( x );
                float dx = (f( x + tolerance ) - fx) / tolerance; // approximate derivative using central difference
                float xNew = x - fx / dx; // calculate new estimate
                if( System.Math.Abs( xNew - x ) < tolerance ) // check for convergence
                {
                    return xNew;
                }

                x = xNew;
                i++;
            }

            throw new Exception( "Failed to converge within maximum iterations." );
        }
    }
}