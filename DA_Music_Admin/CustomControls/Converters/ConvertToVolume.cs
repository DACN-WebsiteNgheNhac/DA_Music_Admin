using System;
using System.Globalization;
using System.Windows.Data;

namespace CustomControls.Converters
{
    public class ConvertToVolume : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is string)
            {
                double curVolume = double.Parse(value.ToString().Split(',')[0]);
                double preVolume = 0;
                try
                {
                    preVolume = double.Parse(value.ToString().Split(',')[1]);

                }
                catch
                {
                    return curVolume;
                }
                
                if(curVolume == 0)
                {
                    return 0;
                }
                else
                {
                    return preVolume;
                }

                
            }
            return 1;
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
