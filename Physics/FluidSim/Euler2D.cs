using Geometry;
using System;
using System.Collections.Generic;
using System.Text;

namespace RandomUsefulThings.Physics.FluidSim
{
    public class Euler2D
    {
        // https://www.youtube.com/watch?v=iKAVRgIrUOU

        // Eulerian fluid sims are ones that use a grid and not particles.


        // compressibility of water = 3% at 10000 kg/cm^2
        public class Incompressible
        {
            public class Cell
            {
                public Vector2 Velocity;
                public float Pressure;
                public float SmokeDensity;
            }

            public Cell[,] current;
            public Cell[,] next;
            int sizeX;
            int sizeY;
            float deltaX;

            public Incompressible( int sX, int sY, float deltaX )
            {
                this.current = new Cell[sX, sY];
                this.next = new Cell[sX, sY];
                this.sizeX = sX;
                this.sizeY = sY;
                this.deltaX = deltaX; // meters

                for( int x = 0; x < sizeX; x++ )
                {
                    for( int y = 0; y < sizeY; y++ )
                    {
                        current[x, y] = new Cell() { Pressure = 0.0f, Velocity = Vector2.Zero, SmokeDensity = 0.0f };
                    }
                }
            }

            public Vector2 fluidAccelerationRelativeToContainer;

            public void Step( float dt )
            {
                this.next = new Cell[sizeX, sizeY];
                for( int x = 0; x < sizeX; x++ )
                {
                    for( int y = 0; y < sizeY; y++ )
                    {
                        Cell currentC = current[x, y];
                        next[x, y] = new Cell() { Pressure = currentC.Pressure, Velocity = currentC.Velocity, SmokeDensity = currentC.SmokeDensity };
                    }
                }

                Accelerate( fluidAccelerationRelativeToContainer, dt );
                MakeIncompressible( dt );
                Advect( dt );

                current = next;
            }

            private void Accelerate( Vector2 acceleration, float dt )
            {
                for( int x = 0; x < sizeX; x++ )
                {
                    for( int y = 0; y < sizeY; y++ )
                    {
                        current[x, y].Velocity += acceleration * dt;
                    }
                }
            }

            float density = 1100.0f;
            public float OverrelaxationFactor = 1.9f;

            public int iterations = 50;

            private void MakeIncompressible( float dt )
            {
                for( int i = 0; i < iterations; i++ )
                {
                    // outflow = inflow = 0
                    for( int x = 0; x < sizeX; x++ )
                    {
                        for( int y = 0; y < sizeY; y++ )
                        {
                            int wallCount = 0;
                            bool wallLeft = false;
                            bool wallRight = false;
                            bool wallDown = false;
                            bool wallUp = false;
                            if( x == 0 )
                            {
                                wallCount++;
                                wallLeft = true;
                            }
                            if( x == sizeX - 1 )
                            {
                                wallCount++;
                                wallRight = true;
                            }
                            if( y == 0 )
                            {
                                wallCount++;
                                wallDown = true;
                            }
                            if( y == sizeY - 1 )
                            {
                                wallCount++;
                                wallUp = true;
                            }

                            // if wall is moving, set the appropriate side's velocity here (before calculating divergence)

                            float vel0X;
                            float vel0Y;
                            float vel1X;
                            float vel1Y;

                            if( wallLeft )
                                vel0X = 0.0f;
                            else
                                vel0X = current[x - 1, y].Velocity.X + current[x, y].Velocity.X; // += d/4

                            if( wallDown )
                                vel0Y = 0.0f;
                            else
                                vel0Y = current[x, y - 1].Velocity.Y + current[x, y].Velocity.Y; // += d/4

                            if( wallRight )
                                vel1X = 0.0f;
                            else
                                vel1X = current[x + 1, y].Velocity.X + current[x, y].Velocity.X; // -= d/4

                            if( wallUp )
                                vel1Y = 0.0f;
                            else
                                vel1Y = current[x, y + 1].Velocity.Y + current[x, y].Velocity.Y; // -= d/4

                            float divergence = OverrelaxationFactor * (vel1X - vel0X + vel1Y - vel0Y);
                            float s = 4 - wallCount;

                            float divCorrectionFactor = divergence / (s * 2);

                            if( !wallLeft )
                            {
                                // 0X
                                next[x - 1, y].Velocity += new Vector2( divCorrectionFactor, 0 );
                                next[x, y].Velocity += new Vector2( divCorrectionFactor, 0 );
                            }
                            if( !wallDown )
                            {
                                // 0Y
                                next[x, y - 1].Velocity += new Vector2( 0, divCorrectionFactor );
                                next[x, y].Velocity += new Vector2( 0, divCorrectionFactor );
                            }
                            if( !wallRight )
                            {
                                // 1X
                                next[x + 1, y].Velocity -= new Vector2( divCorrectionFactor, 0 );
                                next[x, y].Velocity -= new Vector2( divCorrectionFactor, 0 );
                            }
                            if( !wallUp )
                            {
                                // 1Y
                                next[x, y + 1].Velocity -= new Vector2( 0, divCorrectionFactor );
                                next[x, y].Velocity -= new Vector2( 0, divCorrectionFactor );
                            }

                            next[x, y].Pressure += (divergence / s) * ((density * deltaX) / dt);
                        }
                    }
                }
            }

