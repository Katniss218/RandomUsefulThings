using System;
using System.Collections.Generic;
using System.Text;

namespace RandomUsefulThings.Math
{
    public struct Cone3D
    {
        [Obsolete("Unconfirmed")]
        public static double GetConeAngle( double height, double radius )
        {
            double angle = 2 * System.Math.Atan( radius / height );
            return angle; // radians
        }
    }
}
