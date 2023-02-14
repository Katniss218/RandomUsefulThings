using System;
using System.Collections.Generic;
using System.Text;

namespace Geometry
{
    public struct Plane
    {
        public Vector3 Normal { get; }

        public Plane( Vector3 normal )
        {
            this.Normal = normal;
        }
    }
}
