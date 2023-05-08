using ClassLibrary1;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO.Enumeration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using OxyPlot;
using OxyPlot.Legends;
using OxyPlot.Series;

namespace ViewModel2
{
    public enum FRawEnum
    {
        linear,
        cube,
        random
    }
  
    public interface IUIServices
    {
        string GetFileName();
        void ReportError(string message);
    }
    public class ViewData :  ViewModelBase, IDataErrorInfo
    {
        
        public string Error { get; }
        public string this[string property]
        {
            get
            {
                string msg = null;
                switch (property)
                {
                    case "n_rd":
                        if (n_rd < 2) msg = "Raw Data nodes' number must be greater than 1.";
                        break;

                    case "n_sd":
                        if (n_sd <= 2) msg = "Spline Data nodes' number must be greater than 2.";
                        break;
                    case "leftseg":
                        if (leftseg >= rightseg) msg = "The right end of the segment should be greater than the left one.";
                        break;
                    case "rightseg":
                        if (leftseg >= rightseg) msg = "The right end of the segment should be greater than the left one.";
                        break;


                }
                return msg;
            }
        }


        public RawData rd;
        public SplineData sd;
        public PlotModel plotModel { get; set; }

        public List<SplineDataItem> splinedatalist { get; set; } = new List<SplineDataItem>();
        public string[] rawdatalist { get; set; } = { "" };
        public double leftseg { get; set; }
        public double rightseg { get; set; }
        public int n_rd { get; set; } //число узлов сетки
        public bool type;//тип сетки
        public FRaw functype;
        public FRawEnum functype_enum;
        public FRawEnum FunctypeEnum
        {
            get { return functype_enum; }
            set
            {
                functype = value switch
                {
                    FRawEnum.linear => ViewData.Linear,
                    FRawEnum.cube => ViewData.Cube,
                    FRawEnum.random => ViewData.Random,
                };
            }
        }
        public double left_der { get; set; }//левая производная
        public double right_der { get; set; }//правая производная
        public int n_sd { get; set; }
        public string integral { get; set; } = "";
  
        private readonly IUIServices uiServices;
        public ICommand ControlsCommand { get; private set; }
        public ICommand FileCommand { get; private set; }
        public ICommand SaveCommand { get; private set; }
        public ICommand  UniformGridCommand { get; private set; }
        public ICommand NonUniformGridCommand { get; private set; }

