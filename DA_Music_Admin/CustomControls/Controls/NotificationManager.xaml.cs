using CustomControls.Utils;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace CustomControls.Controls
{
  
    public partial class NotificationManager : UserControl
    {
        public NotificationManager()
        {
            InitializeComponent();
        }

        List<NotificationAlert> alerts = new List<NotificationAlert>();

        public Task showNotify(Geometry icon, string title, string message, SolidColorBrush iconColor)
        {
            this.Dispatcher.Invoke(new Action(() =>
            {
                if (spContainer.Children.Count == alerts.Count)
                {
                    NotificationAlert notification = createNotify(icon, title, message, iconColor);
                    alerts.Add(notification);
                    spContainer.Children.Add(notification);
                }
                else
                {
                    foreach (var item in alerts)
                    {
                        if (!spContainer.Children.Contains(item))
                        {
                            spContainer.Children.Add(setDataNofify(item, icon, title, message, iconColor));
                            break;
                        }
                    }
                }
            }));
            return null;
        }

        protected NotificationAlert createNotify(Geometry icon, string title, string message, SolidColorBrush iconColor)
        {
            NotificationAlert notification = new NotificationAlert(icon, title, message);
            notification.viewModel.BackgroundColor = new SolidColorBrush(ColorConst.subBackgroundColor);
            notification.viewModel.ForegroundColor = new SolidColorBrush(ColorConst.foregroundColor);
            if (iconColor != null)
            {
                notification.viewModel.ColorIcon = iconColor;
                notification.viewModel.ColorProgress = iconColor;
            }
            return notification;
        }

        protected NotificationAlert setDataNofify(NotificationAlert notification, Geometry icon, string title, string message
            , SolidColorBrush iconColor)
        {
            notification.viewModel.Icon = icon;
            notification.viewModel.Title = title;
            notification.viewModel.Message = message;

            if(iconColor != null)
            {
                notification.viewModel.ColorIcon = iconColor;
                notification.viewModel.ColorProgress = iconColor;
            }

            return notification;
        }
    }
}
