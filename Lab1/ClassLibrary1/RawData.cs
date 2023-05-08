using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1
{
    public class RawData
    {
        public double[] seg; //границы отрезка
        public int n;//число узлов сетки
        public bool type;//
        public FRaw functype;
        public double[] grid;//массив узлов сетки
        public double[] func_grid;//массив значений поля сетки
        public string filename;
        public RawData(double[] seg, int n, bool type, FRaw functype)
        {
            this.seg = seg;
            this.n = n;
            this.type = type;
            this.functype = functype;
            grid = new double[this.n];
            func_grid= new double[this.n];
            double x;
            for(int i = 0; i <n; i++)
            {
                if (this.type == true)
                    x = seg[0] + ((seg[1] - seg[0]) / (n - 1)) * i;
                else
                {
                    Random x1 = new Random();

                    if (i == 0)
                        x = seg[0];
                    else if (i == (n-1))
                        x = seg[1];
                    else
                       
                        x = grid[i-1] + ((seg[1] - seg[0]) / (n - 1)) * x1.NextDouble();
                }
                grid[i] = x;
                func_grid[i] = functype(x);

            }
        
        }

        public RawData(string filename) { 
        this.filename = filename;
        }
        public static double Linear(double x) { return 2 * x; }
        public static double Cube(double x) { return 3 * x*x*x+ x*x; }
        public static double Random(double x) {
            Random x1 = new Random();
      
            double rnd = x *x1.NextDouble();
            return rnd; }

        public bool Save(string filename)
        {
            try
            {

                using (BinaryWriter writer = new BinaryWriter(File.Open(filename, FileMode.OpenOrCreate), Encoding.UTF8))
                {

                    writer.Write(this.seg[0]);
                    writer.Write(this.seg[1]);
                    writer.Write(this.n);
                    writer.Write(this.type);
                    
                    
                    for (int i = 0; i < this.grid.Length; i++)
                    {
                        writer.Write(this.grid[i]);
                        writer.Write(this.func_grid[i]);
                    }
                    
                    return true;
                }
            }
            catch (Exception ex)
            { throw new Exception(ex.Message); }
           
        }
        

        static public bool Load(string filename, out RawData rd)
        {
            try
            {
                using (BinaryReader reader = new BinaryReader(File.Open(filename, FileMode.Open), Encoding.UTF8))
                {
                    
                    double[] seg1 = new double[2];
                    seg1[0] = reader.ReadDouble();
                    seg1[1] = reader.ReadDouble();
                    int n1 = reader.ReadInt32();
                    bool type1 = reader.ReadBoolean();
                    Console.WriteLine(Convert.ToString(seg1[0]));
                    rd = new RawData(seg1, n1, type1, Linear);
                    double[] grid = new double[rd.n];
                    double[] func_grid = new double[rd.n];

                    for (int i = 0; i < n1; i++)
                    {
                        grid[i] = reader.ReadDouble();
                        func_grid[i] = reader.ReadDouble();
                  
                    }
                    rd.grid = grid;
                    rd.func_grid = func_grid;
                   
                    return true;
                }
            }
            catch (Exception ex)
            { throw new Exception(ex.Message);
                 }
        }
    }
}
