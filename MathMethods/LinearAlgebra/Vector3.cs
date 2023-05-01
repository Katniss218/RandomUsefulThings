using RandomUsefulThings.Math;
using System;
using System.Collections.Generic;
using System.Text;

namespace Geometry
{
    public struct Vector3
    {
        public float X { get; }
        public float Y { get; }
        public float Z { get; }

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
            // `A pointing to B` is equal to `B - A`

            return new Vector3(
                toPoint.X - fromPoint.X,
                toPoint.Y - fromPoint.Y,
                toPoint.Z - fromPoint.Z );
        }

        public static Vector3 AngleAxis( float angle, Vector3 axis )
        {
            // Direction represents the axis, and magnitude represents the angle.
            // useful for e.g. Angular Velocity
            return axis.Normalized() * angle;
        }

        public void ToAngleAxis( out float angle, out Vector3 axis )
        {
            angle = this.Length;
            axis = this / angle; // Normalize the vector (but without calling normalize, which would recalculate the magnitude).
        }

        /// <summary>
        /// Sets the length of the vector to 1.
        /// </summary>
        /// <returns>A new vector with its length (magnitude) equal to 1, and the same direction as the input vector.</returns>
        public Vector3 Normalized()
        {
            float length = this.Length;
            return new Vector3( X / length, Y / length, Z / length );
        }

        /// <summary>
        /// Calculates the dot product of two vectors.
        /// </summary>
        public static float Dot( Vector3 v1, Vector3 v2 )
        {
            // Returns the magnitude of the vector v1, when projected onto the vector v2.
            // When both are unit vectors, returns the cosine of the angle between the 2 vectors.

            // dot(v1, v2) = v1.magnitude * v2.magnitude * cos(angleBetween(v1, v2))
            // - angleBetween in radians.

            return (v1.X * v2.X) + (v1.Y * v2.Y) + (v1.Z * v2.Z);
        }

        public static Vector3 Cross( Vector3 v1, Vector3 v2 )
        {
            // Calculate the cross product of the two vectors (X - cross product)
            // v1, v2, v3, ... - vectors
            // s1, s2, s3, ... - scalars
            // anti-commutative property:       v1 X v2 = -v2 X v1
            // associative property:            (s1*v1) X (s2*v2) = (s1*s2*(v1 X v2)   
            // distributive property:           v1 X (v2 + v3) = v1 X v2 + v1 X v3
            // distributive property:           (v1 + v2) X v3 = v1 X v3 + v2 X v3

            // v1 X v2 = 0, only when v1 and v2 are parallel, or when v1 or v2 = 0

            float x = (v1.Y * v2.Z) - (v1.Z * v2.Y);
            float y = (v1.Z * v2.X) - (v1.X * v2.Z);
            float z = (v1.X * v2.Y) - (v1.Y * v2.X);

            // Return the result as a new Vector3 object
            return new Vector3( x, y, z );
        }

        public static float TripleProduct( Vector3 v1, Vector3 v2, Vector3 v3 )
        {
            // Returns the volume of a parallelepiped with the following side lengths.
            return Dot( v1, Cross( v2, v3 ) );
        }

        /// <summary>
        /// Calculates the square of the distance between the 2 vectors.
        /// </summary>
        public static float DistanceSquared( Vector3 v1, Vector3 v2 )
        {
            float dx = v2.X - v1.X;
            float dy = v2.Y - v1.Y;
            float dz = v2.Z - v1.Z;

            return (dx * dx) + (dy * dy) + (dz * dz);
        }

        /// <summary>
        /// Calculates the distance between the 2 vectors.
        /// </summary>
        /// <remarks>
        /// Slower to compute than <see cref="DistanceSquared"/> due to an additional square root.
        /// </remarks>
        public static float Distance( Vector3 v1, Vector3 v2 )
        {
            return (float)Math.Sqrt( DistanceSquared( v1, v2 ) );
        }

        /// <summary>
        /// Rounds each component to the nearest multiple of the specified value.
        /// </summary>
        public static Vector3 RoundToMultiple( Vector3 value, float multiple )
        {
            return new Vector3(
                MathMethods.RoundToMultiple( value.X, multiple ),
                MathMethods.RoundToMultiple( value.Y, multiple ),
                MathMethods.RoundToMultiple( value.Z, multiple )
                );
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

        /// <summary>
        /// Transforms a point by a matrix.
        /// </summary>
        public static Vector3 Multiply( Vector3 vector, Matrix4x4 matrix )
        {
            return new Vector3(
                (vector.X * matrix.M00) + (vector.Y * matrix.M01) + (vector.Z * matrix.M02) + matrix.M03, // transforming a direction would be without the last addition, I *think*.
                (vector.X * matrix.M10) + (vector.Y * matrix.M11) + (vector.Z * matrix.M12) + matrix.M13,
                (vector.X * matrix.M20) + (vector.Y * matrix.M21) + (vector.Z * matrix.M22) + matrix.M23
            );
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

            // This is somewhat equivalent to the ProjectOntoPlane, except that projectOntoPlane is expected to take in unit vectors.
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

        /// <summary>
        /// Spherically interpolates between two direction vectors.
        /// </summary>
        public static Vector3 Slerp( Vector3 start, Vector3 end, float amount )
        {
            // "How I would intepolate linearly on a sphere is find the average mangitude, then find the average angle (weighted by the magnitudes)"
            //  -- Paculino

            float dot = Math.Clamp( Dot( start, end ), -1.0f, 1.0f );

            // calculate the angle between the start/end vectors, and multiply by how far along that angle we want to interpolate to get the angle between the start and the returned interpolated vector.
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

        // rotate a vector by a quaternion (assuming origin is (0,0,0) ??)
        public static Vector3 operator *( Quaternion q, Vector3 v )
        {
            Quaternion vq = new Quaternion( v.X, v.Y, v.Z, 0 );
            Quaternion r = q * vq * q.Inverse();
            return new Vector3( r.X, r.Y, r.Z );
        }

        public static Quaternion operator *( Vector3 v1, Vector3 v2 )
        {
            // cross-product ?? returns a quaternion, wtf?
            return new Quaternion(
                v1.Y * v2.Z - v1.Z * v2.Y,
                v1.Z * v2.X - v1.X * v2.Z,
                v1.X * v2.Y - v1.Y * v2.X,
                0 );
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
