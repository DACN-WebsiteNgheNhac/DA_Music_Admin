using DA_Music_Admin;

namespace Services.IServices
{
    public interface IAlbumService : IService<Album>
    {
        public Task<List<Album>> SearchAlbums(string query, string topicId, string artistId, int pageNumber = -1, int pageSize = -1);
        public Task<int> GetTotalPages(string query, string topicId, string artistId, int pageSize = -1);

        Task<Album> EditAlbum(Album data, bool newImageFile);
    }
}
