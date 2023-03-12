using System;
using System.Collections.Generic;
using System.Text;

namespace Geometry
{
    public struct Vector2
    {
        public float X { get; }
        public float Y { get; }

        public float LengthSquared { get => (X * X) + (Y * Y); }

        public float Length { get => (float)Math.Sqrt( LengthSquared ); }

        public float this[int i]
        {
            get
            {
                if( i == 0 ) return this.X;
                if( i == 1 ) return this.Y;
                throw new IndexOutOfRangeException( "The index was out of the range for Vector2 - [0 to 1]." );
            }
        }

        public Vector2( float value )
        {
            this.X = value;
            this.Y = value;
        }

        public Vector2( float x, float y )
        {
            this.X = x;
            this.Y = y;
        }

        public static Vector2 Zero { get => new Vector2( 0.0f, 0.0f ); }
        public static Vector2 One { get => new Vector2( 1.0f, 1.0f ); }

        /// <summary>
        /// Returns the vector pointing at +Y, with length of 1.
        /// </summary>
        public static Vector2 Up { get => new Vector2( 0.0f, 1.0f ); }

        /// <summary>
        /// Returns the vector pointing at -Y, with length of 1.
        /// </summary>
        public static Vector2 Down { get => new Vector2( 0.0f, -1.0f ); }

        /// <summary>
        /// Returns the vector pointing at +X, with length of 1.
        /// </summary>
        public static Vector2 Right { get => new Vector2( 1.0f, 0.0f ); }

        /// <summary>
        /// Returns the vector pointing at -X, with length of 1.
        /// </summary>
        public static Vector2 Left { get => new Vector2( -1.0f, 0.0f ); }

        /// <summary>
        /// Computes a vector pointing from one point to another.
        /// </summary>
        /// <returns>The vector representing the displacement from 'from' to 'to'.</returns>
        public static Vector2 PointingAt( Vector2 origin, Vector2 target )
        {
            // A pointing at B equals to `B - A`

            return new Vector2(
                target.X - origin.X,
                target.Y - origin.Y );
        }

        public Vector2 Normalized()
        {
            // To "normalize" a vector means to set its length to 1, while keeping the direction it's pointing in.
            float length = this.Length;
            return new Vector2( X / length, Y / length );
        }

        public static float Dot( Vector2 v1, Vector2 v2 )
        {
            return v1.X * v2.X + v1.Y * v2.Y;
        }

        // atan2 returns the values in range [-pi, pi], the 0 value is at +X axis.
        // atan2(y, x) == atan(y / x)

        // if atan2 returns negative numbers, we can map to the fully positive range by using `v = (v + 360) % 360;` (or `v = (v + (2*PI)) % (2*PI);`)
        // or v = (v < 0) ? (v + 360) : v;
        // or v = (v < 0) ? (v + (2*PI)) : v;

        public static float Cross( Vector2 v1, Vector2 v2 )
        {
            // The cross product of two vectors in two-dimensional space is a scalar value, rather than a vector. It is defined as follows:

            // It is equal to the product of the lengths of the input vectors and the sine of the angle between the input vectors.
            // Its sign indicates the "handedness" of the two vectors. If the cross product is positive, the two vectors are "counterclockwise"(as seen from the positive z - axis), and if the cross product is negative, the two vectors are "clockwise."

            return v1.X * v2.Y - v1.Y * v2.X;
        }

        /// <summary>
        /// Calculates the square of the distance between two points.
        /// </summary>
        public static float DistanceSquared( Vector2 p1, Vector2 p2 )
        {
            float dx = p2.X - p1.X;
            float dy = p2.Y - p1.Y;

            return (dx * dx) + (dy * dy);
        }

        /// <summary>
        /// Calculates the distance between two points.
        /// </summary>
        public static float Distance( Vector2 p1, Vector2 p2 )
        {
            return (float)Math.Sqrt( DistanceSquared( p1, p2 ) );
        }

        public static Vector2 Min( Vector2 a, Vector2 b )
        {
            return new Vector2( Math.Min( a.X, b.X ), Math.Min( a.Y, b.Y ) );
        }

        public static Vector2 Max( Vector2 a, Vector2 b )
        {
            return new Vector2( Math.Max( a.X, b.X ), Math.Max( a.Y, b.Y ) );
        }

        /// <summary>
        /// Rotates the Vector2 (point) around the origin.
        /// </summary>
        /// <param name="angle">The angle [radians].</param>
        public Vector2 Rotate( float angle )
        {
            // Rotates counterclockwise if X+ points right and Y+ points up.
            float newX = (float)((X * Math.Cos( angle )) - (Y * Math.Sin( angle )));
            float newY = (float)((X * Math.Sin( angle )) + (Y * Math.Cos( angle )));

            return new Vector2( newX, newY );
        }

