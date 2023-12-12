namespace DA_Music_Admin
{
    public partial class Comment : SuperBaseModel
    {
        public string Id { get; set; } = null!;
        public string UserId { get; set; } = null!;
        public string SongId { get; set; } = null!;
        public string Content { get; set; } = null!;
        public DateTimeOffset? CreatedAt { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }
       

        public virtual Song Song { get; set; } = null!;
        public virtual User User { get; set; } = null!;

        private DateTimeOffset? _DeletedAt;
        public DateTimeOffset? DeletedAt
        {
            get { return _DeletedAt; }
            set { _DeletedAt = value; OnPropertyChanged(nameof(DeletedAt)); }
        }

    }
}
