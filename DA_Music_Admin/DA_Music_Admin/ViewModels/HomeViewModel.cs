using CustomControls.Utils;
using DA_Music_Admin.Sources.CustomControls;
using DA_Music_Admin.SystemInfor;
using DA_Music_Admin.Views;
using LiveCharts.Wpf;
using LiveCharts;
using MaterialDesignThemes.Wpf;
using Services;
using Services.IServices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Collections.ObjectModel;

namespace DA_Music_Admin.ViewModels
{
    public class HomeViewModel : PageViewModel
    {
        #region DI

        private readonly AccountManager _accountManager;
        private readonly ISongService _songService;
        private readonly IArtistService _artistService;
        private readonly IAlbumService _albumService;
        private readonly IUserService _userService;


        #endregion DI

        #region Start Commands

        public ICommand IsVisibleChanged { get; set; }

        #endregion End Commands

        HomePage @this;

        private ObservableCollection<User> _DataUser;

        public ObservableCollection<User> DataNewUser
        {
            get { return _DataUser; }
            set { _DataUser = value; _ = loadData(nameof(DataNewUser)); }
        } 
        
        private ObservableCollection<Song> _DataSong;

        public ObservableCollection<Song> DataNewSong
        {
            get { return _DataSong; }
            set { _DataSong = value; _ = loadData(nameof(DataNewSong)); }
        }

        private ObservableCollection<string> _DataTime = new ObservableCollection<string> { "7 ngày trước", "2 tuần trước", "3 tuần trước", "4 tuần trước"};
        public ObservableCollection<string> DataTime
        {
            get { return _DataTime; }
            set { _DataTime = value; _ = loadData(nameof(DataTime)); }
        }

        private string _SelectedTime = "7 ngày trước";
        public string SelectedTime
        {
            get { return _SelectedTime; }
            set 
            { 
                _SelectedTime = value; 
                _ = loadData(nameof(SelectedTime));

                var fromDate = DateTime.Now.AddDays(-1); ;
                var toDate = DateTime.Now.AddDays(-1);

                if(value == "7 ngày trước")
                {
                    fromDate.AddDays(-7);
                }
                else if (value == "2 tuần trước")
                {
                    fromDate.AddDays(-7 * 2);

                }
                else if (value == "3 tuần trước")
                {
                    fromDate.AddDays(-7 * 3);

                }
                else if (value == "4 tuần trước")
                {
                    fromDate.AddDays(-7 * 4);

                }
                LoadDataCartesianChart(fromDate, toDate);
            }
        }

        private string _WelcomeText;
        public string WelcomeText
        {
            get { return _WelcomeText; }
            set { _WelcomeText = value; _ = loadData(nameof(WelcomeText)); }
        }

        





        public HomeViewModel(AccountManager accountManager
            , ISongService songService
            , IArtistService artistService
            , IAlbumService albumService
            , IUserService userService) : base()
        {
            InitCommands();

            _accountManager = accountManager;
            _songService = songService;
            _artistService = artistService;
            _albumService = albumService;
            _userService = userService;
        }

        public override async void Page_Loaded_CodeBehind(Page t)
        {
            base.Page_Loaded_CodeBehind(t);

            @this = t as HomePage;

            await loadData();

            ViewHolder.Ins.HideLoadingScreen();
        }

        private async Task loadData()
        {
            WelcomeText = "Xin chào " + _accountManager.Account?.Username;

          

            await LoadCountPieChart();

            await LoadListViewNewUser();

            await LoadListViewNewSong();

            await LoadInformationCards();

            SelectedTime = SelectedTime;
        }

       
        void InitCommands()
        {
            IsVisibleChanged = new RelayCommand<object>(
            (t) =>
            {
                return t != null ? true : false;
            },
            (t) =>
            {
            });
        }


        public async Task LoadListViewNewUser()
        {
            var result = new List<User>();

            var backgroundWorker = new BackgroundWorker();

            backgroundWorker.DoWork += (sender, e) =>
            {
                result = _userService.SearchUsers("", "", "1", new DateTime(), DateTime.Now, 1, 10).Result;
                DataNewUser = new ObservableCollection<User>(result);

            };
           
            backgroundWorker.RunWorkerAsync();
        }

