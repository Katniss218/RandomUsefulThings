using System;
using System.Collections.Generic;
using System.Text;

namespace RandomUsefulThings.Physics.FluidSim
{
    [Obsolete("doesn't seem to work")]
    public class Euler2DReference
    {
        // https://www.youtube.com/watch?v=iKAVRgIrUOU

        public enum Field
        {
            U_FIELD,
            V_FIELD,
            S_FIELD
        }

        public float overRelaxation = 1.9f;

        public float density;
        public int
            numX,
            numY;
        public int numCells;
        public float h;
        public float[] u;
        public float[] v;
        public float[] newU;
        public float[] newV;
        public float[] p;
        public float[] s;
        public float[] m;
        public float[] newM;

        public Euler2DReference( float density, int numX, int numY, float h )
        {
            this.density = density;
            this.numX = numX + 2;
            this.numY = numY + 2;
            this.numCells = this.numX * this.numY;
            this.h = h;
            this.u = new float[this.numCells];
            this.v = new float[this.numCells];
            this.newU = new float[this.numCells];
            this.newV = new float[this.numCells];
            this.p = new float[this.numCells];
            this.s = new float[this.numCells];
            this.m = new float[this.numCells];
            this.newM = new float[this.numCells];

            for( int i = 0; i < this.m.Length; i++ )
                this.m[i] = 1.0f;

            for( int i = 0; i < this.s.Length; i++ )
                this.s[i] = 1.0f;
        }

        public void Integrate( float dt, float gravity )
        {
            var n = this.numY;
            for( var i = 1; i < this.numX; i++ )
            {
                for( var j = 1; j < this.numY - 1; j++ )
                {
                    if( this.s[i * n + j] != 0.0f && this.s[i * n + j - 1] != 0.0f )
                        this.v[i * n + j] += gravity * dt;
                }
            }
        }

        public void SolveIncompressibility( int numIters, float dt )
        {
            int n = this.numY;
            float cp = this.density * this.h / dt;

            for( var iter = 0; iter < numIters; iter++ )
            {
                for( var i = 1; i < this.numX - 1; i++ )
                {
                    for( var j = 1; j < this.numY - 1; j++ )
                    {
                        if( this.s[i * n + j] == 0.0f )
                            continue;

                        float sx0 = this.s[(i - 1) * n + j];
                        float sx1 = this.s[(i + 1) * n + j];
                        float sy0 = this.s[i * n + j - 1];
                        float sy1 = this.s[i * n + j + 1];

                        float s = sx0 + sx1 + sy0 + sy1;
                        if( s == 0.0 )
                            continue;

                        float div = this.u[(i + 1) * n + j] - this.u[i * n + j] +
                            this.v[i * n + j + 1] - this.v[i * n + j];

                        float p = -div / s;
                        p *= overRelaxation;
                        this.p[i * n + j] += cp * p;

                        this.u[i * n + j] -= sx0 * p;
                        this.u[(i + 1) * n + j] += sx1 * p;
                        this.v[i * n + j] -= sy0 * p;
                        this.v[i * n + j + 1] += sy1 * p;
                    }
                }
            }
        }

        public void Extrapolate()
        {
            var n = this.numY;
            for( var i = 0; i < this.numX; i++ )
            {
                this.u[i * n + 0] = this.u[i * n + 1];
                this.u[i * n + this.numY - 1] = this.u[i * n + this.numY - 2];
            }
            for( var j = 0; j < this.numY; j++ )
            {
                this.v[0 * n + j] = this.v[1 * n + j];
                this.v[(this.numX - 1) * n + j] = this.v[(this.numX - 2) * n + j];
            }
        }

        public float SampleField( float x, float y, Field field )
        {
            var n = this.numY;
            var h = this.h;
            float h1 = 1.0f / h;
            float h2 = 0.5f * h;

            x = System.Math.Max( System.Math.Min( x, this.numX * h ), h );
            y = System.Math.Max( System.Math.Min( y, this.numY * h ), h );

            float dx = 0.0f;
            float dy = 0.0f;

            float[] f;

            switch( field )
            {
                case Field.U_FIELD:
                    f = this.u;
                    dy = h2;
                    break;
                case Field.V_FIELD:
                    f = this.v;
                    dx = h2;
                    break;
                case Field.S_FIELD:
                    f = this.m;
                    dx = h2; dy = h2;
                    break;
                default:
                    throw new InvalidOperationException();
            }

            int x0 = System.Math.Min( (int)System.Math.Floor( (x - dx) * h1 ), this.numX - 1 );
            float tx = ((x - dx) - x0 * h) * h1;
            int x1 = System.Math.Min( x0 + 1, this.numX - 1 );

            int y0 = System.Math.Min( (int)System.Math.Floor( (y - dy) * h1 ), this.numY - 1 );
            float ty = ((y - dy) - y0 * h) * h1;
            int y1 = System.Math.Min( y0 + 1, this.numY - 1 );

            float sx = 1.0f - tx;
            float sy = 1.0f - ty;

            float val = sx * sy * f[x0 * n + y0] +
                tx * sy * f[x1 * n + y0] +
                tx * ty * f[x1 * n + y1] +
                sx * ty * f[x0 * n + y1];

            return val;
        }

