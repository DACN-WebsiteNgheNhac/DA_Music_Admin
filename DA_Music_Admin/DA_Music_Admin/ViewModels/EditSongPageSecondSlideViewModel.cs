using CustomControls.Utils;
using DA_Music_Admin.Sources.CustomControls;
using DA_Music_Admin.SystemInfor;
using DA_Music_Admin.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows;
using TaskStatus = CustomControls.Controls.TaskStatus;
using Services.IServices;
using StartUpHelperWPF;

namespace DA_Music_Admin.ViewModels
{
    public class EditSongPageSecondSlideViewModel: UserControlViewModel
    {
        protected const int OPTION_INSERT = 1;
        protected const int OPTION_UPDATE = 2;
        protected const string CONTENT_BUTTON_INSERT = "Thêm";
        protected const string CONTENT_BUTTON_UPDATE = "Cập nhật";
        private readonly ISongService _songService;
        private readonly IArtistService _artistService;
        private readonly IArtistSongService _artistSongService;
        private readonly IAbstractFactory<ArtistPage> _fArtistPage;

        private ArtistPage _artistPage;
        public ArtistPage ArtistPage
        {
            get { return _artistPage; }
            set { _artistPage = value; _ = loadData(nameof(ArtistPage)); }
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

        private Song _Data;
        public Song Data
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

        EditSongPageSecondSlide view;
        public List<Artist> SelectedItems = new List<Artist>();
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


        public EditSongPageSecondSlideViewModel(ISongService songService
            , IArtistService artistService
            , IArtistSongService artistSongService
            , IAbstractFactory<ArtistPage> artistPage) : base()
        {
            InitCommands();
            _songService = songService;
            _artistService = artistService;
            _artistSongService = artistSongService;
            _fArtistPage = artistPage;
        }

        public override void UserControl_Loaded_CodeBehind(UserControl t)
        {
            base.UserControl_Loaded_CodeBehind(t);

            view = t as EditSongPageSecondSlide;

            ArtistPage = _fArtistPage.Create();

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

                var dataItem = t.Data.GetData(DataFormats.Serializable) as ObservableCollection<Artist>;

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

                AddItemCardArtist(dataItem);

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
                //ExcuteActionUpdate();
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
                string title = "Đang thêm bài hát " + Data.Name;
                string message = "Đang upload file";
                ViewHolder.Ins.AddProcessItem(GeometryConst.IconCheck, title, message, ExcuteActionInsert);
            }
            else if (option == OPTION_UPDATE)
            {
                string title = "Đang cập nhật bài hát " + Data.Name;
                string message = "Đang upload file";
                ViewHolder.Ins.AddProcessItem(GeometryConst.IconCheck, title, message, ExcuteActionUpdate);
            }
        }

       

        public async Task<List<Artist>> loadData(Song data, string primaryImageFile, string primaryAudioFile)
        {
            Data = data;
            this.primaryImageFile = primaryImageFile;
            this.primaryAudioFile = primaryAudioFile;

            var res = await _artistService.GetArtistsBySongId(Data.Id);
            var dataArtists = new ObservableCollection<Artist>(res);

            removeAllSelectedItem();
            if (dataArtists.Count > 0)
                AddItemCardArtist(dataArtists);

            return SelectedItems;
        }


        protected void AddItemCardArtist(ObservableCollection<Artist> dataItems)
        {
            foreach (var dataItem in dataItems)
            {
                AddItemCardArtist(dataItem);
            }
        }

        protected void AddItemCardArtist(Artist dataItem)
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
            Artist data = DragItemHelper.Ins.Data as Artist;
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
            Song data = await insertSong();
            if (data != null)
            {
                Data.Id = data.Id;
                Data.Description = data.Description;
                Data.Image = data.Image;
                Data.SongUrl = data.SongUrl;
                CurrentOption = OPTION_UPDATE;
                InsertSongArtist();
            }
            return data == null ? null : new TaskStatus(true);
        }

        public async Task<Song> insertSong()
        {
            var result = await _songService.CreateObject(Data);
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
            Song data = await UpdateSong();
            if (data != null)
            {
                Data = data;
                InsertSongArtist();
            }
            return new TaskStatus(true);
        }

        public async Task<Song> UpdateSong()
        {
            var result = await _songService.EditSong(Data, Data.Image != primaryImageFile, Data.SongUrl != primaryAudioFile);
            if (result != null)
            {
                ViewHolder.Ins.ShowSuccessNotify("Bài hát " + Data.Name + " đã được cập nhật");
                return result;
            }
            else
                ViewHolder.Ins.ShowFailNotify("Xảy ra lỗi khi cập nhật Bài hát " + Data.Name);
            return null;
        }

        public async void InsertSongArtist()
        {
            var resCreate = await InsertSelectedItems();
            if (resCreate.Count != 0)
            {
                ViewHolder.Ins.ShowSuccessNotify("Các nghệ sỹ của bài hát " + Data.Name + " đã được thêm");
            }
            else
            {
                ViewHolder.Ins.ShowFailNotify("Xảy ra lỗi khi thêm nghệ sỹ của bài hát " + Data.Name);
            }
        }

        public async Task<List<ArtistSong>> InsertSelectedItems()
        {
            var insertItems = new List<ArtistSong>();

            foreach (Artist item in SelectedItems)
            {
                insertItems.Add(new ArtistSong { ArtistId = item.Id, SongId = Data.Id });
            }

            if (insertItems.Count == 0)
                insertItems.Add(new ArtistSong{SongId = Data.Id});

            await _artistSongService.AddMultiArtistSong(insertItems);

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
            if (string.IsNullOrEmpty(Data.Name))
                return "Không thể bỏ trống tựa đề bài hát!";
            if (string.IsNullOrEmpty(Data.SongUrl) || string.IsNullOrEmpty(primaryAudioFile))
                return "Không thể bỏ trống file audio!";
            if (string.IsNullOrEmpty(Data.Area))
                return "Không thể bỏ trống khu vực!";
            return "";
        }
    }
}
