using System;
using System.Globalization;
using System.Windows.Data;


namespace CustomControls.Converters
{
    public  class ConverterDateTimeOffsetToDatetime : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var offset = value as DateTimeOffset?;
            if (offset != null)
            {
                if(offset.Value.Year == 1)
                    offset = offset.Value.AddYears(1);
                return new DateTime(offset.Value.Year, offset.Value.Month, offset.Value.Day, offset.Value.Hour, offset.Value.Minute, offset.Value.Second);
            }
            return DateTime.Now;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var dt = value as DateTime?;
            if (dt != null)
            {
                return new DateTimeOffset(dt.Value.Year, dt.Value.Month, dt.Value.Day, dt.Value.Hour, dt.Value.Minute, dt.Value.Second, TimeSpan.FromHours(7));
            }
            return DateTimeOffset.Now;
        }
    }
}
