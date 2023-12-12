using DA_Music_Admin.SystemInfor;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using DA_Music_Admin.Views;
using Services.IServices;
using CustomControls.Utils;
using System.Threading.Tasks;
using System.Windows.Data;
using System.ComponentModel;
using StartUpHelperWPF;
using System.Linq;
using DA_Music_Admin.Const;
using Services;
using System;

namespace DA_Music_Admin.ViewModels
{
    public class SongViewModel : PageViewModel
    {

        #region Start Commands

        #region Start View 

        public ICommand View_Loaded { get; set; }


        #endregion End MainView 

        public ICommand BtnClearQuery_CLicked { get; set; }
        public ICommand BtnSearch_CLicked { get; set; }
        public ICommand DatagridRowClicked { get; set; }
        public ICommand CheckBoxDatagridRowClicked { get; set; }
        public ICommand DropItem { get; set; }

        public ICommand OpenEditItemPage { get; set; }
        public ICommand DeleteItem { get; set; }

        #region Panigation
        public ICommand GoToNextPage { get; set; }
        public ICommand GoToLastPage { get; set; }
        public ICommand GoToPreviousePage { get; set; }
        public ICommand GoToFirstPage { get; set; }
        public ICommand GoToDesPage { get; set; }

        #endregion Panigation


        #endregion End Commands

        SongPage @this;


        private Visibility _ShowModifyControls = Visibility.Visible;
        public Visibility ShowModifyControls
        {
            get { return _ShowModifyControls; }
            set { _ShowModifyControls = value; _ = loadData(nameof(ShowModifyControls)); }
        }


        private ObservableCollection<Song> _DataSong= new ObservableCollection<Song>();
        public ObservableCollection<Song> DataSong
        {
            get { return _DataSong; }
            set
            {
                _DataSong = value;
                _ = loadData("DataSong");
            }
        }

        private ObservableCollection<Artist> _DataArtist = new ObservableCollection<Artist>();
        public ObservableCollection<Artist> DataArtist
        {
            get { return _DataArtist; }
            set
            {
                _DataArtist = value;
                _ = loadData("DataArtist");
            }
        }

        private Artist _SelectedArtist = new Artist();
        public Artist SelectedArtist
        {
            get { return _SelectedArtist; }
            set { _SelectedArtist = value; _ = loadData(nameof(SelectedArtist)); }
        }


        private ObservableCollection<int> _ItemsEntries = new ObservableCollection<int>();
        public ObservableCollection<int> ItemsEntries
        {
            get { return _ItemsEntries; }
            set
            {
                _ItemsEntries = value;
                _ = loadData("ItemsEntries");
            }
        }
        public int SelectedEntry { get; set; } = 10;


        private ObservableCollection<string> _DataAreas = new ObservableCollection<string>(DataConst.DataArea);
        public ObservableCollection<string> DataAreas
        {
            get { return _DataAreas; }
            set
            {
                _DataAreas = value;
                _ = loadData("DataAreas");
            }
        }
        public string SelectedArea { get; set; } = "Chọn khu vực";


        

        private string _Query = "";
        public string Query
        {
            get { return _Query; }
            set
            {
                _Query = value;
                _ = loadData("Query");
                ShowButtonClearQuery = value.Trim() != string.Empty ? Visibility.Visible : Visibility.Hidden;
            }
        }

        private Visibility _ShowButtonClearQuery = Visibility.Hidden;
        public Visibility ShowButtonClearQuery
        {
            get { return _ShowButtonClearQuery; }
            set { _ShowButtonClearQuery = value; _ = loadData("ShowButtonClearQuery"); }
        }

        private bool _IsSelectedAllItem = false;
        private readonly EditArtistPageSecondSlideViewModel _editArtistPageSecondSlideViewModel;
        private readonly EditAlbumPageSecondSlideViewModel _editAlbumPageSecondSlideViewModel;
        private readonly ISongService _songService;
        private readonly IArtistService _artistService;
        private readonly IAbstractFactory<EditSongPage> _editSongPage;

        public bool IsSelectedAllItem
        {
            get { return _IsSelectedAllItem; }
            set
            {
                _IsSelectedAllItem = value;
                _ = loadData("IsSelectedAllItem");
                foreach (Song item in DataSong)
                    item.IsSelected = _IsSelectedAllItem;
            }
        }




