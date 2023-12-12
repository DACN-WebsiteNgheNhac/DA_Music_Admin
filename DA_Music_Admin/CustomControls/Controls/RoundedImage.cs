using System.ComponentModel;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows.Threading;
using System.Windows;

namespace CustomControls.Controls
{
    public class RoundedImage : Border, INotifyPropertyChanged
    {
        private Brush _LoadingColor = Brushes.LightGray;
        public Brush LoadingColor
        {
            get { return _LoadingColor; }
            set { _LoadingColor = value; }
        }

        public string ImageURL
        {
            get { return (string)GetValue(ImageURLProperty); }
            set { SetValue(ImageURLProperty, value); _ = loadData(nameof(ImageURL)); }
        }

        private Stretch _StretchImage = Stretch.UniformToFill;
        public Stretch StretchImage
        {
            get { return _StretchImage; }
            set { _StretchImage = value; }
        }



        //AdornerLayer adornerLayer;
        //ImageLoadingAdorner imageLoadingAdorner;
        //Adorner[] adorners;

        public RoundedImage()
        {
            DefaultProperty();

            DataContextChanged += RoundedImage_DataContextChanged;
            Loaded += RoundedImage_Loaded;
        }

        private void RoundedImage_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            ChangeData();
        }

        private void RoundedImage_Loaded(object sender, RoutedEventArgs e)
        {
            ChangeData();
        }

        public async void ChangeData()
        {
            ShowLoadingEffect();
            await SetImage();
        }

        protected async Task SetImage()
        {
            await Task.Run(() =>
            {
                Dispatcher.Invoke(async () =>
                {
                    ImageBrush imageBrush = new ImageBrush();
                    imageBrush.Stretch = StretchImage;

                    var bitmapImage = await LoadImageAsync(ImageURL);
                    imageBrush.ImageSource = bitmapImage;
                    Background = imageBrush;

                    HideLoadingEffect();
                });
            });
        }

        private async Task<BitmapImage> LoadImageAsync(string imageUrl)
        {
            using (var httpClient = new HttpClient())
            {
                var imageBytes = await httpClient.GetByteArrayAsync(imageUrl);

                var bitmapImage = new BitmapImage();

                using (var stream = new System.IO.MemoryStream(imageBytes))
                {
                    bitmapImage.BeginInit();
                    bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                    bitmapImage.StreamSource = stream;
                    bitmapImage.EndInit();
                }

                bitmapImage.Freeze();
                return bitmapImage;
            }
        }

        protected void ShowLoadingEffect()
        {
            Background = LoadingColor;
        }

        protected void HideLoadingEffect()
        {
        }

        protected void DefaultProperty()
        {
            SnapsToDevicePixels = true;
            Background = LoadingColor;

            //if (adornerLayer == null)
            //    adornerLayer = AdornerLayer.GetAdornerLayer(this);
            //if (imageLoadingAdorner == null)
            //    imageLoadingAdorner = new ImageLoadingAdorner(this);
            //if (adorners == null)
            //    adorners = adornerLayer.GetAdorners(this);
        }


        // Register DependencyProperty for ImageURL
        public static readonly DependencyProperty ImageURLProperty =
        DependencyProperty.Register(
            nameof(ImageURL),
            typeof(string),
            typeof(RoundedImage),
            new PropertyMetadata(default(string))
        );

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

    internal class RectangleImageLoadingAdorner : Adorner
    {
        RoundedImage target;
        double radiusX;
        double radiusY;
        Rect rect;

        Brush loadingColor = Brushes.Red;

        public RectangleImageLoadingAdorner(UIElement adornedElement) : base(adornedElement)
        {
            target = adornedElement as RoundedImage;    
            radiusX = target.CornerRadius.TopLeft;
            radiusY = target.CornerRadius.BottomRight;
            rect = new Rect(0, 0, target.RenderSize.Width, target.RenderSize.Height);
            loadingColor = target.LoadingColor;
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            drawingContext.DrawRoundedRectangle(loadingColor, null, rect, radiusX, radiusY);
        }
    }

}
