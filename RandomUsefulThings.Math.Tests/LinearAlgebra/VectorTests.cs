using NUnit.Framework;
using RandomUsefulThings.Math.LinearAlgebra;

namespace RandomUsefulThings.Math.Tests
{
    public class VectorTests
    {
        [Test]
        public void Vector3_Magnitude()
        {
            // use a pythagorean triple.
            Vector v = new Vector( new double[]
                { 3, 4 } );

            double magn = v.GetMagnitude();

            Assert.IsTrue( magn == 5 );
        }

        [Test]
        public void Vector3_LinearCombination()
        {
            Vector v1 = new Vector( new double[] { 2, 4, 8 } );
            Vector v2 = new Vector( new double[] { 100, 1, 1 } );

            Vector U = Vector.LinearCombination( new[] { (2.0, v1), (100, v2) } );

            Assert.IsTrue( U.Equals( new Vector( new double[]

                { 10004, 108, 116 }
            ) ) );
        }
    }
}