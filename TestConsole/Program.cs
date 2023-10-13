using BenchmarkDotNet.Running;
using Newtonsoft.Json.Linq;
using RandomUsefulThings.Math;
using RandomUsefulThings.Math.LinearAlgebra;
using RandomUsefulThings.Math.LinearAlgebra.NumericalMethods;
using RandomUsefulThings.Misc;
using RandomUsefulThings.Physics.FluidSim;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Numerics;

public class Triangle
{
    public Vector3 Vertex1 { get; set; }
    public Vector3 Vertex2 { get; set; }
    public Vector3 Vertex3 { get; set; }
}

public class Mesh
{
    public List<Triangle> Triangles { get; set; }

    [Obsolete("unconfirmed, but seems to work on a 2x2x2 cube with a corner at the origin, and with the origin outside of the cube completely")]
    public float CalculateVolume()
    {
        float volume = 0;

        foreach( var triangle in Triangles )
        {
            volume += SignedVolumeOfFaceTetrahedron( triangle.Vertex1, triangle.Vertex2, triangle.Vertex3 );
        }

        return Math.Abs( volume );
    }

    private float SignedVolumeOfFaceTetrahedron( Vector3 v1, Vector3 v2, Vector3 v3 )
    {
        return Vector3.Dot( v1, Vector3.Cross( v2, v3 ) ) / 6.0f;
    }
}

namespace TestConsole
{
    public static class Program
    {
        public static double CalculateDistance( double initialVelocity, double acceleration )
        {
            // returns the distance at which the object with a given velocity and acceleration will reach 0 velocity.
            double distance = Math.Pow( initialVelocity, 2 ) / (2 * acceleration);
            return distance;
        }



        public static void Main( string[] args )
        {
            Console.WriteLine( "A" );
            Main(null);
            List<Triangle> triangles = new List<Triangle>
            {
                // Front face
                new Triangle { Vertex1 = new Vector3(0, 0, 0), Vertex2 = new Vector3(2, 0, 0), Vertex3 = new Vector3(0, 2, 0) },
                new Triangle { Vertex1 = new Vector3(2, 0, 0), Vertex2 = new Vector3(2, 2, 0), Vertex3 = new Vector3(0, 2, 0) },

                // Back face
                new Triangle { Vertex1 = new Vector3(0, 0, 2), Vertex2 = new Vector3(0, 2, 2), Vertex3 = new Vector3(2, 0, 2) },
                new Triangle { Vertex1 = new Vector3(2, 0, 2), Vertex2 = new Vector3(0, 2, 2), Vertex3 = new Vector3(2, 2, 2) },

                // Left face
                new Triangle { Vertex1 = new Vector3(0, 0, 0), Vertex2 = new Vector3(0, 2, 0), Vertex3 = new Vector3(0, 0, 2) },
                new Triangle { Vertex1 = new Vector3(0, 2, 0), Vertex2 = new Vector3(0, 2, 2), Vertex3 = new Vector3(0, 0, 2) },

                // Right face
                new Triangle { Vertex1 = new Vector3(2, 0, 0), Vertex2 = new Vector3(2, 0, 2), Vertex3 = new Vector3(2, 2, 0) },
                new Triangle { Vertex1 = new Vector3(2, 0, 2), Vertex2 = new Vector3(2, 2, 2), Vertex3 = new Vector3(2, 2, 0) },

                // Top face
                new Triangle { Vertex1 = new Vector3(0, 2, 0), Vertex2 = new Vector3(2, 2, 0), Vertex3 = new Vector3(0, 2, 2) },
                new Triangle { Vertex1 = new Vector3(2, 2, 0), Vertex2 = new Vector3(2, 2, 2), Vertex3 = new Vector3(0, 2, 2) },

                // Bottom face
                new Triangle { Vertex1 = new Vector3(0, 0, 0), Vertex2 = new Vector3(0, 0, 2), Vertex3 = new Vector3(2, 0, 0) },
                new Triangle { Vertex1 = new Vector3(2, 0, 0), Vertex2 = new Vector3(0, 0, 2), Vertex3 = new Vector3(2, 0, 2) }
            };

            triangles = new List<Triangle>
            {
                // Front face
                new Triangle { Vertex1 = new Vector3(4, 4, 4), Vertex2 = new Vector3(6, 4, 4), Vertex3 = new Vector3(4, 5, 4) },
                new Triangle { Vertex1 = new Vector3(6, 4, 4), Vertex2 = new Vector3(6, 5, 4), Vertex3 = new Vector3(4, 5, 4) },

                // Back face
                new Triangle { Vertex1 = new Vector3(4, 4, 6), Vertex2 = new Vector3(4, 5, 6), Vertex3 = new Vector3(6, 4, 6) },
                new Triangle { Vertex1 = new Vector3(6, 4, 6), Vertex2 = new Vector3(4, 5, 6), Vertex3 = new Vector3(6, 5, 6) },

                // Left face
                new Triangle { Vertex1 = new Vector3(4, 4, 4), Vertex2 = new Vector3(4, 5, 4), Vertex3 = new Vector3(4, 4, 6) },
                new Triangle { Vertex1 = new Vector3(4, 5, 4), Vertex2 = new Vector3(4, 5, 6), Vertex3 = new Vector3(4, 4, 6) },

                // Right face
                new Triangle { Vertex1 = new Vector3(6, 4, 4), Vertex2 = new Vector3(6, 5, 6), Vertex3 = new Vector3(6, 5, 4) },
                new Triangle { Vertex1 = new Vector3(6, 4, 6), Vertex2 = new Vector3(6, 5, 6), Vertex3 = new Vector3(6, 5, 4) },

                // Top face
                new Triangle { Vertex1 = new Vector3(4, 5, 4), Vertex2 = new Vector3(6, 5, 4), Vertex3 = new Vector3(4, 5, 6) },
                new Triangle { Vertex1 = new Vector3(6, 5, 4), Vertex2 = new Vector3(6, 5, 6), Vertex3 = new Vector3(4, 5, 6) },

                // Bottom face
                new Triangle { Vertex1 = new Vector3(4, 4, 4), Vertex2 = new Vector3(4, 4, 6), Vertex3 = new Vector3(6, 4, 4) },
                new Triangle { Vertex1 = new Vector3(6, 4, 4), Vertex2 = new Vector3(4, 4, 6), Vertex3 = new Vector3(6, 4, 6) }
            };

            Mesh m = new Mesh()
            {
                Triangles = triangles
            };

            float v = m.CalculateVolume();

            BenchmarkRunner.Run<EasyBenchmarkTool>();
        }
    }
}
