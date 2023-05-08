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
using OxyPlot;
using ViewModel2;

namespace Wpf1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
   
    public partial class MainWindow : Window, IUIServices
    {
        public string GetFileName()
        {
            var dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.FileName = "rawdata";
            dialog.DefaultExt = ".txt";
            dialog.Filter = "Text documents (.txt)|*.txt";

            bool? result = dialog.ShowDialog();

            if (result == true)
            {
                string filename = dialog.FileName;
                return filename;
            }
            return null;
        }
        public void ReportError(string message) =>
               MessageBox.Show(message, "Error message", MessageBoxButton.OK, MessageBoxImage.Error);
        public MainWindow()
        {  
            InitializeComponent();
            FuncBox.ItemsSource = Enum.GetValues(typeof(FRawEnum));
            FuncBox.SelectedIndex = 0;
            DataContext = new ViewData(this);

        }
    
    }
}
