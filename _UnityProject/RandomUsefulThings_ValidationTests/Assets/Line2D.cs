using System;
using System.Collections.Generic;
using System.Text;

namespace Geometry
{
    [System.Serializable]
    public struct Vector3
    {

        public static implicit operator UnityEngine.Vector3( Vector3 i )
        {
            return new UnityEngine.Vector3( i.X, i.Y, i.Z );
        }

        public float X;
        public float Y;
        public float Z;

        public float LengthSquared { get => (X * X) + (Y * Y) + (Z * Z); }

        public float Length { get => (float)Math.Sqrt( LengthSquared ); }

        public float this[int i]
        {
            get
            {
                if( i == 0 ) return this.X;
                if( i == 1 ) return this.Y;
                if( i == 2 ) return this.Z;
                throw new IndexOutOfRangeException( "The index was out of the range for Vector2 - [0 to 2]." );
            }
        }

        public Vector3( float value )
        {
            this.X = value;
            this.Y = value;
            this.Z = value;
        }

        public Vector3( float x, float y, float z )
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        public static Vector3 Zero { get => new Vector3( 0.0f, 0.0f, 0.0f ); }
        public static Vector3 Forward { get => new Vector3( 0.0f, 0.0f, 1.0f ); }
        public static Vector3 Right { get => new Vector3( 1.0f, 0.0f, 0.0f ); }
        public static Vector3 Up { get => new Vector3( 0.0f, 1.0f, 0.0f ); }
        public static Vector3 One { get => new Vector3( 1.0f, 1.0f, 1.0f ); }

        public static Vector3 PointingAt( Vector3 fromPoint, Vector3 toPoint )
        {
            // A to B = B - A

            return new Vector3(
                toPoint.X - fromPoint.X,
                toPoint.Y - fromPoint.Y,
                toPoint.Z - fromPoint.Z );
        }

        public Vector3 Normalized()
        {
            float length = this.Length;
            return new Vector3( X / length, Y / length, Z / length );
        }

        public static float Dot( Vector3 v1, Vector3 v2 )
        {
            return (v1.X * v2.X) + (v1.Y * v2.Y) + (v1.Z * v2.Z);
        }

        public static Vector3 Cross( Vector3 v1, Vector3 v2 )
        {
            // Calculate the cross product of the two vectors
            float x = (v1.Y * v2.Z) - (v1.Z * v2.Y);
            float y = (v1.Z * v2.X) - (v1.X * v2.Z);
            float z = (v1.X * v2.Y) - (v1.Y * v2.X);

            // Return the result as a new Vector3 object
            return new Vector3( x, y, z );
        }

        public static float DistanceSquared( Vector3 v1, Vector3 v2 )
        {
            float dx = v2.X - v1.X;
            float dy = v2.Y - v1.Y;
            float dz = v2.Z - v1.Z;

            return (dx * dx) + (dy * dy) + (dz * dz);
        }

        public static float Distance( Vector3 v1, Vector3 v2 )
        {
            return (float)Math.Sqrt( DistanceSquared( v1, v2 ) );
        }

        /// <summary>
        /// Adds a scalar value to each component of the vector.
        /// </summary>
        public static Vector3 Add( Vector3 v, float f )
        {
            return new Vector3( v.X + f, v.Y + f, v.Z + f );
        }

        /// <summary>
        /// Adds the corresponding components of 2 vectors.
        /// </summary>
        public static Vector3 Add( Vector3 v1, Vector3 v2 )
        {
            return new Vector3( v1.X + v2.X, v1.Y + v2.Y, v1.Z + v2.Z );
        }

        /// <summary>
        /// Subtracts a scalar value from each component of the vector.
        /// </summary>
        public static Vector3 Subtract( Vector3 v, float f )
        {
            return new Vector3( v.X - f, v.Y - f, v.Z - f );
        }

        /// <summary>
        /// Subtracts the corresponding components of 2 vectors (ret = v1 - v2).
        /// </summary>
        public static Vector3 Subtract( Vector3 v1, Vector3 v2 )
        {
            return new Vector3( v1.X - v2.X, v1.Y - v2.Y, v1.Z - v2.Z );
        }

        /// <summary>
        /// Multiplies each component of the vector by a scalar value.
        /// </summary>
        public static Vector3 Multiply( Vector3 v, float f )
        {
            return new Vector3( v.X * f, v.Y * f, v.Z * f );
        }

