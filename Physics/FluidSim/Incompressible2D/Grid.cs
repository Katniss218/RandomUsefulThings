using Geometry;
using System;
using System.Collections.Generic;
using System.Text;

namespace Physics.FluidSim.Incompressible2D
{
    public class Grid
    {
        public struct Cell
        {
            public float Density;
            public Vector2 Velocity;
        }

        private Cell[,] _cells;
        private int sizeX;
        private int sizeY;

        // https://www.youtube.com/watch?v=qsYE1wMEMPA

        private void Diffuse( int i, int j, float dt )
        {
            const float viscosity = 0.2f;
            float k = dt * viscosity;

            float densityCurrent = _cells[i, j].Density;
            float densityAvg = 0.0f;
            int count = 0;
            if( i > 0 )
            {
                densityAvg += _cells[i - 1, j].Density;
                count++;
            }
            if( j > 0 )
            {
                densityAvg += _cells[i, j - 1].Density;
                count++;
            }
            if( i < sizeX - 1 )
            {
                densityAvg += _cells[i + 1, j].Density;
                count++;
            }
            if( j < sizeY - 1 )
            {
                densityAvg += _cells[i, j + 1].Density;
                count++;
            }
            densityAvg /= count;

#warning TODO - incomplete
            float newDensity = densityCurrent; // we need to know the future value. We can get it using the gauss-sidel method or matrix elimination with backsubstitution.
        }

        public void Step( float dt )
        {

        }
    }
}