        public float AvgU( int i, int j )
        {
            int n = this.numY;

            float u = (this.u[i * n + j - 1] + this.u[i * n + j] +
                this.u[(i + 1) * n + j - 1] + this.u[(i + 1) * n + j]) * 0.25f;

            return u;

        }

        public float AvgV( int i, int j )
        {
            int n = this.numY;

            float v = (this.v[(i - 1) * n + j] + this.v[i * n + j] +
                this.v[(i - 1) * n + j + 1] + this.v[i * n + j + 1]) * 0.25f;

            return v;
        }

        public void AdvectVel( float dt )
        {
            Array.Copy( this.u, this.newU, 0 );
            Array.Copy( this.v, this.newV, 0 );

            int n = this.numY;
            float h = this.h;
            float h2 = 0.5f * h;

            for( var i = 1; i < this.numX; i++ )
            {
                for( var j = 1; j < this.numY; j++ )
                {
                    // u component
                    if( this.s[i * n + j] != 0.0f && this.s[(i - 1) * n + j] != 0.0f && j < this.numY - 1 )
                    {
                        float x = i * h;
                        float y = j * h + h2;
                        float u = this.u[i * n + j];
                        float v = this.AvgV( i, j );
                        //						var v = this.sampleField(x,y, V_FIELD);
                        x = x - dt * u;
                        y = y - dt * v;
                        u = this.SampleField( x, y, Field.U_FIELD );
                        this.newU[i * n + j] = u;
                    }
                    // v component
                    if( this.s[i * n + j] != 0.0f && this.s[i * n + j - 1] != 0.0f && i < this.numX - 1 )
                    {
                        float x = i * h + h2;
                        float y = j * h;
                        float u = this.AvgU( i, j );
                        //						var u = this.sampleField(x,y, U_FIELD);
                        float v = this.v[i * n + j];
                        x = x - dt * u;
                        y = y - dt * v;
                        v = this.SampleField( x, y, Field.V_FIELD );
                        this.newV[i * n + j] = v;
                    }
                }
            }

            Array.Copy( this.newU, this.u, 0 );
            Array.Copy( this.newV, this.v, 0 );
        }

        public void AdvectSmoke( float dt )
        {
            Array.Copy( this.m, this.newM, 0 );

            int n = this.numY;
            var h = this.h;
            var h2 = 0.5f * h;

            for( var i = 1; i < this.numX - 1; i++ )
            {
                for( var j = 1; j < this.numY - 1; j++ )
                {
                    if( this.s[i * n + j] != 0.0f )
                    {
                        float u = (this.u[i * n + j] + this.u[(i + 1) * n + j]) * 0.5f;
                        float v = (this.v[i * n + j] + this.v[i * n + j + 1]) * 0.5f;
                        float x = i * h + h2 - dt * u;
                        float y = j * h + h2 - dt * v;

                        this.newM[i * n + j] = this.SampleField( x, y, Field.S_FIELD );
                    }
                }
            }

            Array.Copy( this.newM, this.m, 0 );
        }

        // ----------------- end of simulator ------------------------------

        public void Simulate( float dt, float gravity, int numIters )
        {
            this.Integrate( dt, gravity );

            for( int i = 0; i < this.p.Length; i++ )
                this.p[i] = 0.0f;

            this.SolveIncompressibility( numIters, dt );

            this.Extrapolate();
            this.AdvectVel( dt );
            this.AdvectSmoke( dt );
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
            int n = this.numY;

            for( int i = 0; i < numX; i++ )
            {
                for( int j = 0; j < numY; j++ )
                {
                    Console.Write( getChar( p[i * n + j] ) );
                }
                Console.WriteLine();
            }
        }
    }
}