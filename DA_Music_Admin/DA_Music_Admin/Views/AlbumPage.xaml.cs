using DA_Music_Admin.ViewModels;
using System.Windows.Controls;

namespace DA_Music_Admin.Views
{
    public partial class AlbumPage : Page
    {
        public AlbumPage(AlbumViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;
        }
    }
}
