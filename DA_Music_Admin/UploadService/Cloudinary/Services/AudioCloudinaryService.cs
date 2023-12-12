using Microsoft.AspNetCore.Http;
using UploadService.Cloudinary.Setting;
using CloudinaryDotNet;
using UploadService.IServices;
using Microsoft.Extensions.Options;
using CloudinaryDotNet.Actions;

namespace UploadService.Cloudinary.Services
{
    public class AudioCloudinary
    {
        public string FileName { get; set; }
        public string? DestinationFolder { get; set; }
        public string? PublicId { get;  set; }
    }
    public class AudioCloudinaryService : IAudioUploadService
    {
        protected readonly CloudinaryDotNet.Cloudinary _cloudinary;

        public AudioCloudinaryService(IOptions<CloudinarySettings> config)
        {
            var acc = new Account(config.Value.CloudName, config.Value.ApiKey, config.Value.ApiSecret);
            _cloudinary = new CloudinaryDotNet.Cloudinary(acc);
        }


        //public async Task<ImageUploadResult> AddAudioAsync(List<IFormFile> files, List<object> AudioDescriptions)
        //{
        //    var uploadResult = new ImageUploadResult();
        //    for (var i = 0; i < files.Count; i++)
        //    {
        //        var file = files[i];
        //        if (file.Length > 0)
        //        {
        //            using var stream = file.OpenReadStream();
        //            var uploadParams = new ImageUploadParams
        //            {
        //                File = new FileDescription(file.FileName, stream),
        //                Folder = AudioDescriptions[i].DestinationFolder,
        //                PublicId = AudioDescriptions[i].PublicId
        //            };
        //            uploadResult = await _cloudinary.UploadLargeAsync(uploadParams);
        //        }
        //    }
        //    return uploadResult;
        //}


        public async Task<object> AddAudioAsync(IFormFile file, object audioDescription)
        {
            var uploadResult = new RawUploadResult();
            var audioCloudinary = audioDescription as AudioCloudinary;
            if (file.Length > 0)
            {
                using var stream = file.OpenReadStream();
                var uploadParams = new RawUploadParams
                {
                    File = new FileDescription(file.FileName, stream),
                    Folder = audioCloudinary.DestinationFolder,
                    PublicId = audioCloudinary.PublicId
                };
                uploadResult = await _cloudinary.UploadLargeAsync(uploadParams);
            }
            return uploadResult;
        }

        public async Task<object> AddAudioAsync(string filename, Stream stream, object audioDescription)
        {
            var uploadResult = new RawUploadResult();
            var audioCloudinary = audioDescription as AudioCloudinary;
            if (!string.IsNullOrEmpty(filename) && stream != null)
            {
                var uploadParams = new RawUploadParams
                {
                    File = new FileDescription(filename, stream),
                    Folder = audioCloudinary.DestinationFolder,
                    PublicId = audioCloudinary.PublicId
                };
                uploadResult = await _cloudinary.UploadLargeAsync(uploadParams);
            }
            return uploadResult;
        }

        public Task<object> DeleteAudioAsync(params string[] publicIds)
        {
            throw new NotImplementedException();
        }
    }
}
