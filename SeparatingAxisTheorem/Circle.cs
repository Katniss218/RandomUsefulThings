using System;
using System.Collections.Generic;
using System.Text;

namespace Geometry
{
    public struct Circle
    {
        public Vector2 Center { get; }

        public float Radius { get; }

        public float Diameter { get => Radius * 2; }

        public Circle( Vector2 center, float radius )
        {
            this.Center = center;
            this.Radius = radius;
        }
        public float Circumference
        {
            get { return 2.0f * (float)(Math.PI * Radius); }
        }

        public float Area
        {
            get { return (float)(Math.PI * (Radius * Radius)); }
        }

        public Vector2 ClosestPointOnCircle( Vector2 point )
        {
            // closest point on a circle is simply that point projected onto the circle, in the direction of the circle's center.
            Vector2 direction = Vector2.PointingAt( Center, point ).Normalized();

            return Center + (direction * Radius);
        }

        public bool ContainsPoint( Vector2 point )
        {
            return Vector2.Distance( Center, point ) < Radius;
        }

        public static bool Intersects( Circle c1, Circle c2 )
        {
            // For small-ish circles. this can theoretically be optimized by squaring the sum of radii instead of square-rooting when taking the distance.
            return Vector2.Distance( c1.Center, c2.Center ) < (c1.Radius + c2.Radius);
        }

        [Obsolete( "Unconfirmed" )]
        public bool IntersectsLine( Vector2 p1, Vector2 p2 )
        {
            // Find the closest point on the line to the center of the circle
            Vector2 intersection = Line2D.ClosestPointOnLine( Center, p1, p2 );

            // Check if that point is within the radius of the circle
            return ContainsPoint( intersection );
        }

        public static Circle FromDiameter( Vector2 center, float diameter )
        {
            return new Circle( center, diameter * 0.5f );
        }

        public static Circle FromCircumference( Vector2 center, float circumference )
        {
            // C = pi * 2 * r
            return new Circle( center, circumference / (2 * (float)Math.PI) );
        }

        public static Circle FromArea( Vector2 center, float area )
        {
            // A = pi * r * r
            return new Circle( center, (float)Math.Sqrt( area / Math.PI ) );
        }

        /// <summary>
        /// Makes a circle from 2 points that lie on its opposite ends.
        /// </summary>
        public static Circle FromTwoPoints( Vector2 p1, Vector2 p2 )
        {
            Vector2 center = Vector2.Midpoint( p1, p2 );
            float radius = Vector2.Distance( p1, p2 ) * 0.5f;

            return new Circle( center, radius );
        }

        public AABB2D BoundingBox
        {
            get
            {
                return new AABB2D( Center, new Vector2( Diameter ) );
            }
        }

        public static Circle? FromThreePoints( Vector2 p1, Vector2 p2, Vector2 p3 ) // works.
        {
            // Find the perpendicular bisectors of the line segments connecting the points
            Vector2 mid1 = (p1 + p2) / 2;
            Vector2 bisector1 = new Vector2( -(p2.Y - p1.Y), p2.X - p1.X );
            Vector2 mid2 = (p2 + p3) / 2;
            Vector2 bisector2 = new Vector2( -(p3.Y - p2.Y), p3.X - p2.X );

            // Find the intersection of the two bisectors to get the center of the circle
            Vector2? center = Line2D.LineLineIntersection( new Line2D( mid1, mid1 + bisector1 ), new Line2D( mid2, mid2 + bisector2 ) );
            if( center == null )
            {
                return null;
            }

            float radius = Vector2.Distance( center.Value, p1 );
            return new Circle( center.Value, radius );
        }

        public Vector2[] GetPoints( int numPoints )
        {
            // First point starts at X+, then next goes to Y+, then X-, then Y-, and repeat.

            Vector2[] points = new Vector2[numPoints];
            double angleIncrement = 2 * Math.PI / numPoints;
            for( int i = 0; i < numPoints; i++ )
            {
                double angle = i * angleIncrement;
                double x = Center.X + Radius * Math.Cos( angle );
                double y = Center.Y + Radius * Math.Sin( angle );
                points[i] = new Vector2( (float)x, (float)y );
            }
            return points;
        }

        public Circle Offset( Vector2 offset )
        {
            return new Circle( Center + offset, Radius );
        }

        public Circle Scaled( float scale )
        {
            return new Circle( Center, Radius * scale );
        }

        public Circle Inflated( float amount )
        {
            return new Circle( Center, Radius + amount );
        }

        public Circle Deflated( float amount )
        {
            return new Circle( Center, Radius - amount );
        }

        [Obsolete("Unconfirmed")]
        public static Vector2[] GetIntersections( Circle c1, Circle c2 )
        {
            float distance = Vector2.Distance( c1.Center, c2.Center );

            // circles do not overlap.
            if( distance > c1.Radius + c2.Radius )
            {
                return new Vector2[] { };
            }

            // does some weird stuff with trigonometry, apparently. who knows
            float a = (c1.Radius * c1.Radius - c2.Radius * c2.Radius + distance * distance) / (2 * distance);
            float h = (float)Math.Sqrt( c1.Radius * c1.Radius - a * a );
            Vector2 p2 = c1.Center + a * (c2.Center - c1.Center) / distance;
            Vector2 p3 = new Vector2( p2.X + h * (c2.Center.Y - c1.Center.Y) / distance, p2.Y - h * (c2.Center.X - c1.Center.X) / distance );
            Vector2 p4 = new Vector2( p2.X - h * (c2.Center.Y - c1.Center.Y) / distance, p2.Y + h * (c2.Center.X - c1.Center.X) / distance );
            if( p3 == p4 )
                return new Vector2[] { p3 };
            else
                return new Vector2[] { p3, p4 };
        }

        public override bool Equals( object obj )
        {
            if( obj is Circle )
            {
                return this == (Circle)obj;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return Center.GetHashCode() ^ Radius.GetHashCode();
        }

        public static bool operator ==( Circle c1, Circle c2 )
        {
            return c1.Center == c2.Center && c1.Radius == c2.Radius;
        }

        public static bool operator !=( Circle c1, Circle c2 )
        {
            return c1.Center != c2.Center || c1.Radius != c2.Radius;
        }
    }
}