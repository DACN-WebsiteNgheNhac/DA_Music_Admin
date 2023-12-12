using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace CustomControls.Converters
{
    public class ConverterCornerRadius : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is double)
            {
                double curWidth = double.Parse(value.ToString());
                double percentWidth = double.Parse(parameter.ToString());
                return new CornerRadius(curWidth * percentWidth / 100 * 0.5);
            }
            return new CornerRadius(0);
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
