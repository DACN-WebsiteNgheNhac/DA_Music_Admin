﻿using CustomControls.Utils;
using DA_Music_Admin.SystemInfor;
using MaterialDesignThemes.Wpf.Transitions;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows;
using DA_Music_Admin.Views;
using System.IO;
using StartUpHelperWPF;
using Services.IServices;
using System.Threading.Tasks;
using TaskStatus = CustomControls.Controls.TaskStatus;
using DA_Music_Admin.Const;

namespace DA_Music_Admin.ViewModels
{
    public class EditArtistViewModel : PageViewModel
    {
        protected const int OPTION_INSERT = 1;
        protected const int OPTION_UPDATE = 2;
        protected const string CONTENT_BUTTON_INSERT = "Thêm";
        protected const string CONTENT_BUTTON_UPDATE = "Cập nhật";

        #region Start Commands


        #region Start View 

        public ICommand View_Loaded { get; set; }
        public ICommand View_Unloaded { get; set; }


        #endregion End MainView 

        #region Image file
        public ICommand OpenImageFileSelector { get; set; }
        public ICommand DragEnterImageFile { get; set; }
        public ICommand DragLeaveImageFile { get; set; }
        public ICommand DropImageFile { get; set; }
        #endregion Image file

        #region Audio file
        public ICommand OpenAudioFileSelector { get; set; }
        public ICommand DragEnterAudioFile { get; set; }
        public ICommand DragLeaveAudioFile { get; set; }
        public ICommand DropAudioFile { get; set; }

        #endregion Audio file

        #region Excute Action

        public ICommand ExcuteAction { get; set; }

        #endregion Excute Action

        public ICommand ContainerImage_SizeChanged { get; set; }

        public ICommand NextSlide { get; set; }

        #endregion End Commands

        EditArtistPage view;
        private readonly IArtistService _artistService;
        private readonly IAbstractFactory<EditArtistPageSecondSlide> _fSecondSlide;

        private EditArtistPageSecondSlide _SecondSlide;

        public EditArtistPageSecondSlide SecondSlide
        {
            get { return _SecondSlide; }
            set
            {
                _SecondSlide = value;
                _ = loadData(nameof(SecondSlide));
            }
        }


        double limitSizeImage = 200;
        private double _SizeImage = 200;
        public double SizeImage
        {
            get { return _SizeImage; }
            set { _SizeImage = value; _ = loadData("SizeImage"); }
        }

        SolidColorBrush NormalForegroundColor = new SolidColorBrush(ColorConst.foregroundColor);
        SolidColorBrush HoveredForegroundColor = new SolidColorBrush(ColorConst.iconColor);

        #region Image file

        private string _SelectedImageFile = "Chưa chọn file...";
        public string SelectedImageFile
        {
            get { return _SelectedImageFile; }
            set
            {
                _SelectedImageFile = value == string.Empty ? "Chưa chọn file..." : value;

                if (value == string.Empty)
                {
                    if (PrimaryImageFile == string.Empty)
                    {
                        value = string.Empty;
                        switchImageState(true);
                    }
                    else
                    {
                        _SelectedImageFile = value = PrimaryImageFile;
                        switchImageState(false);
                    }
                }
                else
                {
                    switchImageState(false);
                }
                Data.Image = value;
                _ = loadData("Data");
                _ = loadData("SelectedImageFile");
            }
        }

        private SolidColorBrush _StrokeRectangleImage;
        public SolidColorBrush StrokeRectangleImage
        {
            get
            {
                if (_StrokeRectangleImage == null)
                    _StrokeRectangleImage = new SolidColorBrush(ColorConst.foregroundColor_80);
                return _StrokeRectangleImage;
            }
            set
            {
                _StrokeRectangleImage = value;
                _ = loadData("StrokeRectangleImage");
            }
        }

        private SolidColorBrush _ForegroundImageFile;
        public SolidColorBrush ForegroundImageFile
        {
            get
            {
                if (_ForegroundImageFile == null)
                    _ForegroundImageFile = NormalForegroundColor;
                return _ForegroundImageFile;
            }
            set { _ForegroundImageFile = value; _ = loadData("ForegroundImageFile"); }
        }

        public void switchImageState(bool isNull)
        {
            if (isNull)
            {
                ForegroundImageFile = NormalForegroundColor;
                StrokeRectangleImage = new SolidColorBrush(ColorConst.foregroundColor_80);
            }
            else
            {
                StrokeRectangleImage = new SolidColorBrush(Colors.Transparent);
                ForegroundImageFile = new SolidColorBrush(Colors.Transparent);
            }
        }

