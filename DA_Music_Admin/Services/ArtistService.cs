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
    public class ArtistService : IArtistService
    {
        private readonly MusicContext _context;
        private readonly IPhotoUploadService _photoUploadService;

        public ArtistService(MusicContext musicContext
            , IPhotoUploadService photoUploadService)
        {
            _context = musicContext;
            _photoUploadService = photoUploadService;
        }
        public async Task<Artist> CreateObject(Artist data)
        {
            if (data == null)
                return null;

            var lastestIndex = await GetLastestIndex();
            var id = "AR" + lastestIndex.ToString("000");

            data.Id = id;

            string image = null;

            if (!string.IsNullOrEmpty(data.Image))
            {
                var publicId = UploadConst.PrefixImage + data.Id;
                image = await UploadImage(data.Image, publicId);
            }

            data.Image = image;
            data.CreatedAt = DateTimeOffset.Now;


            await _context.Set<Artist>().AddAsync(data);
            await _context.SaveChangesAsync();
            _context.Entry(data).State = EntityState.Detached;
            return data;
        }

        public async Task<Artist> EditArtist(Artist data, bool newImageFile)
        {
            if (data == null)
                return null;

            string image = data.Image;

            if (newImageFile && !string.IsNullOrEmpty(data.Image))
            {
                var publicId = UploadConst.PrefixImage + data.Id;
                image = await UploadImage(data.Image, publicId);
            }

            data.Image = image;
            data.UpdatedAt = DateTimeOffset.Now;

            _context.Set<Artist>().Update(data);
            await _context.SaveChangesAsync();
            _context.Entry(data).State = EntityState.Detached;

            return data;
        }

        public async Task<List<Artist>> GetArtistsBySongId(string songId)
        {
            return await _context.Set<Artist>().AsNoTracking()
                .Include(t => t.ArtistSongs)
                .Where(t => t.ArtistSongs.Any(t => t.SongId == songId))
                .Distinct()
                .ToListAsync();
        }

        public async Task<List<Artist>> GetListAlls()
        {
            var _context = new MusicContext();
            return await _context.Set<Artist>().AsNoTracking()
                .OrderByDescending(t => t.Id)
                .ToListAsync();
        }

        public Task<Artist> GetObjectById(params object[] id)
        {
            throw new NotImplementedException();
        }

        public async Task<int> GetTotalPages(string query, string gender, string national, int pageSize = -1)
        {
            var totalPage = (await SearchArtists(query, gender, national)).Count;
            return (int)Math.Ceiling(totalPage / (pageSize * 1.0));
        }

        public async Task<List<Artist>> SearchArtists(string query, string gender, string national, int pageNumber = -1, int pageSize = -1)
        {
            var _context = new MusicContext();
            query = string.IsNullOrEmpty(query) ? "" : query;
            gender = (string.IsNullOrEmpty(gender) || gender == "Chọn giới tính") ? "" : gender;
            national = (string.IsNullOrEmpty(national) || national == "Chọn quốc gia") ? "" : national;
            

            Expression<Func<Artist, bool>> predicate =
             t => (t.Name.Contains(query)
             || string.IsNullOrEmpty(t.Description) ? true : t.Description.Contains(query)
             || t.ArtistSongs.Count == 0 ? true : t.ArtistSongs.Any(t => t.Song.Name.Contains(query) || string.IsNullOrEmpty(t.Song.Description) ? true : t.Song.Description.Contains(query)))
             && (t.DeletedAt == null
             && t.Gender.Contains(gender)
             && string.IsNullOrEmpty(t.National) ? true : t.National.Contains(national));

            if (pageNumber < 0 || pageSize < 0)
                return await _context.Set<Artist>().AsNoTracking()
                    .Include(t => t.ArtistSongs)
                    .ThenInclude(t => t.Song).AsNoTracking()
                    .Where(predicate)
                    .OrderByDescending(t => t.CreatedAt)
                    .ToListAsync();
            return await _context.Set<Artist>().AsNoTracking()
                    .Include(t => t.ArtistSongs)
                    .ThenInclude(t => t.Song).AsNoTracking()
                    .Where(predicate)
                    .OrderByDescending(t => t.CreatedAt)
                    .Skip((pageNumber - 1) * pageSize).Take(pageSize)
                    .ToListAsync();
        }


        public async Task<int> GetLastestIndex()
        {
            var lastObject = await _context.Set<Artist>().AsNoTracking()
                .OrderByDescending(t => t.Id)
                .FirstOrDefaultAsync();

            if (lastObject == null)
                return 1;
            else
            {
                var prefix = "AR";
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
                        DestinationFolder = Const.UploadConst.ArtistFolder.Image, 
                        PublicId = publicId 
                    });

                return ((ImageUploadResult)resultUpload).SecureUrl.ToString();
            }
        }

        public async Task<List<object[]>> GetCountArtistsInEachArea()
        {
            var returnData = new List<object[]>();
            var _context = new MusicContext();
            var data = await _context.Set<Artist>().ToListAsync();
            var groupBy = data.GroupBy(t => t.National).ToList();

            foreach (var item in groupBy)
            {
                var key = item.Key;
                var count = item.ToList().Count;
                returnData.Add(new object[] { key, count.ToString() });
            }

            return returnData;
        }
    }
}
