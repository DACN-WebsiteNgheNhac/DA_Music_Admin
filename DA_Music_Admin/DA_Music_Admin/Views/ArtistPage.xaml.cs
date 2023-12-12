using DA_Music_Admin.ViewModels;
using System.Windows.Controls;

namespace DA_Music_Admin.Views
{
    public partial class ArtistPage : Page
    {
        public ArtistPage(ArtistViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;
        }
    }
}
