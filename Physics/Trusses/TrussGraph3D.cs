using System;
using System.Collections.Generic;
using System.Text;

namespace RandomUsefulThings.Physics.Trusses
{
    /// <summary>
    /// Represents a three-dimensional truss.
    /// </summary>
    public class TrussGraph3D
    {
        public struct Joint
        {
            public double x, y, z; // Position.
            public double fx, fy, fz; // Force in each spatial direction.
        }

        public struct Member
        {
            public int i, j; // Indices of the joints at each end.
            public double length;
            public double axialLoad;
        }

        public Joint[] Joints { get; set; }

        public Member[] Members { get; set; }

        public TrussGraph3D( Joint[] joints, Member[] members )
        {
            this.Joints = joints;
            this.Members = members;
        }

        [Obsolete( "unconfirmed" )]
        public static double CalculateTrussMemberArea( double cylinderArea, int numberOfMembers, double memberAngle )
        {
            // calculate the appropriate circular truss member area for a truss approximation of a vertical hollow cylinder structure.
            // member angle is the angle from vertical in degrees.

            double memberArea = cylinderArea / numberOfMembers;
            double cosTheta = System.Math.Cos( memberAngle * MathMethods.MathMethods.DegToRad );
            double memberAreaWithAngle = memberArea / cosTheta;
            return memberAreaWithAngle;
        }
    }
}
