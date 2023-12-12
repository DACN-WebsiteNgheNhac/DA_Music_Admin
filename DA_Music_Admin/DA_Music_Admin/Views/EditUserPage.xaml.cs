using CustomControls.Controls;
using DA_Music_Admin.ViewModels;
using System.Windows.Controls;

namespace DA_Music_Admin.Views
{

    public partial class EditUserPage : Page
    {
        public EditUserViewModel vm;
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
        public EditUserPage(EditUserViewModel viewModel)
        {
            InitializeComponent();
            DataContext = vm = viewModel;
        }
    }
}