        public ViewData(IUIServices uiServices, double leftseg = 1.0, double rightseg = 5.0, int n_rd = 5, bool type = true, FRawEnum functype_enum = FRawEnum.linear, double left_der = 1, double right_der = 1, int n_sd = 5)
        {
            this.uiServices = uiServices;

            ControlsCommand = new RelayCommand(_ => { OnControls(); },
                _ =>
                {
                    return this["n_rd"] == null && this["n_sd"] == null && this["leftseg"] == null && this["rightseg"] == null;
                });

            FileCommand = new RelayCommand(_ => OnFile());

            SaveCommand = new RelayCommand(_ => { OnSave(); },
                _ =>
                {
                    return rd != null && this["n_rd"] == null && this["n_sd"] == null && this["leftseg"] == null && this["rightseg"] == null;
                });
            UniformGridCommand = new RelayCommand(_ => OnUniform());
            NonUniformGridCommand = new RelayCommand(_ => OnNonUniform());

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
        private void OnControls()
        {
            try
            {
                CreateRawData();
                rawdatalist = new string[rd.grid.Length];
                for (int i = 0; i < rd.grid.Length; i++)
                {
                    rawdatalist[i] = $"x = {rd.grid[i].ToString("F3")}, value = {rd.func_grid[i].ToString("F3")}";
                }

                CreateSplineData();
                ComputeSpline();
                splinedatalist = sd.list;

                integral = sd.integ.ToString("F3");
                plotModel = new PlotModel { Title = "Raw and Spline Data" };
                AddSeries();
                RaisePropertyChanged(nameof(rawdatalist));
                RaisePropertyChanged(nameof(splinedatalist));
                RaisePropertyChanged(nameof(integral));
                RaisePropertyChanged(nameof(plotModel));

            }
            catch (Exception ex) { uiServices.ReportError(ex.Message); }
        }
        private void OnFile()
        {
            try
            {
                var filename = uiServices.GetFileName();
                if (filename == null) uiServices.ReportError("Путь не должен быть пустым");
                else
                {
                    rd = Load(filename);
                    var check = (rd.n >= 2) && (rd.seg[0] < rd.seg[1]) && (rd != null);
                    if (check == false) uiServices.ReportError("Ошибочные данные в файле");

                    CreateSplineData();
                    ComputeSpline();
                    splinedatalist = sd.list;
                    rawdatalist = new string[rd.grid.Length];
                    for (int i = 0; i < rd.grid.Length; i++)
                    {
                        rawdatalist[i] = $"x = {rd.grid[i].ToString("F3")}, value = {rd.func_grid[i].ToString("F3")}";
                    }
                    integral = sd.integ.ToString("F3");
                    plotModel = new PlotModel { Title = "Raw and Spline Data" };
                    AddSeries();
                    RaisePropertyChanged(nameof(rawdatalist));
                    RaisePropertyChanged(nameof(splinedatalist));
                    RaisePropertyChanged(nameof(integral));
                    RaisePropertyChanged(nameof(plotModel));
                }
            }
            catch (Exception ex) { uiServices.ReportError(ex.Message); }
        }
        private void OnSave()
        {
            try
            {
                var filename = uiServices.GetFileName();
                if (filename == null) uiServices.ReportError("Путь не должен быть пустым");
                else if (rd != null) Save(filename);
     
                }
            catch (Exception ex) { uiServices.ReportError(ex.Message); }
        }

        private void OnUniform()
        {
            type = true;
        }
        private void OnNonUniform()
        {
            type = false;
        }

        public static double Random(double x)
        {
            return RawData.Random(x);
        }
        public static double Linear(double x)
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
        public void ComputeSpline()
        {
            sd.ComputeSpline();
        }

        public void Save(string filename)
        {
            try
            {
                rd.Save(filename);
            }
            catch (Exception ex) {
                uiServices.ReportError(ex.Message);}
        }
        public RawData Load(string filename)
        {
            try
            {
                RawData.Load(filename, out rd);

                return rd;
            }
            catch (Exception ex) { uiServices.ReportError(ex.Message); return null; }
        }
        public void AddSeries()
        {
            this.plotModel.Series.Clear();

            OxyColor color_rd = OxyColors.Green;
            LineSeries lineSeries_rd = new LineSeries();
            for (int j = 0; j < rd.grid.Length; j++) lineSeries_rd.Points.Add(new DataPoint(rd.grid[j], rd.func_grid[j]));
            lineSeries_rd.Color = color_rd;

            lineSeries_rd.MarkerType = MarkerType.Circle;
            lineSeries_rd.MarkerSize = 4;
            lineSeries_rd.MarkerStroke = color_rd;
            lineSeries_rd.MarkerFill = color_rd;
            lineSeries_rd.Title = "Raw Data";

            Legend legend_rd = new Legend();
            plotModel.Legends.Add(legend_rd);
            this.plotModel.Series.Add(lineSeries_rd);

            OxyColor color_sd = OxyColors.Blue;
            LineSeries lineSeries_sd = new LineSeries();
            for (int j = 0; j < sd.list.Count; j++) lineSeries_sd.Points.Add(new DataPoint(sd.list[j].x, sd.list[j].value));
            lineSeries_sd.Color = color_sd;

            lineSeries_sd.MarkerType = MarkerType.Circle;
            lineSeries_sd.MarkerSize = 4;
            lineSeries_sd.MarkerStroke = color_sd;
            lineSeries_sd.MarkerFill = color_sd;
            lineSeries_sd.Title = "Spline Data";

            Legend legend_sd = new Legend();
            plotModel.Legends.Add(legend_sd);
            this.plotModel.Series.Add(lineSeries_sd);

        }
    }
}
