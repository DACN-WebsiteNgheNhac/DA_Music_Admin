using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using UploadService.Cloudinary.Setting;
using UploadService.IServices;

namespace UploadService.Cloudinary.Services
{
    public class PhotoCloudinary
    {
        public string FileName { get; set; }
        public string? DestinationFolder { get; set; }
        public string? PublicId { get; set; }
    }

    public class PhotoCloudinaryService : IPhotoUploadService
    {
        protected readonly CloudinaryDotNet.Cloudinary _cloudinary;

        public PhotoCloudinaryService(IOptions<CloudinarySettings> config)
        {
            var acc = new Account(config.Value.CloudName, config.Value.ApiKey, config.Value.ApiSecret);
            _cloudinary = new CloudinaryDotNet.Cloudinary(acc);
        }


        //public async Task<ImageUploadResult> AddPhotoAsync(List<IFormFile> files, List<object> photoDescriptions)
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
        //                Folder = photoDescriptions[i].DestinationFolder,
        //                PublicId = photoDescriptions[i].PublicId
        //            };
        //            uploadResult = await _cloudinary.UploadLargeAsync(uploadParams);
        //        }
        //    }
        //    return uploadResult;
        //}


        public async Task<object> AddPhotoAsync(IFormFile file, object photoDescription)
        {
            var uploadResult = new ImageUploadResult();
            var photoCloudinary = photoDescription as PhotoCloudinary;
            if (file.Length > 0)
            {
                using var stream = file.OpenReadStream();
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, stream),
                    Folder = photoCloudinary.DestinationFolder,
                    //PublicId = photoCloudinary.PublicId
                };
                if (photoCloudinary.PublicId != null && photoCloudinary.PublicId != string.Empty)
                    uploadParams.PublicId = photoCloudinary.PublicId;

                uploadResult = await _cloudinary.UploadLargeAsync(uploadParams);
            }
            return uploadResult;
        }

        public async Task<object> AddPhotoAsync(string filename, Stream stream, object photoDescription)
        {
            var uploadResult = new ImageUploadResult();
            var photoCloudinary = photoDescription as PhotoCloudinary;
            if (!string.IsNullOrEmpty(filename) && stream != null)
            {
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(filename, stream),
                    Folder = photoCloudinary.DestinationFolder,
                };
                if (photoCloudinary.PublicId != null && photoCloudinary.PublicId != string.Empty)
                    uploadParams.PublicId = photoCloudinary.PublicId;

                uploadResult = await _cloudinary.UploadLargeAsync(uploadParams);
            }
            return uploadResult;
        }

        public Task<object> DeletePhotoAsync(params string[] publicIds)
        {
            throw new NotImplementedException();
        }
    }
}