        /// <summary>
        /// Divides each component of the vector by a scalar value.
        /// </summary>
        public static Vector3 Divide( Vector3 v, float f )
        {
            return new Vector3( v.X / f, v.Y / f, v.Z / f );
        }

        public Vector3 Reflect( Vector3 planeNormal )
        {
            // Project the vector onto the plane defined by the normal
            Vector3 projection = ProjectOntoPlane( planeNormal );

            // Reflect the vector off of the plane
            return projection * 2 - this;
        }

        /// <summary>
        /// Projects a vector onto another vector (onto the line defined by its direction).
        /// </summary>
        public Vector3 Project( Vector3 lineDirection )
        {
            return lineDirection * Dot( this, lineDirection ) / lineDirection.LengthSquared;
        }

        public static Vector3 Exclude( Vector3 sourceVector, Vector3 vectorToExclude )
        {
            // Leaves the sourceVector without the component that lied along the vectorToExclude.
            // This is somewhat equivalent to the ProjectOntoPlane, excelt that projectOntoPlane is expected to take in unit vectors.
            return sourceVector - sourceVector.Project( vectorToExclude );
        }

        /// <summary>
        /// Projects the vector onto a plane defined by its normal. Works with unit vectors.
        /// </summary>
        public Vector3 ProjectOntoPlane( Vector3 planeNormal )
        {
            // The projection of vector onto a plane can be calculated by subtracting the component of the vector that is orthogonal to the plane from the original vector.
            // orthogonal component is calculated by first calculating the length, then multiplying the unit direction by the length.
            Vector3 orthogonalComponent = Dot( this, planeNormal ) * planeNormal;

            return this - orthogonalComponent;
        }

        public static Vector3 Slerp( Vector3 start, Vector3 end, float amount )
        {
            // Calculate the dot product of the start and end vectors.
            float dot = Dot( start, end );

            // Clamp the dot product to the range [-1, 1] to prevent any invalid calculations.
            dot = Math.Clamp( dot, -1.0f, 1.0f );

            // Calculate the angle between the two vectors.
            float angle = (float)Math.Acos( dot ) * amount;

            // Calculate the interpolated vector using a formula based on the angle and the start and end vectors.
            Vector3 direction = end - start * dot;
            direction = direction.Normalized();

            return ((start * (float)Math.Cos( angle )) + (direction * (float)Math.Sin( angle ))).Normalized();
        }

        public static Vector3 operator +( float f, Vector3 v1 )
        {
            return Add( v1, f );
        }

        public static Vector3 operator +( Vector3 v1, float f )
        {
            return Add( v1, f );
        }

        public static Vector3 operator +( Vector3 v1, Vector3 v2 )
        {
            return Add( v1, v2 );
        }

        public static Vector3 operator -( Vector3 v1, float f )
        {
            return Subtract( v1, f );
        }
        public static Vector3 operator -( float f, Vector3 v1 )
        {
            return Subtract( v1, f );
        }

        public static Vector3 operator -( Vector3 v1, Vector3 v2 )
        {
            return Subtract( v1, v2 );
        }

        public static Vector3 operator *( Vector3 v, float f )
        {
            return Multiply( v, f );
        }

        public static Vector3 operator *( float f, Vector3 v )
        {
            return Multiply( v, f );
        }

        public static Vector3 operator /( Vector3 v, float f )
        {
            return Divide( v, f );
        }

        public static Vector3 operator /( float f, Vector3 v )
        {
            return Divide( v, f );
        }

        // One of the main uses for this is getting the rotation between two rotations.
        // For example if you wanted to know the Quaternion that would get you from rotationA to rotationB you would do something like this:
        /// FromAtoB = Quaternion.Inverse(rotationA) * rotationB
        /*public Quaternion RotationFromTo( Quaternion from, Quaternion to )
        {
            // Calculate the dot product of the two Quaternions
            float dot = from.x * to.x + from.y * to.y + from.z * to.z + from.w * to.w;

            // If the dot product is negative, negate one of the Quaternions to ensure
            // that the result is a valid rotation
            if( dot < 0 )
            {
                from.x = -from.x;
                from.y = -from.y;
                from.z = -from.z;
                from.w = -from.w;
            }

            // Calculate the Quaternion representing the rotation from the first Quaternion to the second one
            Quaternion result = new Quaternion(
                to.x - from.x,
                to.y - from.y,
                to.z - from.z,
                to.w - from.w
            );

            // Normalize the Quaternion to ensure it is a valid rotation
            result.Normalize();

            return result;
        }*/
        public static bool operator ==( Vector3 v1, Vector3 v2 )
        {
            return v1.X == v2.X && v1.Y == v2.Y && v1.Z == v2.Z;
        }