        public async Task LoadListViewNewSong()
        {
            var result = new List<Song>();

            var backgroundWorker = new BackgroundWorker();

            backgroundWorker.DoWork += (sender, e) =>
            {
                result = _songService.SearchSongs("", "", "", 1, 10).Result;
                DataNewSong = new ObservableCollection<Song>(result);
            };

            backgroundWorker.RunWorkerAsync();
        }

        public async Task LoadCountPieChart()
        {
            var countSong = new List<object[]>();
            var countArtist = new List<object[]>();
         
            var backgroundWorker = new BackgroundWorker();

            backgroundWorker.DoWork += (sender, e) =>
            {
                countSong = _songService.GetCountSongsInEachArea().Result;
                countArtist = _artistService.GetCountArtistsInEachArea().Result;
            };


            backgroundWorker.RunWorkerCompleted += (sender, e) =>
            {
                LoadDataToPieChart(@this.pieChartCountSongInByArea, countSong);
                LoadDataToPieChart(@this.pieChartCountArtistInByArea, countArtist);
            };

            backgroundWorker.RunWorkerAsync();

        }

        public async Task LoadInformationCards()
        {
            var dataSrc = new List<InformationCardModel>();
            var dataTotalUser = new InformationCardModel();
            var backgroundWorker = new BackgroundWorker();

            backgroundWorker.DoWork += (sender, e) =>
            {
                dataTotalUser = GetDataTotalUsers().Result;
                dataSrc = GetDatas().Result;
            };


            backgroundWorker.RunWorkerCompleted += (sender, e) =>
            {
                @this.cTotalUsers.SetData(dataTotalUser);

                var container = @this.gContainerInformationCard;
                for (int i = 0; i < container.Children.Count; i++)
                {
                    var data = dataSrc[i];
                    var control = container.Children[i] as InformationCard;
                    control.SetData(data);
                }

            };
            @this.Dispatcher.Invoke(() =>
            {
                backgroundWorker.RunWorkerAsync();

            });
        }

        public async Task LoadDataCartesianChart(DateTime fromDate, DateTime toDate)
        {
            var backgroundWorker = new BackgroundWorker();
            var dataColumn = new List<double>();
            var valuesX = new List<string>();
            backgroundWorker.DoWork += async (sender, e) =>
            {

                var result = _userService.SearchUsers("", "", "1", fromDate, toDate, 1, 10).Result;
                var groupBy = result.GroupBy(t => new DateTime(t.CreatedAt.Value.Year, t.CreatedAt.Value.Month, t.CreatedAt.Value.Day)).ToList();

                foreach (var item in groupBy)
                {
                    valuesX.Add(item.Key.ToString("dd-MM-yyyy"));
                    dataColumn.Add(item.ToList().Count());
                }
            };


            backgroundWorker.RunWorkerCompleted += (sender, e) =>
            {
                LoadDataToLineColumnChart<double>(@this.cartesianChart, "Thời gian", valuesX.ToArray()
                    , "Tài khoản mới", "", dataColumn.ToArray());
            };
            @this.Dispatcher.Invoke(() =>
            {
                backgroundWorker.RunWorkerAsync();
            });
        }

