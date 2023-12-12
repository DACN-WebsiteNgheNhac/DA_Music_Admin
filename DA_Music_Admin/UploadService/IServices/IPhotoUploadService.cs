using Microsoft.AspNetCore.Http;

namespace UploadService.IServices
{
    public interface IPhotoUploadService
    {
        Task<object> AddPhotoAsync(IFormFile files, object photoDescription);
        Task<object> AddPhotoAsync(string filename, Stream stream, object photoDescription);
        Task<object> DeletePhotoAsync(params string[] publicIds);
    }
}
