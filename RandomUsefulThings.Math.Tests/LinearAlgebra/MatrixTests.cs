using NUnit.Framework;
using RandomUsefulThings.Math.LinearAlgebra;

namespace RandomUsefulThings.Math.Tests
{
    public class MatrixTests
    {
        [Test]
        public void Matrix_Eliminate()
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
        public void Matrix_Eliminate_0Pivot()
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
    }
}