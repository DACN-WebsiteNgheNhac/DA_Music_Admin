using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows;

namespace DA_Music_Admin.ViewModels
{
    public class UserControlViewModel : BaseViewModel
    {
        private System.Windows.Thickness _Margin = new System.Windows.Thickness(0, 0, 0, 0);

        public System.Windows.Thickness Magin
        {
            get { return _Margin; }
            set { _Margin = value; }
        }

        UserControl t;
        Border border;


        public ICommand UserControl_Loaded { get; set; }

        public UserControlViewModel()
        {
            UserControl_Loaded = new RelayCommand<UserControl>(
               (t) =>
               {
                   return t != null ? true : false;
               },
               (t) =>
               {
                   UserControl_Loaded_CodeBehind(t);
               });
        }

        public virtual void UserControl_Loaded_CodeBehind(UserControl t)
        {
            this.t = t;
            DependencyObject parentObj = VisualTreeHelper.GetParent(t);
            border = VisualTreeHelper.GetParent(parentObj) as Border;
            border.SizeChanged += Border_SizeChanged;
        }


        protected void changeSize(UserControl t)
        {
            var width = border.ActualWidth;
            var height = border.ActualHeight;
            t.Width = width;
            t.Height = height;
            if (t.Margin == new System.Windows.Thickness(0))
            {
                t.Width -= (_Margin.Left + _Margin.Right);
                t.Height -= _Margin.Bottom;
                t.Margin = _Margin;
            }
        }

        private void Border_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            changeSize(t);
        }

        public async Task loadData(string nameProperty)
        {
            await Task.Run(() =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    OnPropertyChanged(nameProperty);
                });
            });
        }
    }
}
