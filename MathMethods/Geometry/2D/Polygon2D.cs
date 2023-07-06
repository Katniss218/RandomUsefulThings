using Geometry;
using System;
using System.Collections.Generic;
using System.Text;

namespace RandomUsefulThings.Math
{
    public class Polygon2D
    {
        // A list to store the vertices of the polygon
        private List<Vector2> _vertices;
        
        [Obsolete("alg from stackoverflow, code from GPT, untested.")]
        public bool IsClockwise()
        {/*
        
            https://stackoverflow.com/questions/1165647/how-to-determine-if-a-list-of-polygon-points-are-in-clockwise-order
        determining the winding order of a 2D polygon:
        sum over the edges of the vertices in the order they appear in the indices.

        point[0] = (5,0)   edge[0]: (6-5)(4+0) =   4
        point[1] = (6,4)   edge[1]: (4-6)(5+4) = -18
        point[2] = (4,5)   edge[2]: (1-4)(5+5) = -30
        point[3] = (1,5)   edge[3]: (1-1)(0+5) =   0
        point[4] = (1,0)   edge[4]: (5-1)(0+0) =   0
                                                 ---
                                                 -44  counter-clockwise

        */
            float sum = 0.0f;
            for( int i = 0; i < _vertices.Count; i++ )
            {
                float x1 = _vertices[i].X;
                float y1 = _vertices[i].Y;
                float x2 = _vertices[(i + 1) % _vertices.Count].X;
                float y2 = _vertices[(i + 1) % _vertices.Count].Y;

                float edgeSum = (x2 - x1) * (y2 + y1);
                sum += edgeSum;
            }
            return sum > 0;
        }
    }
}
