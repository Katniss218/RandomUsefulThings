using System;
using System.Collections.Generic;
using System.Text;

namespace RandomUsefulThings.Physics.Trusses
{
    public class TrussSolver1
    {

        [Obsolete("Unconfirmed")]
        static void Solve( TrussGraph graph )
        {
            // Initialize the global stiffness matrix
            double[,] K = new double[3 * graph.Joints.Length, 3 * graph.Joints.Length];
            for( int i = 0; i < K.GetLength( 0 ); i++ )
            {
                for( int j = 0; j < K.GetLength( 1 ); j++ )
                {
                    K[i, j] = 0;
                }
            }

            // Assemble the global stiffness matrix
            foreach( var member in graph.Members )
            {
                // Calculate the direction cosines of the member
                double dx = graph.Joints[member.j].x - graph.Joints[member.i].x;
                double dy = graph.Joints[member.j].y - graph.Joints[member.i].y;
                double dz = graph.Joints[member.j].z - graph.Joints[member.i].z;
                double L = System.Math.Sqrt( dx * dx + dy * dy + dz * dz );
                double l = dx / L;
                double m = dy / L;
                double n = dz / L;

                // Calculate the member stiffness matrix
                double[,] k = new double[6, 6];
                k[0, 0] = l * l;
                k[0, 1] = l * m;
                k[1, 2] = l * n;
                k[2, 0] = k[1, 1];
                k[2, 1] = k[1, 2];
                k[2, 2] = m * m;
                k[3, 3] = l * l;
                k[3, 4] = l * m;
                k[3, 5] = l * n;
                k[4, 3] = k[3, 4];
                k[4, 4] = m * m;
                k[4, 5] = m * n;
                k[5, 3] = k[3, 5];
                k[5, 4] = k[4, 5];
                k[5, 5] = n * n;
                for( int i = 0; i < 6; i++ )
                {
                    for( int j = 0; j < 6; j++ )
                    {
                        k[i, j] *= member.axialLoad / L;
                    }
                }
                // Assemble the member stiffness matrix into the global stiffness matrix
                for( int i = 0; i < 3; i++ )
                {
                    for( int j = 0; j < 3; j++ )
                    {
                        K[3 * member.i + i, 3 * member.i + j] += k[i, j];
                        K[3 * member.i + i, 3 * member.j + j] -= k[i, j];
                        K[3 * member.j + i, 3 * member.i + j] -= k[i + 3, j];
                        K[3 * member.j + i, 3 * member.j + j] += k[i + 3, j + 3];
                    }
                }
            }

            // Solve the system of equations to find the forces at each joint
            double[,] F = new double[3 * graph.Joints.Length, 1];
            for( int i = 0; i < F.GetLength( 0 ); i++ )
            {
                F[i, 0] = 0;
            }
            F[3, 0] = -10; // Apply a 10 kN downward force to joint 3
            double[,] Kinv = MatrixInverse( K );
            double[,] U = MatrixMultiply( Kinv, F );
            for( int i = 0; i < graph.Joints.Length; i++ )
            {
                graph.Joints[i].fx = U[3 * i, 0];
                graph.Joints[i].fy = U[3 * i + 1, 0];
                graph.Joints[i].fz = U[3 * i + 2, 0];
            }
        }

        private static double[,] MatrixMultiply( double[,] kinv, double[,] f )
        {
            throw new NotImplementedException();
        }

        private static double[,] MatrixInverse( double[,] k )
        {
            throw new NotImplementedException();
        }
    }
}
