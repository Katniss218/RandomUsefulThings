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
    }
}