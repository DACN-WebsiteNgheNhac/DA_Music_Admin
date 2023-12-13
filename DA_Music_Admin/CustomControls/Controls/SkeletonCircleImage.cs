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
    public class SkeletonCircleImage : Border, INotifyPropertyChanged
    {
        public static readonly DependencyProperty LoadingColorProperty =
        DependencyProperty.Register(
            nameof(LoadingColor),
            typeof(Brush),
            typeof(SkeletonCircleImage),
            new PropertyMetadata(Brushes.LightGray));

        private Brush _LoadingColor = Brushes.LightGray;
        public Brush LoadingColor
        {
            get { return _LoadingColor; }
            set { _LoadingColor = value; }
        }

        public string ImageURL
        {
            get { return (string)GetValue(ImageURLProperty); }
            set { SetValue(ImageURLProperty, value); _ = loadData(nameof(ImageURL)); ChangeData(); }
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

        public SkeletonCircleImage()
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
            typeof(SkeletonCircleImage),
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

    internal class CircleImageLoadingAdorner : Adorner
    {
        SkeletonCircleImage target;
        double radiusX;
        double radiusY;
        Point centerPoint;

        Brush loadingColor = Brushes.Red;

        public CircleImageLoadingAdorner(UIElement adornedElement) : base(adornedElement)
        {
            target = adornedElement as SkeletonCircleImage;
            var width = target.RenderSize.Width;
            var height = target.RenderSize.Height;
            radiusX = width;
            radiusY = height;
            centerPoint = new Point(width / 2, height / 2);
            loadingColor = target.LoadingColor;
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            drawingContext.DrawEllipse(loadingColor, null, centerPoint, radiusX, radiusY);
        }
    }

}
