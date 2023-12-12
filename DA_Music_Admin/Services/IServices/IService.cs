using DA_Music_Admin;

namespace Services.IServices
{
    public interface IService<T>
    {
        Task<List<T>> GetListAlls();
        Task<T> GetObjectById(params object[] id);
        Task<T> CreateObject(T data);
    }
}
