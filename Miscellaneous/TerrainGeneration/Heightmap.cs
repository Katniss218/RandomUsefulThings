using Geometry;
using System;
using System.Collections.Generic;
using System.Text;

namespace Miscellaneous.TerrainGeneration
{
    class Heightmap
    {
        float[,] heightmap;


        public float Sample( float x, float y )
        {
            // 0 to 1, interpolates the values in the array.
            throw new NotImplementedException();
        }

        public Vector3 GetNormal( float pixelX, float pixelY, float edgePixelLength )
        {
            // x and y are positions.
            // edgepixelLength is used as an offset when sampling the neighboring points to calculate the slope.
            //  this should be equal to the distance between grid points on the mesh.


            throw new NotImplementedException();
        }


        // normalmap can be calculated for a heightmap mesh
        // take the position of the vertex.
        // take the position of the 4/8 neighboring vertices
        //   (using the same formula as used for generation so the tex samples match the mesh)
        // calculate the slope and aspect and calculate the normal vector from there.
    }
}
