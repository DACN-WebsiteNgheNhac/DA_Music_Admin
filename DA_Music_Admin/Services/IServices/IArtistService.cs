using DA_Music_Admin;

namespace Services.IServices
{
    public interface IArtistService : IService<Artist>
    {
        Task<List<Artist>> GetArtistsBySongId(string songId);
        public Task<List<Artist>> SearchArtists(string query, string gender, string national, int pageNumber = -1, int pageSize = -1);
        public Task<int> GetTotalPages(string query, string gender, string national, int pageSize = -1);

        Task<Artist> EditArtist(Artist data, bool newImageFile);

        public Task<List<object[]>> GetCountArtistsInEachArea();
    }
}
