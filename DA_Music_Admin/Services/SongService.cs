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
    public class SongService : ISongService
    {
        private readonly MusicContext _context;
        private readonly IPhotoUploadService _photoUploadService;
        private readonly IAudioUploadService _audioUploadService;

        public SongService(MusicContext musicContext,
            IPhotoUploadService photoUploadService,
            IAudioUploadService audioUploadService
            )
        {
            _context = musicContext;
            _photoUploadService = photoUploadService;
            _audioUploadService = audioUploadService;
        }

        public async Task<List<Song>> GetListAlls()
        {
            var _context = new MusicContext();
            return await _context.Set<Song>().ToListAsync();
        }

        public async Task<int> GetTotalPages(string query, string area, string artistId, int pageSize = -1)
        {
            var totalPage = (await SearchSongs(query, area, artistId)).Count;
            return (int)Math.Ceiling(totalPage / (pageSize * 1.0));
        }

        public async Task<List<Song>> SearchSongs(string query, string area, string artistId, int pageNumber = -1, int pageSize = -1)
        {
            var _context = new MusicContext();
            query = string.IsNullOrEmpty(query) ? "" : query;
            area = (string.IsNullOrEmpty(area) || area == "Chọn khu vực") ? "" : area;
            artistId = (string.IsNullOrEmpty(artistId) || artistId == "Chọn nghệ sỹ") ? "" : artistId;
            

            Expression<Func<Song, bool>> predicate =
             t => (t.Name.Contains(query)
             || string.IsNullOrEmpty(t.Description) ? true : t.Description.Contains(query)
             || t.ArtistSongs.Any(t => t.Artist.Name.Contains(query) || string.IsNullOrEmpty(t.Artist.Description) ? true : t.Artist.Description.Contains(query)))
             && (t.DeletedAt == null
             && string.IsNullOrEmpty(t.Area) ? true : t.Area.Contains(area)
             && t.ArtistSongs.Count == 0 ? true : t.ArtistSongs.Any(t => t.ArtistId.Contains(artistId)));

            if (pageNumber < 0 || pageSize < 0)
                return await _context.Set<Song>().AsNoTracking()
                    .Where(predicate)
                    .OrderByDescending(t => t.Id)
                    .ToListAsync();
            return await _context.Set<Song>().AsNoTracking()
                    .Where(predicate)
                    .OrderByDescending(t => t.Id)
                    .Skip((pageNumber - 1) * pageSize).Take(pageSize)
                    .ToListAsync();
        }

        public async Task<Song> EditSong(Song data, bool newImageFile, bool newAudioFile)
        {
            if (data == null)
                return null;

            string image = data.Image;
            string audio = data.SongUrl;

            if (newImageFile && !string.IsNullOrEmpty(data.Image))
            {
                var publicId = UploadConst.PrefixImage + data.Id;
                image = await UploadImage(data.Image, publicId);
            }

            if (newAudioFile && !string.IsNullOrEmpty(data.SongUrl))
            {
                var publicId = UploadConst.PrefixAudio + data.Id;
                audio = await UploadAudio(data.SongUrl, publicId);
            }

            data.Image = image;
            data.SongUrl = audio;
            data.UpdatedAt = DateTimeOffset.Now;

            _context.Set<Song>().Update(data);
            await _context.SaveChangesAsync();
            _context.Entry(data).State = EntityState.Detached;
            return data;
        }

        public async Task<Song> GetObjectById(params object[] id)
        {
         
            return await _context.Set<Song>().FindAsync(id);
        }

        public async Task<Song> CreateObject(Song data)
        {
            if (data == null)
                return null;

            var lastestIndex = await GetLastestIndex();
            var id = "S" + lastestIndex.ToString("000");

            data.Id = id;

            string image = null;
            string audio = null;

            if(!string.IsNullOrEmpty(data.Image))
            {
                var publicId = UploadConst.PrefixImage + data.Id;
                image = await UploadImage(data.Image, publicId);
            }

            if (!string.IsNullOrEmpty(data.SongUrl))
            {
                var publicId = UploadConst.PrefixAudio + data.Id;
                audio = await UploadAudio(data.SongUrl, publicId);
            }

            data.Image = image;
            data.SongUrl = audio;
            data.CreatedAt = DateTimeOffset.Now;
            await _context.Set<Song>().AddAsync(data);
            await _context.SaveChangesAsync();

            return data;
        }

       

        public async Task<List<Song>> GetSongsByArtistId(string artistId)
        {
            return await _context.Set<Song>().AsNoTracking()
                .Where(t => t.ArtistSongs.Any(t => t.ArtistId == artistId))
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<Song>> GetSongsByAlbumId(string albumId)
        {
            return await _context.Set<Song>().AsNoTracking()
               .Where(t => t.AlbumSongs.Any(t => t.AlbumId == albumId))
               .OrderByDescending(t => t.CreatedAt)
               .ToListAsync();
        }

        public async Task<int> GetLastestIndex()
        {
            var lastObject = await _context.Set<Song>().AsNoTracking()
                .OrderByDescending(t => t.Id)
                .FirstOrDefaultAsync();

            if (lastObject == null)
                return 1;
            else
            {
                var prefix = "S";
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
                        DestinationFolder = UploadConst.SongFolder.Image, 
                        PublicId = publicId 
                    });

                return ((ImageUploadResult)resultUpload).SecureUrl.ToString();
            }
        }

        public async Task<string> UploadAudio(string fileName, string publicId)
        {
            using (FileStream fileStream = File.OpenRead(fileName))
            {
                var resultUpload = await _audioUploadService.AddAudioAsync(fileName, fileStream
                    , new AudioCloudinary
                    { 
                        DestinationFolder = UploadConst.SongFolder.Audio, 
                        PublicId = publicId 
                    });

                return ((RawUploadResult)resultUpload).SecureUrl.ToString();
            }

        }

        public async Task<double> GetTotalDownloads()
        {
            return await _context.Set<Song>().AsNoTracking()
                .Where(t => t.DeletedAt != null)
                .SumAsync(t => t.Downloads);
        }

        public async Task<double> GetTotalListens()
        {
            return await _context.Set<Song>().AsNoTracking()
              .Where(t => t.DeletedAt != null)
              .SumAsync(t => t.Listens);
        }

        public async Task<List<object[]>> GetCountSongsInEachArea()
        {
            var returnData = new List<object[]>();
            var _context = new MusicContext();
            var data = await _context.Set<Song>().ToListAsync();
            var groupBy = data.GroupBy(t => t.Area).ToList();

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
