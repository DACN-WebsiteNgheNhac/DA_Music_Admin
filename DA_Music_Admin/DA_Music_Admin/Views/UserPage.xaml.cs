using DA_Music_Admin.ViewModels;
using System.Windows.Controls;

namespace DA_Music_Admin.Views
{
    public partial class UserPage : Page
    {
        public UserPage(UserViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;
        }
    }
}