        public SongViewModel(EditArtistPageSecondSlideViewModel editArtistPageSecondSlideViewModel
            , EditAlbumPageSecondSlideViewModel editAlbumPageSecondSlideViewModel
            , ISongService songService
            , IArtistService artistService
            , IAbstractFactory<EditSongPage> editSongPage) : base()
        {
            InitCommands();
            _editArtistPageSecondSlideViewModel = editArtistPageSecondSlideViewModel;
            _editAlbumPageSecondSlideViewModel = editAlbumPageSecondSlideViewModel;
            _songService = songService;
            _artistService = artistService;
            _editSongPage = editSongPage;

            
        }


        private void DgSong_LoadingRow(object? sender, DataGridRowEventArgs e)
        {
            ViewHolder.Ins.HideLoadingScreen();
            @this.dgSong.LoadingRow -= DgSong_LoadingRow;
        }

        public override void Page_Loaded_CodeBehind(Page t)
        {
            base.Page_Loaded_CodeBehind(t);

            @this = t as SongPage;
            GetData();
        }


        void InitCommands()
        {
            #region Search
            BtnClearQuery_CLicked = new RelayCommand<Button>(
            (t) =>
            {
                return t != null ? true : false;
            },
            (t) =>
            {
                BtnClearQueryCLicked(t);
            });

            BtnSearch_CLicked = new RelayCommand<Button>(
            (t) =>
            {
                return t != null ? true : false;
            },
            async (t) =>
            {
                CurrentPage = 1;
                await Search();
            });
            #endregion Search


            #region Panigation

            GoToNextPage = new RelayCommand<Button>(
            (t) =>
            {
                return t != null ? true : false;
            },
            async (t) =>
            {
                if (CurrentPage != TotalPage)
                    CurrentPage++;
                await Search();
            });

            GoToLastPage = new RelayCommand<Button>(
            (t) =>
            {
                return t != null ? true : false;
            },
            async (t) =>
            {
                CurrentPage = TotalPage;
                await Search();
            });

            GoToPreviousePage = new RelayCommand<Button>(
            (t) =>
            {
                return t != null ? true : false;
            },
            async (t) =>
            {
                if (CurrentPage != 1)
                    CurrentPage--;
                await Search();
            });

            GoToFirstPage = new RelayCommand<Button>(
            (t) =>
            {
                return t != null ? true : false;
            },
            async (t) =>
            {
                CurrentPage = 1;
                await Search();
            });

            GoToDesPage = new RelayCommand<Button>(
            (t) =>
            {
                return t != null ? true : false;
            },
            async (t) =>
            {
                if (CurrentPage > TotalPage)
                {
                    var message = "Không thể đến trang số " + CurrentPage.ToString()
                        + " trong khi chỉ có " + TotalPage.ToString();
                    MessageBox.Show(
                        "Không thể đến trang số " + CurrentPage.ToString()
                        + " trong khi chỉ có " + TotalPage.ToString());
                    return;
                }
                await Search();
            });
            #endregion Panigation

            OpenEditItemPage = new RelayCommand<Button>(
            (t) =>
            {
                return t != null ? true : false;
            },
            (t) =>
            {
                OpenEditSongPage(t.DataContext);
            });

            #region Datagrid
            DatagridRowClicked = new RelayCommand<DataGridRow>(
            (t) =>
            {
                return t != null ? true : false;
            },
            (t) =>
            {
                if (ShowModifyControls != Visibility.Visible)
                    return;

                var dataCurrentRow = t.DataContext as Song;
                var selectedRows = new ObservableCollection<Song>(DataSong.ToList().Where(item => item.IsSelected == true).ToList());
                selectedRows.Add(dataCurrentRow);
                DragItemHelper.Ins.setDataDymnamicItem(dataCurrentRow);
                DragDrop.DoDragDrop(t, new DataObject(DataFormats.Serializable, selectedRows), DragDropEffects.Move);
            });


            DeleteItem = new RelayCommand<Button>(
            (t) =>
            {
                return t != null ? true : false;
            },
            async (t) =>
            {
                var resultDialog = MessageBox.Show("Bạn có muốn xóa dữ liệu?", "Thông báo", MessageBoxButton.YesNo, MessageBoxImage.Information, MessageBoxResult.No, MessageBoxOptions.RightAlign);
                Song data = t.DataContext as Song;
                if (resultDialog == MessageBoxResult.Yes)
                {

                    var res = await _songService.GetListAlls();
                    if (res == null)
                    {
                        ViewHolder.Ins.ShowFailNotify("Có lỗi khi xóa bài hát " + data.Name);
                    }
                    else
                    {
                        DataSong.Remove(data);
                        ViewHolder.Ins.ShowSuccessNotify("Đã xóa bài hát " + data.Name);
                    }
                }

            });

            DropItem = new RelayCommand<DataGrid>(
            (t) =>
            {
                return t != null ? true : false;
            },
            (t) =>
            {
                _editArtistPageSecondSlideViewModel.removeSelectedItem();
                _editAlbumPageSecondSlideViewModel.removeSelectedItem();
            });

            CheckBoxDatagridRowClicked = new RelayCommand<CheckBox>(
            (t) =>
            {
                return t != null ? true : false;
            },
            (t) =>
            {
                t.IsChecked = !t.IsChecked;
            });

            #endregion Datagrid



        }

