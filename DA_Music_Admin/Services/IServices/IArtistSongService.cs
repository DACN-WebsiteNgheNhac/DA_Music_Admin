using DA_Music_Admin;

namespace Services.IServices
{
    public interface IArtistSongService : IService<ArtistSong>
    {
        Task<List<ArtistSong>> RemoveAllBySongId(string songId);
        Task<List<ArtistSong>> RemoveAllByArtistId(string artistId);
        Task<List<ArtistSong>> AddMultiArtistSong(List<ArtistSong> data, bool isDeleteItemsByArtistId = false);

    }
}
