using DA_Music_Admin.SystemInfor;
using DA_Music_Admin.Themes;
using DA_Music_Admin.ViewModels;
using DA_Music_Admin.Views;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Services;
using Services.IServices;
using StartUpHelperWPF;
using System;
using System.Windows;
using UploadService.Cloudinary.Services;
using UploadService.Cloudinary.Setting;
using UploadService.IServices;

namespace DA_Music_Admin
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static IHost? AppHost { get; private set; }

        public App()
        {
            AppHost = Host.CreateDefaultBuilder()
                .ConfigureServices((hostContext, services) => {

                    AddServices(services);
                })
                .Build();
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            await AppHost!.StartAsync();

            //var startUp = AppHost.Services.GetRequiredService<MainWindow>();
            //startUp.Show();

            var startUp = AppHost.Services.GetRequiredService<LoginPage>();
            startUp.Show();

            this.DispatcherUnhandledException += App_DispatcherUnhandledException;

            base.OnStartup(e);
        }

        private void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show("Đã có lỗi xảy ra.\nHãy chắc rằng kết nối internet đã đc ổn định!", "Thông báo"
                , MessageBoxButton.OK
                , MessageBoxImage.Information
                , MessageBoxResult.OK
                , MessageBoxOptions.RightAlign);
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            await AppHost!.StopAsync();
            base.OnExit(e);
        }

        public void AddServices(IServiceCollection services)
        {
            services.AddSingleton<CloudinarySettings>();
            services.AddTransient<IAudioUploadService, AudioCloudinaryService>();
            services.AddTransient<IPhotoUploadService, PhotoCloudinaryService>();

            services.AddTransient<MusicContext>();

            services.AddWindowFactorySingleton<MainWindow>();
            services.AddSingleton<LoginPage>();

            services.AddSingleton<MainViewModel>();

            

            services.AddSingleton<IArtistSongService, ArtistSongService>();
            services.AddSingleton<IAlbumSongService, AlbumSongService>();

            services.AddWindowFactorySingleton<CommentPage>();
            services.AddSingleton<CommentViewModel>();
            services.AddSingleton<ICommentService, CommentService>();

            AddUserService(services);
            AddHomeService(services);
            AddSongService(services);
            AddArtistService(services);
            AddAlbumService(services);
        }

        private void AddUserService(IServiceCollection services)
        {
            services.AddSingleton<IRoleService, RoleService>();

            services.AddSingleton<AccountManager>();

            services.AddWindowFactorySingleton<UserPage>();
            services.AddSingleton<UserViewModel>();
            services.AddSingleton<IUserService, UserService>();

            services.AddWindowFactorySingleton<EditUserPage>();
            services.AddSingleton<EditUserViewModel>();
        }

        private void AddHomeService(IServiceCollection services)
        {
            services.AddWindowFactoryTransient<HomePage>();
            services.AddSingleton<HomeViewModel>();
        }

        void AddSongService(IServiceCollection services)
        {
            services.AddWindowFactoryTransient<SongPage>();
            services.AddSingleton<SongViewModel>();
            services.AddSingleton<ISongService, SongService>();

            services.AddWindowFactorySingleton<EditSongPage>();
            services.AddSingleton<EditSongViewModel>();

            services.AddWindowFactorySingleton<EditSongPageSecondSlide>();
            services.AddSingleton<EditSongPageSecondSlideViewModel>();
        }

        void AddArtistService(IServiceCollection services)
        {
            services.AddWindowFactorySingleton<ArtistPage>();
            services.AddSingleton<ArtistViewModel>();
            services.AddSingleton<IArtistService, ArtistService>();

            services.AddWindowFactorySingleton<EditArtistPage>();
            services.AddSingleton<EditArtistViewModel>();

            services.AddWindowFactorySingleton<EditArtistPageSecondSlide>();
            services.AddSingleton<EditArtistPageSecondSlideViewModel>();
        }

        private void AddAlbumService(IServiceCollection services)
        {
            services.AddSingleton<ITopicService, TopicService>();


            services.AddWindowFactorySingleton<AlbumPage>();
            services.AddSingleton<AlbumViewModel>();
            services.AddSingleton<IAlbumService, AlbumService>();

            services.AddWindowFactorySingleton<EditAlbumPage>();
            services.AddSingleton<EditAlbumViewModel>();

            services.AddWindowFactorySingleton<EditAlbumPageSecondSlide>();
            services.AddSingleton<EditAlbumPageSecondSlideViewModel>();
        }


        //public void AddServices(IServiceCollection services)
        //{
        //    services.AddSingleton<CloudinarySettings>();
        //    services.AddTransient<IAudioUploadService, AudioCloudinaryService>();
        //    services.AddTransient<IPhotoUploadService, PhotoCloudinaryService>();

        //    services.AddTransient<MusicContext>();

        //    services.AddSingleton<MainWindow>();
        //    services.AddSingleton<MainViewModel>();

        //    services.AddTransient<IArtistSongService, ArtistSongService>();
        //    services.AddTransient<IAlbumSongService, AlbumSongService>();

        //    services.AddWindowFactoryTransient<CommentPage>();
        //    services.AddTransient<CommentViewModel>();
        //    services.AddTransient<ICommentService, CommentService>();

        //    AddSongService(services);
        //    AddArtistService(services);
        //    AddAlbumService(services);
        //}

        //void AddSongService(IServiceCollection services)
        //{
        //    services.AddWindowFactoryTransient<SongPage>();
        //    services.AddTransient<SongViewModel>();
        //    services.AddTransient<ISongService, SongService>();

        //    services.AddWindowFactoryTransient<EditSongPage>();
        //    services.AddTransient<EditSongViewModel>();

        //    services.AddWindowFactoryTransient<EditSongPageSecondSlide>();
        //    services.AddSingleton<EditSongPageSecondSlideViewModel>();
        //}

        //void AddArtistService(IServiceCollection services)
        //{
        //    services.AddWindowFactoryTransient<ArtistPage>();
        //    services.AddTransient<ArtistViewModel>();
        //    services.AddTransient<IArtistService, ArtistService>();

        //    services.AddWindowFactoryTransient<EditArtistPage>();
        //    services.AddTransient<EditArtistViewModel>();

        //    services.AddWindowFactoryTransient<EditArtistPageSecondSlide>();
        //    services.AddSingleton<EditArtistPageSecondSlideViewModel>();
        //}

        //private void AddAlbumService(IServiceCollection services)
        //{
        //    services.AddTransient<ITopicService, TopicService>();


        //    services.AddWindowFactoryTransient<AlbumPage>();
        //    services.AddTransient<AlbumViewModel>();
        //    services.AddTransient<IAlbumService, AlbumService>();

        //    services.AddWindowFactoryTransient<EditAlbumPage>();
        //    services.AddTransient<EditAlbumViewModel>();

        //    services.AddWindowFactoryTransient<EditAlbumPageSecondSlide>();
        //    services.AddSingleton<EditAlbumPageSecondSlideViewModel>();
        //}

    }
}
