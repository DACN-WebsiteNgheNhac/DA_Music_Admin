using DA_Music_Admin;

namespace Services.IServices
{
    public interface ISongService : IService<Song>
    {
        public Task<List<Song>> SearchSongs(string query, string area, string artistId, int pageNumber = -1, int pageSize = -1);
        public Task<int> GetTotalPages(string query, string area, string artistId, int pageSize = -1);


        Task<Song> EditSong(Song data, bool newImageFile, bool newAudioFile);

        Task<List<Song>> GetSongsByArtistId(string artistId);
        Task<List<Song>> GetSongsByAlbumId(string artistId);

        public Task<double> GetTotalDownloads();
        public Task<double> GetTotalListens();

        public Task<List<object[]>> GetCountSongsInEachArea();
    }
}
