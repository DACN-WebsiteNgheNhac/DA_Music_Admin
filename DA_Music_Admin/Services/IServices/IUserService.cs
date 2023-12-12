using DA_Music_Admin;

namespace Services.IServices
{
    public interface IUserService : IService<User>
    {
        public Task<List<User>> SearchUsers(string query, string gender, string roleId
            , DateTime fromDate, DateTime toDate, int pageNumber = -1, int pageSize = -1);
        public Task<int> GetTotalPages(string query, string gender, string roleId
            , DateTime fromDate, DateTime toDate, int pageSize = -1);

        public Task<User> Login(string username, string password);
        public Task<User> EditUser(User data, bool newImage);

    }
}
