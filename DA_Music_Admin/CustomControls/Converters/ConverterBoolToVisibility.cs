using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows;

namespace CustomControls.Converters
{
    public class ConverterBoolToVisibility : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var isTrue = value as bool?;

            var param = parameter as string;
            if (param != null)
            { 
                if(param.Trim().ToLower() == "Reverse".Trim().ToLower())
                {
                    return isTrue == false ? Visibility.Visible : Visibility.Collapsed;
                }
            }

            if(isTrue != null)
                return isTrue == true ? Visibility.Visible : (parameter != null ? Visibility.Hidden : Visibility.Collapsed);
            return Visibility.Visible;
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
