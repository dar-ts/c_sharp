using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1
{
  public struct SplineDataItem
    {
        public double x { get; set; }
        public double[] values;
    
        public double value { get; set; }
        public double first_der { get; set; }
        public SplineDataItem(double x, double[] values)
        {
            this.x = x;
            this.values = values;
            value = values[0];
            first_der = values[1];
        }
        public string ToLongString(string format)
        {
            return $"x={x.ToString(format)}, spline value = {values[0].ToString(format) }, " +
                $"first der = {values[1].ToString(format)}, second der = {values[2].ToString(format)}";
        }
        public override string ToString()
        {
            return $"x={x.ToString("F3")}, spline value = {values[0].ToString("F3")}, " +
                $"first der = {values[1].ToString("F3")}, second der = {values[2].ToString("F3")}";
        }
    }
}
