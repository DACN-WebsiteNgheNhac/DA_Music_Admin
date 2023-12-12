using CustomControls.Controls;
using DA_Music_Admin.ViewModels;
using System.Globalization;
using System;
using System.Windows.Controls;


namespace DA_Music_Admin.Views
{
    public partial class EditAlbumPage : Page
    {
        public EditAlbumViewModel vm;
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
        public EditAlbumPage(EditAlbumViewModel viewModel)
        {
            InitializeComponent();
            DataContext = vm = viewModel;
        }
    }

    
}
