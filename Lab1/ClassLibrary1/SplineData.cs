using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Net;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1
{
    public class SplineData
    {
        public RawData rd;
        public double left_der;
        public double right_der;
        public int n;
        public double integ;
        public List<SplineDataItem> list;
        public SplineData(RawData rd, double left_der, double right_der, int n)
        {
            this.rd = rd;
            this.left_der = left_der;
            this.right_der = right_der;
            this.n = n;
        }
        public void ComputeSpline()
        {
            int nx = rd.n;
            int ny = 1;
            double[] scoeff = new double[4 * ny * (nx - 1)];
            double[] values = rd.func_grid;

            int nsite = this.n;
            int ndorder = 3;
            int[] dorder = new int[3] { 1, 1, 1 };
            int nlim = 1;
            double[] llim = new double[1] { rd.seg[0] };
            double[] rlim = new double[1] { rd.seg[1] };
            int info = 0;
            double[] results = new double[nsite * ny * ndorder];
            double[] res_int = new double[nlim * ny];
            double[]ders = new double[2] { left_der, right_der };
           
            double[] raw_grid;
            double[] new_grid = rd.seg;
            if (rd.type == false) 
                raw_grid = rd.grid;
            else
                raw_grid = rd.seg;

            InterpolateSpline(nx, ny, raw_grid, rd.type, values,  ders, scoeff, nsite, new_grid, results, ndorder, dorder, nlim, llim, rlim, res_int, ref info);

                if (info == -1)
                {
                    throw new Exception("Spline Computing Unsuccessful");
                }
                int i = 0;
            list = new List<SplineDataItem>(nsite);
                for (int k = 0; k < ndorder*nsite; k+=3)
                {   
                    double[] values1 = new double[3];

                    values1[0] = results[k];
                    values1[1] = results[k+1];
                    values1[2] = results[k+2];
                    double x = new_grid[0] + ((new_grid[1] - new_grid[0]) / (nsite-1)) * i;
                    SplineDataItem sdi = new SplineDataItem(x, values1);
                    list.Add(sdi);
                    i++;
                }


                integ = res_int[0];
        }

        [DllImport("\\..\\..\\..\\..\\x64\\Debug\\DLL1.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void InterpolateSpline(int nx, int ny, double[] old_grid, bool type, double[] values, double[] ders, double[] scoeff, int nsite,
        double[] new_grid, double[] results, int norder, int[] dorder, int nlim, double[] llim, double[] rlim, double[] res_int, ref int info);
    }
    }
