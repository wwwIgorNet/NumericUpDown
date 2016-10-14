using System;
using System.Collections.Generic;
using System.Linq;
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

namespace ControlLib
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class NumericUpDown : UserControl
    {
        public event EventHandler ValueChanged;

        private decimal oldValue;

        public NumericUpDown()
        {
            InitializeComponent();
            MaxValue = 100;
            MinValue = 0;
            Increment = 1;
            textBox.Text = 0.ToString();
            oldValue = Value;
        }

        public decimal Increment { get; set; }
        public decimal MaxValue { get; set; }
        public decimal MinValue { get; set; }
        public decimal Value
        {
            get
            {
                if (textBox.Text == string.Empty)
                    return 0M;
                else
                    return decimal.Parse(textBox.Text);
            }
            set
            {
                if (value <= MaxValue && value >= MinValue)
                    textBox.Text = value.ToString();
            }
        }

        private void buttonUp_Click(object sender, RoutedEventArgs e)
        {
            Value += Increment;
        }

        private void buttonDown_Click(object sender, RoutedEventArgs e)
        {
            Value -= Increment;
        }

        private void textBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            int index = textBox.CaretIndex;
            decimal result;
            if (!decimal.TryParse(textBox.Text, out result))
            {
                textBox.Text = textBox.Text.Remove(e.Changes.FirstOrDefault().Offset, e.Changes.FirstOrDefault().AddedLength);
                textBox.CaretIndex = index > 0 ? index - 1 : 0;
            }
            else if (result > MaxValue || result < MinValue)
            {
                Value = oldValue;
                textBox.CaretIndex = index;
            }
            else if (ValueChanged != null)
                ValueChanged.Invoke(this, new EventArgs());

        }

        private void textBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            Console.WriteLine(e.Key);
            oldValue = Value;
            switch (e.Key)
            {
                case Key.D0:
                case Key.D1:
                case Key.D2:
                case Key.D3:
                case Key.D4:
                case Key.D5:
                case Key.D6:
                case Key.D7:
                case Key.D8:
                case Key.D9:
                case Key.OemMinus:
                case Key.OemComma:
                case Key.OemPeriod:
                case Key.Back:
                case Key.Delete:
                case Key.Left:
                case Key.Right:
                    break;
                default:
                    e.Handled = true; break;
            }
        }
    }
}