        public T FindControlByDataContext<T>(DependencyObject parent, object dataContext) where T : FrameworkElement
        {
            if (parent == null)
                return null;

            int childCount = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < childCount; i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(parent, i);
                if (child is T control && control.DataContext == dataContext)
                {
                    return control;
                }

                T foundControl = FindControlByDataContext<T>(child, dataContext);
                if (foundControl != null)
                    return foundControl;
            }

            return null;
        }

        void GetData()
        {
            @this.Dispatcher.Invoke(new System.Action(async () =>
            {
                await GetDataEntries();
                await GetDataArtists();
                await Search();
            }));
        }


        protected void BtnClearQueryCLicked(Button sender)
        {
            Query = "";
        }

        protected async Task Search()
        {
            ViewHolder.Ins.ShowLoadingScreen();

            object itemsLock = new object();

            BindingOperations.EnableCollectionSynchronization(DataSong, itemsLock);

            var backgroundWorker = new BackgroundWorker();
            backgroundWorker.DoWork += async (sender, e) =>
            {
                var area = (SelectedArea == null) ? "" : SelectedArea;
                var artistId = (SelectedArtist == null || SelectedArtist.Id == null) ? "" : SelectedArtist.Id;

                var dataSong = await _songService.SearchSongs(Query, area, artistId, CurrentPage, SelectedEntry);
                TotalPage = await _songService.GetTotalPages(Query, area, artistId, SelectedEntry);
                if (dataSong == null)
                    return;
                DataSong = new ObservableCollection<Song>(dataSong);
                @this.dgSong.Dispatcher.Invoke(() =>
                {
                    if (DataSong.Count == 0)
                        ViewHolder.Ins.HideLoadingScreen();
                });
            };

            backgroundWorker.RunWorkerCompleted += (sender, e) =>
            {
                @this.dgSong.LoadingRow += DgSong_LoadingRow;
            };

            backgroundWorker.RunWorkerAsync();
        }

        protected async Task GetDataArtists()
        {
            object itemsLock = new object();

            BindingOperations.EnableCollectionSynchronization(DataArtist, itemsLock);


            var backgroundWorker = new BackgroundWorker();
            backgroundWorker.DoWork += async (sender, e) =>
            {
                var data = await _artistService.GetListAlls();
                if (data == null)
                    return;
                DataArtist = new ObservableCollection<Artist>(data);
            };

            backgroundWorker.RunWorkerCompleted += (sender, e) =>
            {
            };

            backgroundWorker.RunWorkerAsync();
        }

        public async Task GetDataEntries()
        {
            object itemsLock = new object();

            BindingOperations.EnableCollectionSynchronization(ItemsEntries, itemsLock);


            var backgroundWorker = new BackgroundWorker();
            backgroundWorker.DoWork += async (sender, e) =>
            {
                for (int i = 1; i <= 20; i++)
                {
                    ItemsEntries.Add(i);
                }
            };

            backgroundWorker.RunWorkerCompleted += (sender, e) =>
            {
                SelectedEntry = ItemsEntries[9];
            };

            backgroundWorker.RunWorkerAsync();
        }

        public void OpenEditSongPage(object data)
        {
            var target = _editSongPage.Create();
            target.vm.Data = (data as Song);
            ViewHolder.Ins.DirectPage(target);
        }


        #region Do not delete this code

        #region Page
        private int _CurrentPage = 1;
        public int CurrentPage
        {
            get { return _CurrentPage; }
            set
            {
                _CurrentPage = value;
                _ = loadData("CurrentPage");
            }
        }

        private int _TotalPage = 999;
        public int TotalPage
        {
            get { return _TotalPage; }
            set { _TotalPage = value; _ = loadData("TotalPage"); }
        }

     
        #endregion
     
        
        #endregion Do not delete this code


    }
}
