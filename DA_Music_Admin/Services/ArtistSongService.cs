using DA_Music_Admin;
using Microsoft.EntityFrameworkCore;
using Services.IServices;

namespace Services
{
    public class ArtistSongService : IArtistSongService
    {
        private readonly MusicContext _musicContext;

        public ArtistSongService(MusicContext musicContext
          
            )
        {
            _musicContext = musicContext;
         
        }

        public async Task<List<ArtistSong>> AddMultiArtistSong(List<ArtistSong> data, bool isDeleteItemsByArtistId = false)
        {
            if(!isDeleteItemsByArtistId)
            {
                string songId = "";
                if (data.Count == 0)
                    return null;
                songId = data[0].SongId;

                await RemoveAllBySongId(songId);

                if (data[0].ArtistId != null)
                    await _musicContext.Set<ArtistSong>()
                        .AddRangeAsync(data);
            }
            else
            {
                string artistId = "";
                if (data.Count == 0)
                    return null;
                artistId = data[0].ArtistId;

                await RemoveAllByArtistId(artistId);

                if (data[0].SongId != null)
                    await _musicContext.Set<ArtistSong>()
                        .AddRangeAsync(data);
            }

            await _musicContext.SaveChangesAsync();
            return data;
        }

        public Task<ArtistSong> CreateObject(ArtistSong data)
        {
            throw new NotImplementedException();
        }

        public Task<List<ArtistSong>> GetListAlls()
        {
            throw new NotImplementedException();
        }

        public Task<ArtistSong> GetObjectById(params object[] id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<ArtistSong>> RemoveAllByArtistId(string artistId)
        {
            var _context = new MusicContext();
            var entities = await _musicContext.Set<ArtistSong>()
                .Where(t => t.ArtistId == artistId)
                .ToListAsync();

            if (entities.Count() > 0)
            {
                _musicContext.Set<ArtistSong>()
                    .RemoveRange(entities);
                await _musicContext.SaveChangesAsync();
            }
            return entities;
        }

        public async Task<List<ArtistSong>> RemoveAllBySongId(string songId)
        {
            var _context = new MusicContext();
            var entities = await _musicContext.Set<ArtistSong>()
                .Where(t => t.SongId == songId)
                .ToListAsync();

            if(entities.Count() > 0)
            {
                _musicContext.Set<ArtistSong>()
                    .RemoveRange(entities);
                await _musicContext.SaveChangesAsync();
            }
            return entities;
        }
    }
}
