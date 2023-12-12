using CustomControls.Controls;
using DA_Music_Admin.ViewModels;
using System.Windows.Controls;
namespace DA_Music_Admin.Views
{
    public partial class EditSongPage : Page
    {
        public EditSongViewModel vm;
        private CustomRadioButton _TriggerControl;
        public CustomRadioButton TriggerControl
        {
            get { return _TriggerControl; }
            set
            {
                if (_TriggerControl == null)
                {
                    _TriggerControl = value;
                }
            }
        }
        public EditSongPage(EditSongViewModel viewModel)
        {
            InitializeComponent();
            DataContext = vm = viewModel;
        }
    }
}
