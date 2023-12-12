using System;
using System.Globalization;
using System.Windows.Data;

namespace CustomControls.Converters
{
    public class ConvertWidthGrid : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double minWidth = 50;
            double maxWidth = 245;
            double widthCrollBar = 0;

            double limitWidth = double.Parse(value.ToString()) * 0.20;

            double width = double.Parse(value.ToString());
            if (limitWidth < minWidth || limitWidth > maxWidth)
            {
                if (limitWidth < 50)
                    width -= minWidth;
                else if(limitWidth > 50)
                    width -= maxWidth;
            }
            else
                width *= 0.80;
            return width - widthCrollBar;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
