using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TestNumericUpDown
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void numericUpDown_ValueChanged(object sender, ControlLib.ValueChangedEventArgs e)
        {
            Console.WriteLine("OldValue: " + e.OldValue);
            Console.WriteLine("NewValue: " + e.NewValue);
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            this.numericUpDown.Value = 100;
            Console.WriteLine(this.numericUpDown.MinValue);
            Console.WriteLine(this.numericUpDown.Value);
            Console.WriteLine(this.numericUpDown.MaxValue);
            this.numericUpDown.MaxValue = -2;
            Console.WriteLine(this.numericUpDown.MinValue);
            Console.WriteLine(this.numericUpDown.Value);
            Console.WriteLine(this.numericUpDown.MaxValue);
            this.numericUpDown.MinValue = -100;
            Console.WriteLine(this.numericUpDown.MinValue);
            Console.WriteLine(this.numericUpDown.Value);
            Console.WriteLine(this.numericUpDown.MaxValue);
            //this.numericUpDown.Increment = 200;
        }
    }
}
