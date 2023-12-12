using CustomControls.Utils;
using DA_Music_Admin.Sources.CustomControls;
using DA_Music_Admin.SystemInfor;
using DA_Music_Admin.Views;
using Services.IServices;
using StartUpHelperWPF;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows;
using TaskStatus = CustomControls.Controls.TaskStatus;
using System.Linq;

namespace DA_Music_Admin.ViewModels
{
    public class EditAlbumPageSecondSlideViewModel : UserControlViewModel
    {
        protected const int OPTION_INSERT = 1;
        protected const int OPTION_UPDATE = 2;
        protected const string CONTENT_BUTTON_INSERT = "Thêm";
        protected const string CONTENT_BUTTON_UPDATE = "Cập nhật";
        private readonly IAlbumService _albumService;
        private readonly ISongService _songService;
        private readonly IAlbumSongService _albumSongService;
        private readonly IAbstractFactory<SongPage> _fSongPage;

        private SongPage _songPage;
        public SongPage SongPage
        {
            get { return _songPage; }
            set { _songPage = value; _ = loadData(nameof(SongPage)); }
        }


        #region Commands

        #region Drag and drop
        public ICommand DropItem { get; set; }
        public ICommand DragEnter { get; set; }
        public ICommand DragLeave { get; set; }

        #endregion

        #region Excute Action

        public ICommand ExcuteAction { get; set; }

        #endregion Excute Action
        public ICommand removeAllSelectedItems { get; set; }

        #endregion Commands

        private Album _Data;
        public Album Data
        {
            get { return _Data; }
            set
            {
                _Data = value;
                _ = loadData("Data");

                if (string.IsNullOrEmpty(Data?.Id))
                {
                    CurrentOption = OPTION_INSERT;

                }
                else
                {
                    CurrentOption = OPTION_UPDATE;

                }

            }
        }

        EditAlbumPageSecondSlide view;
        public List<Song> SelectedItems = new List<Song>();
        List<CardObject> poolCardObject = new List<CardObject>();

        public string primaryImageFile = string.Empty;
        public string primaryAudioFile = string.Empty;


        private string _ContentButtonAction = CONTENT_BUTTON_INSERT;
        public string ContentButtonAction
        {
            get { return _ContentButtonAction; }
            set { _ContentButtonAction = value; _ = loadData("ContentButtonAction"); }
        }

        private int _CurrentOption = OPTION_INSERT;
        public int CurrentOption
        {
            get { return _CurrentOption; }
            set
            {
                _CurrentOption = value;
                ContentButtonAction = value == OPTION_INSERT ? CONTENT_BUTTON_INSERT : CONTENT_BUTTON_UPDATE;
            }
        }


        public EditAlbumPageSecondSlideViewModel(IAlbumService AlbumService
            , ISongService songService
            , IAlbumSongService AlbumSongService
            , IAbstractFactory<SongPage> songPage) : base()
        {
            InitCommands();
            _albumService = AlbumService;
            _songService = songService;
            _albumSongService = AlbumSongService;
            _fSongPage = songPage;
        }

        public override void UserControl_Loaded_CodeBehind(UserControl t)
        {
            base.UserControl_Loaded_CodeBehind(t);

            view = t as EditAlbumPageSecondSlide;

            SongPage = _fSongPage.Create();
            SongPage.SwitchActionButtons(Visibility.Visible, null);
            //view.TriggerControl.setChecked(true);

            ViewHolder.Ins.HideLoadingScreen();
        }

