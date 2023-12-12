using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;
using CustomControls.Controls;
using DA_Music_Admin.Views;
using CustomControls.Utils;
using DA_Music_Admin.SystemInfor;
using StartUpHelperWPF;
using System;

namespace DA_Music_Admin.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        #region Start Commands


        #region Start MainView 

        public ICommand MainView_Loaded { get; set; }
        public ICommand MainView_SizeChanged { get; set; }
        public ICommand MainView_Closed { get; set; }


        #endregion End MainView 


        public ICommand ck_OpenMenu_Click { get; set; }


        #region Start RadioButton Menu

        public ICommand RadioButton_Checked { get; set; }

        #endregion End RadioButton Menu


        public ICommand BackPage { get; set; }
        public ICommand NextPage { get; set; }


        public ICommand PreviewDrop { get; set; }
        public ICommand PreviewDragOver { get; set; }


        public ICommand OpenProcess { get; set; }
        public ICommand IsVisibleChangedProcessPanel { get; set; }

        #endregion End Commands

        MainWindow mainView;
        public CustomItemMenu FirstSelectedMenu;

        private string _AvatarUser;
        public string AvatarUser
        {
            get { return _AvatarUser; }
            set { _AvatarUser = value; _ = loadData(nameof(AvatarUser));  }
        }



        private Visibility _VisibilityProcessPanel = Visibility.Collapsed;
        private readonly IAbstractFactory<HomePage> _homePage;
        private readonly IAbstractFactory<SongPage> _songPage;
        private readonly IAbstractFactory<EditSongPage> _editSongPage;
        private readonly IAbstractFactory<ArtistPage> _artistPage;
        private readonly IAbstractFactory<EditArtistPage> _editArtistPage;
        private readonly IAbstractFactory<AlbumPage> _albumPage;
        private readonly IAbstractFactory<EditAlbumPage> _editAlbumPage;
        private readonly IAbstractFactory<CommentPage> _commentPage;
        private readonly IAbstractFactory<UserPage> _userPage;
        private readonly IAbstractFactory<EditUserPage> _editUserPage;
        private readonly AccountManager _accountManager;

        public Visibility VisibilityProcessPanel
        {
            get { return _VisibilityProcessPanel; }
            set { _VisibilityProcessPanel = value; _ = loadData(nameof(VisibilityProcessPanel)); }
        }


        public MainViewModel(IAbstractFactory<HomePage> homePage,
            IAbstractFactory<SongPage> songPage,
            IAbstractFactory<EditSongPage> editSongPage,
            IAbstractFactory<ArtistPage> artistPage,
            IAbstractFactory<EditArtistPage> editArtistPage,
            IAbstractFactory<AlbumPage> albumPage,
            IAbstractFactory<EditAlbumPage> editAlbumPage,
            IAbstractFactory<CommentPage> commentPage,
            IAbstractFactory<UserPage> userPage,
            IAbstractFactory<EditUserPage> editUserPage,
            AccountManager accountManager)
        {
            InitCommands();
            _homePage = homePage;
            _songPage = songPage;
            _editSongPage = editSongPage;
            _artistPage = artistPage;
            _editArtistPage = editArtistPage;
            _albumPage = albumPage;
            _editAlbumPage = editAlbumPage;
            _commentPage = commentPage;
            _userPage = userPage;
            _editUserPage = editUserPage;
            _accountManager = accountManager;

        }

        protected void InitCommands()
        {
            MainView_Loaded = new RelayCommand<MainWindow>(
            (t) =>
            {
                return t != null ? true : false;
            },
            (t) =>
            {
                mainView = t;
                
                

                InitMenu();
                

                ViewHolder.Ins.Register(mainView.loadingScreen, mainView.fContainerView
                    , mainView.notificationManager, mainView.processPanel);
                DragItemHelper.Ins.Register(mainView.dynamicItem);
                ColorConst.Register(t);
                GeometryConst.Register();

                AvatarUser = _accountManager.Account?.Image;

            });



            ck_OpenMenu_Click = new RelayCommand<CheckBox>(
            (t) =>
            {
                return t != null ? true : false;
            },
            (t) =>
            {
                MyAnimation animation = new MyAnimation(120);
                bool? isChecked = t.IsChecked;
                if (isChecked == true)
                    animation.StartAnimationWithEaseFunction(mainView.spContainerMenu, FrameworkElement.WidthProperty, (int)OptionEasingFunction.CubicEase, 75, 0, 125);
                else
                    animation.StartAnimationWithEaseFunction(mainView.spContainerMenu, FrameworkElement.WidthProperty, (int)OptionEasingFunction.CubicEase, 220, 0, 125);

            });

            PreviewDrop = new RelayCommand<DragEventArgs>(
            (t) =>
            {
                return t != null ? true : false;
            },
            (t) =>
            {
                //ViewHolder.Ins.hideDynamicItem();
                DragItemHelper.Ins.hideDynamicItem();
            });

            PreviewDragOver = new RelayCommand<DragEventArgs>(
            (t) =>
            {
                return t != null ? true : false;
            },
            (t) =>
            {
                Point point = t.GetPosition(mainView.cbMainContainer);
                //ViewHolder.Ins.dragDynamicItem(point.X, point.Y);
                DragItemHelper.Ins.dragDynamicItem(point.X, point.Y);
            });

            BackPage = new RelayCommand<Button>(
            (t) =>
            {
                return t != null ? true : false;
            },
            (t) =>
            {
                if (mainView.fContainerView.CanGoBack)
                    mainView.fContainerView.GoBack();
            });

            NextPage = new RelayCommand<Button>(
            (t) =>
            {
                return t != null ? true : false;
            },
            (t) =>
            {
                if (mainView.fContainerView.CanGoForward)
                    mainView.fContainerView.GoForward();
            });

            OpenProcess = new RelayCommand<FrameworkElement>(
            (t) =>
            {
                return t != null ? true : false;
            },
            (t) =>
            {
                VisibilityProcessPanel = VisibilityProcessPanel == Visibility.Collapsed
                ? Visibility.Visible : Visibility.Collapsed;
            });

            IsVisibleChangedProcessPanel = new RelayCommand<FrameworkElement>(
            (t) =>
            {
                return t != null ? true : false;
            },
            (t) =>
            {

            });


        }


        protected void rbtnChecked(RadioButton sender)
        {

        }

        protected void InitMenu()
        {
            if (mainView.spContainerItemMenu == null)
                return;


            mainView.spContainerItemMenu.Children.Add(FirstSelectedMenu = CreateItemMenuHome());
            mainView.spContainerItemMenu.Children.Add(CreateItemMenuSong());
            mainView.spContainerItemMenu.Children.Add(CreateItemMenuArtist());
            mainView.spContainerItemMenu.Children.Add(CreateItemMenuAlbum());
            mainView.spContainerItemMenu.Children.Add(CreateItemMenuComment());
            mainView.spContainerItemMenu.Children.Add(CreateItemMenuUser());

            FirstSelectedMenu.Loaded += FirstSelectedMenu_Loaded;
        }

      

        private void FirstSelectedMenu_Loaded(object sender, RoutedEventArgs e)
        {
            ItemMenuClicked(FirstSelectedMenu.MainItemMenu, new string[] { });
        }

        CustomItemMenu CreateItemMenuHome()
        {
            string pathIconHome = PathDataConst.Home;
            return createItemMenu(pathIconHome, MenuConst.MAIN_HOME, new string[] { }, new string[] { }, true);
        }

        CustomItemMenu CreateItemMenuSong()
        {
            string pathIconSong = PathDataConst.Song;
            string pathIconSongs = PathDataConst.Songs;
            string pathIconEditSong = PathDataConst.EditSong;
            string[] subIconMenu = new string[] { pathIconSongs, pathIconEditSong };
            return createItemMenu(pathIconSong, MenuConst.MAIN_SONG
                , subIconMenu, MenuConst.SUB_SONG);
        }

        CustomItemMenu CreateItemMenuArtist()
        {
            string pathIconArtist = PathDataConst.Artist;
            string pathIconArtists = PathDataConst.Artists;
            string pathIconEditArtist = PathDataConst.EditArtist;
            string[] subIconMenu = new string[] { pathIconArtists, pathIconEditArtist };
            return createItemMenu(pathIconArtist, MenuConst.MAIN_ARTIST
                , subIconMenu, MenuConst.SUB_ARTIST);
        }

        CustomItemMenu CreateItemMenuAlbum()
        {
            string pathIconAlbum = PathDataConst.Album;
            string pathIconAlbums = PathDataConst.Albums;
            string pathIconEditAlbum = PathDataConst.EditAlbum;
            string[] subIconMenu = new string[] { pathIconAlbums, pathIconEditAlbum };
            return createItemMenu(pathIconAlbum, MenuConst.MAIN_ALBUM
                , subIconMenu, MenuConst.SUB_ALBUM);
        }

        CustomItemMenu CreateItemMenuComment()
        {
            string pathIconComment = PathDataConst.Comment;
            string[] subIconMenu = new string[] { };
            return createItemMenu(pathIconComment, MenuConst.MAIN_COMMENT, subIconMenu, new string[] {});
        }

        private CustomItemMenu CreateItemMenuUser()
        {
            string pathIconUser = PathDataConst.User;
            string pathIconUsers = PathDataConst.Users;
            string pathIconEditUser = PathDataConst.EditUser;
            string[] subIconMenu = new string[] { pathIconUsers, pathIconEditUser };
            return createItemMenu(pathIconUser, MenuConst.MAIN_USER
                , subIconMenu, MenuConst.SUB_USER);
        }


        CustomItemMenu createItemMenu(string iconMainMemu, string mainMenu, string[] iconSubMenu, string[] subMenu, bool isFirstChecked = false)
        {
            CustomItemMenu itemMenu = new CustomItemMenu(iconMainMemu, mainMenu
                , iconSubMenu
                , subMenu, isFirstChecked);
            itemMenu.Click += (s, e) =>
            {
                ItemMenuClicked(s as CustomRadioButton, subMenu);
            };

            return itemMenu;
        }

        public void ItemMenuClicked(CustomRadioButton sender, string[] subMenu)
        {
            string strTarget = sender.Text;
            if (subMenu.Length == 0)
            {
                changePage(sender, strTarget);
            }
            else
            {
                if (subMenu.Contains(strTarget))
                {
                    changePage(sender, strTarget);
                }
            }
        }

        public void changePage(CustomRadioButton triggerControl, string strTaget)
        {
            Page target = null; 
            if (strTaget == MenuConst.MAIN_HOME)
            {
                target = _homePage.Create();
            }
            else if (strTaget == MenuConst.SUB_SONG[0])
            {
                var temp = _songPage.Create();
                temp.SwitchActionButtons(Visibility.Visible, null);
                target = temp;
            }
            else if (strTaget == MenuConst.SUB_SONG[1])
            {
                target = _editSongPage.Create();
            }
            else if (strTaget == MenuConst.SUB_ARTIST[0])
            {
                target = _artistPage.Create();
            }
            else if (strTaget == MenuConst.SUB_ARTIST[1])
            {
                target = _editArtistPage.Create();
            }
            else if (strTaget == MenuConst.SUB_ALBUM[0])
            {
                target = _albumPage.Create();
            }
            else if (strTaget == MenuConst.SUB_ALBUM[1])
            {
                target = _editAlbumPage.Create();
            }
            else if (strTaget == MenuConst.MAIN_COMMENT)
            {
                target = _commentPage.Create();
            }
            else if (strTaget == MenuConst.SUB_USER[0])
            {
                target = _userPage.Create();
            }
            else if (strTaget == MenuConst.SUB_USER[1])
            {
                target = _editUserPage.Create();
            }
            ChangePage(target);

        }

        public void ChangePage(Page page)
        {
            ViewHolder.Ins.DirectPage(page);
        }
    }
}
