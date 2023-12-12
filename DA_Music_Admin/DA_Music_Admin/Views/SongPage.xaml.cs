using DA_Music_Admin.ViewModels;
using System.Threading.Tasks;
using System;
using System.Windows;
using System.Windows.Controls;

namespace DA_Music_Admin.Views
{
    public partial class SongPage : Page
    {
        public Func<object, Task<int>> ActionClickRow { get; set; }

        SongViewModel vm;
        public SongPage(SongViewModel vm)
        {
            InitializeComponent();
            DataContext = this.vm = vm;
          
        }
     
        public void SwitchActionButtons(Visibility visibility, Func<object, Task<int>> func)
        {
            if(vm != null)
            {
                vm.ShowModifyControls = visibility;
            }

            if(visibility != Visibility.Visible)
            {
                dgSong.SelectionChanged += DgSong_SelectionChanged;
                if(func != null)
                    ActionClickRow = func;
            }
            else
            {
                dgSong.SelectionChanged -= DgSong_SelectionChanged;
            }
        }

        private void DgSong_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var target = sender as DataGrid;
            var selectedItem = target.SelectedItem;
            if (selectedItem == null)
                return;
            if (ActionClickRow != null)
                ActionClickRow(selectedItem);

        }
      
    }
}
