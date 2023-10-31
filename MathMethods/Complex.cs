using System;
using System.Collections.Generic;
using System.Text;

namespace RandomUsefulThings.Math
{
    public struct Complex
    {
        // A complex number is a scalar + a 2D bivector

        public double R { get; }
        public double I { get; }

        public Complex( double r, double i )
        {
            this.R = r;
            this.I = i;
        }

        public static Complex operator *( Complex c1, Complex c2 )
        {
            return new Complex(
                (c1.R * c2.R) - (c1.I * c2.I),
                (c1.R * c2.I) + (c1.I * c2.R)
                );
        }
    }
}
