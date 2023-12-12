using CustomControls.Utils;
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
using System.Xml.Linq;

namespace DA_Music_Admin.ViewModels
{
    public class EditSongViewModel : PageViewModel
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

        EditSongPage view;
        private readonly ISongService _songService;
        private readonly IAbstractFactory<EditSongPageSecondSlide> _fSecondSlide;

        private EditSongPageSecondSlide _SecondSlide;

        public EditSongPageSecondSlide SecondSlide
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

        #region Audio file

        private string _SelectedAudioFile = "Chưa chọn file...";
        public string SelectedAudioFile
        {
            get { return _SelectedAudioFile; }
            set
            {
                //_SelectedAudioFile = value == string.Empty ? "Chưa chọn file..." : value;
                //_ = loadData("SelectedAudioFile");

                _SelectedAudioFile = value == string.Empty ? "Chưa chọn file..." : value;

                if (value == string.Empty)
                {
                    if (PrimaryAudioFile == string.Empty)
                    {
                        value = string.Empty;
                        switchAudioState(true);
                    }
                    else
                    {
                        _SelectedAudioFile = value = PrimaryAudioFile;
                        switchAudioState(false);
                    }
                }
                else
                {
                    switchAudioState(false);
                }
                Data.SongUrl = value;
                _ = loadData("Data");
                _ = loadData("SelectedAudioFile");
            }
        }

        private SolidColorBrush _StrokeRectangleAudio;
        public SolidColorBrush StrokeRectangleAudio
        {
            get
            {
                if (_StrokeRectangleAudio == null)
                    _StrokeRectangleAudio = new SolidColorBrush(ColorConst.foregroundColor_80);
                return _StrokeRectangleAudio;
            }
            set
            {
                _StrokeRectangleAudio = value;
                _ = loadData("StrokeRectangleAudio");
            }
        }

        private SolidColorBrush _ForegroundAudioFile;
        public SolidColorBrush ForegroundAudioFile
        {
            get
            {
                if (_ForegroundAudioFile == null)
                    _ForegroundAudioFile = NormalForegroundColor;
                return _ForegroundAudioFile;
            }
            set { _ForegroundAudioFile = value; _ = loadData("ForegroundAudioFile"); }
        }


        public void switchAudioState(bool isNull)
        {
            if (isNull)
            {
                ForegroundAudioFile = NormalForegroundColor;
                StrokeRectangleAudio = new SolidColorBrush(ColorConst.foregroundColor_80);
            }
            else
            {
                StrokeRectangleAudio = new SolidColorBrush(Colors.Transparent);
                ForegroundAudioFile = new SolidColorBrush(Colors.Transparent);
            }
        }

        #endregion Audio file

        public List<Artist> SelectedItems = new List<Artist>();

        private ObservableCollection<string> _DataAreas = new ObservableCollection<string>() {"Vpop", "Kpop", "Jpop", "USUK" };
        public ObservableCollection<string> DataAreas
        {
            get { return _DataAreas; }
            set
            {
                _DataAreas = value;
                _ = loadData("DataAreas");
            }
        }
        
        #region Data

        private Song _Data = new Song() { Area = "Vpop" };
        public Song Data
        {
            get { return _Data; }
            set
            {
                string image = string.Empty;
                string audio = string.Empty;
                if (value == null)
                {
                    CurrentOption = OPTION_INSERT;
                    PrimaryAudioFile = PrimaryImageFile = "";
                    SelectedAudioFile = SelectedImageFile = "";
                    value = new Song();
                }
                else
                {
                    image = value.Image;
                    audio = value.SongUrl;
                    CurrentOption = OPTION_UPDATE;
                }
                _Data = value;
                _ = loadData("Data");
                if(!string.IsNullOrEmpty(image))
                {
                    PrimaryImageFile = image;
                    SelectedImageFile = image;
                }
                if (!string.IsNullOrEmpty(audio))
                {
                    PrimaryAudioFile = audio;
                    SelectedAudioFile = audio;
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


        public EditSongViewModel(ISongService songService, 
            IAbstractFactory<EditSongPageSecondSlide> secondSlide) : base()
        {
            initCommands();
            _songService = songService;
            _fSecondSlide = secondSlide;
        }

        public override void Page_Loaded_CodeBehind(Page t)
        {
            base.Page_Loaded_CodeBehind(t);

            view = t as EditSongPage;

            SecondSlide = _fSecondSlide.Create();

            //view.TriggerControl.setChecked(true);

            ViewHolder.Ins.HideLoadingScreen();
        }

        void initCommands()
        {
            View_Unloaded = new RelayCommand<EditSongPage>(
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

            #region Audio file
            OpenAudioFileSelector = new RelayCommand<FrameworkElement>(
             (t) =>
             {
                 return t != null ? true : false;
             },
             (t) =>
             {
                 SelectedAudioFile = chooseAudioFile();
             });

            DragEnterAudioFile = new RelayCommand<FrameworkElement>(
            (t) =>
            {
                return t != null ? true : false;
            },
            (t) =>
            {
                ForegroundAudioFile = HoveredForegroundColor;
            });

            DragLeaveAudioFile = new RelayCommand<FrameworkElement>(
            (t) =>
            {
                return t != null ? true : false;
            },
            (t) =>
            {
                ForegroundAudioFile = NormalForegroundColor;
            });

            DropAudioFile = new RelayCommand<DragEventArgs>(
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
            #endregion Audio file

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
                    string title = "Đang thêm bài hát " + Data.Name;
                    string message = "Đang upload file";
                    ViewHolder.Ins.AddProcessItem(GeometryConst.IconCheck, title, message, ExcuteActionInsert);
                }
                else if (CurrentOption == OPTION_UPDATE)
                {
                    string title = "Đang cập nhật bài hát " + Data.Name;
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
            var result = await _songService.CreateObject(Data);
            if (result != null)
            {
                ViewHolder.Ins.ShowSuccessNotify("Bài hát " + Data.Name + " đã được thêm");
                Data = result;
            }
            else
            {
                ViewHolder.Ins.ShowFailNotify("Xảy ra lỗi khi thêm bài hát " + Data.Name);
            }
            if (result == null)
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
            var result = await _songService.EditSong(Data, Data.Image != PrimaryImageFile, Data.SongUrl != PrimaryAudioFile);
            if (result != null)
            {
                ViewHolder.Ins.ShowSuccessNotify("Bài hát " + Data.Name + " đã được cập nhật");
                Data.Image = PrimaryImageFile = result.Image;
                Data.SongUrl = PrimaryAudioFile = result.SongUrl;
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

            foreach (var item in view.ContainerInputs.Children)
            {
                var temp = item as TextBox;
                if (temp != null)
                {
                    temp.GetBindingExpression(TextBox.TextProperty).UpdateSource();
                }
            }

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
            if (string.IsNullOrEmpty(Data.Name))
                return "Không thể bỏ trống tựa đề bài hát!";
            if (string.IsNullOrEmpty(Data.SongUrl) && string.IsNullOrEmpty(PrimaryAudioFile))
                return "Không thể bỏ trống file audio!"; 
            if (string.IsNullOrEmpty(Data.Area))
                return "Không thể bỏ trống khu vực!";
            return "";
        }



        public void removeSelectedItem()
        {
            if (SecondSlide.vm != null)
                SecondSlide.vm.removeSelectedItem();
        }


    }
}
