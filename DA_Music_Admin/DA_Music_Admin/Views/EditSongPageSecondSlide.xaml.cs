using DA_Music_Admin.ViewModels;
using System.Windows.Controls;

namespace DA_Music_Admin.Views
{
    
    public partial class EditSongPageSecondSlide : UserControl
    {
        public EditSongPageSecondSlideViewModel vm;
        public EditSongPageSecondSlide(EditSongPageSecondSlideViewModel viewModel)
        {
            InitializeComponent();
            DataContext = vm = viewModel;
        }
    }
}
