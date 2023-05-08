using ClassLibrary1;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibraryTests
{
    public class SplineDataItemTest
    {
        [Theory]
        [InlineData(0, new double[2] { 0, 0 })]
        [InlineData(1000, new double[2] { 0.00100, 10000 })]
        [InlineData(60, new double[2] {10, 20})]
        public void ConstructorTest(double x, double[] values)
        {
            
            var sdi = new SplineDataItem(x, values);
            Assert.Equal(x, sdi.x);
            Assert.Equal(values[0], sdi.value);
            Assert.Equal(values[1], sdi.first_der);
        } 
       
    }
}
