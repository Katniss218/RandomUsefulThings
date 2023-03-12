using System;
using System.Collections.Generic;
using System.Text;

namespace RandomUsefulThings.Physics.Trusses
{
    public class TrussGraph
    {
        // Define a struct to hold information about each joint
        public struct Joint
        {
            public double x, y, z; // Coordinates of joint
            public double fx, fy, fz; // Forces on joint
        }

        // Define a struct to hold information about each member
        public struct Member
        {
            public int i, j; // Indices of joints at each end of member
            public double length; // Length of member
            public double axialLoad; // Axial load on member
        }

        public Joint[] Joints { get; set; }
        public Member[] Members { get; set; }


        [Obsolete( "unconfirmed" )]
        public double CalculateTrussMemberArea( double cylinderArea, int numberOfMembers, double memberAngle )
        {
            // calculate the appropriate circular truss member area for a truss approximation of a vertical hollow cylinder structure.
            // member angle is the angle from vertical.

            double memberArea = cylinderArea / numberOfMembers;
            double cosTheta = System.Math.Cos( memberAngle * System.Math.PI / 180.0 );
            double memberAreaWithAngle = memberArea / cosTheta;
            return memberAreaWithAngle;
        }
    }
}