        /// <summary>
        /// Rotates the Vector2 (point) around a pivot point.
        /// </summary>
        /// <param name="pivot">The point to rotate around.</param>
        /// <param name="angle">The angle [radians].</param>
        public Vector2 RotateAround( Vector2 pivot, float angle )
        {
            Vector2 v = this - pivot;

            v = v.Rotate( angle );

            return v + pivot;
        }

        /// <summary>
        /// Calculates the angle between 2 vectors.
        /// </summary>
        /// <returns>The (unsigned) angle between the 2 vectors in radians.</returns>
        public static float Angle( Vector2 a, Vector2 b )
        {
            float dotProduct = Dot( a, b );
            float aLength = a.Length;
            float bLength = b.Length;
            return (float)Math.Acos( dotProduct / (aLength * bLength) );
        }

        /// <summary>
        /// Projects the vector onto a line defined by the direciton of the other vector.
        /// </summary>
        public Vector2 ProjectOnto( Vector2 other ) // Works
        {
            // This can also be thought of as returning the component of this vector that points in the direction of the other vector.
            float dot = Dot( this, other );
            float otherLengthSquared = other.Length * other.Length;

            return other * (dot / otherLengthSquared);
        }

        public static Vector2 Reflect( Vector2 v, Vector2 normal ) // works.
        {
            Vector2 projection = v.ProjectOnto( normal );
            return (projection * 2) - v;
        }

        [Obsolete( "Unconfirmed" )]
        public void ToPolar( out float length, out float angle )
        {
            length = Length;
            angle = (float)Math.Atan2( Y, X );
        }

        [Obsolete( "Unconfirmed" )]
        public static Vector2 FromPolar( float length, float angle )
        {
            return new Vector2(
                length * (float)Math.Cos( angle ),
                length * (float)Math.Sin( angle )
            );
        }

        /// <summary>
        /// Calculates the midpoint of 2 points.
        /// </summary>
        public static Vector2 Midpoint( Vector2 p1, Vector2 p2 )
        {
            return new Vector2( (p1.X + p2.X) / 2, (p1.Y + p2.Y) / 2 );
        }

        public float Slope()
        {
            // Slope is the change in y for a unit (1.0) change in x.
            return this.Y / this.X;
        }

        public static Vector2 FromSlope( float slope )
        {
            // Slope is the change in y for a unit (1.0) change in x.
            return new Vector2( 1.0f, slope );
        }

        /// <summary>
        /// Rounds each component to the nearest multiple of the specified value.
        /// </summary>
        public static Vector2 RoundToMultiple( Vector2 value, float multiple )
        {
            return new Vector2(
                MathMethods.MathMethods.RoundToMultiple( value.X, multiple ),
                MathMethods.MathMethods.RoundToMultiple( value.Y, multiple )
                );
        }

        private static Vector2 Add( Vector2 v1, Vector2 v2 )
        {
            return new Vector2( v1.X + v2.X, v1.Y + v2.Y );
        }

        private static Vector2 Subtract( Vector2 v1, Vector2 v2 )
        {
            return new Vector2( v1.X - v2.X, v1.Y - v2.Y );
        }

        private static Vector2 Multiply( Vector2 v, float scalar )
        {
            return new Vector2( v.X * scalar, v.Y * scalar );
        }

        private static Vector2 Divide( Vector2 v, float scalar )
        {
            return new Vector2( v.X / scalar, v.Y / scalar );
        }

        public static Vector2 operator +( Vector2 v1, Vector2 v2 )
        {
            return Add( v1, v2 );
        }

        public static Vector2 operator -( Vector2 v1, Vector2 v2 )
        {
            return Subtract( v1, v2 );
        }

        public static Vector2 operator *( Vector2 v, float scalar )
        {
            return Multiply( v, scalar );
        }

        public static Vector2 operator *( float scalar, Vector2 v )
        {
            return Multiply( v, scalar );
        }

        public static Vector2 operator /( Vector2 v, float scalar )
        {
            return Divide( v, scalar );
        }
        public static Vector2 operator /( float scalar, Vector2 v )
        {
            return Divide( v, scalar );
        }

        public static bool operator ==( Vector2 v1, Vector2 v2 )
        {
            return v1.X == v2.X && v1.Y == v2.Y;
        }

        public static bool operator !=( Vector2 v1, Vector2 v2 )
        {
            return v1.X != v2.X || v1.Y != v2.Y;
        }
    }
}