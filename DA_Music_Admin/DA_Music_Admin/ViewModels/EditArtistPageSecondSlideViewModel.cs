using CustomControls.Utils;
using DA_Music_Admin.Sources.CustomControls;
using DA_Music_Admin.SystemInfor;
using DA_Music_Admin.Views;
using Services.IServices;
using StartUpHelperWPF;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows;
using TaskStatus = CustomControls.Controls.TaskStatus;
using Services;


namespace DA_Music_Admin.ViewModels
{
    public class EditArtistPageSecondSlideViewModel : UserControlViewModel
    {
        protected const int OPTION_INSERT = 1;
        protected const int OPTION_UPDATE = 2;
        protected const string CONTENT_BUTTON_INSERT = "Thêm";
        protected const string CONTENT_BUTTON_UPDATE = "Cập nhật";
        private readonly IArtistService _artistService;
        private readonly ISongService _songService;
        private readonly IArtistSongService _artistSongService;
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

        private Artist _Data;
        public Artist Data
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

        EditArtistPageSecondSlide view;
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


        public EditArtistPageSecondSlideViewModel(IArtistService artistService
            , ISongService songService
            , IArtistSongService artistSongService
            , IAbstractFactory<SongPage> songPage) : base()
        {
            InitCommands();
            _artistService = artistService;
            _songService = songService;
            _artistSongService = artistSongService;
            _fSongPage = songPage;
        }

        public override void UserControl_Loaded_CodeBehind(UserControl t)
        {
            base.UserControl_Loaded_CodeBehind(t);

            view = t as EditArtistPageSecondSlide;

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
                //Artist dataItem = t.Data.GetData(DataFormats.Serializable) as Artist;
                //if (dataItem == null)
                //    return;
                //addItemCardArtist(dataItem);

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
                string title = "Đang thêm nghệ sỹ " + Data.Name;
                string message = "Đang upload file";
                ViewHolder.Ins.AddProcessItem(GeometryConst.IconCheck, title, message, ExcuteActionInsert);
            }
            else if (option == OPTION_UPDATE)
            {
                string title = "Đang cập nhật nghệ sỹ " + Data.Name;
                string message = "Đang upload file";
                ViewHolder.Ins.AddProcessItem(GeometryConst.IconCheck, title, message, ExcuteActionUpdate);
            }
        }


        public async Task<List<Song>> loadData(Artist data, string primaryImageFile, string primaryAudioFile)
        {
            Data = data;
            this.primaryImageFile = primaryImageFile;
            this.primaryAudioFile = primaryAudioFile;

            var res = await _songService.GetSongsByArtistId(Data.Id);
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
                    if(parent != null)
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
            Artist data = await insertArtist();
            if (data != null)
            {
                Data.Id = data.Id;
                Data.Description = data.Description;
                Data.Image = data.Image;
                CurrentOption = OPTION_UPDATE;
                InsertArtistArtist();
            }
            return data == null ? null : new TaskStatus(true);
        }

        public async Task<Artist> insertArtist()
        {
            var result = await _artistService.CreateObject(Data);
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
            Artist data = await UpdateArtist();
            if (data != null)
            {
                Data = data;
                InsertArtistArtist();
            }
            return new TaskStatus(true);
        }

        public async Task<Artist> UpdateArtist()
        {
            var result = await _artistService.EditArtist(Data, Data.Image != primaryImageFile);
            if (result != null)
            {
                ViewHolder.Ins.ShowSuccessNotify("Nghệ sỹ " + Data.Name + " đã được cập nhật");
                return result;
            }
            else
                ViewHolder.Ins.ShowFailNotify("Xảy ra lỗi khi cập nhật Nghệ sỹ " + Data.Name);
            return null;
        }

        public async void InsertArtistArtist()
        {            
            var resCreate = await InsertSelectedItems();
            if (resCreate.Count != 0)
            {
                ViewHolder.Ins.ShowSuccessNotify("Các bài hát của nghệ sỹ " + Data.Name + " đã được thêm");
            }
            else
            {
                ViewHolder.Ins.ShowFailNotify("Xảy ra lỗi khi thêm bài hát của nghệ sỹ " + Data.Name);
            }
        }

        public async Task<List<ArtistSong>> InsertSelectedItems()
        {
            var insertItems = new List<ArtistSong>();

            foreach (Song item in SelectedItems)
            {
                insertItems.Add(new ArtistSong { SongId = item.Id, ArtistId = Data.Id });
            }

            if (insertItems.Count == 0)
                insertItems.Add(new ArtistSong { ArtistId = Data.Id });

            await _artistSongService.AddMultiArtistSong(insertItems, true);

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
                return "Không thể bỏ trống tên nghệ sỹ!";
            if (string.IsNullOrEmpty(Data.ArtistName))
                return "Không thể bỏ trống tên nghệ danh!";
            if (string.IsNullOrEmpty(Data.Gender))
                return "Không thể bỏ trống giới tính!";
            if (Data.BirthDay == null)
                return "Không thể bỏ trống ngày sinh!";
            if (Data.DebutDate == null)
                return "Không thể bỏ trống ngày debut!";
            if (string.IsNullOrEmpty(Data.National))
                return "Không thể bỏ trống quốc tíchj!";
            return "";
        }


    }
}
