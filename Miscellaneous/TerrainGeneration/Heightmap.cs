using Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Miscellaneous.TerrainGeneration
{
    public class Heightmap
    {
        float[,] heightmap;


        public float Sample( float x, float y )
        {
            // 0 to 1, interpolates the values in the array.
            throw new NotImplementedException();
        }

        [Obsolete( "unconfirmed" )]
        public List<Vector2> SampleNeighbors( Vector2 point, float stepDistance )
        {
            List<Vector2> neighbors = new List<Vector2>();

            for( int i = -1; i <= 1; i++ )
            {
                for( int j = -1; j <= 1; j++ )
                {
                    if( i == 0 && j == 0 )
                        continue;

                    float x = point.X + i * stepDistance;
                    float y = point.Y + j * stepDistance;

                    if( x < 0 || x > 1 || y < 0 || y > 1 )
                        continue;

                    neighbors.Add( new Vector2( x, y ) );
                }
            }

            return neighbors;
        }

        [Obsolete("unconfirmed")]
        public void GetSlopeAndDirection( float x, float y, out float slope, out Vector2 direction )
        {
            // Calculate the distance between adjacent pixels.
            float pixelSize = 1.0f / (float)heightmap.GetLength( 0 );

            // Calculate the height values of neighboring pixels.
            float heightCenter = Sample( x, y );
            float heightLeft = Sample( x - pixelSize, y );
            float heightRight = Sample( x + pixelSize, y );
            float heightUp = Sample( x, y + pixelSize );
            float heightDown = Sample( x, y - pixelSize );

            // Calculate the slopes in the x and y directions.
            float slopeX = (heightRight - heightLeft) / (2 * pixelSize);
            float slopeY = (heightUp - heightDown) / (2 * pixelSize);

            // Calculate the total slope and direction.
            slope = (float)Math.Sqrt( slopeX * slopeX + slopeY * slopeY );
            direction = new Vector2( -slopeX, -slopeY ).Normalized();
        }

        [Obsolete( "unconfirmed" )]
        public Vector2 GetCurvature( float x, float y, float stepSize )
        {
            float h = Sample( x, y );
            float hx1 = Sample( x - stepSize, y );
            float hx2 = Sample( x + stepSize, y );
            float hy1 = Sample( x, y - stepSize );
            float hy2 = Sample( x, y + stepSize );

            float dx = (hx2 - 2 * h + hx1) / (stepSize * stepSize);
            float dy = (hy2 - 2 * h + hy1) / (stepSize * stepSize);

            float dxy = (Sample( x + stepSize, y + stepSize ) - Sample( x + stepSize, y - stepSize ) - Sample( x - stepSize, y + stepSize ) + Sample( x - stepSize, y - stepSize )) / (4 * stepSize * stepSize);

            float denominator = (1 + dx * dx + dy * dy);
            if( denominator == 0 )
            {
                // Avoid division by zero
                return Vector2.Zero;
            }

            float curvatureX = (dx * dy - dxy) / (float)(denominator * Math.Sqrt( denominator ));
            float curvatureY = (dx * dy + dxy) / (float)(denominator * Math.Sqrt( denominator ));

            return new Vector2( curvatureX, curvatureY );
        }

        [Obsolete( "unconfirmed" )]
        public List<Vector2> FollowRidgeOrValley( Vector2 startPoint, float stepSize, bool followRidge = true )
        {
            // Initialize the path with the starting point
            List<Vector2> path = new List<Vector2>();
            path.Add( startPoint );

            // Follow the ridgeline or valley until a local maximum or minimum is reached
            while( true )
            {
                // Get the current position on the path
                Vector2 currentPos = path[path.Count - 1];

                // Find the neighboring points and their heights
                List<Vector2> neighbors = SampleNeighbors( currentPos, stepSize );
                float[] heights = neighbors.Select( n => Sample( n.X, n.Y ) ).ToArray();

                // Find the index of the highest or lowest neighbor, depending on whether we're following a ridge or valley
                int bestIndex = followRidge ? Array.IndexOf( heights, heights.Max() ) : Array.IndexOf( heights, heights.Min() );

                // Stop if we've reached a local maximum or minimum
                if( heights[bestIndex] == Sample( currentPos.X, currentPos.Y ) )
                {
                    break;
                }

                // Add the next point on the path
                Vector2 nextPos = neighbors[bestIndex];
                path.Add( nextPos );
            }

            return path;
        }

        [Obsolete("Unconfirmed")]
        public (List<Vector2>, List<Vector2>) FindLocalExtrema()
        {
            // Initialize lists for local minima and maxima
            List<Vector2> minima = new List<Vector2>();
            List<Vector2> maxima = new List<Vector2>();

            // Iterate over all points in the heightmap
            int width = heightmap.GetLength( 0 );
            int height = heightmap.GetLength( 1 );
            for( int i = 0; i < width; i++ )
            {
                for( int j = 0; j < height; j++ )
                {
                    Vector2 currentPos = new Vector2( i / (float)width, j / (float)height );
                    float currentHeight = Sample( currentPos.X, currentPos.Y );

                    // Check if the current point is a local minimum or maximum
                    bool isMinima = true;
                    bool isMaxima = true;

                    for( int k = -1; k <= 1; k++ )
                    {
                        for( int l = -1; l <= 1; l++ )
                        {
                            if( k == 0 && l == 0 )
                                continue;

                            float neighborHeight = Sample( (i + k) / (float)width, (j + l) / (float)height );

                            if( neighborHeight <= currentHeight )
                                isMaxima = false;

                            if( neighborHeight >= currentHeight )
                                isMinima = false;
                        }
                    }

                    // Add the point to the appropriate list if it is a local minimum or maximum
                    if( isMinima )
                        minima.Add( currentPos );

                    if( isMaxima )
                        maxima.Add( currentPos );
                }
            }

            return (minima, maxima);
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
