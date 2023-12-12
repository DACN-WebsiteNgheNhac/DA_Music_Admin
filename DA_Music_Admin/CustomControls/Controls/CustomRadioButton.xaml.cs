
using CustomControls.Utils;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace CustomControls.Controls
{
    public partial class CustomRadioButton : RadioButton
    {
        #region Colors
        private Color _BorderColor;
        public Color BorderColor
        {
            get { return _BorderColor; }
            set { _BorderColor = value; }
        }

        private Color _BackgroundColor;
        public Color BackgroundColor
        {
            get { return _BackgroundColor; }
            set { _BackgroundColor = value; }
        }

        private Color _ForegroundColor;
        public Color ForegroundColor
        {
            get { return _ForegroundColor; }
            set { _ForegroundColor = value; }
        }

        private Color _HoveredBackgroundColor;
        public Color HoveredBackgroundColor
        {
            get { return _HoveredBackgroundColor; }
            set { _HoveredBackgroundColor = value; }
        }

        private Color _HoveredForegroundColor;
        public Color HoveredForegroundColor
        {
            get { return _HoveredForegroundColor; }
            set { _HoveredForegroundColor = value; }
        }
        #endregion

        Border border;

        public void setChecked(bool isChecked)
        {
            if (isChecked)
            {
                ChangeColor();
            }
            else
            {
                RecoveryColor();
            }
            IsChecked = isChecked;
        }

        private string _Icon;
        public string Icon
        {
            get { return _Icon; }
            set
            {
                _Icon = value;
                icon.Data = Geometry.Parse(value);
            }
        }

        private string _Text;

        public string Text
        {
            get { return _Text; }
            set
            {
                _Text = value;
                tbl.Text = value;
                this.ToolTip = value;
            }
        }

        private bool _IsMainMenu;
        public bool IsMainMenu
        {
            get { return _IsMainMenu; }
            set { _IsMainMenu = value; }
        }


        private Thickness _PaddingLeft;

        public Thickness PaddingLeft
        {
            get { return _PaddingLeft; }
            set { _PaddingLeft = value; }
        }

        public bool IsFirstChecked { get; set; } = false;

        public CustomRadioButton()
        {
            InitializeComponent();

            BorderColor = Colors.Red;

            ForegroundColor = Colors.Black;
            BackgroundColor = Colors.Transparent;

            HoveredForegroundColor = Colors.Red;
            HoveredBackgroundColor = Colors.Yellow;
        }

       

        private void CustomRadioButton_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if(ActualWidth < 80)
            {
                tbl.Visibility = Visibility.Collapsed;
                hiddenIcon.Visibility = Visibility.Collapsed;
            }
            else
            {
                
                tbl.Visibility = Visibility.Visible;
                hiddenIcon.Visibility = Visibility.Visible;
            }
        }

        public CustomRadioButton(string icon, string content, bool isFirstChecked = false)
        {
            InitializeComponent();
            Init(icon, content);
            IsMainMenu = true;
            IsFirstChecked = isFirstChecked;
            SizeChanged += CustomRadioButton_SizeChanged;
        }

        public CustomRadioButton(string icon, string content, Thickness paddingLeft)
        {
            InitializeComponent();
            Init(icon, content);
            IsMainMenu = false;
            PaddingLeft = paddingLeft;
            SizeChanged += CustomRadioButton_SizeChanged;
        }

        private void Init(string icon, string content)
        {
            BorderColor = ColorConst.iconColor;

            ForegroundColor = ColorConst.foregroundColor;
            BackgroundColor = ColorConst.backgroundColor;

            HoveredForegroundColor = ColorConst.foregroundColor;
            HoveredBackgroundColor = ColorConst.hoveredBackgroundColor;

            Icon = icon;
            Text = content;

            RecoveryColor();
        }

        private void MainLoaded(object sender, RoutedEventArgs e)
        {
            GroupName = "CustomRadioButton";
            border = this.Template.FindName("border", this) as Border;

            if (PaddingLeft != null)
            {
                ColumnDefinition firstColumn = grid.ColumnDefinitions[0];
                firstColumn.Width = new GridLength(PaddingLeft.Left);
            }

            if (IsFirstChecked)
                IsChecked = true;
            
        }

        private void MainMouseEnter(object sender, MouseEventArgs e)
        {
            ChangeColor();
        }

        private void MainMouseLeave(object sender, MouseEventArgs e)
        {
            RecoveryColor();
        }


        private void MainChecked(object sender, RoutedEventArgs e)
        {
            border.BorderBrush = new SolidColorBrush(BorderColor);
            ChangeColor();
        }
        private void main_Unchecked(object sender, RoutedEventArgs e)
        {
            RecoveryColor();
        }

        private void main_Click(object sender, RoutedEventArgs e)
        {
            if (IsChecked == true)
            {
                ChangeColor();
            }
            else
            {
                RecoveryColor();
            }

        }

        private void ChangeColor()
        {
            icon.Fill = new SolidColorBrush(HoveredForegroundColor);
            icon2.Fill = new SolidColorBrush(HoveredForegroundColor);
            tbl.Foreground = new SolidColorBrush(HoveredForegroundColor);
            grid.Background = new SolidColorBrush(HoveredBackgroundColor);
        }

        private void RecoveryColor()
        {
            if (IsChecked != true)
            {
                icon.Fill = new SolidColorBrush(ForegroundColor);
                icon2.Fill = new SolidColorBrush(ForegroundColor);
                tbl.Foreground = new SolidColorBrush(ForegroundColor);
                grid.Background = new SolidColorBrush(BackgroundColor);
            }
        }

    }
}
