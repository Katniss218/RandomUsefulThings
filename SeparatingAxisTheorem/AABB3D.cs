using RandomUsefulThings;
using System;
using System.Collections.Generic;
using System.Text;

namespace Geometry
{
    public class AABB3D
    {
        public Vector3 min;
        public Vector3 max;

        public AABB3D( Vector3 min, Vector3 max )
        {
            this.min = min;
            this.max = max;
        }

        [Obsolete( "Unconfirmed" )]
        public bool Intersects( AABB3D other )
        {
            // Check if the min and max of one box are inside the other box
            return min.X <= other.max.X && max.X >= other.min.X &&
                   min.Y <= other.max.Y && max.Y >= other.min.Y &&
                   min.Z <= other.max.Z && max.Z >= other.min.Z;
        }

        [Obsolete( "Unconfirmed" )]
        public Vector3 GetMinimumSeparationVector( AABB3D other )
        {
            // Calculate the separation vector
            var separationX = 0f;
            var separationY = 0f;
            var separationZ = 0f;

            if( min.X > other.max.X )
                separationX = min.X - other.max.X;
            else if( max.X < other.min.X )
                separationX = max.X - other.min.X;

            if( min.Y > other.max.Y )
                separationY = min.Y - other.max.Y;
            else if( max.Y < other.min.Y )
                separationY = max.Y - other.min.Y;

            if( min.Z > other.max.Z )
                separationZ = min.Z - other.max.Z;
            else if( max.Z < other.min.Z )
                separationZ = max.Z - other.min.Z;

            return new Vector3( separationX, separationY, separationZ );
        }
    }
}
