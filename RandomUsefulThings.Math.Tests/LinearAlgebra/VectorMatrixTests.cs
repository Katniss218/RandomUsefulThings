using NUnit.Framework;
using RandomUsefulThings.Math.LinearAlgebra;

namespace RandomUsefulThings.Math.Tests.LinearAlgebra
{
    public class VectorMatrixTests
    {
        [Test]
        public void Multiply_Vector3_with_Matrix3x3()
        {
            Matrix m = new Matrix( new double[,]
            {
                { 2, -1, 0 },
                { -1, 2, -1 },
                { 0, -3, 4 }
            } );
            Vector v = new Vector( new double[]
                { 0, -1, 4 } );

            Vector vr = Matrix.Multiply( m, v );

            Assert.IsTrue( vr.Equals( new Vector( new double[] { 1, -6, 19 } ) ) );
        }

        [Test]
        public void Multiply_Vector3_with_Matrix3x2()
        {
            Matrix m = new Matrix( new double[,]
            {
                { 1, -1, 2 },
                { 0, -3, 1 }
            } );
            Vector v = new Vector( new double[]
                { 2, 1, 0 } );

            Vector vr = Matrix.Multiply( m, v );

            Assert.IsTrue( vr.Equals( new Vector( new double[] { 1, -3 } ) ) );
        }
    }
}