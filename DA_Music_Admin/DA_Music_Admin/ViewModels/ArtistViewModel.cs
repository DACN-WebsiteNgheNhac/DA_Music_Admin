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
using System.Linq;
using DA_Music_Admin.Const;

namespace DA_Music_Admin.ViewModels
{
    public class ArtistViewModel : PageViewModel
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

        ArtistPage @this;

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


        private ObservableCollection<string> _DataNationals = new ObservableCollection<string>(DataConst.DataNational);
        public ObservableCollection<string> DataNationals
        {
            get { return _DataNationals; }
            set
            {
                _DataNationals = value;
                _ = loadData(nameof(DataNationals));
            }
        }
        public string SelectedNational { get; set; }

        private ObservableCollection<string> _DataGenders = new ObservableCollection<string>(DataConst.DataGender);
        public ObservableCollection<string> DataGenders
        {
            get { return _DataGenders; }
            set
            {
                _DataGenders = value;
                _ = loadData(nameof(DataGenders));
            }
        }
        public string SelectedGender { get; set; }


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
        private readonly EditSongPageSecondSlideViewModel _editSongPageSecondSlideViewModel;
        private readonly IArtistService _artistService;
        private readonly IAbstractFactory<EditArtistPage> _editArtistPage;

        public bool IsSelectedAllItem
        {
            get { return _IsSelectedAllItem; }
            set
            {
                _IsSelectedAllItem = value;
                _ = loadData("IsSelectedAllItem");
                foreach (Artist item in DataArtist)
                    item.IsSelected = _IsSelectedAllItem;
            }
        }




        public ArtistViewModel(EditSongPageSecondSlideViewModel editSongPageSecondSlideViewModel
            , IArtistService ArtistService
            , IAbstractFactory<EditArtistPage> editArtistPage) : base()
        {
            InitCommands();
            _editSongPageSecondSlideViewModel = editSongPageSecondSlideViewModel;
            _artistService = ArtistService;
            _editArtistPage = editArtistPage;
        }

        public override void Page_Loaded_CodeBehind(Page t)
        {
            base.Page_Loaded_CodeBehind(t);

            @this = t as ArtistPage;

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
               OpenEditArtistPage(t.DataContext);
           });

            #region Datagrid
            DatagridRowClicked = new RelayCommand<DataGridRow>(
             (t) =>
             {
                 return t != null ? true : false;
             },
             (t) =>
             {
                 Artist dataCurrentRow = t.DataContext as Artist;
                 //dataCurrentRow.IsSelected = true;
                 var selectedRows = new ObservableCollection<Artist>(DataArtist.ToList().Where(item => item.IsSelected == true).ToList());
                 selectedRows.Add(dataCurrentRow);
                 DragItemHelper.Ins.setDataDymnamicItem(dataCurrentRow);
                 DragDrop.DoDragDrop(t, new DataObject(DataFormats.Serializable, selectedRows)
                     , DragDropEffects.Move);
             });


            DeleteItem = new RelayCommand<Button>(
            (t) =>
            {
                return t != null ? true : false;
            },
            async (t) =>
            {
                var resultDialog = MessageBox.Show("Bạn có muốn xóa dữ liệu?", "Thông báo", MessageBoxButton.YesNo, MessageBoxImage.Information, MessageBoxResult.No, MessageBoxOptions.RightAlign);
                Artist data = t.DataContext as Artist;
                if (resultDialog == MessageBoxResult.Yes)
                {

                    var res = await _artistService.GetListAlls();
                    if (res == null)
                    {
                        ViewHolder.Ins.ShowFailNotify("Có lỗi khi xóa bài hát " + data.Name);
                    }
                    else
                    {
                        DataArtist.Remove(data);
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
                _editSongPageSecondSlideViewModel.removeSelectedItem();
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
                await Search();
                await GetDataArtists();
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

            BindingOperations.EnableCollectionSynchronization(DataArtist, itemsLock);


            var backgroundWorker = new BackgroundWorker();
            backgroundWorker.DoWork += async (sender, e) =>
            {
                var dataArtist = await _artistService.SearchArtists(Query, SelectedGender, SelectedNational, CurrentPage, SelectedEntry);
                TotalPage = await _artistService.GetTotalPages(Query, SelectedGender, SelectedNational, SelectedEntry);
                if (dataArtist == null)
                    return;
                DataArtist = new ObservableCollection<Artist>(dataArtist);
                @this.dgvData.Dispatcher.Invoke(() =>
                {
                    if (DataArtist.Count == 0)
                        ViewHolder.Ins.HideLoadingScreen();
                });
            };

            backgroundWorker.RunWorkerCompleted += (sender, e) =>
            {
                @this.dgvData.LoadingRow += DgvData_LoadingRow;
            };

            backgroundWorker.RunWorkerAsync();
        }

        private void DgvData_LoadingRow(object? sender, DataGridRowEventArgs e)
        {
            ViewHolder.Ins.HideLoadingScreen();
            @this.dgvData.LoadingRow -= DgvData_LoadingRow;
        }

        protected async Task GetDataArtists()
        {
            //object itemsLock = new object();

            //BindingOperations.EnableCollectionSynchronization(DataArtist, itemsLock);


            //var backgroundWorker = new BackgroundWorker();
            //backgroundWorker.DoWork += async (sender, e) =>
            //{
            //    var data = await _ArtistService.SearchArtists(Query);
            //    if (data == null)
            //        return;
            //    DataArtist = new ObservableCollection<Artist>(data);
            //};

            //backgroundWorker.RunWorkerCompleted += (sender, e) =>
            //{
            //    ViewHolder.Ins.HideLoadingScreen();
            //};
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

        public void OpenEditArtistPage(object data)
        {
            var target = _editArtistPage.Create();
            target.vm.Data = (data as Artist);
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
