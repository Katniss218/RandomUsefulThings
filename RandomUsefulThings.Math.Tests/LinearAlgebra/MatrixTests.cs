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

            Assert.IsTrue( U.Equals( new Matrix( new double[,]
            {
                { 1, 2, 1 },
                { 0, 2, -2 },
                { 0, 0, 5 },
            } ) ) );
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
        public void Matrix3x3_EliminateAugumented()
        {
            Matrix m = new Matrix( new double[,]
            {
                { 1, 3, 1, 9 },
                { 1, 1, -1, 1 },
                { 3, 11, 5, 35 },
            } );

            Matrix U = m.Eliminate();

            Assert.IsTrue( U.Equals( new Matrix( new double[,]
            {
                { 1, 3, 1, 9 },
                { 0, -2, -2, -8 },
                { 0, 0, 0, 0 },
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

            Matrix m2 = Matrix.Identity( 3 );

            Matrix U1 = Matrix.Multiply( m1, m2 );
            Matrix U2 = Matrix.Multiply( m2, m1 );

            Assert.IsTrue( U1.Equals( m1 ) );
            Assert.IsTrue( U2.Equals( m1 ) );
        }

        [Test]
        public void Matrix3x3_LinearCombination()
        {
            Matrix m1 = new Matrix( new double[,]
            {
                { 1, 2, 1 },
                { 3, 6, 1 },
                { 0, 4, 1 },
            } );
            Matrix m2 = new Matrix( new double[,]
            {
                { 1, 0, 0 },
                { 0, 1, 0 },
                { 0, 0, 1 },
            } );

            Matrix U = Matrix.LinearCombination( new[] { (2.0, m1), (100, m2) } );

            Assert.IsTrue( U.Equals( new Matrix( new double[,]
            {
                { 102, 4, 2 },
                { 6, 112, 2 },
                { 0, 8, 102 },
            } ) ) );
        }


    }
}