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
                // The fx and dx can be substituted and simplified for some specific known symbolic function f(x). This is a general case for an arbitrary function.
                float fx = f( x );
                float dx = (f( x + tolerance ) - fx) / tolerance; // Approximate derivative using forward difference. Uses tolerance as a non-zero dx

                float xNew = x - fx / dx; // Calculate the new estimate for the root.
                if( System.Math.Abs( xNew - x ) < tolerance ) // Check for convergence.
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