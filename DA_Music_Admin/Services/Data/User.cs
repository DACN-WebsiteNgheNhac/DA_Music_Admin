using System;
using System.Collections.Generic;

namespace DA_Music_Admin
{
    public partial class User : BaseModel
    {
        public User()
        {
            Comments = new HashSet<Comment>();
            Favorites = new HashSet<Favorite>();
            Playlists = new HashSet<Playlist>();
        }

        public string Id { get; set; } = null!;
        public string? Name { get; set; }
        public DateTimeOffset? BirthDay { get; set; }
        public string? Gender { get; set; }

        public string? AccountId { get; set; }
        public DateTimeOffset? CreatedAt { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }
        public DateTimeOffset? DeletedAt { get; set; }
        public string Username { get; set; } = null!;
        public string? RoleId { get; set; }

        public virtual Account? Account { get; set; }
        public virtual Role? Role { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Favorite> Favorites { get; set; }

        public virtual ICollection<Playlist> Playlists { get; set; }

        private string _Password;

        public string Password
        {
            get { return _Password; }
            set { _Password = value; OnPropertyChanged(nameof(Password)); }
        }

    }
}
