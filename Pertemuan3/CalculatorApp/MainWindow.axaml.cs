using Avalonia.Controls;
using Avalonia.Interactivity;
using System;
using System.Globalization;

namespace CalculatorApp;

public partial class MainWindow : Window
{
    private double _currentValue = 0;
    private double _storedValue = 0;
    private string _currentOperator = "";
    private bool _isEnteringNewNumber = true;

    public MainWindow()
    {
        InitializeComponent();
    }

    private void NumberButton_Click(object? sender, RoutedEventArgs e)
    {
        if (sender is Button button && button.Content is string digit)
        {
            if (_isEnteringNewNumber)
            {
                txtDisplay.Text = digit;
                _isEnteringNewNumber = false;
            }
            else
            {
                if (txtDisplay.Text == "0")
                {
                    txtDisplay.Text = digit;
                }
                else
                {
                    txtDisplay.Text += digit;
                }
            }
        }
    }

    private void DecimalButton_Click(object? sender, RoutedEventArgs e)
    {
        if (_isEnteringNewNumber)
        {
            txtDisplay.Text = "0.";
            _isEnteringNewNumber = false;
        }
        else if (!txtDisplay.Text!.Contains('.'))
        {
            txtDisplay.Text += ".";
        }
    }

    private void OperatorButton_Click(object? sender, RoutedEventArgs e)
    {
        if (sender is Button button && button.Content is string op)
        {
            if (double.TryParse(txtDisplay.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out double val))
            {
                if (!string.IsNullOrEmpty(_currentOperator) && !_isEnteringNewNumber)
                {
                    Calculate();
                }
                else
                {
                    _storedValue = val;
                }
            }

            _currentOperator = op;
            _isEnteringNewNumber = true;
        }
    }

    private void EqualsButton_Click(object? sender, RoutedEventArgs e)
    {
        if (!string.IsNullOrEmpty(_currentOperator))
        {
            Calculate();
            _currentOperator = "";
            _isEnteringNewNumber = true;
        }
    }

    private void ClearButton_Click(object? sender, RoutedEventArgs e)
    {
        txtDisplay.Text = "0";
        _currentValue = 0;
        _storedValue = 0;
        _currentOperator = "";
        _isEnteringNewNumber = true;
    }

    private void Calculate()
    {
        if (double.TryParse(txtDisplay.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out double currentVal))
        {
            switch (_currentOperator)
            {
                case "+":
                    _storedValue += currentVal;
                    break;
                case "−":
                    _storedValue -= currentVal;
                    break;
                case "×":
                    _storedValue *= currentVal;
                    break;
                case "÷":
                    if (currentVal != 0)
                    {
                        _storedValue /= currentVal;
                    }
                    else
                    {
                        txtDisplay.Text = "Error";
                        _isEnteringNewNumber = true;
                        return;
                    }
                    break;
            }

            txtDisplay.Text = _storedValue.ToString(CultureInfo.InvariantCulture);
        }
    }
}