using CustomControls.Utils;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace CustomControls.Controls
{
    public class TaskStatus
    {
        public bool Success { get; set; }
        public Object Result { get; set; }

        public TaskStatus()
        {

        }

        public TaskStatus(bool Success)
        {
            this.Success = Success;
        }
    }

    public partial class ProcessItem : UserControl
    {
      
        public ProcessItemViewModel viewModel;

        public ProcessItem()
        {
            InitializeComponent();
            this.DataContext = viewModel = new ProcessItemViewModel(this);
        }

        public ProcessItem(Geometry icon, string title, string message)
        {
            InitializeComponent();
            this.DataContext = viewModel = new ProcessItemViewModel(this, icon, title, message);
        }

        public ProcessItem(string title, string message, Func<Task<TaskStatus>> task)
        {
            InitializeComponent();
            this.DataContext = viewModel = new ProcessItemViewModel(this, title, message, task);
        }

        public string Title { get; }
        public string Message { get; }
        public Func<Task<TaskStatus>> Test { get; }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (viewModel != null)
                viewModel.UserControl_Loaded(sender, e);
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            if (viewModel != null)
                viewModel.UserControl_Unloaded(sender, e);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(viewModel != null)
                viewModel.BtnClose_Clicked(sender, e);
        }
    }

    public class ProcessItemViewModel : BaseViewModel
    {
        #region Colos
        private SolidColorBrush _BackgroundColor = new SolidColorBrush(Colors.CornflowerBlue);
        public SolidColorBrush BackgroundColor
        {
            get { return _BackgroundColor; }
            set { _BackgroundColor = value; _ = loadData("BackgroundColor"); }
        }

        private SolidColorBrush _ForegroundColor = new SolidColorBrush(Colors.Black);
        public SolidColorBrush ForegroundColor
        {
            get { return _ForegroundColor; }
            set { _ForegroundColor = value; _ = loadData("ForegroundColor"); }
        }

        private SolidColorBrush _ColorIcon = new SolidColorBrush(Colors.LightGreen);
        public SolidColorBrush ColorIcon
        {
            get { return _ColorIcon; }
            set { _ColorIcon = value; _ = loadData("ColorIcon"); }
        }

        private SolidColorBrush _HoveredColorIcon = new SolidColorBrush(Color.FromRgb(217, 0, 0));
        public SolidColorBrush HoveredColorIcon
        {
            get { return _HoveredColorIcon; }
            set { _HoveredColorIcon = value; _ = loadData("HoveredColorIcon"); }
        }

        private SolidColorBrush _ColorProgress = new SolidColorBrush(Colors.LightGreen);
        public SolidColorBrush ColorProgress
        {
            get { return _ColorProgress; }
            set { _ColorProgress = value; _ = loadData("ColorProgress"); }
        }
        #endregion Colos

        #region Data
        private Geometry _Icon = Geometry.Empty;
        public Geometry Icon
        {
            get { return _Icon; }
            set { _Icon = value; _ = loadData("Icon"); }
        }


        private string _Title;
        public string Title
        {
            get { return _Title; }
            set { _Title = value; _ = loadData("Title"); }
        }

        private string _Message;
        public string Message
        {
            get { return _Message; }
            set { _Message = value; _ = loadData("Message"); }
        }

        private Func<Task<TaskStatus>> _MissionTask;
        public Func<Task<TaskStatus>> MissionTask
        {
            get { return _MissionTask; }
            set { _MissionTask = value; _ = loadData("MissionTask"); }
        }
        #endregion Data

        private Visibility _VisibilityProgressBar = Visibility.Visible;
        public Visibility VisibilityProgressBar
        {
            get { return _VisibilityProgressBar; }
            set { 
                _VisibilityProgressBar = value; 
                _ = loadData(nameof(VisibilityProgressBar)); 
                VisibilityIcon = value == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
            }
        }

        private Visibility _VisibilityIcon = Visibility.Hidden;
        public Visibility VisibilityIcon
        {
            get { return _VisibilityIcon; }
            private set { _VisibilityIcon = value; _ = loadData(nameof(VisibilityIcon)); }
        }


        private double _CurrentValueProgess = 1;

        public double CurrentValueProgress
        {
            get { return _CurrentValueProgess; }
            set { _CurrentValueProgess = value; _ = loadData("CurrentValueProgress"); }
        }

        ProcessItem process;
        Panel parent;
        Storyboard hideAnimation;

        public ProcessItemViewModel(ProcessItem process)
        {
            this.process = process;
        }

        public ProcessItemViewModel(ProcessItem alert, Geometry icon, string title, string message)
        {
            this.process = alert;
            this.Icon = icon;
            this.Title = title;
            this.Message = message;
        }

        public ProcessItemViewModel(ProcessItem alert, string title, string message, Func<Task<TaskStatus>> task)
        {
            this.process = alert;
            this.Title = title;
            this.Message = message;
            this.MissionTask = task;
        }

        public async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (hideAnimation == null)
            {
                hideAnimation = (Storyboard)process.FindResource("Hide");
                hideAnimation.Completed += HideAnimation_Completed;
            }

            if (MissionTask != null)
            {
                Task<TaskStatus> task = MissionTask();
                await task.ContinueWith(t =>
                {
                    VisibilityProgressBar = Visibility.Hidden;

                    if (t == null || t.Result == null || t.Result.Success == false)
                    {
                        Icon = GeometryConst.IconError;
                        process.Dispatcher.Invoke(new Action(() =>
                        {
                            ColorIcon = new SolidColorBrush(Colors.Red);
                        }));
                        Message = "Thao tác thất bại";
                    }
                    else
                    {
                        Icon = GeometryConst.IconCheck;
                        process.Dispatcher.Invoke(new Action(() =>
                        {
                            ColorIcon = new SolidColorBrush(Colors.LightGreen);
                        }));
                        Message = "Thao tác thành công";
                    }
                });

                Task.WaitAll(task);
            }

        }

        private void HideAnimation_Completed(object sender, System.EventArgs e)
        {
            if (parent != null)
                parent.Children.Remove(process);
        }

        public void BtnClose_Clicked(object sender, RoutedEventArgs e)
        {
            hideProcessItem();
        }

        public void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            hideProcessItem();
        }

        public void hideProcessItem(Panel container = null)
        {
            if (container == null)
            {
                container = process.Parent as Panel;
            }
            parent = container;
            hideAnimation.Begin();
        }
    }

    public class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public async Task loadData(string nameProperty)
        {
            await Task.Run(() =>
            {
                OnPropertyChanged(nameProperty);
            });
        }
    }
    class RelayCommand<T> : ICommand
    {
        private readonly Predicate<T> _canExecute;
        private readonly Action<T> _execute;

        public RelayCommand(Predicate<T> canExecute, Action<T> execute)
        {
            if (execute == null)
                throw new ArgumentNullException("execute");
            _canExecute = canExecute;
            _execute = execute;
        }

        public bool CanExecute(object parameter)
        {
            try
            {
                return _canExecute == null ? true : _canExecute((T)parameter);
            }
            catch
            {
                return true;
            }
        }

        public void Execute(object parameter)
        {
            _execute((T)parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
}
