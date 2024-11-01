using System;
using System.Collections.Generic;
using System.Globalization;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace StandardCalculator
{
    public sealed partial class MainPage : Page
    {
        private double? num1, num2;
        private string operand;
        private bool operandPressed = false;
        private bool isScientificMode = false;
        private readonly Dictionary<string, Func<double, double>> scientificFunctions;

        public MainPage()
        {
            this.InitializeComponent();
            scientificFunctions = new Dictionary<string, Func<double, double>>()
            {
                { "sin", value => Math.Sin(DegreeToRadian(value)) },
                { "cos", value => Math.Cos(DegreeToRadian(value)) },
                { "tan", value => Math.Tan(DegreeToRadian(value)) },
                { "sqrt", value => value >= 0 ? Math.Sqrt(value) : double.NaN }
            };
        }

        private void SetCalculatorMode(bool scientificMode)
        {
            SinButton.Visibility = CosButton.Visibility = TanButton.Visibility = SqrtButton.Visibility =
                scientificMode ? Visibility.Visible : Visibility.Collapsed;

            BasicModeButton.IsEnabled = scientificMode;
            ScientificModeButton.IsEnabled = !scientificMode;
        }

        private void ToggleMode_Click(object sender, RoutedEventArgs e)
        {
            isScientificMode = !isScientificMode;
            SetCalculatorMode(isScientificMode);
        }

        private void number_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            if (txtresult.Text == "0" || operandPressed)
            {
                txtresult.Text = btn.Content.ToString();
                operandPressed = false;
            }
            else
            {
                txtresult.Text += btn.Content.ToString();
            }
        }

        private void operator_Click(object sender, RoutedEventArgs e)
        {
            if (double.TryParse(txtresult.Text, out double parsedNum1))
            {
                num1 = parsedNum1;
                operand = (sender as Button)?.Content.ToString();
                operandPressed = true;
            }
        }

        private void btnEquals_Click(object sender, RoutedEventArgs e)
        {
            if (num1.HasValue && double.TryParse(txtresult.Text, out double parsedNum2))
            {
                num2 = parsedNum2;
                txtresult.Text = Calculate(num1.Value, num2.Value, operand).ToString(CultureInfo.InvariantCulture);
                operandPressed = true;
            }
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            txtresult.Text = "0";
            num1 = num2 = null;
            operand = null;
        }

        private void btnbackspace_Click(object sender, RoutedEventArgs e)
        {
            txtresult.Text = txtresult.Text.Length > 1
                ? txtresult.Text.Substring(0, txtresult.Text.Length - 1)
                : "0";
        }

        private void btnDecimal_Click(object sender, RoutedEventArgs e)
        {
            if (!txtresult.Text.Contains("."))
            {
                txtresult.Text += ".";
            }
        }

        private void btnPlusOrMinus_Click(object sender, RoutedEventArgs e)
        {
            if (double.TryParse(txtresult.Text, out double currentNumber))
            {
                txtresult.Text = (-currentNumber).ToString();
            }
        }

        private void ScientificFunction_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            string function = button?.Tag.ToString();

            if (double.TryParse(txtresult.Text, out double value) && function != null && scientificFunctions.ContainsKey(function))
            {
                txtresult.Text = scientificFunctions[function](value).ToString(CultureInfo.InvariantCulture);
            }
        }

        private static double Calculate(double num1, double num2, string operand)
        {
            return operand switch
            {
                "+" => num1 + num2,
                "-" => num1 - num2,
                "×" => num1 * num2,
                "÷" => num2 != 0 ? num1 / num2 : double.NaN,
                "%" => num2 != 0 ? num1 % num2 : double.NaN,
                _ => double.NaN
            };
        }

        private static double DegreeToRadian(double degree)
        {
            return degree * Math.PI / 180;
        }
    }
}
