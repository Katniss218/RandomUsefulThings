using NUnit.Framework;
using RandomUsefulThings.Math.LinearAlgebra.NumericalMethods;
using RandomUsefulThings.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RandomUsefulThings.Math.Tests.LinearAlgebra.NumericalMethods
{
    public class SuccessiveOverRelaxationTests
    {
        [Test]
        public void Solve2_2x2()
        {

            double[,] M = new double[,]
            {
                { 2, 1 },
                { 1, 1 },
            };
            double[] v = new double[]
                { 1, 2 };

            double[] s = SuccessiveOverRelaxation.Solve2( M, v, 1.5 );

            Assert.IsTrue( s.SequenceEqual( new double[] { -1, 3 }, new DelegateEqualityComparer<double>( ( l, r ) => System.Math.Abs( l - r ) < 0.001f ) ) );
        }

        [Test]
        public void SolveFastIsh()
        {

            double[,] M = new double[,]
            {
                { 2, 1 },
                { 1, 1 },
            };
            double[] v = new double[]
                { 1, 2 };

            double[] s = SuccessiveOverRelaxation.SolveFastIsh( M, v, 1.5 );

            Assert.IsTrue( s.SequenceEqual( new double[] { -1, 3 }, new DelegateEqualityComparer<double>( ( l, r ) => System.Math.Abs( l - r ) < 0.001f ) ) );
        }
    }
}