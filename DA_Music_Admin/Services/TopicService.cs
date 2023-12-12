using DA_Music_Admin;
using Microsoft.EntityFrameworkCore;
using Services.IServices;

namespace Services
{
    public class TopicService : ITopicService
    {
        private readonly MusicContext _context;

        public TopicService(MusicContext context) 
        {
            _context = context;
        }
        public Task<Topic> CreateObject(Topic data)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Topic>> GetListAlls()
        {
            var _context = new MusicContext();
            return await _context.Set<Topic>().AsNoTracking()
                .OrderByDescending(t => t.Id)
                .ToListAsync();
        }

        public Task<Topic> GetObjectById(params object[] id)
        {
            throw new NotImplementedException();
        }
    }
}