        public static bool operator !=( Vector3 v1, Vector3 v2 )
        {
            return v1.X != v2.X || v1.Y != v2.Y || v1.Z != v2.Z;
        }
    }
}


namespace Geometry
{
    [System.Serializable]
    public struct Triangle2D
    {
        public Vector2 P1;
        public Vector2 P2;
        public Vector2 P3;

        /// <summary>
        /// Returns the geometric middle of the triangle (the point where it would balance).
        /// </summary>
        public Vector2 Midpoint { get => (P1 + P2 + P3) / 3.0f; }

        public Triangle2D( Vector2 p1, Vector2 p2, Vector2 p3 )
        {
            this.P1 = p1;
            this.P2 = p2;
            this.P3 = p3;
        }

        public (float a, float b, float c) GetEdges()
        {
            return (Vector2.Distance( P1, P2 ), Vector2.Distance( P2, P3 ), Vector2.Distance( P3, P1 ));
        }

        public static float Area( float a, float b, float c )
        {
            float halfPerimeter = (a + b + c) / 2;

            return (float)Math.Sqrt( halfPerimeter * (halfPerimeter - a) * (halfPerimeter - b) * (halfPerimeter - c) );

            // there's also this shoelace formula ` Math.Abs(x1*y2 + x2*y3 + x3*y1 - x1*y3 - x2*y1 - x3*y2) * 0.5f` using vertices (x,y 1 through 3) instead of edges.
        }

        public float Area()
        {
            (float a, float b, float c) = GetEdges();

            return Area( a, b, c );
        }

        // Returns true if the point is inside the triangle, false otherwise
        public bool PointInTriangle( Vector2 p )
        {
            // Calculate the area of the triangle.
            float totalArea = Area();

            // Calculate the areas of the three triangles formed by the point and the sides of the triangle
            float area1 = new Triangle2D( p, P1, P2 ).Area();
            float area2 = new Triangle2D( p, P2, P3 ).Area();
            float area3 = new Triangle2D( p, P3, P1 ).Area();

            // If the sum of the three triangle areas is equal to the total area, then the point is inside the triangle
            return (Math.Abs(totalArea - (area1 + area2 + area3)) < 0.00001f);
        }

        public bool IsPointInTriangle( Vector2 p )
        {
            // Check if the point is inside the triangle using barycentric coordinates
            float alpha = (((P2.Y - P3.Y) * (p.X - P3.X)) + ((P3.X - P2.X) * (p.Y - P3.Y))) /
                          (((P2.Y - P3.Y) * (P1.X - P3.X)) + ((P3.X - P2.X) * (P1.Y - P3.Y)));

            float beta = (((P3.Y - P1.Y) * (p.X - P3.X)) + ((P1.X - P3.X) * (p.Y - P3.Y))) /
                         (((P2.Y - P3.Y) * (P1.X - P3.X)) + ((P3.X - P2.X) * (P1.Y - P3.Y)));

            float gamma = 1.0f - alpha - beta;

            return alpha > 0.0f && beta > 0.0f && gamma > 0.0f;
        }

        public static float InterpolateTriangle( Vector2 p, Vector2 t1, Vector2 t2, Vector2 t3, float value1, float value2, float value3 )
        {
            // Compute the barycentric coordinates of the point p with respect to the triangle (v1, v2, v3)
            Vector3 barycentric = new Triangle2D( t1, t2, t3 ).BarycentricCoordinates( p );

            // Interpolate the values at the vertices using the barycentric coordinates
            return (value1 * barycentric.X) + (value2 * barycentric.Y) + (value3 * barycentric.Z);
        }

        public Vector3 BarycentricCoordinates( Vector2 point )
        {
            float areaABC = new Triangle2D( P1, P2, P3 ).Area();
            float areaPBC = new Triangle2D( point, P2, P3 ).Area();
            float areaPCA = new Triangle2D( P1, point, P3 ).Area();

            float barycentricCoord1 = areaPBC / areaABC;
            float barycentricCoord2 = areaPCA / areaABC;
            float barycentricCoord3 = 1 - barycentricCoord1 - barycentricCoord2;

            return new Vector3( barycentricCoord1, barycentricCoord2, barycentricCoord3 );
        }
    }
}
