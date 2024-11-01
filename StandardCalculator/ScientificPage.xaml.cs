using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace StandardCalculator
{
    public sealed partial class ScientificPage : Page
    {
        public ScientificPage()
        {
            this.InitializeComponent();
        }

        private void BackToMain_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));  // Navigate back to MainPage
        }

        private void ScientificFunction_Click(object sender, RoutedEventArgs e)
        {
            if (double.TryParse(txtScientificResult.Text, out double value))
            {
                Button button = sender as Button;
                string function = button.Tag.ToString();
                double result = function switch
                {
                    "sin" => Math.Sin(value * Math.PI / 180),
                    "cos" => Math.Cos(value * Math.PI / 180),
                    "tan" => Math.Tan(value * Math.PI / 180),
                    "ln" => value > 0 ? Math.Log(value) : double.NaN,
                    "log" => value > 0 ? Math.Log10(value) : double.NaN,
                    "sqrt" => value >= 0 ? Math.Sqrt(value) : double.NaN,
                    "pow" => Math.Pow(value, 2),
                    "pi" => Math.PI,
                    _ => double.NaN
                };

                txtScientificResult.Text = result.ToString();
            }
        }
    }
}
