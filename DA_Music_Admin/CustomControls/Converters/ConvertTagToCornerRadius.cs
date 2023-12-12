using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;


namespace CustomControls.Converters
{
    public class ConvertTagToCornerRadius : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string content = value.ToString();
            if (string.IsNullOrEmpty(content))
            {
                return new CornerRadius(0);
            }
            else
            {
                string[] splitContent = content.Split(' ');
                if (splitContent.Length == 1)
                {
                    double thickness = 0;
                    try
                    {
                        thickness = double.Parse(splitContent[0].ToString());
                    }
                    catch
                    {
                        return new CornerRadius(0);
                    }
                    return new CornerRadius(thickness);
                }
                else if (splitContent.Length == 2)
                {
                    double top_bottom = 0;
                    double left_right = 0;
                    try
                    {
                        top_bottom = double.Parse(splitContent[0]);
                        left_right = double.Parse(splitContent[1]);
                    }
                    catch
                    {
                        return new CornerRadius(0);
                    }
                    return new CornerRadius(left_right, top_bottom, left_right, top_bottom);
                }
                else
                {
                    double left = 0;
                    double top = 0;
                    double right = 0;
                    double bottom = 0;
                    try
                    {
                        left = double.Parse(splitContent[0]);
                        top = double.Parse(splitContent[1]);
                        right = double.Parse(splitContent[2]);
                        bottom = double.Parse(splitContent[3]);
                    }
                    catch
                    {
                        return new CornerRadius(0);
                    }

                    return new CornerRadius(left, top, right, bottom);
                }
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
