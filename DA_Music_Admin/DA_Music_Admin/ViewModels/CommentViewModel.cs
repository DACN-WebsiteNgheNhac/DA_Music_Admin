using DA_Music_Admin.Const;
using DA_Music_Admin.SystemInfor;
using DA_Music_Admin.Views;
using Services.IServices;
using StartUpHelperWPF;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows;
using System;

namespace DA_Music_Admin.ViewModels
{
    public class CommentViewModel : PageViewModel
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
        public ICommand SwitchVisibilityCommentCommand { get; set; }
        //public ICommand GetAllComment { get; set; }

        #region Panigation
        public ICommand GoToNextPage { get; set; }
        public ICommand GoToLastPage { get; set; }
        public ICommand GoToPreviousePage { get; set; }
        public ICommand GoToFirstPage { get; set; }
        public ICommand GoToDesPage { get; set; }

        #endregion Panigation


        #endregion End Commands

        CommentPage @this;

        private ObservableCollection<Comment> _DataComment = new ObservableCollection<Comment>();
        public ObservableCollection<Comment> DataComment
        {
            get { return _DataComment; }
            set
            {
                _DataComment = value;
                _ = loadData("DataComment");
            }
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

        private bool _GetAllComment = true;
        public bool GetAllComment
        {
            get { return _GetAllComment; }
            set 
            { 
                _GetAllComment = value;
                GetOne = !value;
                _ = loadData(nameof(GetAllComment));
                Search();
            }
        }

        private bool _GetOne;
        public bool GetOne
        {
            get { return _GetOne; }
            set 
            {
                _GetOne = value;
                _ = loadData(nameof(GetOne));
            }
        }

        private bool _OrderByDecs = true;
        public bool OrderByDecs
        {
            get { return _OrderByDecs; }
            set 
            { 
                _OrderByDecs = value; 
                _= loadData(nameof(OrderByDecs)); 
                Search();
                TipOrderBy = value ? "Bình luận mới nhất" : "Bình luận sớm nhất";
            }
        }

        private string _TipOrderBy = "Bình luận mới nhất";
        public string TipOrderBy
        {
            get { return _TipOrderBy; }
            set { _TipOrderBy = value; _ = loadData(nameof(TipOrderBy)); }
        }




        private SongPage _SongPage;
        public SongPage SongPage
        {
            get { return _SongPage; }
            set { _SongPage = value; _ = loadData(nameof(SongPage)); }
        }

        private Song _SelectedSong;
        public Song SelectedSong
        {
            get { return _SelectedSong; }
            set { _SelectedSong = value; GetAllComment = false; }
        }


        private bool _IsSelectedAllItem = false;
        private readonly EditSongPageSecondSlideViewModel _editSongPageSecondSlideViewModel;
        private readonly ICommentService _commentService;
        private readonly ITopicService _topicService;
        private readonly IArtistService _artistService;
        private readonly IAbstractFactory<SongPage> _fSongPage;

        public CommentViewModel(EditSongPageSecondSlideViewModel editSongPageSecondSlideViewModel
            , ICommentService CommentService
            , ITopicService topicService
            , IArtistService artistService
            , IAbstractFactory<SongPage> songPage) : base()
        {
            InitCommands();
            _editSongPageSecondSlideViewModel = editSongPageSecondSlideViewModel;
            _commentService = CommentService;
            _topicService = topicService;
            _artistService = artistService;
            _fSongPage = songPage;
        }

        public override void Page_Loaded_CodeBehind(Page t)
        {
            base.Page_Loaded_CodeBehind(t);

            @this = t as CommentPage;
            SongPage = _fSongPage.Create();
            SongPage.SwitchActionButtons(Visibility.Collapsed, ActionSongItemClick);
            GetData();
        }

        async Task<int> ActionSongItemClick(object data)
        {
            var temp = data as Song;
            if (temp == null)
                return 0;

            SelectedSong = temp;

            return 0;
        }

        void InitCommands()
        {
            SwitchVisibilityCommentCommand = new RelayCommand<Comment>(
            (t) =>
            {
                return t != null ? true : false;
            },
            async (t) =>
            {
                SwitchVisibilityComment(t);
            });

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

        }

       

        void GetData()
        {
            @this.Dispatcher.Invoke(new System.Action(async () =>
            {
                await GetDataEntries();
                await Search();
            }));
        }

        async Task SwitchVisibilityComment(Comment t)
        {
            t.DeletedAt = t.DeletedAt == null ? DateTimeOffset.Now : null;
            await _commentService.UpdateComment(t);
        }


        protected void BtnClearQueryCLicked(Button sender)
        {
            Query = "";
        }

        protected async Task Search()
        {
            ViewHolder.Ins.ShowLoadingScreen();
            object itemsLock = new object();

            BindingOperations.EnableCollectionSynchronization(DataComment, itemsLock);


            var backgroundWorker = new BackgroundWorker();
            backgroundWorker.DoWork += async (sender, e) =>
            {
                var songId = SelectedSong != null ? SelectedSong?.Id : "";

                if (GetAllComment)
                    songId = "";
                else
                {
                    if (songId == "")
                        songId = "-1";
                }

                var dataComment = await _commentService.GetCommentsBySongId(songId, CurrentPage, SelectedEntry, OrderByDecs);
                TotalPage = await _commentService.GetTotalPages(songId, SelectedEntry);

                if (dataComment == null)
                    return;
                DataComment = new ObservableCollection<Comment>(dataComment);
            };

            backgroundWorker.RunWorkerCompleted += (sender, e) =>
            {
                ViewHolder.Ins.HideLoadingScreen();
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
