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

namespace Wpf1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    { FRawEnum F { get; set; }
        bool type1 = true;
        SplineDataItem elem { get; set; }
        public MainWindow()
        {  
            InitializeComponent();
            FuncBox.ItemsSource = Enum.GetValues(typeof(FRawEnum));
            FuncBox.SelectedIndex = 0;
            F =(FRawEnum)FuncBox.SelectedItem;
            
        }

        private void SaveClick(object sender, RoutedEventArgs e)
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
                    int n_rd = Convert.ToInt32(RawNumber.Text);
                    double[] seg = new double[2];
                    seg[0] = Convert.ToDouble(LeftSeg.Text);
                    seg[1] = Convert.ToDouble(RightSeg.Text);
                    int n_sd = Convert.ToInt32(SplineNumber.Text);
                    double left_der = Convert.ToDouble(LeftDer.Text);
                    double right_der = Convert.ToDouble(RightDer.Text);
                    FRaw functype = F switch
                    {
                        FRawEnum.linear => ViewData.Linear,
                        FRawEnum.cube => ViewData.Cube,
                        FRawEnum.random => ViewData.Random,
                    };

                    ViewData vd = new ViewData(seg, n_rd, type1, functype, left_der, right_der, n_sd);
                    vd.CreateRawData();
                    vd.Save(filename);
                }
                catch { MessageBox.Show("Enter numbers"); }

            }
        }

        private void FileClick(object sender, RoutedEventArgs e)
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
                    int n_sd = Convert.ToInt32(SplineNumber.Text);
                    double left_der = Convert.ToDouble(LeftDer.Text);
                    double right_der = Convert.ToDouble(RightDer.Text);
               
                    ViewData vd = new ViewData(left_der, right_der, n_sd);
                    vd.rd = vd.Load(filename);
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
                }
                catch { MessageBox.Show("Enter numbers"); }

            }
        }

        private void ControlsClick(object sender, RoutedEventArgs e)
        {
            try
            {
                int n_rd = Convert.ToInt32(RawNumber.Text);
                double[] seg = new double[2];
                seg[0] = Convert.ToDouble(LeftSeg.Text);
                seg[1] = Convert.ToDouble(RightSeg.Text);
                int n_sd = Convert.ToInt32(SplineNumber.Text);
                double left_der = Convert.ToDouble(LeftDer.Text);
                double right_der = Convert.ToDouble(RightDer.Text);
                
                FRaw functype = F switch
                {
                    FRawEnum.linear => ViewData.Linear,
                    FRawEnum.cube => ViewData.Cube,
                    FRawEnum.random => ViewData.Random,
                };

                ViewData vd = new ViewData(seg, n_rd, type1, functype, left_der, right_der, n_sd);
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
                
            }
            catch (Exception ex) { MessageBox.Show(Convert.ToString(ex)); }
        }

        private void NonUniformGrid_Checked(object sender, RoutedEventArgs e)
        {
            type1 = false;
        }

        private void UniformGrid_Checked(object sender, RoutedEventArgs e)
        {
            type1 = true;
        }

        private void FuncBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (FuncBox.SelectedItem != null)
            {
                F = (FRawEnum)FuncBox.SelectedItem;
            }
        }

        private void SplineDataList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SplineDataList.SelectedItem != null)
            {
                elem = (SplineDataItem)SplineDataList.SelectedItem;
                SplineElemBlock.Text = elem.ToLongString("F3");
            }
        }
    }
}
