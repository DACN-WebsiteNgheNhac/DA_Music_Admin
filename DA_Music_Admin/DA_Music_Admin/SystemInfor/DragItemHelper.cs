using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Image = System.Windows.Controls.Image;

namespace DA_Music_Admin.SystemInfor
{
    public class DragItemHelper
    {
        private static DragItemHelper _Ins;
        public static DragItemHelper Ins
        {
            get
            {
                if (_Ins == null)
                    _Ins = new DragItemHelper();
                return _Ins;
            }
            private set { _Ins = value; }
        }

        private Image _DynamicItem;
        public Image DynamicItem
        {
            get { return _DynamicItem; }
            private set { _DynamicItem = value; }
        }

        private BaseModel _Data;
        public BaseModel Data
        {
            get { return _Data; }
            private set { _Data = value; }
        }

        private bool _IsDragging = false;
        public bool IsDragging
        {
            get { return _IsDragging; }
            set { _IsDragging = value; }
        }



        public DragItemHelper()
        {

        }

        public void Register(Image dynamicImage)
        {
            DynamicItem = dynamicImage;
        }

        public void showDynamicItem()
        {
            if (DynamicItem == null)
                return;
            DynamicItem.Visibility = System.Windows.Visibility.Visible;
        }

        public void hideDynamicItem()
        {
            if (DynamicItem == null)
                return;
            DynamicItem.Visibility = System.Windows.Visibility.Collapsed;
        }

        public void setDataDymnamicItem(BaseModel data)
        {
            Data = data;
            DynamicItem.DataContext = data;
            if(Data?.Image != null && Data.Image != string.Empty)
            {
                try
                {
                    DynamicItem.Source = new BitmapImage(new System.Uri(data.Image));
                }
                catch { };

            }
            if (data != null)
                showDynamicItem();
        }

        public void dragDynamicItem(double x, double y)
        {
            Canvas.SetLeft(DynamicItem, x + 5);
            Canvas.SetTop(DynamicItem, y + 5);
        }
    }
}
