using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows;

namespace CustomControls.Converters
{
    public class ConverterDateTimeOffsetToVisibility : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return parameter == null ? Visibility.Collapsed : Visibility.Visible;
            return parameter == null ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double)
            {
                double Volume = double.Parse(value.ToString());
                return Math.Round(Volume, 2).ToString();
            }
            return "1";
        }
    }
}
