using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows;

namespace DA_Music_Admin.ViewModels
{
    public class PageViewModel : BaseViewModel
    {
        private System.Windows.Thickness _Margin = new System.Windows.Thickness(0, 0, 0, 0);

        public System.Windows.Thickness Magin
        {
            get { return _Margin; }
            set { _Margin = value; }
        }

        Page t;
        Border border;

        bool IsTheFirstLoad = true;

        public ICommand Page_Loaded { get; set; }
        public ICommand Page_UnLoaded { get; set; }
        public ICommand Page_Initialized { get; set; }

        public PageViewModel()
        {
            Page_Loaded = new RelayCommand<Page>(
            (t) =>
            {
                return t != null ? true : false;
            },
            (t) =>
            {
                Page_Loaded_CodeBehind(t);
            });

            Page_UnLoaded = new RelayCommand<Page>(
            (t) =>
            {
                return t != null ? true : false;
            },
            (t) =>
            {
                Page_UnLoaded_CodeBehind(t);
            });
        }

        public virtual void Page_Loaded_CodeBehind(Page t)
        {
            this.t = t;
            DependencyObject parentObj = VisualTreeHelper.GetParent(t);
            border = VisualTreeHelper.GetParent(parentObj) as Border;
            border.SizeChanged += Border_SizeChanged;
            if (IsTheFirstLoad)
            {
                Refresh();
                IsTheFirstLoad = false;
            }
        }

        public virtual void Page_UnLoaded_CodeBehind(Page t)
        {
        }

        public virtual void Refresh()
        {

        }

        protected void changeSize(Page t)
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
