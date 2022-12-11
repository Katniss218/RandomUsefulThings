using System;
using System.Collections.Generic;
using System.Text;

namespace Geometry
{
    public class Triangle
    {
        [Obsolete("Unconfirmed")]
        // A function that calculates the area of a triangle given the lengths of its sides
        public static double Area( double a, double b, double c )
        {
            if( a <= 0 || b <= 0 || c <= 0 )
            {
                throw new ArgumentException( "Sides must be positive numbers." );
            }
            double s = (a + b + c) / 2;
            return Math.Sqrt( s * (s - a) * (s - b) * (s - c) );
        }
    }
}
