using DA_Music_Admin.ViewModels;
using System.Windows.Controls;

namespace DA_Music_Admin.Views
{
    public partial class HomePage : Page
    {
        public HomePage(HomeViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;
        }

        private void Page_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {

        }

        private void Page_IsVisibleChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
        {

        }
    }
}
