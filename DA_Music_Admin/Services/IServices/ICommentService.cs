using DA_Music_Admin;
    
namespace Services.IServices
{
    public interface ICommentService : IService<Comment>
    {
        Task<List<Comment>> GetCommentsBySongId(string songId, int pageNumber = -1, int pageSize = -1, bool orderByDesc = true);
        public Task<int> GetTotalPages(string songId, int pageSize = -1);

        Task<Comment> UpdateComment(Comment data);

    }
}
