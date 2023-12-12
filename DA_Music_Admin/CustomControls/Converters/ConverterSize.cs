using System;
using System.Globalization;
using System.Windows.Data;

namespace CustomControls.Converters 
{
    public class ConverterSize : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double)
            {
                double curWidth = double.Parse(value.ToString());
                double percentWidth = double.Parse(parameter.ToString());
                return curWidth * percentWidth / 100;
            }
            return 1;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
