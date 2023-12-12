using System.Threading;
using System.Windows;
using System.Windows.Media;

namespace CustomControls.Utils
{
    public static class ColorConst
    {
        public static Color backgroundColor = Color.FromRgb(15, 14, 14);
        public static Color subBackgroundColor = Color.FromRgb(51, 51, 51);
        public static Color foregroundColor = Color.FromRgb(240, 240, 240);
        public static Color foregroundColor_20 = Color.FromArgb(20, 240, 240, 240);
        public static Color foregroundColor_60 = Color.FromArgb(60, 240, 240, 240);
        public static Color foregroundColor_80 = Color.FromArgb(80, 240, 240, 240);
        public static Color iconColor = Color.FromRgb(138, 105, 250);
        public static Color hoveredBackgroundColor = Color.FromRgb(51, 51, 51);

        public static void Register(Window window)
        {
            backgroundColor = (window.FindResource("BackgroundColor") as SolidColorBrush).Color;
            subBackgroundColor = (window.FindResource("SubBackgroundColor") as SolidColorBrush).Color;
            foregroundColor = (window.FindResource("ForegroundColor") as SolidColorBrush).Color;
            foregroundColor_20 = (window.FindResource("ForegroundColor_20") as SolidColorBrush).Color;
            foregroundColor_60 = (window.FindResource("ForegroundColor_60") as SolidColorBrush).Color;
            foregroundColor_80 = (window.FindResource("ForegroundColor_80") as SolidColorBrush).Color;
            iconColor = (window.FindResource("IconColor") as SolidColorBrush).Color;
            hoveredBackgroundColor = (window.FindResource("SubBackgroundColor") as SolidColorBrush).Color;
        }
    }
}
