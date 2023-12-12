using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace CustomControls.Utils
{
    public enum OptionEasingFunction
    {
        BackEase = 1,
        BounceEase,
        CircleEase,
        CubicEase,
        ElasticEase,
        ExponentialEase,
        PowerEase,
        QuadraticEase,
        QuarticEase,
        QuinticEase,
        SineEase
    }

    public enum OptionTargetTransform
    {
        ScaleTranform = 1,
        RotateTranform,
        SkewTransform,
        TranslateTransform
    }

    public class MyAnimation
    {
        private int _FPS = 144;
        
        public int FPS
        {
            get { return _FPS; }
            set {
                if (FPS < 0)
                    value = 144;
                _FPS = value;
                
            }
        }

        private EasingMode _EasingMode = EasingMode.EaseIn;

        public EasingMode EasingMode
        {
            get { return _EasingMode; }
            set { _EasingMode = value; }
        }


        public MyAnimation()
        {
            FPS = 144;
        }

        public MyAnimation(int FPS)
        {
            this.FPS = FPS;
        }
        public MyAnimation(int FPS, EasingMode EasingMode)
        {
            this.FPS = FPS;
            this.EasingMode = EasingMode;
        }


        #region Tranform
        public static DependencyProperty RotateTranform = RotateTransform.AngleProperty;
        public static DependencyProperty ScaleXTranform = ScaleTransform.ScaleXProperty;
        public static DependencyProperty ScaleYTranform = ScaleTransform.ScaleYProperty;
        public static DependencyProperty SkewXTransform = SkewTransform.AngleXProperty;
        public static DependencyProperty SkewYTransform = SkewTransform.AngleXProperty;
        public static DependencyProperty TranslateXTransform = TranslateTransform.XProperty;
        public static DependencyProperty TranslateYTransform = TranslateTransform.YProperty;

        #endregion


        public virtual void StartAnimation(FrameworkElement target, DependencyProperty typeAnimation, double toValue, int beginTime, int Duration, double fromValue = -1, bool IsUnlimited = false, bool AutoReverse = false, FillBehavior FillBehavior = FillBehavior.HoldEnd)
        {
            target.Dispatcher.Invoke(new Action(() =>
            {

                Storyboard storyboard = new Storyboard();

                DoubleAnimation animation = new DoubleAnimation(toValue, TimeSpan.FromMilliseconds(Duration));
                if (IsUnlimited)
                    animation.RepeatBehavior = RepeatBehavior.Forever;
                if (AutoReverse)
                    animation.AutoReverse = true;
                animation.FillBehavior = FillBehavior;

                animation.BeginTime = TimeSpan.FromMilliseconds(beginTime);
                storyboard.Children.Add(animation);

                Timeline.SetDesiredFrameRate(animation, FPS);

                Storyboard.SetTargetProperty(animation, new PropertyPath(typeAnimation));
                Storyboard.SetTarget(animation, target);
                storyboard.Begin();

            }));
        }

        public virtual void StartAnimationWithEaseFunction(FrameworkElement target, DependencyProperty typeAnimation, int optionEasingFunction, double toValue, int beginTime, int Duration, double fromValue = -1, bool IsUnlimited = false, bool AutoReverse = false, FillBehavior FillBehavior = FillBehavior.HoldEnd)
        {
            target.Dispatcher.Invoke(new Action(() =>
            {
                Storyboard storyboard = new Storyboard();

                DoubleAnimation animation = new DoubleAnimation(toValue, TimeSpan.FromMilliseconds(Duration));
                if (IsUnlimited)
                    animation.RepeatBehavior = RepeatBehavior.Forever;
                if (AutoReverse)
                    animation.AutoReverse = true;
                animation.FillBehavior = FillBehavior;
                animation.BeginTime = TimeSpan.FromMilliseconds(beginTime);
                animation.EasingFunction = GetEasingFunction(optionEasingFunction);

                storyboard.Children.Add(animation);

                Timeline.SetDesiredFrameRate(animation, FPS);

                Storyboard.SetTargetProperty(animation, new PropertyPath(typeAnimation));
                Storyboard.SetTarget(animation, target);
                storyboard.Begin();

            }));
        }


        /// <summary>
        /// Start animation related to RenderTransform
        /// Width option = 
        /// <list type="bullet">
        /// <item>
        /// <description>1 - ScaleTransform</description>
        /// </item>
        /// <item>
        /// <description>2 - RotateTransform</description>
        /// </item>
        /// <item>
        /// <description>3 - SkewTransform</description>
        /// </item>
        /// <item>
        /// <description>4 - TranslateTransform</description>
        /// </item>
        /// </list>
        /// </summary>
        public virtual void StartRenderTransformAnimation(FrameworkElement target, DependencyProperty typeAnimation, int option, double toValue, int beginTime, int Duration, double fromValue = -1, bool IsUnlimited = false, bool AutoReverse = false)
        {
            target.Dispatcher.Invoke(new Action(() =>
            {
                DoubleAnimation animation = new DoubleAnimation(toValue, TimeSpan.FromMilliseconds(Duration));
                if (IsUnlimited)
                    animation.RepeatBehavior = RepeatBehavior.Forever; 
                if (AutoReverse)
                    animation.AutoReverse = true;

                animation.BeginTime = TimeSpan.FromMilliseconds(beginTime);
                Timeline.SetDesiredFrameRate(animation, FPS);

                Transform targetTransform = GetTransform(option);
                target.RenderTransform = targetTransform;
              
                targetTransform.BeginAnimation(typeAnimation, animation);
            }));
        }

        /// <summary>
        /// Start animation related to RenderTransform
        /// Width option = 
        /// <list type="bullet">
        /// <item>
        /// <description>1 - ScaleTransform</description>
        /// </item>
        /// <item>
        /// <description>2 - RotateTransform</description>
        /// </item>
        /// <item>
        /// <description>3 - SkewTransform</description>
        /// </item>
        /// <item>
        /// <description>4 - TranslateTransform</description>
        /// </item>
        /// </list>
        /// </summary> 
        /// Width option = 
        /// <list type="bullet">
        /// <item>
        /// <description>1 - BackEase</description>
        /// </item>
        /// <item>
        /// <description>2 - BounceEase</description>
        /// </item>
        /// <item>
        /// <description>3 - CircleEase</description>
        /// </item>
        /// <item>
        /// <description>4 - CubicEase</description>
        /// </item> 
        /// <item>
        /// <description>5 - ElasticEase</description>
        /// </item> 
        /// <item>
        /// <description>6 - ExponentialEase</description>
        /// </item> 
        /// <item>
        /// <description>7 - PowerEase</description>
        /// </item> 
        /// <item>
        /// <description>8 - QuadraticEase</description>
        /// </item> 
        /// <item>
        /// <description>9 - QuarticEase</description>
        /// </item> 
        /// <item>
        /// <description>10 - QuinticEase</description>
        /// </item>  
        /// <item>
        /// <description>11 - SineEase</description>
        /// </item> 
        /// </list>
        /// </summary>
        public virtual void StartRenderTransformAnimationWithEaseFunction(FrameworkElement target, DependencyProperty typeAnimation, int optionTargetTransform, int optionEasingFunction, double toValue, int beginTime, int Duration, double fromValue = -1, bool IsUnlimited = false, bool AutoReverse = false)
        {
            target.Dispatcher.Invoke(new Action(() =>
            {
                DoubleAnimation animation = new DoubleAnimation(toValue, TimeSpan.FromMilliseconds(Duration));
                if (IsUnlimited)
                    animation.RepeatBehavior = RepeatBehavior.Forever;
                if (AutoReverse)
                    animation.AutoReverse = true;

                animation.EasingFunction = GetEasingFunction(optionEasingFunction);

                animation.BeginTime = TimeSpan.FromMilliseconds(beginTime);
                Timeline.SetDesiredFrameRate(animation, FPS);

                Transform targetTransform = GetTransform(optionTargetTransform);
                target.RenderTransform = targetTransform;

                targetTransform.BeginAnimation(typeAnimation, animation);
            }));
        }
 

        /// <summary>
        /// Get transform
        /// Width option = 
        /// <list type="bullet">
        /// <item>
        /// <description>1 - ScaleTransform</description>
        /// </item>
        /// <item>
        /// <description>2 - RotateTransform</description>
        /// </item>
        /// <item>
        /// <description>3 - SkewTransform</description>
        /// </item>
        /// <item>
        /// <description>4 - TranslateTransform</description>
        /// </item> 
        /// </list>
        /// </summary>
        protected virtual Transform GetTransform(int option)
        {
            Transform returnValue = new TranslateTransform();

            switch (option)
            {
                case 1:
                    returnValue = new ScaleTransform();
                    break;
                case 2:
                    returnValue = new RotateTransform();
                    break;
                case 3:
                    returnValue = new SkewTransform();
                    break;
                case 4:
                    returnValue = new TranslateTransform();
                    break;
                
                default:
                    break;
            }

            return returnValue;
        }

        /// <summary>
        /// Get transform
        /// Width option = 
        /// <list type="bullet">
        /// <item>
        /// <description>1 - BackEase</description>
        /// </item>
        /// <item>
        /// <description>2 - BounceEase</description>
        /// </item>
        /// <item>
        /// <description>3 - CircleEase</description>
        /// </item>
        /// <item>
        /// <description>4 - CubicEase</description>
        /// </item> 
        /// <item>
        /// <description>5 - ElasticEase</description>
        /// </item> 
        /// <item>
        /// <description>6 - ExponentialEase</description>
        /// </item> 
        /// <item>
        /// <description>7 - PowerEase</description>
        /// </item> 
        /// <item>
        /// <description>8 - QuadraticEase</description>
        /// </item> 
        /// <item>
        /// <description>9 - QuarticEase</description>
        /// </item> 
        /// <item>
        /// <description>10 - QuinticEase</description>
        /// </item>  
        /// <item>
        /// <description>11 - SineEase</description>
        /// </item> 
        /// </list>
        /// </summary>
        protected virtual EasingFunctionBase GetEasingFunction(int option)
        {
            EasingFunctionBase returnValue = new SineEase();
             
            switch (option)
            {
                case 1:
                    returnValue = new BackEase();
                    break;
                case 2:
                    returnValue = new BounceEase();
                    break;
                case 3:
                    returnValue = new CircleEase();
                    break;
                case 4:
                    returnValue = new CubicEase();
                    break;
                case 5:
                    returnValue = new ElasticEase(); 
                    break;
                case 6:
                    returnValue = new ExponentialEase();
                    break;
                case 7:
                    returnValue = new PowerEase();
                    break;
                case 8:
                    returnValue = new QuadraticEase();
                    break;
                case 9:
                    returnValue = new QuarticEase();
                    break;
                case 10:
                    returnValue = new QuinticEase();
                    break;
                case 11:
                    returnValue = new SineEase();
                    break;
                default:
                    break;
            }

            returnValue.EasingMode = this.EasingMode;
            return returnValue;
        }
    }
}