        #endregion Image file

        public List<Artist> SelectedItems = new List<Artist>();

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

        private DateTime _BirthDay = DateTime.Now;
        public DateTime BirthDay
        {
            get { return _BirthDay; }
            set 
            { 
                _BirthDay = value;
                Data.BirthDay = value;
                _ = loadData(nameof(BirthDay));
            }
        }

        private DateTime _DebutDate = DateTime.Now;
        public DateTime DebutDate
        {
            get { return _DebutDate; }
            set
            {
                _DebutDate = value;
                Data.DebutDate = value;
                _ = loadData(nameof(DebutDate));
            }
        }


        #region Data

        private Artist _Data = new Artist() {  };
        public Artist Data
        {
            get { return _Data; }
            set
            {
                string image = string.Empty;
                if (value == null)
                {
                    CurrentOption = OPTION_INSERT;
                    PrimaryImageFile = "";
                    SelectedImageFile = "";
                    value = new Artist();
                }
                else
                {
                    image = value.Image;
                    CurrentOption = OPTION_UPDATE;
                }
                _Data = value;



                _ = loadData("Data");
                if (!string.IsNullOrEmpty(image))
                {
                    PrimaryImageFile = image;
                    SelectedImageFile = image;
                }
            }
        }

        public string PrimaryImageFile = string.Empty;
        public string PrimaryAudioFile = string.Empty;

        private List<string> _Gender = new List<string>() { "Nam", "Nữ" };
        public List<string> Gender
        {
            get { return _Gender; }
            set { _Gender = value; }
        }

        private ObservableCollection<string> _Region;
        public ObservableCollection<string> Region
        {
            get { return _Region; }
            set { _Region = value; _ = loadData("Region"); }
        }
        #endregion Data

        private int _CurrentOption = OPTION_INSERT;

        private string _ContentButtonAction = CONTENT_BUTTON_INSERT;
        public string ContentButtonAction
        {
            get { return _ContentButtonAction; }
            set { _ContentButtonAction = value; _ = loadData("ContentButtonAction"); }
        }


        public int CurrentOption
        {
            get { return _CurrentOption; }
            set
            {
                _CurrentOption = value;
                ContentButtonAction = value == OPTION_INSERT ? CONTENT_BUTTON_INSERT : CONTENT_BUTTON_UPDATE;
            }
        }


        public EditArtistViewModel(IArtistService ArtistService,
            IAbstractFactory<EditArtistPageSecondSlide> secondSlide) : base()
        {
            initCommands();
            _artistService = ArtistService;
            _fSecondSlide = secondSlide;
        }

        public override void Page_Loaded_CodeBehind(Page t)
        {
            base.Page_Loaded_CodeBehind(t);

            view = t as EditArtistPage;

            SecondSlide = _fSecondSlide.Create();

            //view.TriggerControl.setChecked(true);

            ViewHolder.Ins.HideLoadingScreen();
        }

        void initCommands()
        {
            View_Unloaded = new RelayCommand<EditArtistPage>(
            (t) =>
            {
                return t != null ? true : false;
            },
            (t) =>
            {
                Transitioner.MoveFirstCommand.Execute(null, SecondSlide.btnBackSlide);
            });

            #region Image file
            OpenImageFileSelector = new RelayCommand<FrameworkElement>(
            (t) =>
            {
                return t != null ? true : false;
            },
            (t) =>
            {
                SelectedImageFile = chooseImageFile();
            });

            DragEnterImageFile = new RelayCommand<FrameworkElement>(
            (t) =>
            {
                return t != null ? true : false;
            },
            (t) =>
            {
                ForegroundImageFile = HoveredForegroundColor;
            });

            DragLeaveImageFile = new RelayCommand<FrameworkElement>(
            (t) =>
            {
                return t != null ? true : false;
            },
            (t) =>
            {
                ForegroundImageFile = NormalForegroundColor;
            });

            DropImageFile = new RelayCommand<DragEventArgs>(
            (t) =>
            {
                return t != null ? true : false;
            },
            (t) =>
            {
                if (t.Data.GetDataPresent(DataFormats.FileDrop))
                {
                    string[] files = (string[])t.Data.GetData(DataFormats.FileDrop);
                    string extension = Path.GetExtension(files[0]);
                    string[] imageExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".bmp" };
                    if (imageExtensions.Contains(extension.ToLower()))
                    {
                        string filename = Path.GetFullPath(files[0]);
                        SelectedImageFile = filename;
                    }
                }
            });
            #endregion Image file


