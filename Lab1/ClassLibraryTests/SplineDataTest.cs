using ClassLibrary1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibraryTests
{
    public class SplineDataTest
    {
        [Theory]
        [InlineData(0, 0, 0)]
        [InlineData(-1, -2, 10)]
        [InlineData(0.033, 0.5, 70)]
        public void ConstructorTest(double left_der, double right_der, int n)
        {
            var rd = new RawData(new double[2] { -1, 2 }, n, false, RawData.Cube);
            var sd = new SplineData(rd, left_der, right_der, n);
            Assert.Equal(rd, sd.rd);
            Assert.Equal(left_der, sd.left_der);
            Assert.Equal(n, rd.n);
            Assert.Equal(right_der, sd.right_der);
        }
        [Theory]
        [InlineData(0, 0, 1)]
        [InlineData(-1.0, -1.0, 10)]
        [InlineData(10.0, 0.6, 70)]
        public void SplineTest(double left_der, double right_der, int n)
        {
            var rd = new RawData(new double[2] { -1, 2 }, 10, true, RawData.Cube);
            var sd = new SplineData(rd, left_der, right_der, n);
            sd.ComputeSpline();
            Assert.Equal(sd.list.Count, n);
            Assert.Equal(sd.list[0].first_der, left_der, 10);
            Assert.Equal(sd.list[sd.list.Count - 1].first_der, right_der, 10);

        }
    }
}
