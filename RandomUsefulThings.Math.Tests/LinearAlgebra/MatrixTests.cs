using NUnit.Framework;
using RandomUsefulThings.Math.LinearAlgebra;

namespace RandomUsefulThings.Math.Tests
{
    public class MatrixTests
    {
        [Test]
        public void Matrix3x3_Eliminate_Simple()
        {

            Matrix m = new Matrix( new double[,]
            {
                { 1, 2, 1 },
                { 3, 8, 1 },
                { 0, 4, 1 },
            } );

            Matrix U = m.Eliminate();

            Assert.IsTrue( U.Equals(new Matrix( new double[,]
            {
                { 1, 2, 1 },
                { 0, 2, -2 },
                { 0, 0, 5 },
            } ) ));
        }

        [Test]
        public void Matrix3x3_Eliminate0Pivot_ShouldSwap()
        {
            Matrix m = new Matrix( new double[,]
            {
                { 1, 2, 1 },
                { 3, 6, 1 },
                { 0, 4, 1 },
            } );

            Matrix U = m.Eliminate();

            Assert.IsTrue( U.Equals( new Matrix( new double[,]
            {
                { 1, 2, 1 },
                { 0, 4, 1 },
                { 0, 0, -2 },
            } ) ) );
        }


        [Test]
        public void Matrix3x3_MultiplyIdentity_ShouldNotChange()
        {
            // Multiplying with the identity matrix on either side shouldn't change the result.
            Matrix m1 = new Matrix( new double[,]
            {
                { 1, 2, 1 },
                { 3, 6, 1 },
                { 0, 4, 1 },
            } );

            Matrix m2 = Matrix.Identity(3);

            Matrix U1 = Matrix.Multiply( m1, m2 );
            Matrix U2 = Matrix.Multiply( m2, m1 );

            Assert.IsTrue( U1.Equals( m1 ) );
            Assert.IsTrue( U2.Equals( m1 ) );
        }


    }
}