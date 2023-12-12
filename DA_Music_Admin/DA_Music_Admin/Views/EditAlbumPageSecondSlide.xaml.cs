using DA_Music_Admin.ViewModels;
using System.Windows.Controls;


namespace DA_Music_Admin.Views
{
    public partial class EditAlbumPageSecondSlide : UserControl
    {
        public EditAlbumPageSecondSlideViewModel vm;
        public EditAlbumPageSecondSlide(EditAlbumPageSecondSlideViewModel viewModel)
        {
            InitializeComponent();
            DataContext = vm = viewModel;
            
        }
    }
}