        protected void InitCommands()
        {
            #region Drag and drop
            DragEnter = new RelayCommand<WrapPanel>(
            (t) =>
            {
                return t != null ? true : false;
            },
            (t) =>
            {
                t.Background = (SolidColorBrush)t.FindResource("ForegroundColor_20");
            });

            DragLeave = new RelayCommand<WrapPanel>(
            (t) =>
            {
                return t != null ? true : false;
            },
            (t) =>
            {
                t.Background = (SolidColorBrush)t.FindResource("SubBackgroundColor");
            });

            DropItem = new RelayCommand<DragEventArgs>(
            (t) =>
            {
                return t != null ? true : false;
            },
            (t) =>
            {
                view.DropZone.Background = (SolidColorBrush)view.DropZone.FindResource("SubBackgroundColor");
                //Album dataItem = t.Data.GetData(DataFormats.Serializable) as Album;
                //if (dataItem == null)
                //    return;
                //addItemCardAlbum(dataItem);

                var dataItem = t.Data.GetData(DataFormats.Serializable) as ObservableCollection<Song>;

                // When drag item of dropzone to drop dropzone (change position)
                if (dataItem == null)
                    return;

                foreach (var item in dataItem)
                {
                    if (!item.IsSelected)
                    {
                        item.IsSelected = true;
                    }
                }

                AddItemCardSongs(dataItem);

            });
            #endregion Drag and drop

            #region Excute Action
            ExcuteAction = new RelayCommand<Button>(
            (t) =>
            {
                return t != null ? true : false;
            },
            (t) =>
            {
                if (CurrentOption == OPTION_INSERT)
                    AddProcessItem(OPTION_INSERT);
                else if (CurrentOption == OPTION_UPDATE)
                    AddProcessItem(OPTION_UPDATE);
            });

            removeAllSelectedItems = new RelayCommand<BaseViewModel>(
            (t) =>
            {
                return t != null ? true : false;
            },
            async (t) =>
            {
                SelectedItems.ForEach(item => item.IsSelected = false);
                await Task.Run(() =>
                {
                    removeAllSelectedItem();
                });
            });


            #endregion
        }

        public void AddProcessItem(int option)
        {
            if (!CheckInputs())
                return;
            if (option == OPTION_INSERT)
            {
                string title = "Đang thêm album " + Data.Name;
                string message = "Đang upload file";
                ViewHolder.Ins.AddProcessItem(GeometryConst.IconCheck, title, message, ExcuteActionInsert);
            }
            else if (option == OPTION_UPDATE)
            {
                string title = "Đang cập nhật album " + Data.Name;
                string message = "Đang upload file";
                ViewHolder.Ins.AddProcessItem(GeometryConst.IconCheck, title, message, ExcuteActionUpdate);
            }
        }


        public async Task<List<Song>> loadData(Album data, string primaryImageFile, string primaryAudioFile)
        {
            Data = data;
            this.primaryImageFile = primaryImageFile;
            this.primaryAudioFile = primaryAudioFile;

            var res = await _songService.GetSongsByAlbumId(Data.Id);
            var dataSongs = new ObservableCollection<Song>(res);

            removeAllSelectedItem();
            if (dataSongs.Count > 0)
                AddItemCardSongs(dataSongs);

            return SelectedItems;
        }


        protected void AddItemCardSongs(ObservableCollection<Song> dataItems)
        {
            foreach (var dataItem in dataItems)
            {
                AddItemCardSong(dataItem);
            }
        }

        protected void AddItemCardSong(Song dataItem)
        {
            if (SelectedItems.Where(m => m.Id == dataItem.Id).ToList().Count == 0)
            {
                CardObject cardObject = null;
                if (view.DropZone.Children.Count == poolCardObject.Count)
                {
                    cardObject = new CardObject(dataItem);
                    poolCardObject.Add(cardObject);
                }
                else
                {
                    foreach (var item in poolCardObject)
                    {
                        if (!view.DropZone.Children.Contains(item))
                        {
                            cardObject = item;
                            // Update data
                            cardObject.Data = dataItem;
                            break;
                        }
                    }
                }
                SelectedItems.Add(dataItem);
                if (!view.DropZone.Children.Contains(cardObject))
                {
                    var parent = cardObject.Parent as Panel;
                    if (parent != null)
                        parent.Children.Remove(cardObject);
                    view.DropZone.Children.Add(cardObject);

                }
            }
        }

