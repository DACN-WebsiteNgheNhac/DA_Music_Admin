using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace CustomControls.Controls
{

    public partial class NotificationAlert : UserControl
    {
        public NotificationAlterViewModel viewModel;

        public NotificationAlert()
        {
            InitializeComponent();
            this.DataContext = viewModel = new NotificationAlterViewModel(this);
        }

        public NotificationAlert(Geometry icon, string title, string message)
        {
            InitializeComponent();
            this.DataContext = viewModel = new NotificationAlterViewModel(this, icon, title, message);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            viewModel.UserControl_Loaded(sender, e);
        }

        private void BtnClose_Clicked(object sender, RoutedEventArgs e)
        {
            viewModel.BtnClose_Clicked(sender, e);
        }

        public void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            viewModel.hideNotify();
        }
    }

    public class NotificationAlterViewModel : INotifyPropertyChanged
    {

        #region Colos
        private SolidColorBrush _BackgroundColor = new SolidColorBrush(Colors.CornflowerBlue);
        public SolidColorBrush BackgroundColor
        {
            get { return _BackgroundColor; }
            set { _BackgroundColor = value; OnPropertyChanged("BackgroundColor"); }
        }

        private SolidColorBrush _ForegroundColor = new SolidColorBrush(Colors.Black);
        public SolidColorBrush ForegroundColor
        {
            get { return _ForegroundColor; }
            set { _ForegroundColor = value; OnPropertyChanged("ForegroundColor"); }
        }

        private SolidColorBrush _ColorIcon = new SolidColorBrush(Colors.LightGreen);
        public SolidColorBrush ColorIcon
        {
            get { return _ColorIcon; }
            set { _ColorIcon = value; OnPropertyChanged("ColorIcon"); }
        }

        private SolidColorBrush _HoveredColorIcon = new SolidColorBrush(Color.FromRgb(217, 0, 0));
        public SolidColorBrush HoveredColorIcon
        {
            get { return _HoveredColorIcon; }
            set { _HoveredColorIcon = value; OnPropertyChanged("HoveredColorIcon"); }
        }

        private SolidColorBrush _ColorProgress = new SolidColorBrush(Colors.LightGreen);
        public SolidColorBrush ColorProgress
        {
            get { return _ColorProgress; }
            set { _ColorProgress = value; OnPropertyChanged("ColorProgress"); }
        }
        #endregion Colos

        private Geometry _Icon = Geometry.Empty;
        public Geometry Icon
        {
            get { return _Icon; }
            set { _Icon = value; OnPropertyChanged("Icon"); }
        }


        private string _Title;
        public string Title
        {
            get { return _Title; }
            set { _Title = value; OnPropertyChanged("Title"); }
        }

        private string _Message;
        public string Message
        {
            get { return _Message; }
            set { _Message = value; OnPropertyChanged("Message"); }
        }

        private double _CurrentValueProgess = 1;

        public double CurrentValueProgress
        {
            get { return _CurrentValueProgess; }
            set { _CurrentValueProgess = value; OnPropertyChanged("CurrentValueProgress"); }
        }

        NotificationAlert alert;
        Panel parent;
        Storyboard hideAnimation;
        DispatcherTimer timer = new DispatcherTimer();

        private int curTime;
        private int _TimeLive = 3000;
        public int TimeLive
        {
            get { return _TimeLive; }
            set { _TimeLive = value; }
        }

        public NotificationAlterViewModel(NotificationAlert alert)
        {
            this.alert = alert;
        }

        public NotificationAlterViewModel(NotificationAlert alert, Geometry icon, string title, string message)
        {
            this.alert = alert;
            this.Icon = icon;
            this.Title = title;
            this.Message = message;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            RenderOptions.ProcessRenderMode = System.Windows.Interop.RenderMode.Default;
            if (hideAnimation == null)
            {
                hideAnimation = (Storyboard)alert.FindResource("Hide");
                hideAnimation.Completed += HideAnimation_Completed;
                timer.Interval = new System.TimeSpan(0, 0, 1);
                timer.Tick += Timer_Tick;
            }
            timer.Start();
        }

        private void Timer_Tick(object sender, System.EventArgs e)
        {
            if (curTime < TimeLive - 1000)
            {
                curTime += 1000;
            }
            else
            {
                hideNotify();
            }
        }

        private void HideAnimation_Completed(object sender, System.EventArgs e)
        {
            if (parent != null)
                parent.Children.Remove(alert);
        }

        public void BtnClose_Clicked(object sender, RoutedEventArgs e)
        {
            hideNotify();
        }

        public void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            hideNotify();
        }

        public void hideNotify(Panel container = null)
        {
            if (container == null)
            {
                container = alert.Parent as Panel;
            }
            parent = container;
            timer.Stop();
            curTime = 0;
            this.alert.Dispatcher.Invoke(() =>
            {
                hideAnimation.Begin();
            });
        }
    }
}
