using MaterialDesignThemes.Wpf;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;

namespace DA_Music_Admin.Sources.CustomControls
{
    public partial class InformationCard : UserControl
    {
        private PackIconKind _Icon = PackIconKind.Music;
        public PackIconKind Icon
        {
            get { return _Icon; }
            set { _Icon = value; PackIcon.Kind = value; }
        }

        private string _Title;
        public string Title
        {
            get { return _Title; }
            set { _Title = value; tblTitle.Text = value; }
        }

        private string _SubTitle;
        public string SubTitle
        {
            get { return _SubTitle; }
            set { _SubTitle = value; tblSubTitle.Text = value; }
        }


        private SolidColorBrush _BackgroundIconColor = new SolidColorBrush(Colors.CornflowerBlue);
        public SolidColorBrush BackgroundIconColor
        {
            get { return _BackgroundIconColor; }
            set { _BackgroundIconColor = value; BackgroundIcon.Background = value; }
        }

        private SolidColorBrush _ForegroundIconColor = new SolidColorBrush(Colors.White);
        public SolidColorBrush ForegroundIconColor
        {
            get { return _ForegroundIconColor; }
            set { _ForegroundIconColor = value; PackIcon.Foreground = value; }
        }

        private SolidColorBrush _BackgroundCardColor = new SolidColorBrush(Colors.Black);
        public SolidColorBrush BackgroundCardColor
        {
            get { return _BackgroundCardColor; }
            set { _BackgroundCardColor = value; Border.Background = value; }
        }
        
        private SolidColorBrush _ForegroundTitle = new SolidColorBrush(Colors.CornflowerBlue);
        public SolidColorBrush ForegroundTitle
        {
            get { return _ForegroundTitle; }
            set { _ForegroundTitle = value; tblTitle.Foreground = value; }
        }  
        
        private SolidColorBrush _ForegroundSubTitle = new SolidColorBrush(Colors.CornflowerBlue);
        public SolidColorBrush ForegroundSubTitle
        {
            get { return _ForegroundSubTitle; }
            set { _ForegroundSubTitle = value; tblSubTitle.Foreground = value; }
        }

        public void SetData(InformationCardModel model)
        {
            Title = model.Title.ToString();
            SubTitle = model.SubTitle.ToString();
            Icon = model.Icon;
            BackgroundCardColor = new SolidColorBrush(model.BackgroundColor);
            BackgroundIconColor = new SolidColorBrush(model.IconColor);
            ForegroundIconColor = new SolidColorBrush(model.ForegroundIconColor);
            ForegroundTitle = new SolidColorBrush(model.ForegroundTitle);
            ForegroundSubTitle = new SolidColorBrush(model.ForegroundSubTitle);
        }

        public InformationCard()
        {
            InitializeComponent();
        }
    }

    public class InformationCardModel
    {
        public PackIconKind Icon { get; set; }
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public Color BackgroundColor { get; set; }
        public Color IconColor { get; set; }
        public Color ForegroundIconColor { get; set; } = Colors.White;
        public Color ForegroundTitle { get; set; }
        public Color ForegroundSubTitle { get; set; }
    }
}
