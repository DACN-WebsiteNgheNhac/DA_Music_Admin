
using DA_Music_Admin;

namespace Services.IServices
{
    public interface IAlbumSongService : IService<AlbumSong>
    {
        Task<List<AlbumSong>> RemoveAllBySongId(string songId);
        Task<List<AlbumSong>> RemoveAllByAlbumId(string albumiId);
        Task<List<AlbumSong>> AddMultiAlbumSong(List<AlbumSong> data);
    }
}
