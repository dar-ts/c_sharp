using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ClassLibrary1;
using OxyPlot;

namespace Wpf1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
   
    public partial class MainWindow : Window
    {
        bool type1 = true;
        private int _errors = 0;
        ViewData vd = new ViewData(1, 5, 5, true, FRawEnum.linear, 1, 1, 5);
      
        public static RoutedCommand ControlsCommand = new RoutedCommand("ControlsCommand", typeof(Wpf1.MainWindow));
        public static RoutedCommand FileCommand = new RoutedCommand("FileCommand", typeof(Wpf1.MainWindow));
        OxyPlotModel oxyPlotMod;
        public MainWindow()
        {  
            InitializeComponent();
            FuncBox.ItemsSource = Enum.GetValues(typeof(FRawEnum));
            FuncBox.SelectedIndex = 0;
            this.DataContext = vd;
            
        }

       

        private void FileClick(object sender, RoutedEventArgs e)
        {
            FileCommand.Execute(sender, null);
        }

      
        private void NonUniformGrid_Checked(object sender, RoutedEventArgs e)
        {
            type1 = false;
            vd.type = type1;
        }

        private void UniformGrid_Checked(object sender, RoutedEventArgs e)
        {
            type1 = true;
            vd.type = type1;
        }

        private void CanControlsCommandHandler(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = (_errors == 0);
            
        }
        private void ControlsCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                vd.CreateRawData();
                string[] rawdatalist = new string[vd.rd.grid.Length];
                for (int i = 0; i < vd.rd.grid.Length; i++)
                {
                    rawdatalist[i] = $"x = {vd.rd.grid[i].ToString("F3")}, value = {vd.rd.func_grid[i].ToString("F3")}";
                }
                RawDataList.ItemsSource = rawdatalist;

                vd.CreateSplineData();
                vd.ComputeSpline();
                SplineDataList.ItemsSource = vd.sd.list;

                IntegralBlock.Text = (vd.sd.integ.ToString("F3"));
                oxyPlotMod = new OxyPlotModel(vd.rd, vd.sd);
                OxyPlot.DataContext = oxyPlotMod;


            }
            catch (Exception ex) { MessageBox.Show(Convert.ToString(ex)); }

        }
        private void CanFileCommandHandler(object sender, CanExecuteRoutedEventArgs e)
        {
            
            var dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.FileName = "rawdata";
            dialog.DefaultExt = ".txt";
            dialog.Filter = "Text documents (.txt)|*.txt";

            bool? result = dialog.ShowDialog();

            if (result == true)
            {
                string filename = dialog.FileName;
                try
                {
                    vd.rd = vd.Load(filename);
                    e.CanExecute = (vd.rd.n>=2) && (vd.rd.seg[0] < vd.rd.seg[1]) && (vd.rd != null);
                    if (e.CanExecute == false) MessageBox.Show("Ошибочные данные в файле");


                }
                catch (Exception ex) { e.CanExecute = false; MessageBox.Show(Convert.ToString(ex)); }

            }
        }
       private void FileCommandHandler(object sender, ExecutedRoutedEventArgs e)
       {
            vd.CreateSplineData();
            vd.ComputeSpline();
            SplineDataList.ItemsSource = vd.sd.list;
            string[] rawdatalist = new string[vd.rd.grid.Length];
            for (int i = 0; i < vd.rd.grid.Length; i++)
            {
                rawdatalist[i] = $"x = {vd.rd.grid[i].ToString("F3")}, value = {vd.rd.func_grid[i].ToString("F3")}";
            }
            RawDataList.ItemsSource = rawdatalist;

            IntegralBlock.Text = (vd.sd.integ.ToString("F3"));
            oxyPlotMod = new OxyPlotModel(vd.rd, vd.sd);
            OxyPlot.DataContext = oxyPlotMod;
        }
        private void CanSaveCommandHandler(object sender, CanExecuteRoutedEventArgs e)
        {
            vd.CreateRawData();
            e.CanExecute = (_errors == 0) && (vd.rd != null);

        }
        private void SaveCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            var dialog = new Microsoft.Win32.SaveFileDialog();
            dialog.FileName = "rawdata";
            dialog.DefaultExt = ".txt";
            dialog.Filter = "Text documents (.txt)|*.txt";

            bool? result = dialog.ShowDialog();


            if (result == true)
            {

                string filename = dialog.FileName;
                try
                {
                    
                    vd.Save(filename);
                }
                catch (Exception ex) { MessageBox.Show(Convert.ToString(ex)); }

            }
        }

        private void Validation_Error(object sender, ValidationErrorEventArgs e)
        {
            if (e.Action == ValidationErrorEventAction.Added)
                _errors++;
            else
                _errors--;
        }
    }
}
