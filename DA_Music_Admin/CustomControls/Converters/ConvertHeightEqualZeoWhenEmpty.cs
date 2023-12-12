using System;
using System.Globalization;
using System.Windows.Data;

namespace CustomControls.Converters
{
    public class ConvertHeightEqualZeoWhenEmpty : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is string)
            {
                if(parameter != null)
                {
                    if (string.IsNullOrEmpty(value.ToString()))
                        return 0;
                    else
                        return 62;
                }
                else
                {
                    if (string.IsNullOrEmpty(value.ToString()))
                        return 62;
                    else
                        return 0;
                }
            }
            return 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return 1;
        }
    }
}
