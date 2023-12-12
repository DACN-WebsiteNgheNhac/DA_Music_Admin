using DA_Music_Admin.ViewModels;
using System.Windows.Controls;

namespace DA_Music_Admin.Views
{
    public partial class EditArtistPageSecondSlide : UserControl
    {
        public EditArtistPageSecondSlideViewModel vm;
        public EditArtistPageSecondSlide(EditArtistPageSecondSlideViewModel viewModel)
        {
            InitializeComponent();
            DataContext = vm = viewModel;
        }
    }
}
