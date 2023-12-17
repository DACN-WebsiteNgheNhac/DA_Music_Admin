using CustomControls.Utils;
using DA_Music_Admin.Themes;
using DA_Music_Admin.ViewModels;
using System.Windows;
using System.Windows.Media.Animation;

namespace DA_Music_Admin.Views
{
    public partial class MainWindow : Window
    {
        Storyboard highLightAnimation;
        MainViewModel vm;
        public MainWindow(MainViewModel vm)
        {
            InitializeComponent();
            this.DataContext = this.vm = vm;
            processPanel.ProcessChanged += ProcessPanel_ProcessChanged;
        }

        private void ProcessPanel_ProcessChanged(object? sender, System.EventArgs e)
        {
            if (highLightAnimation == null)
            {
                highLightAnimation = (Storyboard)FindResource("HighLight");
            }

            highLightAnimation.Begin();
        }

        private void WindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Hide();
            if (Tag == null)
                e.Cancel = true;
            else
                e.Cancel = false;
        }

        private void Window_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if(e.NewValue as bool? == true)
            {
                if(vm != null)
                {
                    if(vm.FirstSelectedMenu != null && vm.FirstSelectedMenu.MainItemMenu != null)
                    {
                        vm.ItemMenuClicked(vm.FirstSelectedMenu.MainItemMenu, new string[] { });
                        vm.FirstSelectedMenu.MainItemMenu.IsChecked = true;
                    }
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //ThemeController.ChangeTheme(new System.Uri("Themes/Light.xaml", System.UriKind.Relative));
            //ColorConst.Register(this);
        }
    }
}
