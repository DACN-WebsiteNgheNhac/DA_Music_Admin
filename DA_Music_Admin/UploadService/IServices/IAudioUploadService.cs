using Microsoft.AspNetCore.Http;

namespace UploadService.IServices
{
    public interface IAudioUploadService
    {
        Task<object> AddAudioAsync(IFormFile files, object AudioDescription);
        Task<object> AddAudioAsync(string filename, Stream stream, object AudioDescription);
        Task<object> DeleteAudioAsync(params string[] publicIds);
    }
}
