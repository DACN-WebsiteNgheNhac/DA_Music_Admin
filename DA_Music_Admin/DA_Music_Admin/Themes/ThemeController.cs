using System;
using System.Windows;

namespace DA_Music_Admin.Themes
{
    public class ThemeController
    {
        public static void ChangeTheme(Uri themeUri)
        {
            ResourceDictionary theme = new ResourceDictionary { Source = themeUri};

            App.Current.Resources.Clear();
            App.Current.Resources.MergedDictionaries.Add(theme);
        }
    }
}
