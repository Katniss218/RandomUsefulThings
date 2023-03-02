using System;
using System.Collections.Generic;
using System.Text;

namespace Geometry
{
    [System.Serializable]
    /// <summary>
    /// Represents a 2-dimensional Axis-Aligned Bounding Box (a rectangle).
    /// </summary>
    public struct AABB2D
    {
        public Vector2 Center;

        public Vector2 Size;

        public Vector2 HalfSize { get => Size / 2.0f; }

        public Vector2 Min { get => Center - HalfSize; }

        public Vector2 Max { get => Center + HalfSize; }

        /// <summary>
        /// Returns the top-left point (-X, +Y).
        /// </summary>
        public Vector2 TopLeft { get => new Vector2( Center.X - HalfSize.X, Center.Y + HalfSize.Y ); }

        /// <summary>
        /// Returns the top-right point (+X, +Y).
        /// </summary>
        public Vector2 TopRight { get => new Vector2( Center.X + HalfSize.X, Center.Y + HalfSize.Y ); }

        /// <summary>
        /// Returns the bottom-left point (-X, -Y).
        /// </summary>
        public Vector2 BottomLeft { get => new Vector2( Center.X - HalfSize.X, Center.Y - HalfSize.Y ); }

        /// <summary>
        /// Returns the bottom-right point (+X, -Y).
        /// </summary>
        public Vector2 BottomRight { get => new Vector2( Center.X + HalfSize.X, Center.Y - HalfSize.Y ); }

        public AABB2D( Vector2 center, Vector2 size )
        {
            this.Center = center;
            this.Size = size;
        }

        public static AABB2D FromMinMax( Vector2 min, Vector2 max )
        {
            throw new NotImplementedException();
        }

        public float Area()
        {
            return Size.X * Size.Y;
        }

        public bool Contains( Vector2 point )
        {
            Vector2 min = this.Min;
            Vector2 max = this.Max;

            return (point.X >= min.X && point.X <= max.X)
                && (point.Y >= min.Y && point.Y <= max.Y);
        }

        public bool Intersects( Line2D line )
        {
            throw new NotImplementedException();
        }

        [Obsolete( "Unconfirmed" )]
        public bool Intersects( Ray2D Ray )
        {
            // Slab method, separate intervals for x and y slabs.
            Vector2 min = Min;
            Vector2 max = Max;

            float txMin = (min.X - Ray.Origin.X) / Ray.Direction.X;
            float txMax = (max.X - Ray.Origin.X) / Ray.Direction.X;
            if( txMin > txMax )
            {
                float temp = txMin;
                txMin = txMax;
                txMax = temp;
            }

            float tyMin = (min.Y - Ray.Origin.Y) / Ray.Direction.Y;
            float tyMax = (max.Y - Ray.Origin.Y) / Ray.Direction.Y;
            if( tyMin > tyMax )
            {
                float temp = tyMin;
                tyMin = tyMax;
                tyMax = temp;
            }

            if( txMin > tyMax || tyMin > txMax )
            {
                return false;
            }

            txMin = Math.Max( txMin, tyMin );
            txMax = Math.Min( txMax, tyMax );
            return txMax > 0;
        }

        [Obsolete( "Unconfirmed" )]
        public static bool Intersects( AABB2D self, AABB2D other )
        {
            Vector2 min1 = self.Min;
            Vector2 max1 = self.Max;

            Vector2 min2 = other.Min;
            Vector2 max2 = other.Max;

            return (min1.X <= max2.X && max1.X >= min2.X)
                && (min1.Y <= max2.Y && max1.Y >= min2.Y);
        }

        [Obsolete( "Unconfirmed" )]
        public static AABB2D Enclose( AABB2D a, AABB2D b )
        {
            // The smallest AABB needed to contain 2 AABBs.
            Vector2 min = Vector2.Min( a.Center - a.Size / 2, b.Center - b.Size / 2 );
            Vector2 max = Vector2.Max( a.Center + a.Size / 2, b.Center + b.Size / 2 );

            Vector2 size = max - min;
            Vector2 center = min + size / 2;

            return new AABB2D( center, size );
        }

        [Obsolete( "Unconfirmed" )]
        public static AABB2D Intersection( AABB2D a, AABB2D b )
        {
            Vector2 min = Vector2.Max( a.Center - a.Size / 2, b.Center - b.Size / 2 );
            Vector2 max = Vector2.Min( a.Center + a.Size / 2, b.Center + b.Size / 2 );

            if( min.X > max.X || min.Y > max.Y )
            {
                return new AABB2D( Vector2.Zero, Vector2.Zero );
            }

            Vector2 size = max - min;
            Vector2 center = min + size / 2;

            return new AABB2D( center, size );
        }

        /// <summary>
        /// Expands the bounds of the AABB by a fixed amount in all directions.
        /// </summary>
        public AABB2D ExpandedBy( float amount )
        {
            Vector2 newSize = Size + new Vector2( amount * 2, amount * 2 );
            return new AABB2D( Center, newSize );
        }

        [Obsolete( "Unconfirmed" )]
        public float DistanceTo( Vector2 point )
        {
            Vector2 min = Min;
            Vector2 max = Max;

            float dx = Math.Max( Math.Abs( point.X - min.X ), Math.Abs( point.X - max.X ) );
            float dy = Math.Max( Math.Abs( point.Y - min.Y ), Math.Abs( point.Y - max.Y ) );

            return (float)Math.Sqrt( dx * dx + dy * dy );
        }

        [Obsolete( "Unconfirmed" )]
        public static bool LineSegmentIntersectsBoundingBox( Vector2 min, Vector2 max, Vector2 p1, Vector2 p2 )
        {
            Vector2 direction = p2 - p1;
            Vector2 boxExtents = max - min;
            Vector2 lineToBox = p1 - min;
            float tmin = 0.0f;
            float tmax = 1.0f;

            for( int i = 0; i < 2; i++ )
            {
                float projection = direction[i];
                float lineCoord = lineToBox[i];
                float boxExtent = boxExtents[i];

                if( projection == 0 && lineCoord < 0 )
                {
                    return false;
                }

                float t = lineCoord / projection;

                if( projection < 0 )
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

        /*[Obsolete( "Unconfirmed" )]
        public static bool DoLineSegmentIntersectBoundingBox( Vector2 p1, Vector2 p2, float minX, float minY, float maxX, float maxY )
        {
            // Check if the line segment is fully inside the bounding box
            if( p1.X >= minX && p1.X <= maxX && p1.Y >= minY && p1.Y <= maxY &&
                p2.X >= minX && p2.X <= maxX && p2.Y >= minY && p2.Y <= maxY )
            {
                return true;
            }

            // Check if the line segment intersects any of the four sides of the bounding box
            if( LineSegment2D.DoLineSegmentsIntersect( p1, p2, new Vector2( minX, minY ), new Vector2( maxX, minY ) ) ||
                LineSegment2D.DoLineSegmentsIntersect( p1, p2, new Vector2( maxX, minY ), new Vector2( maxX, maxY ) ) ||
                LineSegment2D.DoLineSegmentsIntersect( p1, p2, new Vector2( maxX, maxY ), new Vector2( minX, maxY ) ) ||
                LineSegment2D.DoLineSegmentsIntersect( p1, p2, new Vector2( minX, maxY ), new Vector2( minX, minY ) ) )
            {
                return true;
            }

            return false;
        }*/
    }
    public struct Ray2D
    {
        public Vector2 Origin { get; }
        public Vector2 Direction { get; }

        public Ray2D( Vector2 origin, Vector2 direction )
        {
            this.Origin = origin;
            this.Direction = direction;
        }
    }
}