        public async Task<List<InformationCardModel>> GetDatas()
        {
            var dataSrc = new List<InformationCardModel>();

            dataSrc.Add(new InformationCardModel
            {
                Icon = PackIconKind.Music,
                Title = (await _songService.GetTotalPages("", "", "", pageSize: 1)).ToString(),
                SubTitle = "Tổng bài hát",
                BackgroundColor = ColorConst.subBackgroundColor,
                IconColor = Color.FromArgb(255, 253, 96, 176),
                ForegroundTitle = Color.FromArgb(255, 253, 96, 176),
                ForegroundSubTitle = ColorConst.foregroundColor
            });
            dataSrc.Add(new InformationCardModel
            {
                Icon = PackIconKind.Artist,
                Title = (await _artistService.GetTotalPages("", "", "", pageSize: 1)).ToString(),
                SubTitle = "Tổng nghệ sỹ",
                BackgroundColor = ColorConst.subBackgroundColor,
                IconColor = Color.FromArgb(255, 251, 172, 88),
                ForegroundTitle = Color.FromArgb(255, 251, 172, 88),
                ForegroundSubTitle = ColorConst.foregroundColor
            });
            dataSrc.Add(new InformationCardModel
            {
                Icon = PackIconKind.Album,
                Title = (await _albumService.GetTotalPages("", "", "", pageSize: 1)).ToString(),
                SubTitle = "Tổng album",
                BackgroundColor = ColorConst.subBackgroundColor,
                IconColor = ColorConst.iconColor,
                ForegroundTitle = ColorConst.iconColor,
                ForegroundSubTitle = ColorConst.foregroundColor
            });
            dataSrc.Add(new InformationCardModel
            {
                Icon = PackIconKind.MusicBox,
                Title = (await _songService.GetTotalListens()).ToString(),
                SubTitle = "Tổng lượt nghe",
                BackgroundColor = ColorConst.subBackgroundColor,
                IconColor = Color.FromArgb(255, 125, 216, 216),
                ForegroundTitle = Color.FromArgb(255, 125, 216, 216),
                ForegroundSubTitle = ColorConst.foregroundColor
            });
            dataSrc.Add(new InformationCardModel
            {
                Icon = PackIconKind.Downloads,
                Title = (await _songService.GetTotalDownloads()).ToString(),
                SubTitle = "Tổng lượt tải",
                BackgroundColor = ColorConst.subBackgroundColor,
                IconColor = Color.FromArgb(255, 252, 49, 110),
                ForegroundTitle = Color.FromArgb(255, 252, 49, 110),
                ForegroundSubTitle = ColorConst.foregroundColor
            });

            return dataSrc;
        }

        public async Task<InformationCardModel> GetDataTotalUsers()
        {
            return new InformationCardModel
            {
                Icon = PackIconKind.UserBadge,
                Title = (await _userService.GetTotalPages("", "", ""
                        , new System.DateTime(), DateTime.Now, pageSize: 1)).ToString(),
                SubTitle = "Tổng tài khoản người dùng",
                BackgroundColor = ColorConst.subBackgroundColor,
                IconColor = Colors.CornflowerBlue,
                ForegroundTitle = Colors.CornflowerBlue,
                ForegroundSubTitle = ColorConst.foregroundColor
            };
        }

        async void LoadDataToPieChart(PieChart target, List<object[]> data)
        {
            target.SeriesColors = new ColorsCollection
            {
                ColorConst.iconColor,
                Color.FromArgb(255, 125, 216, 216),
                Color.FromArgb(255, 251, 172, 88),
                Color.FromArgb(255, 253, 96, 176),
                Color.FromArgb(255, 252, 49, 110),
            };

            target.InnerRadius = target.ActualHeight * 0.1;
            target.LegendLocation = LegendLocation.Right;
            var series = new SeriesCollection();
            for (int i = 0; i < data.Count; i++)
            {
                var item = data[i];
                series.Add(new PieSeries
                {
                    Title = item[0] == null ? "Another" : item[0].ToString(),
                    Values = new ChartValues<double> { double.Parse(item[1].ToString()) },
                    PushOut = 7,
                    DataLabels = true,
                    StrokeThickness = 0,
                    FontSize = 13,
                });
            }

            target.Series = series;
        }

        void LoadDataToLineColumnChart<T>(CartesianChart target, string titleX, string[] valuesX, string titleY
            , string titleColumn, T[] dataColumn)
        {
            target.Series.Clear();
            target.AxisX.Clear();
            target.AxisY.Clear();

            target.Series = new SeriesCollection
            {
                new LineSeries
                {
                    Title = titleColumn,
                    Values = new ChartValues<T>(dataColumn)
                }
            };


            target.AxisX.Add(new Axis
            {
                Title = titleX,
                Labels = valuesX,
                Foreground = new SolidColorBrush(ColorConst.foregroundColor),

            });

            target.AxisY.Add(new Axis
            {
                Title = titleY,
                Foreground = new SolidColorBrush(ColorConst.foregroundColor),
                LabelFormatter = value => value.ToString("#,##0 tài khoản mới")
            });

        }

    }
}
