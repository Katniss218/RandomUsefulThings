using System;
using System.Collections.Generic;
using System.Text;

namespace Transformations
{
    public struct Vector2
    {
        public float X { get; private set; }
        public float Y { get; private set; }

        public float LengthSquared { get => (X * X) + (Y * Y); }

        public float Length { get => (float)Math.Sqrt( LengthSquared ); }

        public Vector2( float x, float y )
        {
            this.X = x;
            this.Y = y;
        }

        public static float Dot( Vector2 v1, Vector2 v2 )
        {
            return v1.X * v2.X + v1.Y * v2.Y;
        }

        // Method to normalize a Vector2
        public void Normalize()
        {
            float length = (float)Math.Sqrt( X * X + Y * Y );
            X /= length;
            Y /= length;
        }


        private static Vector2 Subtract( Vector2 v1, Vector2 v2 )
        {
            return new Vector2( v1.X - v2.X, v1.Y - v2.Y );
        }

        private static Vector2 Multiply( Vector2 v, float scalar )
        {
            return new Vector2( v.X * scalar, v.Y * scalar );
        }

        public static Vector2 operator *( Vector2 v, float scalar )
        {
            return Multiply( v, scalar );
        }

        public static Vector2 operator -( Vector2 v1, Vector2 v2 )
        {
            return Subtract( v1, v2 );
        }
    }
}