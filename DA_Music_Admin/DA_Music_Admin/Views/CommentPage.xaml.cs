using DA_Music_Admin.ViewModels;
using System.Windows.Controls;

namespace DA_Music_Admin.Views
{
    
    public partial class CommentPage : Page
    {
        public CommentPage(CommentViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;
            
        }

        private void ContextMenu_Opened(object sender, System.Windows.RoutedEventArgs e)
        {
            var target = sender as ContextMenu;
            var dataContext = target.DataContext as Comment;
            if(dataContext!= null)
            {
                if(dataContext.DeletedAt == null)
                {
                    (target.Items[0] as MenuItem).Visibility = System.Windows.Visibility.Visible;
                    (target.Items[1] as MenuItem).Visibility = System.Windows.Visibility.Collapsed;
                }
                else
                {
                    (target.Items[0] as MenuItem).Visibility = System.Windows.Visibility.Collapsed;
                    (target.Items[1] as MenuItem).Visibility = System.Windows.Visibility.Visible;
                }
            }
        }
    }
}
