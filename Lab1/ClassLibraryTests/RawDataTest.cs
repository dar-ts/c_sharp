using ClassLibrary1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibraryTests
{
    public class RawDataTest
    {
        [Fact]
        public void LengthTest()
        {   var n = 50;
            var rd = new RawData(new double[2] { -1, 2 }, n, false, RawData.Cube);
            Assert.Equal(n, rd.func_grid.Length);
            Assert.Equal(n, rd.grid.Length);
        }
    }
}
