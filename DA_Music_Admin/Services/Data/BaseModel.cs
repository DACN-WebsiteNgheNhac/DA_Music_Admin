using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace DA_Music_Admin
{
    public class BaseModel : SuperBaseModel
    {
        public string? Image { get; set; }

        private bool _IsDropped = false;
        [NotMapped]
        public bool IsDropped
        {
            get => _IsDropped;
            set
            {
                _IsDropped = value;
                OnPropertyChanged("IsDropped");
            }
        }

        private bool _IsSelected = false;
        [NotMapped]
        public bool IsSelected
        {
            get => _IsSelected;
            set
            {
                _IsSelected = value;
                OnPropertyChanged("IsSelected");
            }
        }

    }

    public class SuperBaseModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
