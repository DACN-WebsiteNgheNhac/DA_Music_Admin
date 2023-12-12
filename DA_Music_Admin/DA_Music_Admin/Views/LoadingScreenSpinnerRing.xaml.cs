using CustomControls.Utils;
using System;
using System.Windows;
using System.Windows.Controls;

namespace DA_Music_Admin.Views
{

    public partial class LoadingScreenSpinnerRing : UserControl
    {
        MyAnimation animation = new MyAnimation(60);
        public LoadingScreenSpinnerRing()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            int index = 1;
            foreach (FrameworkElement item in g_Container.Children)
            {
                animation.StartRenderTransformAnimationWithEaseFunction(item, MyAnimation.RotateTranform, (int)OptionTargetTransform.RotateTranform, (int)OptionEasingFunction.BackEase, 360, 100 * index, 3000, IsUnlimited: true, AutoReverse: true);
                index++;
            }
        }

       public void showLoadingScreen()
       {
            this.Visibility = Visibility.Visible;
            //if(!parent.Children.Contains(this))
            //    parent.Children.Add(this);
        }

       public void hideLoadingScreen()
       {
            this.Visibility = Visibility.Hidden;
            GC.Collect();
            //if (this.Parent == null)
            //    return;
            //if (!(this.Parent is Panel))
            //    return;
            //parent.Children.Remove(this);
       }
       
    }
}
