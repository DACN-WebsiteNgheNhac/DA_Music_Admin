using CustomControls.Utils;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace CustomControls.Controls
{
    public partial class ProcessManager : UserControl
    {
        public event EventHandler ProcessChanged;

        List<ProcessItem> lstProcess = new List<ProcessItem>();

        public ProcessManager()
        {
            InitializeComponent();
            Loaded += ProcessManager_Loaded;
            Unloaded += ProcessManager_Unloaded;
            
        }
   

        private void ProcessManager_Loaded(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.PreviewMouseMove += MainWindow_PreviewMouseMove;
            Application.Current.MainWindow.PreviewMouseLeftButtonUp += MainWindow_PreviewMouseLeftButtonUp;
        }

        private void ProcessManager_Unloaded(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.PreviewMouseMove -= MainWindow_PreviewMouseMove;
            Application.Current.MainWindow.PreviewMouseLeftButtonUp -= MainWindow_PreviewMouseLeftButtonUp;
        }
       

        public Task showProcessItem(Geometry icon, string title, string message, Func<Task<TaskStatus>> task)
        {
            if(ProcessChanged != null)
            {
                ProcessChanged(this, new EventArgs());
            }
            Dispatcher.Invoke(new Action(() =>
            {
                if (spContainer.Children.Count == lstProcess.Count)
                {
                    ProcessItem process = createProcessItem(icon, title, message, task);
                    lstProcess.Add(process);
                    spContainer.Children.Insert(0, process);
                }
                else
                {
                    foreach (var item in lstProcess)
                    {
                        if (!spContainer.Children.Contains(item))
                        {
                            spContainer.Children.Insert(0, setDataProcessItem(item, icon, title, message, task));
                            break;
                        }
                    }
                }
            }));
            return null;
        }

        protected ProcessItem createProcessItem(Geometry icon, string title, string message, Func<Task<TaskStatus>> task)
        {
            ProcessItem processItem = new ProcessItem(title, message, task);
            processItem.viewModel.BackgroundColor = new SolidColorBrush(ColorConst.subBackgroundColor);
            processItem.viewModel.ForegroundColor = new SolidColorBrush(ColorConst.foregroundColor_60);
            processItem.viewModel.MissionTask = task;
            processItem.HorizontalAlignment = HorizontalAlignment.Stretch;
            return processItem;
        }

        protected ProcessItem setDataProcessItem(ProcessItem processItem, Geometry icon, string title, string message, Func<Task<TaskStatus>> task)
        {
            processItem.viewModel.Icon = icon;
            processItem.viewModel.Title = title;
            processItem.viewModel.Message = message;
            return processItem;
        }

        #region Resize
        private bool isResizing;
        public bool IsResizing
        {
            get { return isResizing; } 
            set
            {
                isResizing = value;
                leftEdge.Fill = value == true ? new SolidColorBrush(ColorConst.iconColor) 
                    : new SolidColorBrush(Colors.Transparent);
            }
        }

        private double originalMouseX;
        private double originalWidth;

        private void LeftGrip_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            IsResizing = true;
            originalMouseX = e.GetPosition(Application.Current.MainWindow).X;
            originalWidth = ActualWidth;
        }

        private void MainWindow_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (IsResizing)
            {
                double curMouseX = e.GetPosition(Application.Current.MainWindow).X;
                double widthDiff =  originalMouseX - curMouseX;
                double newWidth = originalWidth + widthDiff;
                Width = newWidth > 0 ? originalWidth + widthDiff : originalWidth;
            }
        }

        private void MainWindow_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            IsResizing = false;
        }
        #endregion Resize
    }
}
