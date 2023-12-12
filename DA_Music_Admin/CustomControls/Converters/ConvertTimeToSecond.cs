using System;
using System.Globalization;
using System.Windows.Data;

namespace CustomControls.Converters
{
    public class ConvertTimeToSecond : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is string)
            {
                var timeSplit = value.ToString().Split(':');
                int totalSeconds = 0;
                try
                {
                    totalSeconds = int.Parse(timeSplit[0]) * 60 + int.Parse(timeSplit[1]);
                }
                catch { return 0; }   
                return totalSeconds;
            }
            else if(value is double)
            {               
                double totalSeconds = (double)value;
                int minutes = (int)totalSeconds / 60;
                int seconds = (int)totalSeconds % 60;
                return minutes.ToString("00") + ":" + seconds.ToString("00");
                
            }
            else 
            {
                int totalSeconds = (int)value;
                int minutes = (int)totalSeconds / 60;
                int seconds = (int)totalSeconds % 60;
                return minutes.ToString("00") + ":" + seconds.ToString("00");
            }
                
                    

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return 1;
        }
    }
}
