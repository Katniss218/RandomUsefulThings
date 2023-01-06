using System;
using System.Collections.Generic;
using System.Text;

namespace Geometry
{
    public struct AABB3D
    {
        public Vector3 Center { get; }

        public Vector3 Size { get; }

        public Vector3 HalfSize { get => Size / 2.0f; }

        public Vector3 Min { get => Center - HalfSize; }

        public Vector3 Max { get => Center + HalfSize; }

        public AABB3D( Vector3 center, Vector3 size )
        {
            this.Center = center;
            this.Size = size;
        }

        [Obsolete( "Unconfirmed" )]
        public static bool LineSegmentIntersectsBoundingBox( Vector3 min, Vector3 max, Vector3 p1, Vector3 p2 )
        {
            Vector3 d = p2 - p1;
            Vector3 e = max - min;
            Vector3 m = p1 - min;
            float tmin = 0.0f;
            float tmax = 1.0f;

            for( int i = 0; i < 3; i++ )
            {
                float p = d[i];
                float q = m[i];
                float r = e[i];

                if( p == 0 && q < 0 )
                {
                    return false;
                }

                float t = q / p;

                if( p < 0 )
                {
                    tmax = Math.Min( tmax, t );
                }
                else
                {
                    tmin = Math.Max( tmin, t );
                }

                if( tmax < tmin )
                {
                    return false;
                }
            }

            return true;
        }

        [Obsolete( "Unconfirmed" )]
        public bool Intersects( AABB3D other )
        {
            Vector3 min = Min;
            Vector3 max = Max;
            Vector3 otherMin = other.Min;
            Vector3 otherMax = other.Max;

            // Check if the min and max of one box are inside the other box
            return min.X <= otherMax.X && max.X >= otherMin.X &&
                   min.Y <= otherMax.Y && max.Y >= otherMin.Y &&
                   min.Z <= otherMax.Z && max.Z >= otherMin.Z;
        }

        [Obsolete( "Unconfirmed" )]
        public Vector3 GetMinimumSeparationVector( AABB3D other )
        {
            Vector3 min = Min;
            Vector3 max = Max;
            Vector3 otherMin = other.Min;
            Vector3 otherMax = other.Max;

            // Calculate the separation vector
            var separationX = 0f;
            var separationY = 0f;
            var separationZ = 0f;

            if( min.X > otherMax.X )
                separationX = min.X - otherMax.X;
            else if( max.X < otherMin.X )
                separationX = max.X - otherMin.X;

            if( min.Y > otherMax.Y )
                separationY = min.Y - otherMax.Y;
            else if( max.Y < otherMin.Y )
                separationY = max.Y - otherMin.Y;

            if( min.Z > otherMax.Z )
                separationZ = min.Z - otherMax.Z;
            else if( max.Z < otherMin.Z )
                separationZ = max.Z - otherMin.Z;

            return new Vector3( separationX, separationY, separationZ );
        }
    }
}
