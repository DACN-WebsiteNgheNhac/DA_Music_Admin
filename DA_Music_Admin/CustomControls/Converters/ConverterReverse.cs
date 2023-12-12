using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace CustomControls.Converters
{
    public class ConverterReverse : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Visibility)
            {
                if (targetType.Name == nameof(Visibility))
                {
                    return ConvertVisibilityToVisibility((Visibility) value, parameter);
                }
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //if (value is double)
            //{
            //    double Volume = double.Parse(value.ToString());
            //    return Math.Round(Volume, 2).ToString();
            //}
            return null;
        }

        private Visibility ConvertVisibilityToVisibility(Visibility value, object parameter)
        {
            if (value != Visibility.Visible)
            {
                return Visibility.Visible;
            }
            else
            {
                if (parameter != null)
                {
                    return Visibility.Collapsed;
                }
                else
                {
                    return Visibility.Hidden;
                }
            }
        }
    }
}
