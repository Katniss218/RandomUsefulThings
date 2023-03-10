using System;
using System.Collections.Generic;
using System.Text;

namespace Geometry
{
    public struct Plane3D
    {
        public Vector3 Normal { get; }

        public Vector3 Point { get; set; }

        public Plane3D( Vector3 normal, Vector3 point )
        {
            this.Normal = normal;
            this.Point = point;
        }

        // equation for the plane in 3D with `point` point and `normal` normal vector. `x/y/z` are every point in the coordinate space.
        // normal.x*(x - point.x) + normal.y*(y - point.y) + normal.z*(z - point.z) = 0
    }
}
