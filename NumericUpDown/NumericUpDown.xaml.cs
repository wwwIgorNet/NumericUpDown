using System;
using System.Collections.Generic;
using System.Globalization;
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
        public delegate void ValueChangedEventHandler(object sender, ValueChangedEventArgs e);
        public NumericUpDown()
        {
            InitializeComponent();
        }

        // Определение события 
        public static readonly RoutedEvent ValueChangedEvent = EventManager.RegisterRoutedEvent(
            "ValueChanged", RoutingStrategy.Direct,
            typeof(ValueChangedEventHandler), typeof(NumericUpDown));
        
        // Традиционная оболочка события 
        public event ValueChangedEventHandler ValueChanged
        {
            add
            {
                base.AddHandler(NumericUpDown.ValueChangedEvent, value);
            }
            remove
            {
                base.RemoveHandler(NumericUpDown.ValueChangedEvent, value);
            }
        }



        public double MaxValue
        {
            get { return (double)GetValue(MaxValueProperty); }
            set { SetValue(MaxValueProperty, value); }
        }
        public static readonly DependencyProperty MaxValueProperty =
            DependencyProperty.Register("MaxValue", typeof(double), typeof(NumericUpDown), new FrameworkPropertyMetadata(100D, maxValueChangedCallback, coerceMaxValueCallback));
        private static object coerceMaxValueCallback(DependencyObject d, object value)
        {
            double minValue = ((NumericUpDown)d).MinValue;
            if ((double)value < minValue)
                return minValue;

            return value;
        }
        private static void maxValueChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            NumericUpDown numericUpDown = ((NumericUpDown)d);
            numericUpDown.CoerceValue(MinValueProperty);
            numericUpDown.CoerceValue(ValueProperty);
        }

        public double MinValue
        {
            get { return (double)GetValue(MinValueProperty); }
            set { SetValue(MinValueProperty, value); }
        }
        public static readonly DependencyProperty MinValueProperty =
            DependencyProperty.Register("MinValue", typeof(double), typeof(NumericUpDown), new FrameworkPropertyMetadata(0D, minValueChangedCallback, coerceMinValueCallback));
        private static object coerceMinValueCallback(DependencyObject d, object value)
        {
            double maxValue = ((NumericUpDown)d).MaxValue;
            if ((double)value > maxValue)
                return maxValue;

            return value;
        }
        private static void minValueChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            NumericUpDown numericUpDown = ((NumericUpDown)d);
            numericUpDown.CoerceValue(NumericUpDown.MaxValueProperty);
            numericUpDown.CoerceValue(NumericUpDown.ValueProperty);
        }

        public double Increment
        {
            get { return (double)GetValue(IncrementProperty); }
            set { SetValue(IncrementProperty, value); }
        }
        public static readonly DependencyProperty IncrementProperty =
            DependencyProperty.Register("Increment", typeof(double), typeof(NumericUpDown), new FrameworkPropertyMetadata(1D, null, coerceIncrementCallback));
        private static object coerceIncrementCallback(DependencyObject d, object value)
        {
            NumericUpDown numericUpDown = ((NumericUpDown)d);
            double i = numericUpDown.MaxValue - numericUpDown.MinValue;
            if ((double)value > i)
                return i;

            return value;
        }

        public double Value
        {
            get { return (double)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(double), typeof(NumericUpDown), new FrameworkPropertyMetadata(0D, valueChangedCallback, coerceValueCallback), validateValueCallback);
        private static void valueChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            NumericUpDown numericUpDown = (NumericUpDown)d;
            ValueChangedEventArgs ea = 
                new ValueChangedEventArgs(NumericUpDown.ValueChangedEvent, d, (double)e.OldValue, (double)e.NewValue);
            numericUpDown.RaiseEvent(ea);
            //if (ea.Handled) numericUpDown.Value = (double)e.OldValue;
            //else 
            numericUpDown.textBox.Text = e.NewValue.ToString();
        }
        private static bool validateValueCallback(object value)
        {
            double val = (double)value;
            if (val > double.MinValue && val < double.MaxValue)
                return true;
            else
                return false;
        }
        private static object coerceValueCallback(DependencyObject d, object value)
        {
            double val = (double)value;
            double minValue = ((NumericUpDown)d).MinValue;
            double maxValue = ((NumericUpDown)d).MaxValue;
            double result;
            if (val < minValue)
                result = minValue;
            else if (val > maxValue)
                result = maxValue;
            else
                result = (double)value;

            return result;
        }

        private void buttonUp_Click(object sender, RoutedEventArgs e)
        {
            Value += Increment;
        }
        private void buttonDown_Click(object sender, RoutedEventArgs e)
        {
            Value -= Increment;
        }

        private void textBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
                e.Handled = true;
        }
        private void textBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            int index = textBox.CaretIndex;
            double result;
            if (!double.TryParse(textBox.Text, out result))
            {
                var changes = e.Changes.FirstOrDefault();
                textBox.Text = textBox.Text.Remove(changes.Offset, changes.AddedLength);
                textBox.CaretIndex = index > 0 ? index - changes.AddedLength : 0;
            }
            else if (result < MaxValue && result > MinValue)
                Value = result;
            else
            {
                textBox.Text = Value.ToString();
                textBox.CaretIndex = index > 0 ? index - 1 : 0;
            }
        }
    }

    public class ValueChangedEventArgs : RoutedEventArgs
    {
        public ValueChangedEventArgs(RoutedEvent routedEvent, object source, double oldValue, double newValue)
            : base(routedEvent, source)
        {
            OldValue = oldValue;
            NewValue = newValue;
        }
        public double OldValue { get; private set; }
        public double NewValue { get; private set; }
    }
}
