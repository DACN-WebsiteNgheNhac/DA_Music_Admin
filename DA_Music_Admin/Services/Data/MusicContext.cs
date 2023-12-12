using Microsoft.EntityFrameworkCore;

namespace DA_Music_Admin
{
    public partial class MusicContext : DbContext
    {
        public MusicContext()
        {
        }

        public MusicContext(DbContextOptions<MusicContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Account> Accounts { get; set; } = null!;
        public virtual DbSet<Album> Albums { get; set; } = null!;
        public virtual DbSet<AlbumSong> AlbumSongs { get; set; } = null!;
        public virtual DbSet<Artist> Artists { get; set; } = null!;
        public virtual DbSet<ArtistSong> ArtistSongs { get; set; } = null!;
        public virtual DbSet<Comment> Comments { get; set; } = null!;
        public virtual DbSet<Favorite> Favorites { get; set; } = null!;
        public virtual DbSet<Home> Homes { get; set; } = null!;
        public virtual DbSet<MusicVideo> MusicVideos { get; set; } = null!;
        public virtual DbSet<Playlist> Playlists { get; set; } = null!;
        public virtual DbSet<PlaylistSong> PlaylistSongs { get; set; } = null!;
        public virtual DbSet<Role> Roles { get; set; } = null!;
        public virtual DbSet<Song> Songs { get; set; } = null!;
        public virtual DbSet<Topic> Topics { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("workstation id=ViteMusic.mssql.somee.com;packet size=4096;user id=Nhom6_LTWeb_SQLLogin_1;pwd=ykqtec2w6g;data source=ViteMusic.mssql.somee.com;persist security info=False;initial catalog=ViteMusic");
                //optionsBuilder.UseSqlServer("Data Source=.;Initial Catalog=Music2;Integrated Security=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>(entity =>
            {
                entity.ToTable("Account");

                entity.Property(e => e.Id).HasMaxLength(36);
            });

            modelBuilder.Entity<Album>(entity =>
            {
                entity.ToTable("Album");

                entity.HasIndex(e => e.TopicId, "IX_Album_TopicId");

                entity.Property(e => e.Id).HasMaxLength(36);

                entity.Property(e => e.TopicId).HasMaxLength(36);

                entity.HasOne(d => d.Topic)
                    .WithMany(p => p.Albums)
                    .HasForeignKey(d => d.TopicId);
            });

            modelBuilder.Entity<AlbumSong>(entity =>
            {
                entity.HasKey(e => new { e.AlbumId, e.SongId });

                entity.ToTable("AlbumSong");

                entity.HasIndex(e => e.SongId, "IX_AlbumSong_SongId");

                entity.Property(e => e.AlbumId).HasMaxLength(36);

                entity.Property(e => e.SongId).HasMaxLength(36);

                entity.HasOne(d => d.Album)
                    .WithMany(p => p.AlbumSongs)
                    .HasForeignKey(d => d.AlbumId);

                entity.HasOne(d => d.Song)
                    .WithMany(p => p.AlbumSongs)
                    .HasForeignKey(d => d.SongId);
            });

            modelBuilder.Entity<Artist>(entity =>
            {
                entity.ToTable("Artist");

                entity.Property(e => e.Id).HasMaxLength(36);

                entity.Property(e => e.ArtistName).HasMaxLength(200);

                entity.Property(e => e.Image).HasMaxLength(200);

                entity.Property(e => e.Name).HasMaxLength(200);

                entity.HasMany(d => d.MusicVideos)
                    .WithMany(p => p.Artists)
                    .UsingEntity<Dictionary<string, object>>(
                        "ArtistMusicVideo",
                        l => l.HasOne<MusicVideo>().WithMany().HasForeignKey("MusicVideoId"),
                        r => r.HasOne<Artist>().WithMany().HasForeignKey("ArtistId"),
                        j =>
                        {
                            j.HasKey("ArtistId", "MusicVideoId");

                            j.ToTable("ArtistMusicVideo");

                            j.HasIndex(new[] { "MusicVideoId" }, "IX_ArtistMusicVideo_MusicVideoId");

                            j.IndexerProperty<string>("ArtistId").HasMaxLength(36);

                            j.IndexerProperty<string>("MusicVideoId").HasMaxLength(36);
                        });
            });

            modelBuilder.Entity<ArtistSong>(entity =>
            {
                entity.HasKey(e => new { e.ArtistId, e.SongId });

                entity.ToTable("ArtistSong");

                entity.HasIndex(e => e.SongId, "IX_ArtistSong_SongId");

                entity.Property(e => e.ArtistId).HasMaxLength(36);

                entity.Property(e => e.SongId).HasMaxLength(36);

                entity.HasOne(d => d.Artist)
                    .WithMany(p => p.ArtistSongs)
                    .HasForeignKey(d => d.ArtistId);

                entity.HasOne(d => d.Song)
                    .WithMany(p => p.ArtistSongs)
                    .HasForeignKey(d => d.SongId);
            });

            modelBuilder.Entity<Comment>(entity =>
            {
                entity.ToTable("Comment");

                entity.HasIndex(e => e.SongId, "IX_Comment_SongId");

                entity.HasIndex(e => e.UserId, "IX_Comment_UserId");

                entity.Property(e => e.Id).HasMaxLength(36);

                entity.Property(e => e.SongId).HasMaxLength(36);

                entity.Property(e => e.UserId).HasMaxLength(36);

                entity.HasOne(d => d.Song)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(d => d.SongId);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<Favorite>(entity =>
            {
                entity.HasKey(e => new { e.SongId, e.UserId });

                entity.ToTable("Favorite");

                entity.HasIndex(e => e.UserId, "IX_Favorite_UserId");

                entity.Property(e => e.SongId).HasMaxLength(36);

                entity.Property(e => e.UserId).HasMaxLength(36);

                entity.HasOne(d => d.Song)
                    .WithMany(p => p.Favorites)
                    .HasForeignKey(d => d.SongId);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Favorites)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<Home>(entity =>
            {
                entity.ToTable("Home");
            });

            modelBuilder.Entity<MusicVideo>(entity =>
            {
                entity.ToTable("MusicVideo");

                entity.Property(e => e.Id).HasMaxLength(36);
            });

            modelBuilder.Entity<Playlist>(entity =>
            {
                entity.ToTable("Playlist");

                entity.Property(e => e.Id).HasMaxLength(36);
            });

            modelBuilder.Entity<PlaylistSong>(entity =>
            {
                entity.HasKey(e => new { e.PlaylistId, e.SongId });

                entity.ToTable("PlaylistSong");

                entity.HasIndex(e => e.SongId, "IX_PlaylistSong_SongId");

                entity.Property(e => e.PlaylistId).HasMaxLength(36);

                entity.Property(e => e.SongId).HasMaxLength(36);

                entity.HasOne(d => d.Playlist)
                    .WithMany(p => p.PlaylistSongs)
                    .HasForeignKey(d => d.PlaylistId);

                entity.HasOne(d => d.Song)
                    .WithMany(p => p.PlaylistSongs)
                    .HasForeignKey(d => d.SongId);
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("Role");

                entity.Property(e => e.Id).HasMaxLength(36);
            });

            modelBuilder.Entity<Song>(entity =>
            {
                entity.ToTable("Song");

                entity.Property(e => e.Id).HasMaxLength(36);
            });

            modelBuilder.Entity<Topic>(entity =>
            {
                entity.ToTable("Topic");

                entity.Property(e => e.Id).HasMaxLength(36);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");

                entity.HasIndex(e => e.AccountId, "IX_User_AccountId")
                    .IsUnique()
                    .HasFilter("([AccountId] IS NOT NULL)");

                entity.HasIndex(e => e.RoleId, "IX_User_RoleId");

                entity.HasIndex(e => e.Username, "IX_User_Username")
                    .IsUnique();

                entity.Property(e => e.Id).HasMaxLength(36);

                entity.Property(e => e.AccountId).HasMaxLength(36);

                entity.Property(e => e.Password).HasDefaultValueSql("(N'')");

                entity.Property(e => e.RoleId).HasMaxLength(36);

                entity.HasOne(d => d.Account)
                    .WithOne(p => p.User)
                    .HasForeignKey<User>(d => d.AccountId);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.RoleId);

                entity.HasMany(d => d.Playlists)
                    .WithMany(p => p.Users)
                    .UsingEntity<Dictionary<string, object>>(
                        "UserPlaylist",
                        l => l.HasOne<Playlist>().WithMany().HasForeignKey("PlaylistId"),
                        r => r.HasOne<User>().WithMany().HasForeignKey("UserId"),
                        j =>
                        {
                            j.HasKey("UserId", "PlaylistId");

                            j.ToTable("UserPlaylist");

                            j.HasIndex(new[] { "PlaylistId" }, "IX_UserPlaylist_PlaylistId");

                            j.IndexerProperty<string>("UserId").HasMaxLength(36);

                            j.IndexerProperty<string>("PlaylistId").HasMaxLength(36);
                        });
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