        public void removeAllSelectedItem()
        {
            view.DropZone.Dispatcher.Invoke(() =>
            {
                while (view.DropZone.Children.Count > 0)
                {
                    view.DropZone.Children.RemoveAt(0);
                    SelectedItems.RemoveAt(0);
                }

                SelectedItems.Clear();
            });
        }

        public void removeSelectedItem()
        {
            CardObject item = CardObject.findItem(findPanelContainerCard(view, "DropZone"), DragItemHelper.Ins.Data);
            if (item == null)
                return;
            view.DropZone.Children.Remove(item);
            var data = DragItemHelper.Ins.Data as Song;
            SelectedItems.Remove(data);
            data.IsSelected = false;
        }

        public Panel findPanelContainerCard(UserControl pageContainer, string nameContainer)
        {
            if (pageContainer == null)
                return null;

            Panel container = pageContainer.FindName(nameContainer) as Panel;
            return container;
        }

        public async Task<TaskStatus> ExcuteActionInsert()
        {
            Album data = await insertAlbum();
            if (data != null)
            {
                Data.Id = data.Id;
                Data.Description = data.Description;
                Data.Image = data.Image;
                CurrentOption = OPTION_UPDATE;
                InsertAlbumSong();
            }
            return data == null ? null : new TaskStatus(true);
        }

        public async Task<Album> insertAlbum()
        {
            var result = await _albumService.CreateObject(Data);
            if (result != null)
            {
                ViewHolder.Ins.ShowSuccessNotify("Bài hát " + Data.Name + " đã được thêm");
                return result;
            }
            else
            {
                ViewHolder.Ins.ShowFailNotify("Xảy ra lỗi khi thêm bài hát " + Data.Name);
            }
            return null;
        }


        public async Task<TaskStatus> ExcuteActionUpdate()
        {
            Album data = await UpdateAlbum();
            if (data != null)
            {
                Data = data;
                InsertAlbumSong();
            }
            return new TaskStatus(true);
        }

        public async Task<Album> UpdateAlbum()
        {
            var result = await _albumService.EditAlbum(Data, Data.Image != primaryImageFile);
            if (result != null)
            {
                ViewHolder.Ins.ShowSuccessNotify("album " + Data.Name + " đã được cập nhật");
                return result;
            }
            else
                ViewHolder.Ins.ShowFailNotify("Xảy ra lỗi khi cập nhật Album " + Data.Name);
            return null;
        }

        public async void InsertAlbumSong()
        {
            var resCreate = await InsertSelectedItems();
            if (resCreate.Count != 0)
            {
                ViewHolder.Ins.ShowSuccessNotify("Các bài hát của Album " + Data.Name + " đã được thêm");
            }
            else
            {
                ViewHolder.Ins.ShowFailNotify("Xảy ra lỗi khi thêm bài hát của album " + Data.Name);
            }
        }

        public async Task<List<AlbumSong>> InsertSelectedItems()
        {
            var insertItems = new List<AlbumSong>();

            foreach (Song item in SelectedItems)
            {
                insertItems.Add(new AlbumSong { SongId = item.Id, AlbumId = Data.Id });
            }

            if (insertItems.Count == 0)
                insertItems.Add(new AlbumSong { AlbumId = Data.Id });

            await _albumSongService.AddMultiAlbumSong(insertItems);

            return insertItems;
        }

        public bool CheckInputs()
        {
            var msg = ValidateData();

            if (msg != "")
            {
                ViewHolder.Ins.ShowFailNotify(msg);
                return false;
            }
            return true;
        }
      
        string ValidateData()
        {
            if (string.IsNullOrEmpty(Data.Image))
                return "Không thể bỏ trống hình ảnh!";
            if (string.IsNullOrEmpty(Data.Name))
                return "Không thể bỏ trống tên album!";
            if (string.IsNullOrEmpty(Data.TopicId))
                return "Không thể bỏ trống chủ đề!";
            return "";
        }

    }
}
