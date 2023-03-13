using ClassLibrary1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Wpf1
{
    public class ViewData
    {
        public RawData rd;
        public SplineData sd;
        public double[] seg; //границы отрезка
        public int n_rd;//число узлов сетки
        public bool type;//тип сетки
        public FRaw functype;
        public double left_der;//левая производная
        public double right_der;//правая производная
        public int n_sd;

        public ViewData(double[] seg, int n_rd, bool type, FRaw functype, double left_der, double right_der, int n_sd)
        {
          
            this.seg = seg;
            this.n_rd = n_rd;
            this.type = type;
            this.functype = functype;
            this.left_der = left_der;
            this.right_der = right_der;
            this.n_sd = n_sd;
        }
        public ViewData(double left_der, double right_der, int n_sd)
        {
            this.left_der = left_der;
            this.right_der = right_der;
            this.n_sd = n_sd;
        }
        public static double Random(double x)
        {
            return RawData.Random(x);
        }
        public static  double Linear(double x)
        {
            return RawData.Linear(x);
        }
        public static double Cube(double x)
        {
            return RawData.Cube(x);
        }
        public void CreateRawData()
        {
            rd = new RawData(seg, n_rd, type, functype);
        }
        public void CreateSplineData()
        {
            sd = new SplineData(rd, left_der, right_der, n_sd);
        }
        public void ComputeSpline() {
            sd.ComputeSpline();
        }

        public void Save(string filename)
        {
            try
            {
                rd.Save(filename);
            }
            catch(Exception ex) { MessageBox.Show(Convert.ToString(ex)); }
        }
        public RawData Load(string filename)
        {
            try
            {  
               RawData.Load(filename,out rd);
                return rd;
            }
            catch (Exception ex) { MessageBox.Show(Convert.ToString(ex)); return null; }
        }
    }
}
