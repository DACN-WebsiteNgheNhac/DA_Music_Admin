using DA_Music_Admin.SystemInfor;
using Services;
using Services.IServices;
using StartUpHelperWPF;
using System.Windows;

namespace DA_Music_Admin.Views
{
  
    public partial class LoginPage : Window
    {
        private readonly IAbstractFactory<MainWindow> _mainWindow;
        private readonly IUserService _userService;
        private readonly AccountManager _accountManager;

        public LoginPage(IAbstractFactory<MainWindow> mainWindow
            , IUserService userService
            , AccountManager accountManager)
        {
            InitializeComponent();
            _mainWindow = mainWindow;
            _userService = userService;
            _accountManager = accountManager;
        }

        private void Close(object sender, RoutedEventArgs e)
        {
            var window = _mainWindow.Create();
            window.Tag = "Force Close";
            window.Close();
            Close();
        }

        private void Minimize(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
           
        }

        private async void Login(object sender, RoutedEventArgs e)
        {
            LoadingIndicator.Visibility = Visibility.Visible;
            var username = txtUsername.Text;
            var password = txtPass.Password;
            var account = await _userService.Login(username, password);
            if(account == null)
            {
                LoadingIndicator.Visibility = Visibility.Hidden;
                MessageBox.Show("Sai tài khoản hoặc mật khẩu!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                LoadingIndicator.Visibility = Visibility.Hidden;
                this.Hide();
                _accountManager.SaveAccount(account);
                var window = _mainWindow.Create();
                window.Tag = null;
                window.ShowDialog();
                this.ShowDialog();
            }
        }
    }
}
