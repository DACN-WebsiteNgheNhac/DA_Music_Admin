using CustomControls.Controls;
using DA_Music_Admin.Views;
using System;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using TaskStatus = CustomControls.Controls.TaskStatus;

namespace DA_Music_Admin.SystemInfor
{
    public class ViewHolder
    {
        private static ViewHolder _Ins;
        public static ViewHolder Ins
        {
            get
            {
                if (_Ins == null)
                    _Ins = new ViewHolder();
                return _Ins;
            }
            private set { _Ins = value; }
        }
              
        public ViewHolder()
        {
            
        }

        public void Register(LoadingScreenSpinnerRing loadingScreen, Frame fContainer
            , NotificationManager notificationManager, ProcessManager processManager)
        {
            LoadingScreen = loadingScreen;
            FContainer = fContainer;
            NotificationManager = notificationManager;
            ProcessManager = processManager;
        }

        private Frame _FContainer;
        public Frame FContainer
        {
            get { return _FContainer; }
            set { _FContainer = value; }
        }

        public void DirectPage(Page target)
        {
            if (FContainer.Content == target)
                return;
            FContainer.Dispatcher.Invoke(() =>
            {
                FContainer.Content = target;
                ViewHolder.Ins.ShowLoadingScreen();

            });
        }



        #region Loading Screen
        private LoadingScreenSpinnerRing _LoadingScreen;
        public LoadingScreenSpinnerRing LoadingScreen
        {
            get 
            { 
                return _LoadingScreen;
            }
            set { _LoadingScreen = value; }
        }

       

        public void ShowLoadingScreen()
        {
            LoadingScreen.showLoadingScreen();
        }

        public void HideLoadingScreen()
        {
            LoadingScreen.hideLoadingScreen();
        }
        #endregion


        #region Notification

        private NotificationManager _NotificationManager;
        public NotificationManager NotificationManager
        {
            get { return _NotificationManager; }
            set { _NotificationManager = value; }
        }

        public void ShowNotify(Geometry icon, string title, string message, SolidColorBrush iconColor)
        {
            if(NotificationManager != null)
            {
                NotificationManager.showNotify(icon, title, message, iconColor);
            }
        }

        public void ShowFailNotify(string msg)
        {
            var iconColor = new SolidColorBrush(Colors.Red);
            ShowNotify(CustomControls.Utils.GeometryConst.IconError, "Thao tác thất bại", msg, iconColor);
        }

        public void ShowSuccessNotify(string msg)
        {
            var iconColor = new SolidColorBrush(Colors.LightGreen);
            ShowNotify(CustomControls.Utils.GeometryConst.IconCheck, "Thao tác thành công", msg, iconColor);
        }

        #endregion Notification

        #region Process

        private ProcessManager _ProcessManager;
        public ProcessManager ProcessManager
        {
            get { return _ProcessManager; }
            set { _ProcessManager = value; }
        }

        public void AddProcessItem(Geometry icon, string title, string message, Func<Task<TaskStatus>> task)
        {
            if(ProcessManager != null)
            {
                ProcessManager.showProcessItem(icon, title, message, task);
            }
        }
        #endregion Process



    }
}
