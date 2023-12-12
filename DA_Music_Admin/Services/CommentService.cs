using DA_Music_Admin;
using Microsoft.EntityFrameworkCore;
using Services.IServices;

namespace Services
{
    public class CommentService : ICommentService
    {
        private readonly MusicContext _context;

        public CommentService(MusicContext context)
        {
            _context = context;
        }

        public Task<Comment> CreateObject(Comment data)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Comment>> GetCommentsBySongId(string songId, int pageNumber = -1, int pageSize = -1, bool orderByDesc = true)
        {
            songId = string.IsNullOrEmpty(songId) ? "" : songId;

            var _context = new MusicContext();
            if(orderByDesc)
                if (pageNumber < 0 || pageSize < 0)
                    return await _context.Set<Comment>().AsNoTracking()
                         .Where(t => songId == "" ? t.SongId.Contains(songId) : t.SongId == songId)
                         .OrderByDescending(t => t.CreatedAt)
                         .Include(t => t.User)
                         .Include(t => t.Song)
                         .ToListAsync();
                else 
                    return await _context.Set<Comment>().AsNoTracking()
                        .Where(t => songId == "" ? t.SongId.Contains(songId) : t.SongId == songId)
                        .OrderByDescending(t => t.CreatedAt)
                        .Skip((pageNumber - 1) * pageSize).Take(pageSize)
                        .Include(t => t.User)
                        .Include(t => t.Song)
                        .ToListAsync();

            return await GetCommentsBySongIdAsc(songId, pageNumber, pageSize);
        }

        private async Task<List<Comment>> GetCommentsBySongIdAsc(string songId, int pageNumber = -1, int pageSize = -1)
        {
            songId = string.IsNullOrEmpty(songId) ? "" : songId;

            var _context = new MusicContext();
            if (pageNumber < 0 || pageSize < 0)
                return await _context.Set<Comment>().AsNoTracking()
                     .Where(t => songId == "" ? t.SongId.Contains(songId) : t.SongId == songId)
                     .OrderBy(t => t.CreatedAt)
                     .Include(t => t.User)
                     .Include(t => t.Song)
                     .ToListAsync();
            else
                return await _context.Set<Comment>().AsNoTracking()
                    .Where(t => songId == "" ? t.SongId.Contains(songId) : t.SongId == songId)
                    .OrderBy(t => t.CreatedAt)
                    .Skip((pageNumber - 1) * pageSize).Take(pageSize)
                    .Include(t => t.User)
                    .Include(t => t.Song)
                    .ToListAsync();
        }

        public async Task<List<Comment>> GetListAlls()
        {
            var _context = new MusicContext();
            return await _context.Set<Comment>().AsNoTracking()
                .OrderByDescending(t => t.CreatedAt)
                .Include(t => t.User)
                .Include(t => t.Song)
                .ToListAsync();
        }

        public Task<Comment> GetObjectById(params object[] id)
        {
            throw new NotImplementedException();
        }

        public async Task<int> GetTotalPages(string songId, int pageSize = -1)
        {
            var totalPage = (await GetCommentsBySongId(songId)).Count;
            return (int)Math.Ceiling(totalPage / (pageSize * 1.0));
        }

        public async Task<Comment> UpdateComment(Comment data)
        {
            try
            {
                _context.Set<Comment>()
                .Update(data);
                await _context.SaveChangesAsync();
                _context.Entry(data).State = EntityState.Detached;

                return data;
            }
            catch { return null; }
        }
    }
}
