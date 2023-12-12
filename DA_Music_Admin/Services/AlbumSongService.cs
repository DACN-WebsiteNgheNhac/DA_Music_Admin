using DA_Music_Admin;
using Microsoft.EntityFrameworkCore;
using Services.IServices;

namespace Services
{
    public class AlbumSongService : IAlbumSongService
    {
        private readonly MusicContext _context;

        public AlbumSongService(MusicContext context)
        {
            _context = context;
        }
        public async Task<List<AlbumSong>> AddMultiAlbumSong(List<AlbumSong> data)
        {
            string albumId = "";
            if (data.Count == 0)
                return null;
            albumId = data[0].AlbumId;

            await RemoveAllByAlbumId(albumId);

            if (data[0].SongId != null)
            await _context.Set<AlbumSong>()
                .AddRangeAsync(data);

            await _context.SaveChangesAsync();
            return data;
        }

        public Task<AlbumSong> CreateObject(AlbumSong data)
        {
            throw new NotImplementedException();
        }

        public Task<List<AlbumSong>> GetListAlls()
        {
            throw new NotImplementedException();
        }

        public Task<AlbumSong> GetObjectById(params object[] id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<AlbumSong>> RemoveAllByAlbumId(string albumId)
        {
            var _context = new MusicContext();
            var entities = await _context.Set<AlbumSong>().AsNoTracking()
               .Where(t => t.AlbumId == albumId)
               .ToListAsync();

            if (entities.Count() > 0)
            {
                _context.Set<AlbumSong>()
                    .RemoveRange(entities);
                await _context.SaveChangesAsync();
            }
            return entities;
        }

        public Task<List<AlbumSong>> RemoveAllBySongId(string songId)
        {
            throw new NotImplementedException();
        }
    }
}
