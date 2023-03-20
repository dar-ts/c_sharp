using ClassLibrary1;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        public double leftseg { get; set; }
        public double rightseg { get; set; }
        public int n_rd { get; set; } //число узлов сетки
        public bool type;//тип сетки
        public FRaw functype;
        public FRawEnum functype_enum;
        public FRawEnum FunctypeEnum { get { return functype_enum; } 
            set {
                functype = value switch
                {
                    FRawEnum.linear => ViewData.Linear,
                    FRawEnum.cube => ViewData.Cube,
                    FRawEnum.random => ViewData.Random,
                };
            } }
        public double left_der { get; set; }//левая производная
        public double right_der { get; set; }//правая производная
        public int n_sd { get; set; }




        public ViewData(double leftseg, double rightseg, int n_rd, bool type, FRawEnum functype_enum, double left_der, double right_der, int n_sd)
        {
             this.leftseg = leftseg;
            this.rightseg = rightseg;
            this.n_rd = n_rd;
            this.type = type;
            this.functype_enum = functype_enum;
             this.functype = functype_enum switch
            {
                FRawEnum.linear => ViewData.Linear,
                FRawEnum.cube => ViewData.Cube,
                FRawEnum.random => ViewData.Random,
            };

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
            double[] seg = new double[2] { leftseg, rightseg };
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
