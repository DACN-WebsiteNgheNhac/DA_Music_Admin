using CloudinaryDotNet.Actions;
using DA_Music_Admin;
using Microsoft.EntityFrameworkCore;
using Services.Const;
using Services.IServices;
using System.Linq.Expressions;
using UploadService.Cloudinary.Services;
using UploadService.IServices;

namespace Services
{
    public class AlbumService : IAlbumService
    {
        private readonly MusicContext _context;
        private readonly IPhotoUploadService _photoUploadService;

        public AlbumService(MusicContext context
            , IPhotoUploadService photoUploadService)
        {
            _context = context;
            _photoUploadService = photoUploadService;
        }

        public async Task<Album> CreateObject(Album data)
        {
            if (data == null)
                return null;

            var lastestIndex = await GetLastestIndex();
            var id = "AL" + lastestIndex.ToString("000");

            data.Id = id;

            string image = null;

            if (!string.IsNullOrEmpty(data.Image))
            {
                var publicId = UploadConst.PrefixImage + data.Id;
                image = await UploadImage(data.Image, publicId);
            }

            data.Image = image;
            data.CreatedAt = DateTimeOffset.Now;


            await _context.Set<Album>().AddAsync(data);
            await _context.SaveChangesAsync();

            return data;
        }

        public async Task<Album> EditAlbum(Album data, bool newImageFile)
        {
            if (data == null)
                return null;
            data.Topic = null;
            string image = data.Image;

            if (newImageFile && !string.IsNullOrEmpty(data.Image))
            {
                var publicId = UploadConst.PrefixImage + data.Id;
                image = await UploadImage(data.Image, publicId);
            }

            data.Image = image;
            data.UpdatedAt = DateTimeOffset.Now;

            _context.Set<Album>().Update(data);
            await _context.SaveChangesAsync();
            _context.Entry(data).State = EntityState.Detached;
            return data;
        }

        public Task<List<Album>> GetListAlls()
        {
            throw new NotImplementedException();
        }

        public Task<Album> GetObjectById(params object[] id)
        {
            throw new NotImplementedException();
        }

        public async Task<int> GetTotalPages(string query, string topicId, string artistId, int pageSize = -1)
        {
            var totalPage = (await SearchAlbums(query, topicId, artistId)).Count;
            return (int)Math.Ceiling(totalPage / (pageSize * 1.0));
        }

        public async Task<List<Album>> SearchAlbums(string query, string topicId, string artistId, int pageNumber = -1, int pageSize = -1)
        {
            var _context = new MusicContext();
            query = string.IsNullOrEmpty(query) ? "" : query;
            topicId = (string.IsNullOrEmpty(topicId) || topicId == "Chọn chủ đề") ? "" : topicId;
            artistId = (string.IsNullOrEmpty(artistId) || artistId == "Chọn nghệ sỹ") ? "" : artistId;
          
            Expression<Func<Album, bool>> predicate =
             t => (t.Name.Contains(query)
             || string.IsNullOrEmpty(t.Description) ? true : t.Description.Contains(query)
             || t.AlbumSongs.Count == 0 ? true : t.AlbumSongs.Any(t => t.Song.Name.Contains(query) ||  t.Song.Description.Contains(query))
             || t.Topic.Name.Contains(query))
             && (t.DeletedAt == null
             && t.TopicId.Contains(topicId)
             && t.AlbumSongs.Count == 0 ? true : t.AlbumSongs.Any(t => t.Song.ArtistSongs.Count == 0 ? true : t.Song.ArtistSongs.Any(t => t.ArtistId.Contains(artistId))));

            if (pageNumber < 0 || pageSize < 0)
                return await _context.Set<Album>().AsNoTracking()
                    .Where(predicate)
                    .Include(t => t.Topic)
                    .OrderByDescending(t => t.CreatedAt)
                    .ToListAsync();
            return await _context.Set<Album>().AsNoTracking()
                    .Where(predicate)
                    .Include(t => t.Topic)
                    .OrderByDescending(t => t.CreatedAt)
                    .Skip((pageNumber - 1) * pageSize).Take(pageSize)
                    .ToListAsync();
        }


        public async Task<int> GetLastestIndex()
        {
            var _context = new MusicContext();
            var lastObject = await _context.Set<Album>().AsNoTracking()
                .OrderByDescending(t => t.Id)
                .FirstOrDefaultAsync();

            if (lastObject == null)
                return 1;
            else
            {
                var prefix = "AL";
                var split = lastObject.Id.Split(prefix);
                var id = 0;
                if (split.Count() <= 0)
                {
                    id = 1;
                }
                else
                {
                    var temp = int.Parse(split[1]);
                    id = ++temp;
                }
                return id;
            }

        }

        public async Task<string> UploadImage(string fileName, string publicId)
        {
            using (FileStream fileStream = File.OpenRead(fileName))
            {
                var resultUpload = await _photoUploadService.AddPhotoAsync(fileName, fileStream
                    , new PhotoCloudinary
                    {
                        DestinationFolder = Const.UploadConst.AlbumFolder.Image,
                        PublicId = publicId
                    });

                return ((ImageUploadResult)resultUpload).SecureUrl.ToString();
            }
        }



    }
}