            ContainerImage_SizeChanged = new RelayCommand<Grid>(
             (t) =>
             {
                 return t != null ? true : false;
             },
             (t) =>
             {
                 double width = view.containerImage.ActualWidth;
                 double height = view.containerImage.ActualHeight;
                 double targetSize = width < height ? width : height;
                 SizeImage = targetSize < limitSizeImage ? targetSize : limitSizeImage;
             });


            #region Excute Action
            ExcuteAction = new RelayCommand<Button>(
            (t) =>
            {
                return t != null ? true : false;
            },
            async (t) =>
            {
                if (!CheckInputs())
                    return;
                if (CurrentOption == OPTION_INSERT)
                {
                    string title = "Đang thêm nghệ sỹ " + Data.Name;
                    string message = "Đang upload file";
                    ViewHolder.Ins.AddProcessItem(GeometryConst.IconCheck, title, message, ExcuteActionInsert);
                }
                else if (CurrentOption == OPTION_UPDATE)
                {
                    string title = "Đang cập nhật nghệ sỹ " + Data.Name;
                    string message = "Đang upload file";
                    ViewHolder.Ins.AddProcessItem(GeometryConst.IconCheck, title, message, ExcuteActionUpdate);
                }
            });


            #endregion

            NextSlide = new RelayCommand<Button>(
            (t) =>
            {
                return t != null ? true : false;
            },
            (t) =>
            {
                LoadSecondSlide();
            });

        }

        string chooseImageFile()
        {
            return openFileDialog("Chọn ảnh", "Files|*.jpg;*.jpeg;*.png;...");
        }

        string chooseAudioFile()
        {
            return openFileDialog("Chọn file nhạc", "Files|*.mp3;...");
        }

        string openFileDialog(string title, string filter)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Title = title;
            fileDialog.Filter = filter;
            fileDialog.Multiselect = false;
            bool? dialogResult = fileDialog.ShowDialog();
            if (dialogResult == true)
            {
                return fileDialog.FileName;
            }
            return string.Empty;
        }


        public async Task<bool> InsertItem()
        {
            if (!CheckInputs())
                return false;
            var result = await _artistService.CreateObject(Data);
            if (result != null)
            {
                ViewHolder.Ins.ShowSuccessNotify("Nghệ sỹ " + Data.Name + " đã được thêm");
                Data = result;
            }
            else
            {
                ViewHolder.Ins.ShowFailNotify("Xảy ra lỗi khi thêm nghệ sỹ " + Data.Name);
            }
            if(result == null)
                return false;
            return true;
        }

        public async Task<TaskStatus> ExcuteActionInsert()
        {
            var result = await InsertItem();
            return new TaskStatus(result);
        }

        public async Task<TaskStatus> ExcuteActionUpdate()
        {
            var result = await UpdateItem();
            return new TaskStatus(result);
        }


        public async Task<bool> UpdateItem()
        {
            if (!CheckInputs())
                return false;
            var result = await _artistService.EditArtist(Data, Data.Image != PrimaryImageFile);
            if (result != null)
            {
                ViewHolder.Ins.ShowSuccessNotify("Bài hát " + Data.Name + " đã được cập nhật");
                Data.Image = PrimaryImageFile = result.Image;
            }
            else
                ViewHolder.Ins.ShowFailNotify("Xảy ra lỗi khi cập nhật Bài hát " + Data.Name);
            if (result == null)
                return false;
            return true;
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

       
        public async void LoadSecondSlide()
        {
            ViewHolder.Ins.ShowLoadingScreen();
                  
            await SecondSlide.vm.loadData(Data, PrimaryImageFile, PrimaryAudioFile);

            ViewHolder.Ins.HideLoadingScreen();
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
                return "Không thể bỏ trống quốc tịch!";
            if (Data.BirthDay >= DateTime.Now)
                return "Ngày sinh không thể lớn hơn ngày hiện tại!";
            if (Data.BirthDay >= Data.DebutDate)
                return "Ngày debut không thể lớn hơn ngày sinh!";
            return "";
        }



        public void removeSelectedItem()
        {
            if (SecondSlide.vm != null)
                SecondSlide.vm.removeSelectedItem();
        }


    }
}