            public Cell GetAverage( float x, float y )
            {
                int iX0 = (int)x;
                int iY0 = (int)y;
                int iX1 = iX0 + 1;
                int iY1 = iY0 + 1;
                bool iX0Avail = iX0 >= 0 && iX0 < sizeX;
                bool iY0Avail = iY0 >= 0 && iY0 < sizeY;
                bool iX1Avail = iX1 >= 0 && iX1 < sizeX;
                bool iY1Avail = iY1 >= 0 && iY1 < sizeY;
                int availCount = 0;

                Cell total = new Cell();
                if( iX0Avail && iY0Avail )
                {
                    availCount++;
                    total.SmokeDensity += current[iX0, iY0].SmokeDensity;
                    total.Velocity += current[iX0, iY0].Velocity;
                }
                if( iX1Avail && iY0Avail )
                {
                    availCount++;
                    total.SmokeDensity += current[iX1, iY0].SmokeDensity;
                    total.Velocity += current[iX1, iY0].Velocity;
                }
                if( iX0Avail && iY1Avail )
                {
                    availCount++;
                    total.SmokeDensity += current[iX0, iY1].SmokeDensity;
                    total.Velocity += current[iX0, iY1].Velocity;
                }
                if( iX1Avail && iY1Avail )
                {
                    availCount++;
                    total.SmokeDensity += current[iX1, iY1].SmokeDensity;
                    total.Velocity += current[iX1, iY1].Velocity;
                }

                total.Velocity /= availCount;
                total.SmokeDensity /= availCount;
                return total;
            }

            private void Advect( float dt )
            {
                for( int x = 0; x < sizeX; x++ )
                {
                    for( int y = 0; y < sizeY; y++ )
                    {
                        Vector2 v = new Vector2( x, y ) - next[x, y].Velocity * dt;
                        Cell avg = GetAverage( v.X, v.Y );
                        next[x, y] = avg;
                    }
                }
            }

            char getChar( float val )
            {
                if( val < 0.1f ) return ' ';
                if( val < 0.2f ) return '.';
                if( val < 0.3f ) return '-';
                if( val < 0.4f ) return ':';
                if( val < 0.5f ) return '^';
                if( val < 0.6f ) return '*';
                if( val < 0.7f ) return '?';
                if( val < 0.8f ) return '&';
                if( val < 0.9f ) return '%';
                return '@';
            }

            public void Print()
            {
                Console.Clear();

                for( int x = 0; x < sizeX; x++ )
                {
                    for( int y = 0; y < sizeY; y++ )
                    {
                        Console.Write( getChar( current[x, y].Pressure ) );
                    }
                    Console.WriteLine();
                }
            }
        }
    }
}