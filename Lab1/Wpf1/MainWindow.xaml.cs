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
    {
        bool type1 = true;
        
        ViewData vd = new ViewData(1, 5, 5, true, FRawEnum.linear, 1, 1, 5);

        public MainWindow()
        {  
            InitializeComponent();
            FuncBox.ItemsSource = Enum.GetValues(typeof(FRawEnum));
            FuncBox.SelectedIndex = 0;
            this.DataContext = vd;
            
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
                try { 
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
            vd.type = type1;
        }

        private void UniformGrid_Checked(object sender, RoutedEventArgs e)
        {
            type1 = true;
            vd.type = type1;
        }

       
    }
}
