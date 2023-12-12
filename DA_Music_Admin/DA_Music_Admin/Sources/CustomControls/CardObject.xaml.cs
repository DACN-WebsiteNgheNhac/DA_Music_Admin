using DA_Music_Admin.SystemInfor;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace DA_Music_Admin.Sources.CustomControls
{
    public partial class CardObject : UserControl
    {
        public CardObjectViewModel vm;

        private BaseModel _Data;

        public BaseModel Data
        {
            get { return _Data = _Data == null ? vm.Data : _Data ; }
            set { _Data = value; vm.Data = value; }
        }


        public CardObject()
        {
            InitializeComponent();
            this.DataContext = vm = new CardObjectViewModel(this);
        }

        public CardObject(BaseModel data)
        {
            InitializeComponent();
            this.DataContext = vm = new CardObjectViewModel(this, data);
        }

        private void UserControl_PreviewMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (vm != null)
                vm.UserControl_PreviewMouseLeftButtonDown(sender, e);
        }

        private void UserControl_PreviewQueryContinueDrag(object sender, QueryContinueDragEventArgs e)
        {
            if (vm != null)
                vm.UserControl_PreviewQueryContinueDrag(sender, e);
        }


        public static CardObject findItem(Panel container, BaseModel data)
        {
            if (container == null)
                return null;
            for (int i = 0; i < container.Children.Count; i++)
            {
                CardObject item = container.Children[i] as CardObject;
                if (item != null)
                {
                    if (item.Data == data)
                    {
                        return item;
                    }
                }
            }
            return null;
        }
    }

    public class CardObjectViewModel : INotifyPropertyChanged
    {
        private BaseModel _Data;
        public BaseModel Data
        {
            get { return _Data; }
            set
            {
                _Data = value;
                if (value is Song)
                {
                    Song temp = value as Song;
                    Title = temp!.Name;
                    Description = temp.Area ;
                    if (temp != null && temp.CreatedAt != null)
                        Description += " - " + temp.CreatedAt.Value.ToString("dd-MM-yyyy");

                }
                else if (value is Artist)
                {
                    Artist temp = value as Artist;
                    Title = temp.ArtistName;
                    Description = temp.Name + " - " + temp.Gender + " - " + temp.National;
                }
                ImageFile = value.Image == string.Empty ? null : new BitmapImage(new System.Uri(value.Image));
                OnPropertyChanged("Data");
            }
        }

        private BitmapImage _ImageFile;
        public BitmapImage ImageFile
        {
            get
            {
                //if (_ImageFile == null)
                //    _ImageFile = convertBitmapToBitmapImage(Properties.Resources.defaultImage);
                return _ImageFile;
            }
            set
            {
                _ImageFile = value /*== null ? convertBitmapToBitmapImage(Properties.Resources.defaultImage) : value*/;
                OnPropertyChanged("ImageFile");
            }
        }

        CardObject view;

        public CardObjectViewModel(CardObject view)
        {
            this.view = view;
        }

        public CardObjectViewModel(CardObject view, BaseModel data)
        {
            this.view = view;
            this.Data = data;
        }

        //private BitmapImage convertBitmapToBitmapImage(Bitmap bitmap)
        //{
        //    BitmapImage bitmapImage;
        //    using (MemoryStream memoryStream = new MemoryStream())
        //    {
        //        bitmap.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png);

        //        bitmapImage = new BitmapImage();

        //        bitmapImage.BeginInit();
        //        bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
        //        bitmapImage.StreamSource = memoryStream;
        //        bitmapImage.EndInit();
        //        bitmapImage.Freeze();
        //    }
        //    return bitmapImage;
        //}

        private string _Title = "";
        public string Title
        {
            get { return _Title; }
            set { _Title = value; OnPropertyChanged("Title"); }
        }

        private string _Description = "";
        public string Description
        {
            get { return _Description; }
            set { _Description = value; OnPropertyChanged("Description"); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void UserControl_PreviewMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            DragItemHelper.Ins.setDataDymnamicItem(Data);
            view.Opacity = 0.6;
            view.Focus();
            DragDrop.DoDragDrop(view, new DataObject(DataFormats.Serializable, Data), DragDropEffects.Move);
        }

        public void UserControl_PreviewQueryContinueDrag(object sender, QueryContinueDragEventArgs e)
        {
            if ((e.KeyStates & DragDropKeyStates.LeftMouseButton) == 0)
            {
                view.Opacity = 1;
            }
        }
       
    }
}
