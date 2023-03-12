using System;
using System.Collections.Generic;
using System.Text;

namespace Geometry
{
    public struct Ray3D
    {
		Vector3 Point;
		Vector3 Direction;

		[Obsolete("unconfirmed")]
		public float? Intersects( Vector3 center, float radius )
		{
			float toSphereX = center.X - Point.X;
			float toSphereY = center.Y - Point.Y;
			float toSphereZ = center.Z - Point.Z;
			float distanceToSphereSquared = toSphereX * toSphereX + toSphereY * toSphereY + toSphereZ * toSphereZ;
			float sqRadius = radius * radius;
			if( distanceToSphereSquared <= sqRadius )
			{
				return 0f;
			}
			float num6 = toSphereX * Direction.X + toSphereY * Direction.Y + toSphereZ * Direction.Z;
			if( num6 < 0f )
			{
				return null;
			}
			float num7 = distanceToSphereSquared - num6 * num6;
			if( num7 > sqRadius )
			{
				return null;
			}
			float num8 = (float)Math.Sqrt( sqRadius - num7 );
			return num6 - num8;
		}
	}
}
