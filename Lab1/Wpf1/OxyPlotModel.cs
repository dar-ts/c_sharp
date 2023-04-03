using OxyPlot.Legends;
using OxyPlot.Series;
using OxyPlot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;
using ClassLibrary1;
using System.Windows.Controls;

namespace Wpf1
{
    class OxyPlotModel
    {
        RawData rd;
        SplineData sd;
        public PlotModel plotModel { get; private set; }
        public OxyPlotModel(RawData rd, SplineData sd)
        {
            this.rd = rd;
            this.sd = sd;
            this.plotModel = new PlotModel { Title = "Raw and Spline Data" };
            this.AddSeries();
        }

        public void AddSeries()
        {
            this.plotModel.Series.Clear();
           
            OxyColor color_rd =  OxyColors.Green;
            LineSeries lineSeries_rd = new LineSeries();
            for (int j = 0; j < rd.grid.Length; j++) lineSeries_rd.Points.Add(new DataPoint(rd.grid[j], rd.func_grid[j]));
            lineSeries_rd.Color = color_rd;

            lineSeries_rd.MarkerType = MarkerType.Circle;
            lineSeries_rd.MarkerSize = 4;
            lineSeries_rd.MarkerStroke = color_rd;
            lineSeries_rd.MarkerFill = color_rd;
            lineSeries_rd.Title ="Raw Data";

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
