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
using Services;
using System.Linq;

namespace DA_Music_Admin.ViewModels
{
    public class UserViewModel : PageViewModel
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

        UserPage @this;

        private ObservableCollection<User> _DataUser = new ObservableCollection<User>();
        public ObservableCollection<User> DataUser
        {
            get { return _DataUser; }
            set
            {
                _DataUser = value;
                _ = loadData("DataUser");
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


        private ObservableCollection<string> _DataGender = new ObservableCollection<string>(DataConst.DataGenderWithNullOption);
        public ObservableCollection<string> DataGender
        {
            get { return _DataGender; }
            set { _DataGender = value; _ = loadData(nameof(DataGender)); }
        }

        private string _SelectedGender;
        public string SelectedGender
        {
            get { return _SelectedGender; }
            set { _SelectedGender = value; _ = loadData(nameof(SelectedGender)); }
        }

        private ObservableCollection<Role> _DataRole = new ObservableCollection<Role>();
        public ObservableCollection<Role> DataRole
        {
            get { return _DataRole; }
            set { _DataRole = value; _ = loadData(nameof(DataRole)); }
        }

        private Role _SelectedRole;
        public Role SelectedRole
        {
            get { return _SelectedRole; }
            set 
            { 
                _SelectedRole = value; 
                _ = loadData(nameof(SelectedRole)); 
            }
        }

        private DateTime _FromDate;
        public DateTime FromDate
        {
            get { return _FromDate; }
            set { _FromDate = value; _ = loadData(nameof(FromDate)); }
        } 
        
        private DateTime _ToDate = DateTime.Now;
        public DateTime ToDate
        {
            get { return _ToDate; }
            set { _ToDate = value; _ = loadData(nameof(ToDate)); }
        }


        private Visibility _ShowButtonClearQuery = Visibility.Hidden;
        public Visibility ShowButtonClearQuery
        {
            get { return _ShowButtonClearQuery; }
            set { _ShowButtonClearQuery = value; _ = loadData("ShowButtonClearQuery"); }
        }

        private bool _IsSelectedAllItem = false;
        private readonly EditSongPageSecondSlideViewModel _editSongPageSecondSlideViewModel;
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;
        private readonly IAbstractFactory<EditUserPage> _editUserPage;


        public UserViewModel(EditSongPageSecondSlideViewModel editSongPageSecondSlideViewModel
            , IUserService userService
            , IRoleService roleService
            , IAbstractFactory<EditUserPage> editUserPage) : base()
        {
            InitCommands();
            _editSongPageSecondSlideViewModel = editSongPageSecondSlideViewModel;
            _userService = userService;
            _roleService = roleService;
            _editUserPage = editUserPage;
        }

        public override void Page_Loaded_CodeBehind(Page t)
        {
            base.Page_Loaded_CodeBehind(t);

            @this = t as UserPage;

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
                OpenEditUserPage(t.DataContext);
            });

            #region Datagrid
            DatagridRowClicked = new RelayCommand<DataGridRow>(
            (t) =>
            {
                return t != null ? true : false;
            },
            (t) =>
            {
                User dataCurrentRow = t.DataContext as User;
              
            });


            DeleteItem = new RelayCommand<Button>(
            (t) =>
            {
                return t != null ? true : false;
            },
            async (t) =>
            {
                var resultDialog = MessageBox.Show("Bạn có muốn xóa dữ liệu?", "Thông báo", MessageBoxButton.YesNo, MessageBoxImage.Information, MessageBoxResult.No, MessageBoxOptions.RightAlign);
                User data = t.DataContext as User;
                if (resultDialog == MessageBoxResult.Yes)
                {

                    var res = await _userService.GetListAlls();
                    if (res == null)
                    {
                        ViewHolder.Ins.ShowFailNotify("Có lỗi khi xóa bài hát " + data.Name);
                    }
                    else
                    {
                        DataUser.Remove(data);
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
                await GetDataRoles();
            }));
        }


        protected void BtnClearQueryCLicked(Button sender)
        {
            Query = "";
        }
        DateTimeOffset StartDate(DateTime? date)
        {
            if (date != null)
            {
                if (date.Value.Year == 1)
                    date = date.Value.AddYears(1);
            }
            return date == null
                ? new DateTimeOffset(new DateTime(2000, 1, 1, 0, 0, 0), TimeSpan.FromHours(7))
                : new DateTimeOffset(date.Value.Year, date.Value.Month, date.Value.Day, 1, 0, 0, TimeSpan.FromHours(7));
        }

        DateTimeOffset EndDate(DateTime? date)
        {
            date = date == null ? DateTime.Now : date;
            return new DateTimeOffset(new DateTime(date.Value.Year, date.Value.Month, date.Value.Day, 23, 59, 59));
        }
        protected async Task Search()
        {
            ViewHolder.Ins.ShowLoadingScreen();
            object itemsLock = new object();

            BindingOperations.EnableCollectionSynchronization(DataUser, itemsLock);


            var backgroundWorker = new BackgroundWorker();
            backgroundWorker.DoWork += async (sender, e) =>
            {
                var gender = SelectedGender;
                var roleId = SelectedRole == null ? "" : SelectedRole.Id;

                var dataUser = await _userService.SearchUsers(Query, gender, roleId, FromDate, ToDate, CurrentPage, SelectedEntry);

                gender = string.IsNullOrEmpty(gender) ? "" : gender;

                var temp = dataUser.Where(t => t.Gender.Contains(gender)).ToList();
                TotalPage = await _userService.GetTotalPages(Query, gender, roleId, FromDate, ToDate, SelectedEntry);
                if (dataUser == null)
                    return;
                DataUser = new ObservableCollection<User>(dataUser);
                @this.dgvData.Dispatcher.Invoke(() =>
                {
                    if (DataUser.Count == 0)
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

        protected async Task GetDataRoles()
        {
            object itemsLock = new object();

            BindingOperations.EnableCollectionSynchronization(DataRole, itemsLock);

            var backgroundWorker = new BackgroundWorker();
            backgroundWorker.DoWork += async (sender, e) =>
            {
                var data = await _roleService.GetListAlls();
                if (data == null)
                    return;
                DataRole = new ObservableCollection<Role>(data);
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

        public void OpenEditUserPage(object data)
        {
            var target = _editUserPage.Create();
            target.vm.Data = (data as User);
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
